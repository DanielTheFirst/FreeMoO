using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Linq;

using SdlDotNet.Core;
using SdlDotNet.Graphics;

using FreemooSDL;
using FreemooSDL.Collections;
using FreemooSDL.Reverse;
    
namespace FreemooSDL.Service
{
    public class ImageService
    {
        // contains the code that prerenders the master of orion graphics and maintains sdl surfaces of all of them
        // because I don't know how you're supposed to do it i am just going to pre render all frames of all images from all
        // lbx graphic files.  it can't be that big.  this game was made in 1994.  most people had less than a meg of ram.

        // apparently once you cross refrence it by archive name and image index and pre render all the frames
        // as SDL surfaces.  so maybe i need to rethink this...
        // it's really fucking big. about 1/2 a gig.  

        // need to find the middle ground between space and speed.  modern computers are pretty fast.  maybe if i store the decoeded
        // image data and write to a surface on the fly at the byte level...

        private FreemooGame mGame = null;

        private ImageCollection mImageCollection = new ImageCollection();
        private List<FreemooImage> mImageList = new List<FreemooImage>();
        Dictionary<string, Color[]> mPalettes = null;
        Dictionary<string, Surface> mImageCache = new Dictionary<string, Surface>();
        public ImageCollection Images
        {
            get
            {
                return mImageCollection;
            }
        }

        public ImageService(FreemooGame pGame)
        {
            mGame = pGame;
        }

        private Surface makeSurface(int[,] pBuffer, Color[] pPalette)
        {
            Surface surf = null;
            if (pBuffer != null)
            {
                //surf = Video.CreateRgbSurface(pBuffer.GetLength(0), pBuffer.GetLength(1));
                surf = new Surface(pBuffer.GetLength(0), pBuffer.GetLength(1));

                Color[] internalPal = new Color[256];
                
                

                //surf.TransparentColor = Color.Magenta;
                //Color[,] decodedImage = new Color[surf.Width, surf.Height];
                //surf.Fill(Color.Magenta);
                byte[] decodedImage = new byte[surf.Width * surf.Height * 4];
                for (int x = 0; x < decodedImage.Length; x++) decodedImage[x] = 0;
                //unsafe
                {
                    //byte* decodedImage = (byte *)surf.Pixels.ToPointer();
                    int currAddy = 0;
                    for (int i = 0; i < surf.Height; i++)
                    {
                        for (int j = 0; j < surf.Width; j++)
                        {
                            //decodedImage[j,i] = pPalette[pBuffer[j, i]];
                            // ugh, couldn't this have been documented somewhere???
                            decodedImage[currAddy + 0] = pPalette[pBuffer[j, i]].B;
                            decodedImage[currAddy + 1] = pPalette[pBuffer[j, i]].G;
                            decodedImage[currAddy + 2] = pPalette[pBuffer[j, i]].R;
                            decodedImage[currAddy + 3] = pPalette[pBuffer[j, i]].A;
                            currAddy += 4;
                        }
                    }
                }
                // this one takes almost 3 minutes
                //surf.SetPixels(new Point(0, 0), decodedImage);
                // this one and the unsafe one take about a minute each...stay safe?
                //surf.SourceColorKey = Color.FromArgb(0xff, 0x00, 0xff);
                
                surf.Lock();
                Marshal.Copy(decodedImage, 0, surf.Pixels, decodedImage.Length);
                surf.Unlock();
                //surf.InvertColors();
                //surf.SaveBmp("test.bmp");
                //surf.SourceColorKey = Color.Magenta;
                surf.TransparentColor = Color.Magenta;//Color.FromArgb(0xff, 0x00, 0xff);
                surf.Transparent = true;
                
            }
            else
            {
                surf = Video.CreateRgbSurface(1, 1);
            }
            return surf;
        }

        private Archive GetArchive(ArchiveEnum ae)
        {
            string fn = Config.DataFolder + "\\LBX\\" + ae.ToString() + ".LBX";
            Archive ar = new Archive(fn);

            return ar;
        }

        private int GetPictureIndex(ArchiveEnum ae, string imgStringIdx)
        {
            XmlDocument xdoc = new XmlDocument();
            string fn = Config.DataFolder + "\\" + FreemooConstants.FREEMOO_GRAPHICS;
            xdoc.Load(fn);

            string xpath = String.Format("/lbx/archive [@name='{0}.LBX']/img [@name='{1}']", ae.ToString(), imgStringIdx);

            XmlNode node = xdoc.SelectSingleNode(xpath);

            return Convert.ToInt32(node.Attributes[1].Value);
        }

        public void loadImages()
        {
            // first load the xml into memory
            XmlDocument xdoc = new XmlDocument();
            string fn = Config.DataFolder + "\\" + FreemooConstants.FREEMOO_GRAPHICS;
            xdoc.Load(fn);

            // now query the xml to find the names of each of the lbx archives
            Dictionary<string, Archive> archives = new Dictionary<string, Archive>();
            XmlNodeList xarchives = xdoc.SelectNodes("/lbx/archive");
            foreach (XmlNode a in xarchives)
            {
                string archiveFn = Config.DataFolder + "\\LBX\\" + a.Attributes["name"].Value;
                Archive ar = new Archive(archiveFn);
                archives.Add(a.Attributes["name"].Value, ar);
            }

            // load fonts.lbx as well since it contains the palette info
            string fontFn = Config.DataFolder + "\\LBX\\FONTS.LBX";
            Archive fontArchive = new Archive(fontFn);
            Dictionary<string, Color[]> palettes = new Dictionary<string, Color[]>();
            for (int i = 2; i <= 12; i++)
            {
                string name = fontArchive.Names[i].Split(' ')[0];
                Color[] colors = fontArchive.readPalette(i);
                palettes.Add(name, colors);
            }
            mPalettes = palettes;

            // now archive by archive we're going to create the FreemooImage objects and add them to the 
            // image collection
            foreach (XmlNode a in xarchives)
            {
                string archiveName = a.Attributes["name"].Value;
                XmlNodeList imgs = a.SelectNodes("img");
                foreach (XmlNode img in imgs)
                {
                    int idx = int.Parse(img.Attributes["idx"].Value);
                    string name = img.Attributes["name"].Value;
                    string pal = img.Attributes["palette"].Value;
                    Archive.PictureInfo pic = archives[archiveName][idx];

                    FreemooImage fi = new FreemooImage();
                    int[,] frame0 = archives[archiveName].decodePicture(idx, 0);
                    fi.addFrame(frame0);
                    fi.ImageIndex = name;
                    fi.Palette = pal;
                    fi.NumFrames = pic.frames;
                    fi.FrameRate = pic.frameRate;

                    fi.HasInternalPalette = pic.useInternalPalette;
                    if (fi.HasInternalPalette)
                    {
                        fi.InternalPalette = new byte[pic.internalPalette.Length];
                        Array.Copy(pic.internalPalette, fi.InternalPalette, pic.internalPalette.Length);
                        fi.InternalColorCount = pic.numInternalColors;
                        fi.PaletteOffset = pic.paletteOffset;
                    }

                    //Surface surf0 = makeSurface(frame0, palettes[pal]);
                    //if (name == "STARBAK2")
                    //{
                    //    surf0.SaveBmp(name + ".bmp");
                    //}
                    //fi.addFrame(surf0);
                    //if (pic.frames > 1)
                    //{
                    //    for (int i = 1; i < pic.frames; i++)
                    //    {
                    //        int[,] framei = archives[archiveName].decodePicture(idx, i);
                    //        Surface surfi = makeSurface(framei, palettes[pal]);
                    //        if (pic.frames2 == 0)
                    //        {
                    //            Surface resultSurf = Video.CreateRgbSurface(surf0.Width, surf0.Height);
                    //            resultSurf.Blit(surf0);
                    //            resultSurf.Blit(surfi);
                    //            fi.addFrame(resultSurf);
                    //        }
                    //        else
                    //        {
                    //            fi.addFrame(surfi);
                    //        }
                    //    }
                    //}
                    string an = archiveName.Split('.')[0];
                    ArchiveEnum ae = (ArchiveEnum)Enum.Parse(typeof(ArchiveEnum), an);
                    fi.Archive = ae;
                    mImageList.Add(fi);
                    //mImageCollection.add(ae, name, fi);
                }
            }
            foreach (string k in archives.Keys)
            {
                archives[k].close();
            }
        }

        private Color[] buildInternalPalette(Color[] pal, FreemooImage img)
        {
            // probably should put an assert in here to make sure this is never called in error
            Color[] newPal = new Color[256];

            for (int i = 0; i < 256; i++)
            {
                if (i >= img.PaletteOffset && i < img.PaletteOffset + img.InternalColorCount)
                {
                    newPal[i] = Color.FromArgb(0xff, img.InternalPalette[(i - img.PaletteOffset) * 3 + 0] * 4,
                                        img.InternalPalette[(i - img.PaletteOffset) * 3 + 1] * 4,
                                        img.InternalPalette[(i - img.PaletteOffset) * 3 + 2] * 4);
                }
                else
                {
                    newPal[i] = pal[i];
                }
            }

            return newPal;
        }

        public Surface getSurface(ArchiveEnum arc, string imageIdx, int frame)
        {
            return getSurface(arc, imageIdx, frame, 0);
        }

        // adding offset parameter for things like the ships that have more than one image with the same name
        public Surface getSurface(ArchiveEnum pArch, string pImageIdx, int pFrame, int offset)
        {
            string idx = pArch.ToString() + "_" + pImageIdx + "_" + pFrame + "_" + offset;
            if (mImageCache.ContainsKey(idx))
            {
                return mImageCache[idx];
            }
            else
            {
                var fiList = mImageList.Where(x => x.Archive == pArch && x.ImageIndex == pImageIdx).ToList();
                FreemooImage fi = null;
                if (fiList.Count > offset)
                {
                    fi = fiList[offset];
                }
                else
                {
                    fi = fiList[0];
                }
                //FreemooImage fi = mImageList.Single(x => x.Archive == pArch && x.ImageIndex == pImageIdx);
                Surface surf = null;
                if (pFrame >= fi.BufferCount)
                {
                    Archive ar = GetArchive(pArch);

                    //int[,] currFrame = ar.decodePicture(GetPictureIndex(pArch, pImageIdx), pFrame);
                    //int[,] frameN = ar.decodePicture(GetPictureIndex(pArch, pImageIdx), pFrame);
                    if (pFrame > 0)
                    {
                        //int[,] frame0 = fi.getBuffer(0);
                        //currFrame = ar.mergeFrames(frame0, frameN);
                        //fi.addFrame(ar.mergeFrames(frame0, frameN));
                        int[,] frameN = ar.decodePicture(GetPictureIndex(pArch, pImageIdx), pFrame, fi.getBuffer(pFrame - 1));
                        fi.addFrame(frameN);
                    }
                    else
                    {
                        int[,] frameN = ar.decodePicture(GetPictureIndex(pArch, pImageIdx), pFrame);
                        fi.addFrame(frameN);
                    }
                    //fi.addFrame(currFrame);
                    // this system will still fail if for some reason we're seeking frame 6 and we just loaded frame 3
                    
                }
                if (fi.HasInternalPalette)
                {
                    surf = makeSurface(fi.getBuffer(pFrame), buildInternalPalette(mPalettes[fi.Palette], fi));
                }
                else
                {
                    surf = makeSurface(fi.getBuffer(pFrame), mPalettes[fi.Palette]);
                }
                
                mImageCache.Add(idx, surf);
                return surf;
            }
        }

        public FreemooImage getImage(ArchiveEnum pArch, string pImageIdx)
        {
            FreemooImage fi = mImageList.Single(x => x.Archive == pArch && x.ImageIndex == pImageIdx);
            return fi;
        }
    }
}
