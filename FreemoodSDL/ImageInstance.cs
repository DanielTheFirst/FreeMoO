using System;
using System.Diagnostics;

using FreeMoO.Service;

using SdlDotNet.Graphics;

namespace FreeMoO
{
    public class ImageInstance
    {
        private Image mImageRef = null;
        private ImageService mImgServiceRef = null;
        private int mCurrentFrame = 0;
        private ArchiveEnum mArchiveEnum;
        private string mImageIndex = string.Empty;
        public Boolean Animate { get; set; }
        public Boolean AnimateLoop { get; set; }
        public Double AnimationTimer { get; set; }
        public int Offset { get; set; }
        public long FrameRate
        {
            get
            {
                return mImageRef.FrameRate;
            }
        }

        public ImageInstance(ArchiveEnum pArchive, string pImageIndex, ImageService pImgService)
        {
            //mImageRef = pImgService.Images[pArchive, pImageIndex];
            mImgServiceRef = pImgService;
            mArchiveEnum = pArchive;
            mImageIndex = pImageIndex;
            mImageRef = mImgServiceRef.getImage(pArchive, pImageIndex);
            mCurrentFrame = 0;
            Offset = 0;
            if (mImageRef.FrameRate == 0) mImageRef.FrameRate = 100; // just do animations at 10 frames per second if none is specified.
        }

        public ImageInstance(ArchiveEnum pArchive, string pImageIndex, int offset, ImageService pImgService)
        {
            mImgServiceRef = pImgService;
            mArchiveEnum = pArchive;
            mImageIndex = pImageIndex;
            mImageRef = mImgServiceRef.getImageWithOffset(pArchive, pImageIndex,offset);
            mCurrentFrame = 0;
            Offset = offset;
            if (mImageRef.FrameRate == 0) mImageRef.FrameRate = 100; // just do animations at 10 frames per second if none is specified.
        }

        public void ChangeImageReference(ArchiveEnum archive, string imgIndex)
        {
            mArchiveEnum = archive;
            mImageIndex = imgIndex;
            mImageRef = mImgServiceRef.getImage(mArchiveEnum, mImageIndex);
            ResetAnimation();
            if (mImageRef.FrameRate == 0) mImageRef.FrameRate = 100; // just do animations at 10 frames per second if none is specified.
        }

        public void ChangeImageReference(ArchiveEnum archive, string imgIndex, int offset)
        {
            mArchiveEnum = archive;
            mImageIndex = imgIndex;
            Offset = offset;
            mImageRef = mImgServiceRef.getImageWithOffset(mArchiveEnum, mImageIndex, Offset);
            ResetAnimation();
            if (mImageRef.FrameRate == 0) mImageRef.FrameRate = 100; // just do animations at 10 frames per second if none is specified
            
        }

        public Surface getCurrentFrame()
        {
            //return mImageRef[mCurrentFrame];
            return mImgServiceRef.getSurface(mArchiveEnum, mImageIndex, mCurrentFrame, Offset);
        }

        public Surface getCurrentFrame(int offset)
        {
            return mImgServiceRef.getSurface(mArchiveEnum, mImageIndex, mCurrentFrame, offset);
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

        public void UpdateAnimation(Timer timer)
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
            set
            {
                mCurrentFrame = value;
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
