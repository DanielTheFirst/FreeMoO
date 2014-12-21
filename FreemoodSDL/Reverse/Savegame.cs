using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using FreemooSDL.Service;
using FreemooSDL.Game;

namespace FreemooSDL.Reverse
{
    public class Savegame
    {
        // CHUNK OFFSETS
        const int PLANET_OFFSET = 0x0000;
        const int FLEET_DATA_OFFSET = 0x4da0;
        const int TRANSPORT_DATA_OFFSET = 0x6a10;
        const int PLAYER_DATA_OFFSET = 0x7118;
        const int FLEET_INORBIT_OFFSET = 0x74ba;
        const int STARSHIP_DATA_OFFSET = 0xc410;
        const int PLAYER_RESEARCH_OFFSET = 0xc638;

        // RECORD Lengths
        const int PLANET_DATA_LEN = 0xb8;
        const int FLEET_DATA_LEN = 28; // pretend i put it in hex...
        const int PLAYER_DATA_LEN = 0x0dd4;
        const int PLANET_FLEET_LEN = 24;
        const int STARSHIP_SINGLE_LEN = 0x44;
        const int PLAYER_RESEARCH_LEN = 540;
        const int TRANSPORT_DATA_LEN = 0x12;
        
        
        // OTHER
        const int FLEET_INORBIT_OFFSET2 = 0x0c;
        const int STARSHIP_DATA_LEN = 0x468;
        const int TRANSPORT_DATA_MAX_RECORDS = 0x64; // it looks like there are a maximum allowed 100 transports at any given time.  maybe...
                                                        // not sure if any other data between transports and player data


        private string _fileName = string.Empty;
        private int _fileSize = 0;
        private byte[] _fileData = null;
        private Game.Game _game;

        public Savegame(int pIndex, Game.Game pGame)
        {
            _game = pGame;
            _fileName = "C:\\Users\\Daniel\\Documents\\Visual Studio 2013\\Freemoo Data\\data\\SAVE" + pIndex + ".GAM";
            Debug.Assert(File.Exists(_fileName), "Save game file does not exist.");
            BinaryReader br = new BinaryReader(new FileStream(_fileName, FileMode.Open));
            _fileSize = (int)br.BaseStream.Length;
            _fileData = br.ReadBytes(_fileSize);
            br.Close();            
        }

        public Galaxy parseGalaxy()
        {
            Galaxy g = new Galaxy();

            // g.num_stars = Util.build_int(@_filedata[0xe2d6], @_filedata[0xe2d7])
            g.NumStars = Util.buildInt(_fileData[0xe2d6], _fileData[0xe2d7]);
            g.GalaxySize = _fileData[0xe2d8];
            g.Difficulty = _fileData[0xe2d4];
            g.Turn = Util.buildInt(_fileData[0xe232], _fileData[0xe233]);
            g.PlanetFocus = Util.buildInt(_fileData[0xe236], _fileData[0xe237]);
            return g;
        }

        public List<Player> parsePlayers()
        {
            List<Player> players = new List<Player>();
            for (int i = 0; i < 6; i++)
            {
                Player p = parsePlayer(i, Util.slice(_fileData, PLAYER_DATA_OFFSET + (i * PLAYER_DATA_LEN), PLAYER_DATA_LEN));
                if (p != null)
                {
                    players.Add(p);
                }
            }
            return players;
        }

        private Player parsePlayer(int pId, byte[] rawPlayerData)
        {
            Player p = null;
            // apparently you can test byte 0x386 and if it's zero, no more players
            // i noted in the ruby version that there has got to be a better way
            int test = rawPlayerData[0x386];
            if (test > 0)
            {
                p = new Player(pId);
                p.ColorId = Util.buildInt(rawPlayerData[2], rawPlayerData[3]); // why a number with the value 0-5 needs two bytes...

                // parse the number of known techs
                // 0x386 is the offset...which is probably why I am testing it above to determine if there is a player here
                int techOffset = 0x386;
                foreach (TechTypeEnum t in Enum.GetValues(typeof(TechTypeEnum)))
                {
                    int techNum = (int)t * 2 + techOffset;
                    int techCount = Util.buildInt(rawPlayerData[techNum], rawPlayerData[techNum + 1]);
                    p.setKnowTechCount(t, techCount);
                }
            }
            return p;
        }

        public List<Planet> parsePlanets(int pNumPlanets)
        {
            List<Planet> planets = new List<Planet>();
            for (int i = 0; i < pNumPlanets; i++)
            {
                Planet p = this.parsePlanet(i, Util.slice(_fileData, i * PLANET_DATA_LEN + PLANET_OFFSET, PLANET_DATA_LEN));
                planets.Add(p);
            }
            return planets;
        }

        private Planet parsePlanet(int pId, byte[] rawPlanetData)
        {
            Planet p = new Planet(pId, _game);
            byte[] nameBuffer = Util.slice(rawPlanetData, 0, 12);
            
            
            p.Name = Util.GetZString(nameBuffer);

            // i think this way of doing it is bad and i should feel bad
            // and then i got rid of it and I don't feel bad anymore
            //StringBuilder sb = new StringBuilder();
            //bool foundZero = false;
            //for (int i = 0; i < nameBuffer.Length && !foundZero; i++)
            //{
            //    if (nameBuffer[i] == 0)
            //    {
            //        foundZero = true;
            //    }
            //    else
            //    {
            //        sb.Append((char)nameBuffer[i]);
            //    }
            //}
            //p.Name = sb.ToString();

            int x = Util.buildInt(rawPlanetData[12], rawPlanetData[13]) * 2;
            int y = Util.buildInt(rawPlanetData[14], rawPlanetData[15]) * 2;
            
            p.StarSize = rawPlanetData[18];
            if (p.StarSize == 0)
            {
                x += 5;
                y += 5;
            }
            else
            {
                x += 3;
                y += 3;
            }
            p.relocate(x, y);
            p.PlayerId = rawPlanetData[54];
            p.StarColor = rawPlanetData[16];
            p.Player0Explored = rawPlanetData[124] == 1 ? true : false;
            p.PlanetType = (PlanetTypeEnum)Util.buildInt(rawPlanetData[0x20], rawPlanetData[0x21]);
            p.EcologyType = (EcologyTypeEnum)Util.buildInt(rawPlanetData[0x26], rawPlanetData[0x27]);
            p.Wealth = (PlanetWealthEnum)Util.buildInt(rawPlanetData[0x28], rawPlanetData[0x29]);
            p.SmallPlanetImageIndex = rawPlanetData[36];
            p.MaxPopulation = Util.buildInt(rawPlanetData[30], rawPlanetData[31]);
            

            if (p.PlayerId != 0xff)
            {
                p.IsColonized = true;
                p.CurrentPopulation = Util.buildInt(rawPlanetData[58], rawPlanetData[59]);
                p.AmtWaste = Util.buildInt(rawPlanetData[52], rawPlanetData[53]);
                p.AmtFactories = Util.buildInt(rawPlanetData[62], rawPlanetData[63]);
                p.AmtBases = Util.buildInt(rawPlanetData[95], rawPlanetData[96]);
                p.AmtProductivity = Util.buildInt(rawPlanetData[64], rawPlanetData[65]);
                p.MaxProductivity = Util.buildInt(rawPlanetData[66], rawPlanetData[67]);
                p.CurrShip = (int)rawPlanetData[0x5a];
                p.RelocPlanet = Util.buildInt(rawPlanetData[0x5c], rawPlanetData[0x5d]);
                p.ShipBuildBank = Util.buildInt(rawPlanetData[0x2c], rawPlanetData[0x2d]);
                p.BaseBuildBank = Util.buildInt(rawPlanetData[0x60], rawPlanetData[0x61]);
                p.Reserve = Util.buildInt(rawPlanetData[0x30], rawPlanetData[0x31]);
                p.Stargate = rawPlanetData[0x64] == 1 ? true : false;
                
                PlanetaryProduction pp = new PlanetaryProduction();
                pp.Ship.Value = Util.buildInt(rawPlanetData[80], rawPlanetData[81]);
                pp.Ship.Locked = rawPlanetData[0x72] == 1 ? true : false;
                pp.Defense.Value = Util.buildInt(rawPlanetData[82], rawPlanetData[83]);
                pp.Defense.Locked = rawPlanetData[0x74] == 1 ? true : false;
                pp.Industry.Value = Util.buildInt(rawPlanetData[84], rawPlanetData[85]);
                pp.Industry.Locked = rawPlanetData[0x76] == 1 ? true : false;
                pp.Ecology.Value = Util.buildInt(rawPlanetData[86], rawPlanetData[87]);
                pp.Ecology.Locked = rawPlanetData[0x78] == 1;
                pp.Technology.Value = Util.buildInt(rawPlanetData[88], rawPlanetData[89]);
                pp.Technology.Locked = rawPlanetData[0x7a] == 1;
                p.Production = pp;

                
            }

            return p;
        }

        public List<Fleet> parseFleetsIntransit(int numPlayers)
        {
            List<Fleet> fleets = new List<Fleet>();
            for (int i = 0; i < 260; i++) // only room for 260 fleets in the save game
            {
                //fleets.Add(parseFleetIntransit(Util.getNextFleetId(), Util.slice(mFileData, i * FLEET_DATA_LEN + FLEET_DATA_OFFSET, FLEET_DATA_LEN)));
                Fleet f = parseFleetIntransit(Util.getNextFleetId(), Util.slice(_fileData, i * FLEET_DATA_LEN + FLEET_DATA_OFFSET, FLEET_DATA_LEN), numPlayers);
                if (f != null)
                {
                    fleets.Add(f);
                }
            }
            return fleets;
        }

        private Fleet parseFleetIntransit(int pId, byte[] rawFleetData, int numPlayers)
        {
            Fleet f = null;
            int test = Util.buildInt(rawFleetData[0], rawFleetData[1]);
            if (test < numPlayers)
            {
                f = new Fleet(pId);
                f.InTransit = true;
                f.PlayerId = Util.buildInt(rawFleetData[0], rawFleetData[1]);
                int x = Util.buildInt(rawFleetData[2], rawFleetData[3]) * 2;
                int y = Util.buildInt(rawFleetData[4], rawFleetData[5]) * 2;
                f.relocate(x, y);
                f.PlanetId = Util.buildInt(rawFleetData[6], rawFleetData[7]);
                for (int i = 0; i < 6; i++)
                {
                    int ii = i * 2 + 0xa;
                    f[i] = Util.buildInt(rawFleetData[ii], rawFleetData[ii + 1]);
                }
            }

            return f;
        }

        public List<Fleet> parseFleetsInOrbit(int pNumPlanets, int pNumPlayers)
        {
            List<Fleet> fleets = new List<Fleet>();
            for (int playerId = 0; playerId < pNumPlayers; playerId++)
            {
                for (int planetId = 0; planetId < pNumPlanets; planetId++)
                {
                    int idx = (playerId * PLAYER_DATA_LEN) + (PLANET_FLEET_LEN * planetId) + FLEET_INORBIT_OFFSET;
                    byte[] slice = Util.slice(_fileData, idx, PLANET_FLEET_LEN);
                    if (slice[0] > 0)
                    {
                        Fleet f = new Fleet(Util.getNextFleetId());
                        f.PlayerId = playerId;
                        f.InTransit = false;
                        f.PlanetId = planetId;
                        for (int j = 0; j < 6; j++)
                        {
                            int jj = (j * 2) + FLEET_INORBIT_OFFSET2;
                            f[j] = Util.buildInt(slice[jj], slice[jj + 1]);
                        }
                        fleets.Add(f);
                    }
                }
            }
            return fleets;
        }

        public List<Starship> parseStarshipsDesigns(int pNumPlayers)
        {
            List<Starship> starships = new List<Starship>();
            for (int playerId = 0; playerId < pNumPlayers; playerId++)
            {
                for (int starshipId = 0; starshipId < 6; starshipId++)
                {
                    int currIdx = (playerId * STARSHIP_DATA_LEN) + (starshipId * STARSHIP_SINGLE_LEN) + STARSHIP_DATA_OFFSET;
                    // so so mucch I don't know.  such as, how do I know if the design is even still valid? it could havw been deleted.
                    Starship ship = parseSingleStarshipDesign(playerId, starshipId, Util.slice(_fileData, currIdx, STARSHIP_SINGLE_LEN));
                    starships.Add(ship);
                }
            }
            return starships;
        }

        private Starship parseSingleStarshipDesign(int pPlayerId, int pStarshipId, byte[] pRawData)
        {
            Starship ship = new Starship(pStarshipId);
            ship.Name = Util.GetZString(Util.slice(pRawData, 0, 12));

            ship.Weapon1 = pRawData[0x1c];
            ship.Weapon1Count = pRawData[0x24];
            ship.Weapon2 = pRawData[0x1e];
            ship.Weapon2Count = pRawData[0x26];
            ship.Weapon3 = pRawData[0x20];
            ship.Weapon3Count = pRawData[0x28];
            ship.Weapon4 = pRawData[0x22];
            ship.Weapon4Count = pRawData[0x2a];

            ship.Special1 = pRawData[0x32];
            ship.Special2 = pRawData[0x34];
            ship.Special3 = pRawData[0x36];

            ship.Ecm = pRawData[0x3a];
            ship.Computer = pRawData[0x3c];
            ship.Armor = pRawData[0x3e];
            ship.Maneuver = pRawData[0x40];
            ship.HitPoints = Util.buildInt(pRawData[0x42], pRawData[0x43]);

            ship.PlayerID = pPlayerId;
            ship.ImageIdx = pRawData[0x1a];
            ship.Cost = Util.buildInt(pRawData[0x14], pRawData[0x15]);
            // any address up to 0x44 that is not here, is currently unknown.
            return ship;
        }

        public List<PlayerTech> parsePlayerTechnologies(List<Player> pPlayers, Game.Game pGame)
        {
            List<PlayerTech> pt = new List<PlayerTech>();
            for (int i = 0; i < pPlayers.Count; i++)
            {
                pt.Add(parsePlayerTechnology(pPlayers[i], pGame, Util.slice(_fileData, PLAYER_RESEARCH_OFFSET + (PLAYER_RESEARCH_LEN * i), PLAYER_RESEARCH_LEN)));
            }

            return pt;
        }

        private PlayerTech parsePlayerTechnology(Player pPlayer, Game.Game pGame, byte [] pRawData)
        {
            PlayerTech pt = new PlayerTech(pGame);

            // get the list of known techs
            int offset = 180;
            foreach (TechTypeEnum t in Enum.GetValues(typeof(TechTypeEnum)))
            {
                int addy = ((int)t * 60) + offset;
                int knownTechCount = pPlayer.getKnownTechCount(t);
                for (int i = 0; i < knownTechCount; i++)
                {
                    if (pRawData[addy + i] != 0)
                    {
                        pt.learnTech(t, pRawData[addy + i]);
                    }
                }
            }

            return pt;
        }


        public List<Transport> parseTransports()
        {
            List<Transport> transports = new List<Transport>();
            for (int i = 0; i < TRANSPORT_DATA_MAX_RECORDS; i++)
            {
                Transport t = parseTransport(Util.slice(_fileData, i * TRANSPORT_DATA_LEN + TRANSPORT_DATA_OFFSET, TRANSPORT_DATA_LEN), i);
                transports.Add(t);
            }
            return transports;
        }

        private Transport parseTransport(byte[] rawData, int id)
        {
            Transport t = new Transport(id);
            int x = Util.buildInt(rawData[0x02], rawData[0x03]) * 2;
            int y = Util.buildInt(rawData[0x04], rawData[0x05]) * 2;
            t.relocate(x, y);
            t.DestPlanetId = Util.buildInt(rawData[0x06], rawData[0x07]);
            t.PlayerId = Util.buildInt(rawData[0x00], rawData[0x01]);
            t.SizeInMillions = Util.buildInt(rawData[0x08], rawData[0x09]);
            return t;
        }
    }
}
