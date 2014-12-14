using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using MoreLinq;

using FreemooSDL.Reverse;

namespace FreemooSDL.Game
{
    public struct Galaxy
    {
        public int NumStars;
        public int GalaxySize;
        public int Difficulty;
        public int Turn;
        public int PlanetFocus;
    }

    public class Game
    {
        private List<Player> mPlayers = null;
        private List<Planet> mPlanets = null;
        private List<Fleet> mFleets = null;
        private List<Starship> mStarshipDesigns = null;
        private List<PlayerTech> mPlayerTech = null;
        private List<Transport> mTransports = null;
        private Galaxy mGalaxy;
        private TechTree mTechTree = null;
        private List<Nebula> _nebulaList = null;

        #region Properties
        public List<Planet> Planets
        {
            get
            {
                return mPlanets;
            }
        }
        public List<Fleet> Fleets
        {
            get
            {
                return mFleets;
            }
        }
        public Galaxy GalaxyData
        {
            get
            {
                return mGalaxy;
            }
        }
        public List<Player> Players
        {
            get
            {
                return mPlayers;
            }
        }
        public TechTree Tech
        {
            get
            {
                return mTechTree;
            }
        }
        public List<Starship> Starships
        {
            get
            {
                return mStarshipDesigns;
            }
        }
        #endregion
        public Game()
        {
            mTechTree = new TechTree();
            //Technology test = mTechTree.getByName("testing...");
        }

        public void newGame()
        {
        }

        public void loadGame(int pIdx)
        {
            Savegame saveGame = new Savegame(pIdx, this);
            mPlayers = saveGame.parsePlayers();
            mGalaxy = saveGame.parseGalaxy();
            mPlanets = saveGame.parsePlanets(mGalaxy.NumStars);
            mFleets = saveGame.parseFleetsIntransit(mPlayers.Count);
            // and then concatenate with fleets in orbit
            List<Fleet> orbits = saveGame.parseFleetsInOrbit(mPlanets.Count, mPlayers.Count);
            mFleets.AddRange(orbits);
            mStarshipDesigns = saveGame.parseStarshipsDesigns(mPlayers.Count);
            mPlayerTech = saveGame.parsePlayerTechnologies(mPlayers, this);
            for (int i = 0; i < mPlayers.Count; i++)
            {
                mPlayers[i].SetTechList(mPlayerTech[i]);
            }
            mTransports = saveGame.parseTransports();
        }

        public void saveGame(int pIdx)
        {

        }


        #region Next Turn Code

        public void AIUpdate()
        {
            // do the "next turn" AI determinations
        }

        public Transport QueueNewTransport(int originPlanet, int destPlanet, int size, int playerId)
        {
            Transport t = new Transport(_getNextTransportId());
            t.Launched = false;
            t.OriginPlanetId = originPlanet;
            t.DestPlanetId = destPlanet;
            t.SizeInMillions = size;
            t.PlayerId = playerId;
            mTransports.Add(t);
            return t;
        }

        public void LaunchTransports()
        {
            // transports are just pending until "next turn"
            // need to figure out where this is specified in the save file
            // andlogic here is loop through planets and see if any colonys have
            // pending transports and instantiate a transport object and deduct costs and population
            var unlaunchedTransports = mTransports.Where(o => o.Launched == false).ToList();
            foreach (var trans in unlaunchedTransports)
            {
                var colony = mPlanets.FirstOrDefault(p => p.ID == trans.OriginPlanetId);
                Debug.Assert(colony != null, "Should never have a transporting unlaunched and invalid origin planet.");
                Debug.Assert(colony.CurrentPopulation > trans.SizeInMillions, "Shouldn't even be able to launch transport with more people than are currently on the planet.");
                colony.CurrentPopulation -= trans.SizeInMillions;
                trans.relocate(colony.X, colony.Y);
                trans.Launched = true;
            }
        }

        

        public void LandTransports()
        {
            // 1. friendly colony, added to existing colony, fits, anyone whodoesn't fit dies
            // 2. enemy colony, ground combat.  first roll to see how many transports actually land
            // 3. no colony, everybody dies,much sadness
        }

        public void CalculateProduction()
        {
            // loop through planets and calculatehow many production points were accrued during
            // the turn for colonies.
            foreach (var p in mPlanets)
            {
                p.ProductionPoints = 0;
                if (p.IsColonized)
                {
                    // first, calc what is generated by the peoples
                    int playerId = p.PlayerId;
                    double perWorker = (double)mPlayers[playerId].GetKnownTechLevel(TechTypeEnum.Computer) * 3 + 50;
                    perWorker /= 100;
                    p.ProductionPoints = (int)(perWorker * p.CurrentPopulation);

                    // then the factories
                    // need robotic control level, num workers, num factories
                    int roboticTechLevel = mPlayerTech[playerId].GetRoboticTechLevel();
                    int numSupportableFactories = p.CurrentPopulation * roboticTechLevel;
                    if (numSupportableFactories <= p.AmtFactories)
                    {
                        p.ProductionPoints += numSupportableFactories;
                    }
                    else
                    {
                        p.ProductionPoints += p.AmtFactories;
                    }
                }
            }
        }

        public void PopulationGrowth()
        {
            // loop through planets and calculate the growth in population based
            // on natural growth rate, eco points modifier, racial modifier, and 
            // cloning technology.
        }

        public void CalculateTradeGrowth()
        {
            // i know that as a trade agreement ages it gets more lucrative but
            // not sure if income is calculated at this time
        }


        public void CalculateNewSpies()
        {
            // enough in the spy reserve that any players get a new spy?
        }

        public void CalculateResearchPoints()
        {
        }

        public void MoveShips()
        {
            // first we do fleets, then we do transports
            var shipsToMove = mFleets.Where(s => s.InTransit == true);
            foreach (var ship in shipsToMove)
            {
                Point newLocation = _calculateNewLocation(ship.X, ship.Y, ship.PlanetId, ship.PlayerId, true);

            }

            var transportsToMove = mTransports.Where(t => t.Launched == true);
            foreach (var trans in transportsToMove)
            {
 
            }
        }

        private Point _calculateNewLocation(int currX, int currY, int destPlanetId, int playerId, bool isFleet)
        {

            Point ret = new Point();

            // calculate a move for the vessel given the tech levels of the player who owns the ship
            // determine if the ship falls within an accepted range of the destination...or maybe just greater than or equal
            // and if so, set ret to distination


            return ret;
        }

        #endregion 
 
        #region Util Functions
        private int _getNextTransportId()
        {
            return mTransports.MaxBy(o => o.ID).ID + 1;
        }
        public void UpdatePlanetFocus(int id)
        {
            mGalaxy.PlanetFocus = id;
        }
        public int CalcPlayer0Range(int x, int y)
        {
            // in the shiny and new save game, Klystron is 2 parsecs while Dunatis is 4
            var planets = mPlanets.Where(p => p.PlayerId == 0 && p.IsColonized == true);
            double x1, x2, y1, y2;
            x1 = ((double)x) ;
            y1 = ((double)y) ;
            int maxRange = int.MaxValue;
            foreach(var p in planets)
            {
                x2 = ((double)p.X) ;
                y2 = ((double)p.Y) ;
                double range = Math.Sqrt(((x1 - x2) * (x1 - x2)) + ((y1 - y2) * (y1 - y2)));
                //Console.WriteLine("Range calculated at " + range);
                int rangeI = (int)Math.Ceiling(range / 20D);
                if (rangeI < maxRange) maxRange = rangeI;
            }
            //Console.WriteLine("Maxrange = " + maxRange);
            return maxRange;
        }
        #endregion
    }
}
