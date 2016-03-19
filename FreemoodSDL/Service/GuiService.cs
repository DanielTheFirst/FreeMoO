using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

using FreeMoO.Reverse;

using SdlDotNet.Core;
using SdlDotNet.Graphics;

namespace FreeMoO.Service
{
    // all drawing is routed through here.  is this a good idea?  hell if i know.
    public class GuiService
    {
        private FreemooGame mGame = null;

        private Surface mScreen = null;
        private Surface mBuffer = null;

        private int mStretchRatio = 1;

        private Dictionary<FontEnum, Reverse.Font> mFontCollection = new Dictionary<FontEnum, Reverse.Font>();
        private Dictionary<FontPaletteEnum, Color[]> mFontPalettes = new Dictionary<FontPaletteEnum, Color[]>();

        const int ORION_WIDTH = 320; // the original resolution of MoO was 320x200
        const int ORION_HEIGHT = 200;

        public GuiService(FreemooGame pGame)
        {
            mGame = pGame;

            mStretchRatio = Config.StretchRatio;

            initializeFonts();
            initializeFontPalettes();
        }

        private void initializeFonts()
        {
            Archive arc = new Archive(Config.DataFolder + "\\LBX\\FONTS.LBX");
            mFontCollection.Add(FontEnum.font_0, arc.readFont(0));
            mFontCollection.Add(FontEnum.font_1, arc.readFont(1));
            mFontCollection.Add(FontEnum.font_2, arc.readFont(2)); // this font has no space character so lowercase and numbers won't work unless i compensate for it somehow
            mFontCollection.Add(FontEnum.font_3, arc.readFont(3));
            mFontCollection.Add(FontEnum.font_4, arc.readFont(4));
            mFontCollection.Add(FontEnum.font_5, arc.readFont(5));
            arc.close();
        }

        private void initializeFontPalettes()
        {
            // to be used with a font that is a single collor on a transparent background
            Color[] singleColor = new Color[2];
            singleColor[0] = Color.FromArgb(0x00, 0xff, 0x00, 0xff);
            singleColor[1] = Color.FromArgb(0x00, 0x00, 0x00);
            mFontPalettes.Add(FontPaletteEnum.SingleColor, singleColor);

            // more to follow
            // pal = [0xff00ff, 0xDFBA55, 0xD3A624 , 0xDFBA55, 0xD3A624, 0xD3A624, 0xDFBA55, 0xE7CF8A, 0xF3E7C3, 0xF3E7C3]
            Color[] font4Colors = new Color[10];
            font4Colors[0] = Color.FromArgb(0x00, 0xff, 0x00, 0xff);
            font4Colors[1] = Color.FromArgb(0xdf, 0xba, 0x55);
            font4Colors[2] = Color.FromArgb(0xd3, 0xa6, 0x24);
            font4Colors[3] = Color.FromArgb(0xdf, 0xbA, 0x55);
            font4Colors[4] = Color.FromArgb(0xd3, 0xa6, 0x24);
            font4Colors[5] = Color.FromArgb(0xd3, 0xa6, 0x24);
            font4Colors[6] = Color.FromArgb(0xdf, 0xba, 0x55);
            font4Colors[7] = Color.FromArgb(0xe7, 0xcf, 0x8a);
            font4Colors[8] = Color.FromArgb(0xf3, 0xe7, 0xc3);
            font4Colors[9] = Color.FromArgb(0xf3, 0xe7, 0xc3);
            mFontPalettes.Add(FontPaletteEnum.Font4Colors, font4Colors);

            Color[] fontEnabledMainMenuBtn = new Color[10];
            int [] colors = {0xFF00FF,0xCB9600,0xCB9600,0xD3A624,0xDFBA55,0xCB9600,0xD3A624,0xDFBA55,0xDFBA55,0xDFBA55};
            for (int i = 0; i < colors.Length; i++)
            {
                fontEnabledMainMenuBtn[i] = Color.FromArgb(colors[i]);
            }
            mFontPalettes.Add(FontPaletteEnum.MainMenuBtnEnabled, fontEnabledMainMenuBtn);

            Color[] fontDisabledMainMenuBtn = new Color[10];
            int [] colors2 = {0xFF00FF,0x454545,0x555555,0x696969,0x797979,0x696969,0x797979,0x8E8E8E,0x8E8E8E,0x8E8E8E};
            for (int i = 0; i < colors2.Length; i++)
            {
                fontDisabledMainMenuBtn[i] = Color.FromArgb(colors2[i]);
            }
            mFontPalettes.Add(FontPaletteEnum.MainMenuBtnDisabled, fontDisabledMainMenuBtn);

            int[] colors3 = { 0xFF00FF, 0x9A969A, 0xAAAAAA, 0xC7C7C7, 0xD7D7D7, 0xB2B2B2, 0xC7C7C7, 0xD7D7D7, 0xF7F7F7, 0xF7F7F7 };
            Color[] fontActiveMainMenuBtn = new Color[10];
            for (int i = 0; i < colors3.Length; i++)
            {
                fontActiveMainMenuBtn[i] = Color.FromArgb(colors3[i]);
            }
            mFontPalettes.Add(FontPaletteEnum.MainMenuBtnActive, fontActiveMainMenuBtn);

            Color[] fontUnexRange = new Color[256];
            fontUnexRange[0] = Color.FromArgb(0x00, 0xff, 0x00, 0xff);
            fontUnexRange[1] = Color.FromArgb(134, 134, 134);
            fontUnexRange[2] = Color.FromArgb(105, 105, 105);
            fontUnexRange[3] = Color.Blue;  // hunh?
            fontUnexRange[4] = Color.FromArgb(105, 105, 105);
            //fontUnexRange[2] = fontUnexRange[3] = Color.Blue;'
            for (int i = 5; i < 256; i++) fontUnexRange[i] = Color.Blue;
            mFontPalettes.Add(FontPaletteEnum.UnexploredRange, fontUnexRange);

            // really need to externalize this...make it a tasking
            // [0xff00ff, 0xFFDF51, 0xff88ff, 0xff88ff, 0xCB9600]
            Color[] planetTypePal = buildPalette(new int[] {0xff00ff, 0xFFDF51, 0xff88ff, 0xff88ff, 0xCB9600});
            mFontPalettes.Add(FontPaletteEnum.PlanetType, planetTypePal);
            mFontPalettes.Add(FontPaletteEnum.PlanetBluePal, buildPalette(new int[] { 0xff00ff, 0x7196BE, 0x4D79AA, 0x7196BE, 0x4D79AA }));
            mFontPalettes.Add(FontPaletteEnum.PopulationGreen, buildPalette(new int[] { 0xff00ff, 0x49CF24, 0xef00ef, 0xef00ef, 0xff0000 }));

            Color[] loadScreenGray = buildPalette(new int[] { 0xff00ff, 0x9E9EBE, 0xff00fe, 0xff00fe, 0x75759E });
            mFontPalettes.Add(FontPaletteEnum.LoadScreenGray, loadScreenGray);

            Color[] loadScreenGreen = buildPalette(new int[] { 0xff00ff, 0x00BE00, 0xff00fe, 0xff00fe, 0x007900 });
            mFontPalettes.Add(FontPaletteEnum.LoadScreenGreen, loadScreenGreen);

            Color[] fleetPanel = buildPalette(new int[] { 0xff00ff, 0xFFDF51, 0xff00fe, 0xff00fe, 0xcb9600 });
            mFontPalettes.Add(FontPaletteEnum.FleetPanelYellow, fleetPanel);
        }

        private Color[] buildPalette(int[] pColors)
        {
            Color[] pal = new Color[pColors.Length];
            for (int i = 0; i < pColors.Length; i++)
            {
                pal[i] = Color.FromArgb(pColors[i]);
            }
            return pal;
        }

        //private ConfigService getConfigs()
        //{
        //    // probably going to check this one frequently
        //    return (ConfigService)mGame.Services[ServiceEnum.ConfigService];
        //}

        public void initializeVideo()
        {
            bool full = false;
            mScreen = Video.SetVideoMode(1280, 800, false, false, full, true);
            //mScreen.TransparentColor = Color.FromArgb(255, 0, 255);
            //mScreen.Transparent = true;
            mBuffer = new Surface(320, 200);
        }

        public void blank()
        {
            //mScreen.Fill(Color.Black);
            mBuffer.Fill(Color.Black);
        }

        static int frames = 0;

        public void flip()
        {
            //mScreen = mScreen.CreateStretchedSurface(new Size(1024, 768));
            frames++;
            Surface tmp = mBuffer.CreateStretchedSurface(new Size(1280, 800));
            mScreen.Blit(tmp, new Point(0,0));
            tmp.Dispose();
            mScreen.Update();
        }

        public void drawImage(Surface pSurf, int x, int y)
        {
            //mScreen.Blit(pSurf, new Point(x, y));
            mBuffer.Blit(pSurf, new Point(x, y));
        }

        private bool checkFont2(FontEnum pFont)
        {
            return pFont.Equals(FontEnum.font_2);
        }

        public void drawString(string pString, int x, int y, FontEnum pFont, Color pColor)
        {
            Color[] pal = mFontPalettes[FontPaletteEnum.SingleColor];
            pal[1] = pColor;
            
            drawString(pString, x, y, pFont, pal);
        }

        public void drawString(string pString, Rectangle pRect, FontEnum pFont, FontPaletteEnum pPal)
        {
            // centers a string within a given rectangle 
            

            drawString(pString, pRect, pFont, pPal, TextAlignEnum.Center, TextVAlignEnum.Center);
        }

        public void drawString(string pString, Rectangle pRect, FontEnum pFont, Color pColor)
        {
            Color[] pal = mFontPalettes[FontPaletteEnum.SingleColor];
            pal[1] = pColor;
            drawString(pString, pRect, pFont, pal, TextAlignEnum.Center, TextVAlignEnum.Center);
        }

        public void drawString(string pString, Rectangle pRect, FontEnum pFont, Color pColor, TextAlignEnum pAlign, TextVAlignEnum pValign)
        {
            Color[] pal = mFontPalettes[FontPaletteEnum.SingleColor];
            pal[1] = pColor;
            drawString(pString, pRect, pFont, pal, pAlign, pValign);
        }

        public void drawString(string pString, Rectangle pRect, FontEnum pFont, FontPaletteEnum pPal, TextAlignEnum pAlign)
        {
            drawString(pString, pRect, pFont, pPal, pAlign, TextVAlignEnum.Center);
        }

        public void drawString(string pString, Rectangle pRect, FontEnum pFont, FontPaletteEnum pPal, TextVAlignEnum pAlign)
        {
            drawString(pString, pRect, pFont, pPal, TextAlignEnum.Center, pAlign);
        }

        private int calcHAlign(Rectangle pRect, int pWidth, TextAlignEnum pAlign)
        {
            int ret = 0;
            switch (pAlign)
            {
                case TextAlignEnum.Left:
                    ret = pRect.X;
                    break;
                case TextAlignEnum.Right:
                    ret = pRect.X + pRect.Width - pWidth;
                    break;
                case TextAlignEnum.Center:
                    ret = pRect.X + (pRect.Width/2) - (pWidth / 2);
                    break;
                case TextAlignEnum.None:
                    ret = pRect.X;
                    break;
            }
            return ret;
        }

        private int calcVAlign(Rectangle pRect, int pHeight, TextVAlignEnum pValign)
        {
            int ret = 0;
            switch (pValign)
            {
                case TextVAlignEnum.Top:
                    ret = pRect.Y;
                    break;
                case TextVAlignEnum.Center:
                    ret = pRect.Y + (pRect.Height/2) - (pHeight/2);
                    break;
                case TextVAlignEnum.Bottom:
                    ret = pRect.Y + pRect.Height - (pHeight);
                    break;
                case TextVAlignEnum.None:
                    ret = pRect.Y;
                    break;
            }
            return ret;
        }

        public void drawString(string pString, Rectangle pRect, FontEnum pFont, FontPaletteEnum pPal, TextAlignEnum pAlign, TextVAlignEnum pValign)
        {
            List<Glyph> renderData = mFontCollection[pFont].getRenderData(pString);
            // need to make width,height calc a static function in the glyph class
            int w = 0;
            int h = 0;
            int letterSpace = 1;
            foreach (Glyph g in renderData)
            {
                w += g.Width + letterSpace;
                h = g.Height > h ? g.Height : h;
            }
            //w /= 2;
            //h /= 2;
            int x = calcHAlign(pRect, w, pAlign);
            //x -= w;
            int y = calcVAlign(pRect, h, pValign); //(pRect.Y + (pRect.Height / 2)) - h;
            drawString(pString, x, y, pFont, pPal);
        }

        public void drawString(string pString, Rectangle pRect, FontEnum pFont, Color[] pPal, TextAlignEnum pAlign, TextVAlignEnum pValign)
        {
            List<Glyph> renderData = mFontCollection[pFont].getRenderData(pString);
            // need to make width,height calc a static function in the glyph class
            int w = 0;
            int h = 0;
            int letterSpace = 1;
            foreach (Glyph g in renderData)
            {
                w += g.Width + letterSpace;
                h = g.Height > h ? g.Height : h;
            }
            //w /= 2;
            //h /= 2;
            int x = calcHAlign(pRect, w, pAlign);
            //x -= w;
            int y = calcVAlign(pRect, h, pValign); //(pRect.Y + (pRect.Height / 2)) - h;
            drawString(pString, x, y, pFont, pPal);
        }

        public void drawString(string pString, int x, int y, FontEnum pFont, FontPaletteEnum pPal)
        {
            Color[] pal = mFontPalettes[pPal];

            drawString(pString, x, y, pFont, pal);
        }

        public void drawString(string pString, int x, int y, FontEnum pFont, Color[] pPalette)
        {
            List<Glyph> renderData = mFontCollection[pFont].getRenderData(pString);
            int w = 0;
            int h = 0;
            int letterSpace = 1;
            foreach (Glyph g in renderData)
            {
                w += g.Width + letterSpace;
                h = g.Height > h ? g.Height : h;
            }
            Surface fontSurf = new Surface(w, h, 32);
            fontSurf.Fill(Color.FromArgb(0x00, 0xff, 0x00, 0xff));
            byte[] rawSurface = new byte[w * h * 4];
            for (int i = 0; i < rawSurface.Length; i+= 4)
            {
                rawSurface[i] = 0xFF;
                rawSurface[i + 1] = 0x00;
                rawSurface[i + 2] = 0xff;
                rawSurface[i + 3] = 0x00;
            }
            int xx = 0;
            for (int c = 0; c < renderData.Count; c++)
            {
                if (c > 0)
                {
                    if (renderData[c - 1] != null)
                    {
                        xx += renderData[c - 1].Width + 1;
                    }
                }
                //int currAddy = 0;
                for (int j = 0; j < renderData[c].Height; j++)
                {
                    for (int i = 0; i < renderData[c].Width; i++)
                    {
                        int idx = ((i + xx) * 4) + (j * w * 4);
                        
                        Color col = pPalette[renderData[c][i,j]];
                        rawSurface[idx] = col.B;
                        rawSurface[idx+1] = col.G;
                        rawSurface[idx+2] = col.R;
                        rawSurface[idx+3] = col.A;
                    }
                }
            }
            fontSurf.Lock();
            Marshal.Copy(rawSurface, 0, fontSurf.Pixels, rawSurface.Length);
            fontSurf.Unlock();
            fontSurf.Transparent = true;
            fontSurf.TransparentColor = Color.Magenta;
            mBuffer.Blit(fontSurf, new Point(x,y));
        }

        public void drawRect(int x, int y, int w, int h, Color pColor)
        {
            mBuffer.Fill(new Rectangle(x, y, w, h), pColor);

        }

        public void takescreenshot()
        {
            //mBuffer.CreateStretchedSurface(new Size(1280, 800)).Bitmap.Save("", System.Drawing.Imaging.ImageFormat.Png);
            Surface s = mBuffer.CreateStretchedSurface(new Size(1280, 800));
            string fn = Config.DataFolder + "\\screenshots\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "." + DateTime.Now.Minute + "." + DateTime.Now.Second + ".png";
            s.Bitmap.Save(fn, System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}
