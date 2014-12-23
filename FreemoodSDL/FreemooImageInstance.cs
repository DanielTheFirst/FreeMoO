using System;
using System.Diagnostics;

using FreemooSDL.Service;

using SdlDotNet.Graphics;

namespace FreemooSDL
{
    public class FreemooImageInstance
    {
        private FreemooImage mImageRef = null;
        private ImageService mImgServiceRef = null;
        private int mCurrentFrame = 0;
        private ArchiveEnum mArchiveEnum;
        private string mImageIndex = string.Empty;
        public Boolean Animate { get; set; }
        public Boolean AnimateLoop { get; set; }
        public Double AnimationTimer { get; set; }
        public long FrameRate
        {
            get
            {
                return mImageRef.FrameRate;
            }
        }

        public FreemooImageInstance(ArchiveEnum pArchive, string pImageIndex, ImageService pImgService)
        {
            //mImageRef = pImgService.Images[pArchive, pImageIndex];
            mImgServiceRef = pImgService;
            mArchiveEnum = pArchive;
            mImageIndex = pImageIndex;
            mImageRef = mImgServiceRef.getImage(pArchive, pImageIndex);
            mCurrentFrame = 0;
            if (mImageRef.FrameRate == 0) mImageRef.FrameRate = 100; // just do animations at 10 frames per second if none is specified.
        }

        public Surface getCurrentFrame()
        {
            //return mImageRef[mCurrentFrame];
            return mImgServiceRef.getSurface(mArchiveEnum, mImageIndex, mCurrentFrame);
        }

        public Surface getNextFrame()
        {
            gotoNextFrame();
            return getCurrentFrame();
        }

        public void gotoNextFrame()
        {
            mCurrentFrame++;
            //FreemooImage img = mImgServiceRef.getImage(mArchiveEnum, mImageIndex);
            Debug.Assert(mImageRef.FrameCount > 1, "Code smell, you shouldn't be calling the next frame on a static image.");
            if (mCurrentFrame >= mImageRef.FrameCount)
            {
                mCurrentFrame = 0;
            }
        }

        public void UpdateAnimation(FreemooTimer timer)
        {
            if (Animate)
            {
                AnimationTimer += timer.MillisecondsElapsed;
                //mImageRef.FrameRate = 48;
                if (AnimationTimer > mImageRef.FrameRate)
                {
                    mCurrentFrame++;
                    AnimationTimer = 0;
                    if (mCurrentFrame >= mImageRef.FrameCount)
                    {
                        if (AnimateLoop)
                        {
                            mCurrentFrame = 0;
                        }
                        else
                        {
                            mCurrentFrame = mImageRef.FrameCount - 1;
                        }
                    }
                }
            }
        }

        public void ResetAnimation()
        {
            mCurrentFrame = 0;
            AnimationTimer = 0;
        }

        public int CurrentFrameNum
        {
            get
            {
                return mCurrentFrame;
            }
        }

        public int FrameCount
        {
            get
            {
                return mImageRef.FrameCount;
            }
        }
    }
}
