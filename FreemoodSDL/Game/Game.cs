using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using MoreLinq;

using FreeMoO.Reverse;

namespace FreeMoO.Game
{
    //public struct Galaxy
    //{
    //    public int NumStars;
    //    public int GalaxySize;
    //    public int Difficulty;
    //    public int Turn;
    //    public int PlanetFocus;
    //}

    public class Game
    {
        private List<Player> _players = null;
        private List<Planet> _planets = null;
        private List<Fleet> _fleets = null;
        private List<Starship> _starshipDesigns = null;
        private List<PlayerTech> _playerTech = null;
        private List<Transport> _transports = null;
        private Galaxy _galaxy;
        private TechTree _techTree = null;
        private List<Nebula> _nebulaList = null;
        private List<string> _saveGameNames = new List<string>();

        private const int CONFIG_NAME_OFFSET = 0x22;
        private const int CONFIG_NAME_LENGTH = 0X14;

        #region Properties
        public List<Planet> Planets
        {
            get
            {
                return _planets;
            }
        }
        public List<Fleet> Fleets
        {
            get
            {
                return _fleets;
            }
        }
        public Galaxy GalaxyData
        {
            get
            {
                return _galaxy;
            }
        }
        public List<Player> Players
        {
            get
            {
                return _players;
            }
        }
        public TechTree Tech
        {
            get
            {
                return _techTree;
            }
        }
        public List<Starship> Starships
        {
            get
            {
                return _starshipDesigns;
            }
        }
        #endregion
        public Game()
        {
            _techTree = new TechTree();
            LoadSaveGameNames();
            //Technology test = mTechTree.getByName("testing...");
        }

        public void newGame()
        {
        }

        public void loadGame(int pIdx)
        {
            Savegame saveGame = new Savegame(pIdx, this);
            _players = saveGame.parsePlayers();
            _galaxy = saveGame.parseGalaxy();
            _planets = saveGame.parsePlanets(_galaxy.NumStars);
            _fleets = saveGame.parseFleetsIntransit(_players.Count);
            // and then concatenate with fleets in orbit
            List<Fleet> orbits = saveGame.parseFleetsInOrbit(_planets.Count, _players.Count);
            _fleets.AddRange(orbits);
            _starshipDesigns = saveGame.parseStarshipsDesigns(_players.Count);
            _playerTech = saveGame.parsePlayerTechnologies(_players, this);
            for (int i = 0; i < _players.Count; i++)
            {
                _players[i].SetTechList(_playerTech[i]);
            }
            _transports = saveGame.parseTransports();
        }

        public void saveGame(int pIdx)
        {

        }

        private void LoadSaveGameNames()
        {
            _saveGameNames.Clear();
            // config.moo is a bit of a mystery right now.  It's been about 20 years since I've seen an installation of 
            // master of orion without all six save games so...I'm going to for now assume that all six are there
            using (BinaryReader br = new BinaryReader(new FileStream(Config.DataFolder.PathCmb("CONFIG.MOO"), FileMode.Open)))
            {
                byte[] buffer = new byte[br.BaseStream.Length];
                br.Read(buffer, 0, buffer.Length);
                br.Close();
                for (int i = 0; i < 6; i++)
                {
                    _saveGameNames.Add(Util.slice(buffer, CONFIG_NAME_OFFSET + (i * CONFIG_NAME_LENGTH), CONFIG_NAME_LENGTH).GetZString());
                }
            }
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
            _transports.Add(t);
            return t;
        }

        public void LaunchTransports()
        {
            // transports are just pending until "next turn"
            // need to figure out where this is specified in the save file
            // andlogic here is loop through planets and see if any colonys have
            // pending transports and instantiate a transport object and deduct costs and population
            List<Transport> unlaunchedTransports = _transports.Where(o => o.Launched == false).ToList();
            foreach (var trans in unlaunchedTransports)
            {
                var colony = _planets.FirstOrDefault(p => p.ID == trans.OriginPlanetId);
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
            foreach (var p in _planets)
            {
                p.ProductionPoints = 0;
                if (p.IsColonized)
                {
                    // first, calc what is generated by the peoples
                    int playerId = p.PlayerId;
                    double perWorker = (double)_players[playerId].GetKnownTechLevel(TechTypeEnum.Computer) * 3 + 50;
                    perWorker /= 100;
                    p.ProductionPoints = (int)(perWorker * p.CurrentPopulation);

                    // then the factories
                    // need robotic control level, num workers, num factories
                    int roboticTechLevel = _playerTech[playerId].GetRoboticTechLevel();
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
            var shipsToMove = _fleets.Where(s => s.InTransit == true);
            foreach (var ship in shipsToMove)
            {
                Point newLocation = _calculateNewLocation(ship.X, ship.Y, ship.PlanetId, ship.PlayerId, true);

            }

            var transportsToMove = _transports.Where(t => t.Launched == true);
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
            return _transports.MaxBy(o => o.ID).ID + 1;
        }
        public void UpdatePlanetFocus(int id)
        {
            _galaxy.PlanetFocus = id;
        }
        public int CalcPlayer0Range(int x, int y)
        {
            // in the shiny and new save game, Klystron is 2 parsecs while Dunatis is 4
            var planets = _planets.Where(p => p.PlayerId == 0 && p.IsColonized == true);
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
