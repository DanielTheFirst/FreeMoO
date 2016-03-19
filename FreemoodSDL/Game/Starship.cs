using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeMoO.Game
{
    public class Starship
        : GameObject
    {
        private int mPlayerId;
        private int mCost;
        private int mAvailableSpace;
        private int mImageIdx;
        private int mSize;

        public string Name { get; set; }
        public int HitPoints { get; set; }
        public ShipSizeEnum HullSize { get; set; }
        public int Engine { get; set; }
        public int Ecm { get; set; }
        public int Computer { get; set; }
        public int Armor { get; set; }
        public int Maneuver { get; set; }
        public int Special1 { get; set; }
        public int Special2 { get; set; }
        public int Special3 { get; set; }
        public int Weapon1 { get; set; }
        public int Weapon2 { get; set; }
        public int Weapon3 { get; set; }
        public int Weapon4 { get; set; }
        public int Weapon1Count { get; set; }
        public int Weapon2Count { get; set; }
        public int Weapon3Count { get; set; }
        public int Weapon4Count { get; set; }
        public int UnusedSpace { get; set; } // not sure yet the reason this is kept in the save file

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
