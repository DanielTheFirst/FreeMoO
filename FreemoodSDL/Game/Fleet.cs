using System;
using System.Diagnostics;

namespace FreeMoO.Game
{
    public class Fleet : SpaceObject
    {
        private bool mInTransit = false;
        private int mPlanetId = 0; // if intransit this is dest, otherwise it is location
        private int mPlayerId = 0;
        private int [] mShipCountArray = new int[6];

        public Fleet(int pId)
            :base(pId)
        {
        }

        public bool InTransit
        {
            get
            {
                return mInTransit;
            }
            set
            {
                mInTransit = value;
            }
        }

        public int PlanetId
        {
            get
            {
                return mPlanetId;
            }
            set
            {
                mPlanetId = value;
            }
        }

        public int PlayerId
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

        public int this[int i]
        {
            get
            {
                return mShipCountArray[i];
            }
            set
            {
                mShipCountArray[i] = value;
            }
        }

        public int eta()
        {
            Debug.Assert(mInTransit == true, "why are you getting an eta on a fleet in orbit?");
            int est = 0;

            return est;
        }
    }
}
