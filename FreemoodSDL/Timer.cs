using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeMoO
{
    public class Timer
    {
        private DateTime _currentTime;
        private TimeSpan _sinceLastFrame;
        private DateTime _startTime;
        private TimeSpan _sinceBeginning;

        public Timer()
        {
            _startTime = DateTime.Now;
            _sinceBeginning = DateTime.Now.Subtract(_startTime);
            _currentTime = _startTime;
            _sinceLastFrame = _sinceBeginning;
        }

        public void Update()
        {
            DateTime tmpNOw = DateTime.Now;
            _sinceLastFrame = tmpNOw.Subtract(_currentTime);
            _sinceBeginning = tmpNOw.Subtract(_startTime);
            _currentTime = tmpNOw;
        }

        public double MillisecondsElapsed
        {
            get
            {
                return _sinceLastFrame.TotalMilliseconds;
            }
        }

        public double TotalMilliseconds
        {
            get
            {
                return _sinceBeginning.TotalMilliseconds;
            }
        }

        public double SecondsElapsed
        {
            get
            {
                return _sinceLastFrame.TotalSeconds;
            }
        }

        public double TotalSeconds
        {
            get
            {
                return _sinceBeginning.TotalSeconds;
            }
        }
    }
}
