using System;
using System.Collections.Generic;

namespace FreeMoO.Reverse
{
    public class Glyph
    {
        private int mWidth;
        private int mHeight;
        private byte[,] mBitmask;

        public int Width
        {
            get
            {
                return mWidth;
            }
        }

        public int Height
        {
            get
            {
                return mHeight;
            }
        }

        public byte this[int i, int  j]
        {
            get
            {
                return mBitmask[i, j];
            }
        }

        public Glyph(byte[] rawData)
        {
            calcWidth(rawData);
            calcHeight(rawData);
            mBitmask = new byte[mWidth, mHeight];
            for (int i = 0; i < mBitmask.GetLength(0); i++)
            {
                for (int j = 0; j < mBitmask.GetLength(1); j++)
                {
                    mBitmask[i, j] = 0;
                }
            }
            int curr_col = 0;
            int curr_row = 0;
            for (int idx = 0; idx < rawData.Length; idx++)
            {
                byte b = rawData[idx];
                if (b == 0x80)
                {
                    curr_row = 0;
                    curr_col++;
                }
                else if ((b & 0x80) == 0x80)
                {
                    curr_row += (b & 0x0f);
                }
                else
                {
                    int run = (b & 0xf0) >> 4;
                    for (int j = 0; j < run; j++)
                    {
                        mBitmask[curr_col, curr_row] = (byte)((b & 0x0f) + 1);
                        curr_row++;
                    }
                }
            }
        }

        private void calcWidth(byte[] rawData)
        {
            mWidth = 0;
            foreach (byte r in rawData)
            {
                if (r == 0x80)
                {
                    mWidth++;
                }
            }
        }

        private void calcHeight(byte[] rawData)
        {
            mHeight = 0;
            int curr_col = 0;
            int curr_row = 0;
            for (int idx = 0; idx < rawData.Length; idx++)
            {
                byte b = rawData[idx];
                if (b == 0x80)
                {
                    curr_row += 1;
                    if (mHeight < curr_row)
                    {
                        mHeight = curr_row;
                    }
                    curr_row = 0;
                    curr_col += 1;
                }
                else if ((b & 0x80) == 0x80)
                {
                    curr_row += (b & 0x0f);
                }
                else
                {
                    int run = (b & 0xf0) >> 4;
                    curr_row += run;
                }
            }
        }
    }

    public class Font
    {
        //      # the moo programmers really loved vertical...maybe it was a thing back then
        //# but all the images and the fonts too were encoded for vertical rendering
        //# but I finally cracked the code
        //# basically 0x80 indicates got to the next column
        //# if the high bit is set, ex. 0x82 then the next x pixels are transparent,
        //# where x is the value of the low nibble (0x82 & 0x0f)
        //# for everything else the top 4 bits indicate a run length
        //# and the bottom 4 bbits indicate a color (still trying to figure out
        //# where the palette values come from)...maybe that big glob of data before
        //# the offset table...

        private List<Glyph> mGlyphs = new List<Glyph>();

        public Font(byte[] pFontData, int pFontId)
        {
            List<int> offsets = getOffsets(pFontData, pFontId);
            for (int idx = 32; idx < 127; idx++)
            {
                int offset = offsets[idx - 32];
                int offset_end = offsets[idx - 31];
                if (offset_end != null)
                {
                    int lim = offset_end - offset;
                    if (lim > 0)
                    {
                        //byte[] glyph_data = pFontData.Take(\
                        byte[] glyph_data = new byte[lim];
                        Array.Copy(pFontData, offset, glyph_data, 0, lim);
                        mGlyphs.Add(new Glyph(glyph_data));
                    }
                }
            }
        }

        private List<int> getOffsets(byte[] pFontData, int pFontId)
        {
            List<int> offsets = new List<int>();

            int offset_start = 0x49a;
            int offset_len = 96;
            int curr_addy = offset_start + (offset_len * pFontId * 2);
            for (int i = 0; i < offset_len; i++)
            {
                int val = pFontData[curr_addy] + (pFontData[curr_addy + 1] << 8);
                curr_addy += 2;
                offsets.Add(val);
            }

            return offsets;
        }

        public List<Glyph> getRenderData(string pRenderText)
        {
            List<Glyph> renderList = new List<Glyph>();
            for (int i = 0; i < pRenderText.Length; i++)
            {
                char c = pRenderText[i];
                int idx = (int)c - 32;
                renderList.Add(mGlyphs[idx]);
            }
            return renderList;
        }
    }
}
