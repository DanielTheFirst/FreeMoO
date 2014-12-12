using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreemooSDL.Game
{
    public class Starship
        : GameObject
    {
        private int mPlayerId;
        private int mCost;
        private int mAvailableSpace;
        private int mImageIdx;
        private int mSize;

        public int PlayerID
        {
            get
            {
                return mPlayerId;
            }
            set
            {
                mPlayerId = value;
            }
        }

        public int Cost
        {
            get
            {
                return mCost;
            }
            set
            {
                mCost = value;
            }
        }

        public int ImageIdx
        {
            get
            {
                return mImageIdx;
            }
            set
            {
                mImageIdx = value;
            }
        }

        public Starship(int pId)
            : base(pId)
        {
        }
    }
}
