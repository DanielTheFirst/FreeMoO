using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeMoO.Game
{
    // represents a single piece of technology
    public class Technology
        : GameObject
    {
        private int mLevel;
        private string mDescription;
        private TechTypeEnum mTechType;

        public int Level
        {
            get
            {
                return mLevel;
            }
            set
            {
                mLevel = value;
            }
        }

        public string Description
        {
            get
            {
                return mDescription;
            }
            set
            {
                mDescription = value;
            }
        }

        public TechTypeEnum TechType
        {
            get
            {
                return mTechType;
            }
            set
            {
                mTechType = value;
            }
        }


        static int[] DifficultModifier = { 20, 25, 30, 35, 40 };


        public Technology(int pId)
            : base(pId)
        {
        }

        public int getBaseCost(int pDifficulty, RacialEnum pRace)
        {
            float cost = (float)DifficultModifier[pDifficulty] * (float)mLevel * (float)mLevel * LookupVals.getTechModifier(pRace, mTechType);
            return (int)cost;
        }
    }
}
