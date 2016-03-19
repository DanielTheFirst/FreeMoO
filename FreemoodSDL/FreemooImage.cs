using System;
using System.Collections.Generic;
using System.Diagnostics;

using SdlDotNet.Core;
using SdlDotNet.Graphics;

namespace FreeMoO   
{
    public class FreemooImage
    {
        private List<Surface> mSurfaces = new List<Surface>();
        private int mCurrFrame = 0;

        public ArchiveEnum Archive;
        public string ImageIndex;
        public int NumFrames;
        public int FrameRate; // in milliseconds? not in milliseconds...too fast...maybe ticks assuming 60 frames per second?
        private List<int[,]> mBuffers = new List<int[,]>();
        public string Palette;
        public bool HasInternalPalette = false;
        public byte[] InternalPalette = null;
        public int PaletteOffset = 0;
        public int InternalColorCount = 0;


        public FreemooImage()
        {
        }

        public int FrameCount
        {
            get
            {
                return NumFrames;
            }
        }

        public void addFrame(Surface pSurf)
        {
            //mSurfaces.Add(pSurf);
        }

        public void addFrame(int[,] pBuffer)
        {
            mBuffers.Add(pBuffer);
        }

        public int[,] getBuffer(int pFrame)
        {
            return mBuffers[pFrame];
        }

        public int BufferCount
        {
            get
            {
                return mBuffers.Count;
            }
        }

        //public Surface getCurrentFrame()
        //{
        //    return mSurfaces[mCurrFrame];
        //}

        //public Surface getNextFrame()
        //{
        //    Debug.Assert(FrameCount > 1, "We're gonna say it's a code smell if you try to call this on an image with only one frame.");
        //    mCurrFrame++;
        //    return getCurrentFrame();
        //}
        public Surface this[int i]
        {
            get
            {
                //Debug.Assert(i < mSurfaces.Count, "that frame does not exist.");
                //return mSurfaces[i];
                return new Surface(1, 1);
            }
        }

    }
}
