using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using MoreLinq;

namespace FreemooSDL.Game
{
    public class KnownTech
    {
        public TechTypeEnum TechType { get; set; }
        public int TechLevel { get; set; }
    }

    public class PlayerTech
    {
        //private Dictionary<TechTypeEnum, List<int>> mKnownTechs = new Dictionary<TechTypeEnum, List<int>>();

        private List<KnownTech> _knownTechs = new List<KnownTech>();

        private Game mGame = null;

        public PlayerTech(Game pGame)
        {
            mGame = pGame;
        }
        
        private void assertUnknownTech(TechTypeEnum techType, int techLevel)
        {
            var tech = _knownTechs.Where(o => o.TechLevel == techLevel && o.TechType == techType).ToList();
            Debug.Assert(tech.Count == 0, "This technology is already known.");
        }

        public void learnTech(TechTypeEnum pTechType, int pTechLevel)
        {
            
            assertUnknownTech(pTechType, pTechLevel);
            KnownTech kt = new KnownTech();
            kt.TechType = pTechType;
            kt.TechLevel = pTechLevel;
            _knownTechs.Add(kt);
        }

        public int GetKnownTechLevel(TechTypeEnum tech)
        {
            return _knownTechs.Where(pt => pt.TechType == tech).MaxBy(p => p.TechLevel).TechLevel;
        }

        public int GetRoboticTechLevel()
        {
            int techLevel = 2; // default is 2
            List<int> roboticTechLevels = mGame.Tech.GetRoboticTechs();
            for (int i = roboticTechLevels.Count - 1; i >= 0; i--)
            {
                var test = _knownTechs.Where(o => o.TechLevel == roboticTechLevels[i]);
                if (test != null)
                {
                    techLevel = i + 3;
                    break;
                }
            }
            return techLevel;
            //var test = _knownTechs.Where(o => roboticTechLevels.Contains(o.TechLevel)).MaxBy(t => t.TechLevel);

        }
    }
}
