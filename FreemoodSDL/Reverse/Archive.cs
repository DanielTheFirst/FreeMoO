using System;
using System.IO;
using System.Drawing;
using System.Text;
using System.Collections.Generic;

// hey look!  it's poorly commented code directly transcribed from ruby.
namespace FreeMoO.Reverse
{
    class Archive
    {
        struct Offset
        {
            public int addy;
            public int size;
        }

        public struct PictureInfo
        {
            public int height;
            public int width;
            public int frames;
            public int flags;
            public int tblOffset;
            public int frameRate;
            public string name;
            public int[] offsets;
            public byte[] rawData;
            public int internalPaletteOffset;
            public int colorOffset;
            public int paletteOffset;
            public int numInternalColors;
            public bool useInternalPalette;
            public byte[] internalPalette;
        }

        private BinaryReader mBinaryReader = null;
        private string mFileName = string.Empty;
        private int mFileSize = 0;
        private int mFileType = 0;
        private int mNumFiles = 0;
        private List<string> mNameTable = new List<string>();
        private List<Offset> mOffsetTable = new List<Offset>();
        private int mVersion = 0; //i don't even remember what this is

        public List<string> Names
        {
            get
            {
                return mNameTable;
            }
        }

        public PictureInfo this[int i]
        {
            get
            {
                return readPicture(i);
            }
        }


        public Archive(string pFileName)
        {
            //mFileName = "./LBX/" + pFileName;
            mFileName = pFileName;
            mBinaryReader = new BinaryReader(new FileStream(mFileName, FileMode.Open, FileAccess.Read));

            loadArchiveInfo();
            loadNameTable();


        }

        public void close()
        {
            mBinaryReader.Close();

            mBinaryReader.Dispose();
            mBinaryReader = null;
        }

        private void loadArchiveInfo()
        {
            mBinaryReader.BaseStream.Seek(0, SeekOrigin.Begin);
            mFileSize = (int)mBinaryReader.BaseStream.Length;
            mNumFiles = readTwoBytes();
            int sig = readFourBytes(); // just gonna throw it out anyway
            mVersion = readTwoBytes();
            int off2 = readFourBytes();
            for (int i = 0; i <= mNumFiles; i++)
            {
                int off1 = off2;
                if (i == mNumFiles)
                {
                    off2 = mFileSize;
                }
                else
                {
                    off2 = readFourBytes();
                }
                Offset nextOffset = new Offset();
                nextOffset.size = off2 - off1;
                nextOffset.addy = off1;
                mOffsetTable.Add(nextOffset);
            }
            // cheap hack, the last one is invalid so dump it
            mOffsetTable.RemoveAt(mOffsetTable.Count - 1);
        }

        private void loadNameTable()
        {
            mBinaryReader.BaseStream.Seek(0x200, SeekOrigin.Begin);
            for (int i = 0; i < mOffsetTable.Count; i++)
            {
                byte[] raw = readBytes(32);
                for (int j = 0; j < raw.Length; j++)
                {
                    if (raw[j] == 0x00)
                    {
                        raw[j] = (byte)' ';
                    }
                }
                //string name = Encoding.
                string name = Encoding.UTF8.GetString(raw).Trim();
                mNameTable.Add(name);
            }
        }

        private byte[] readFile(int pFileIdx)
        {
            byte[] data = new byte[mOffsetTable[pFileIdx].size];
            mBinaryReader.BaseStream.Seek(mOffsetTable[pFileIdx].addy, SeekOrigin.Begin);
            for (int i = 0; i < mOffsetTable[pFileIdx].size; i++)
            {
                data[i] = mBinaryReader.ReadByte();
            }
            return data;
        }

        private PictureInfo readPicture(int pIdx)
        {
            byte[] data = readFile(pIdx);
            PictureInfo p = new PictureInfo();
            if (data != null)
            {
                if (data.Length > 0)
                {
                    p.width = buildInt(data[0], data[1]);
                    p.height = buildInt(data[2], data[3]);
                    p.frames = buildInt(data[6], data[7]);
                    p.flags = buildInt(data[0x10], data[0x11]);
                    p.tblOffset = buildInt(data[0x0e], data[0x0f]);
                    p.frameRate = buildInt(data[8], data[9]); // apparently this is the frame delay, probably in miliseconds
                    p.name = mNameTable[pIdx];
                    p.rawData = data;
                    int numOffsets = p.frames + 1;
                    p.offsets = new int[numOffsets];
                    for (int i = 0; i < numOffsets; i++)
                    {
                        int jj = 0x12 + (i * 4);
                        p.offsets[i] = buildInt(data[jj], data[jj + 1], data[jj + 2], data[jj + 3]);
                    }
                    p.internalPaletteOffset = buildInt(data[14], data[15]);
                    p.useInternalPalette = false;
                    if (p.internalPaletteOffset > 0)
                    {
                        p.useInternalPalette = true;
                        p.colorOffset = buildInt(data[p.internalPaletteOffset], data[p.internalPaletteOffset + 1]);
                        p.paletteOffset = buildInt(data[p.internalPaletteOffset + 2], data[p.internalPaletteOffset + 3]);
                        p.numInternalColors = buildInt(data[p.internalPaletteOffset + 4], data[p.internalPaletteOffset + 5]);
                        p.internalPalette = new byte[p.numInternalColors * 3];
                        Array.Copy(data, p.colorOffset, p.internalPalette, 0, p.internalPalette.Length);
                        //for (int c = 0; c < (p.numInternalColors * 3); c++)
                        //{
                        //    p.internalPalette[c] = data[p.colorOffset + c];
                        //}
                    }
                }
            }
            return p;
        }

        // THIS IS an ugly and likely slow way of doing this.  what I'm going with for now though.
        //
        // ok, supposed to merge frame n with frame n-1, not frame 0.  
        public int[,] mergeFrames(int[,] frame0, int[,] frameN)
        {
            int[,] buffer = new int[frame0.GetLength(0), frame0.GetLength(1)];
            for (int i = 0; i < frame0.GetLength(0); i++)
            {
                for (int j = 0; j < frame0.GetLength(1); j++)
                {
                    //buffer[i, j] = frame0[i, j];
                    if (frameN[i, j] != 0)
                    {
                        buffer[i, j] = frameN[i, j];
                    }
                    else
                    {
                        buffer[i, j] = frame0[i, j];
                    }
                }
            }

            return buffer;
        }

        public Font readFont(int pFontId)
        {
            // file 0 in fonts.lbx contain the actual font data
            return new Font(readFile(0), pFontId);
        }

        public int[,] decodePicture(int pPictureIndex, int pFrameIndex)
        {
            return decodePicture(pPictureIndex, pFrameIndex, null);
        }

        public int[,] decodePicture(int pPictureIndex, int pFrameIndex, int [,] prevFrame)
        {
            PictureInfo pic = readPicture(pPictureIndex);
            int[,] buffer = null;
            if (pic.width > 0)
            {
                buffer = new int[pic.width, pic.height];
                for (int i = 0; i < buffer.GetLength(0); i++)
                {
                    for (int j = 0; j < buffer.GetLength(1); j++)
                    {
                        buffer[i, j] = 0;
                    }
                }
                if (prevFrame != null)
                {
                    Buffer.BlockCopy(prevFrame, 0, buffer, 0, prevFrame.Length * sizeof(int));
                }
                byte[] data = pic.rawData;

                int idx = pic.offsets[pFrameIndex] + 1;
                int lim_idx = pic.offsets[pFrameIndex + 1];
                int x_offset = 0;

                while (idx < lim_idx)
                {
                    byte b = data[idx];
                    if (b == 0xff)
                    {
                        idx += 1;
                    }
                    else
                    {
                        int col_data_size = data[idx + 1];
                        int col_data_size2 = col_data_size - 2;
                        int initial_run = data[idx + 2];
                        int y_offset = data[idx + 3];
                        bool has_run = false;
                        if (b == 0x80)
                        {
                            has_run = true;
                        }
                        List<byte> raw_img_data = new List<byte>();
                        for (int i = 0; i < col_data_size2; i++)
                        {
                            raw_img_data.Add(data[idx + i + 4]);
                        }
                        int img_idx = 0;
                        int byte_counter = 0;
                        while (img_idx < raw_img_data.Count)
                        {
                            byte bb = raw_img_data[img_idx];
                            if ((bb & 0xe0) == 0xe0)
                            {
                                if (has_run)
                                {
                                    byte nb = raw_img_data[img_idx + 1];
                                    int run_len = (bb & 0x1f) + 1;
                                    for (int k = 0; k < run_len; k++)
                                    {
                                        buffer[x_offset, y_offset] = nb;
                                        y_offset++;
                                    }
                                    img_idx += 2;
                                    byte_counter += 2;
                                }
                                else
                                {
                                    buffer[x_offset, y_offset] = bb;
                                    y_offset++;
                                    byte_counter++;
                                    img_idx += 1;
                                }
                            }
                            else
                            {
                                buffer[x_offset, y_offset] = bb;
                                y_offset++;
                                byte_counter++;
                                img_idx += 1;
                            }
                            if ((byte_counter >= initial_run) && (img_idx < (raw_img_data.Count - 1)))
                            {
                                initial_run += raw_img_data[img_idx];
                                y_offset += raw_img_data[img_idx + 1];
                                img_idx += 2;
                            }
                        }
                        idx += 2 + col_data_size;
                    }
                    x_offset++;
                }
            }
            return buffer;


        }

        public byte[] readMusic(int pMusicId)
        {
            return readFile(pMusicId);
        }

        public byte[] readSoundFx(int pFxId)
        {
            byte[] fxData = readFile(pFxId);
            FreemooSoundAdapter sfa = new FreemooSoundAdapter();
            return sfa.convert(fxData);
        }

        //public int[] readPalette(int palIdx)
        public Color [] readPalette(int palIdx)
        {
            byte[] data = readFile(palIdx);
            //int[] pal = new int[256];
            Color[] pal = new Color[256];
            if (data != null)
            {
                for (int i = 0; i < 256; i++)
                {
                    int r = data[(i * 3)] * 4;
                    int g = data[(i * 3) + 1] * 4;
                    int b = data[(i * 3) + 2] * 4;
                    //pal[i] = (r << 16) + (g << 8) + b;
                    pal[i] = Color.FromArgb(0xff, r, g, b);
                }
            }
            pal[0] = Color.FromArgb(0,255,0,255);
            return pal;
        }

        private int buildInt(int low, int high)
        {
            return low + (high << 8);
        }

        private int buildInt(int a, int b, int c, int d)
        {
            return buildInt(a, b) + (buildInt(c, d) << 16);
        }

        private int readTwoBytes()
        {
            int low = mBinaryReader.ReadByte();
            int high = mBinaryReader.ReadByte();
            return low + (high << 8);
        }

        private int readFourBytes()
        {
            int low = readTwoBytes();
            int high = readTwoBytes();
            return low + (high << 16);
        }

        private byte[] readBytes(int pNumBytes)
        {
            return mBinaryReader.ReadBytes(pNumBytes);
        }


    }
}
