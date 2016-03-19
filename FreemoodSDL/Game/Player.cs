using System;
using System.Collections.Generic;
using System.Linq;

namespace FreeMoO.Game
{
    public class Player : GameObject
    {
        public RacialEnum Race { get; set; }
        private int mColorId;

        private Dictionary<TechTypeEnum, int> mNumKnownTechs = new Dictionary<TechTypeEnum, int>();
        private PlayerTech _techList = null;

        public int ColorId
        {
            get
            {
                return mColorId;
            }
            set
            {
                mColorId = value;
            }
        }

        public int ValidShipDesigns { get; set; }

        public Player(int pId)
            : base(pId)
        {
            foreach(TechTypeEnum t in Enum.GetValues(typeof(TechTypeEnum)))
            {
                mNumKnownTechs.Add(t, 0);
            }
        }
        
        public void setKnowTechCount(TechTypeEnum pTech, int pCount)
        {
            mNumKnownTechs[pTech] = pCount;
        }

        public int getKnownTechCount(TechTypeEnum pTech)
        {
            return mNumKnownTechs[pTech];
        }

        // I might want to make this less ungly in the future
        public void SetTechList(PlayerTech pt)
        {
            _techList = pt;
        }

        public int GetKnownTechLevel(TechTypeEnum tech)
        {
            return _techList.GetKnownTechLevel(tech);
        }
    }
}
