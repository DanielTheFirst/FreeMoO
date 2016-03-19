using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeMoO
{
    // technicall i could just call it timer since it's already in the FreemooSDL namespace
    public class Timer
    {
        private DateTime mCurrentTime;
        private TimeSpan mSinceLastFrame;
        private DateTime mStartTime;
        private TimeSpan mSinceBeginning;

        public Timer()
        {
            mStartTime = DateTime.Now;
            mSinceBeginning = DateTime.Now.Subtract(mStartTime);
            mCurrentTime = mStartTime;
            mSinceLastFrame = mSinceBeginning;
        }

        public void Update()
        {
            DateTime tmpNOw = DateTime.Now;
            mSinceLastFrame = tmpNOw.Subtract(mCurrentTime);
            mSinceBeginning = tmpNOw.Subtract(mStartTime);
            mCurrentTime = tmpNOw;
        }

        public double MillisecondsElapsed
        {
            get
            {
                return mSinceLastFrame.TotalMilliseconds;
            }
        }

        public double TotalMilliseconds
        {
            get
            {
                return mSinceBeginning.TotalMilliseconds;
            }
        }

        public double SecondsElapsed
        {
            get
            {
                return mSinceLastFrame.TotalSeconds;
            }
        }

        public double TotalSeconds
        {
            get
            {
                return mSinceBeginning.TotalSeconds;
            }
        }
    }
}
