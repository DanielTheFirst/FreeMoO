using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace FreemooSDL.Game
{
    public struct ProductionSettings
    {
        public int Value;
        public bool Locked;
    }

    public struct PlanetaryProduction
    {
        public ProductionSettings Ship;
        public ProductionSettings Defense;
        public ProductionSettings Industry;
        public ProductionSettings Ecology;
        public ProductionSettings Technology;

        public int[] getArrayOfValues()
        {
            int[] vals = new int[5];
            vals[0] = Ship.Value;
            vals[1] = Defense.Value;
            vals[2] = Industry.Value;
            vals[3] = Ecology.Value;
            vals[4] = Technology.Value;
            return vals;
        }

        public bool[] getArrayOfLocked()
        {
            bool[] vals = new bool[5];
            vals[0] = Ship.Locked;
            vals[1] = Defense.Locked;
            vals[2] = Industry.Locked;
            vals[3] = Ecology.Locked;
            vals[4] = Technology.Locked;
            return vals;
        }
    }

    public class Planet 
        : SpaceObject
    {
        //          attr_accessor :is_colonized, :player_id, :max_population, :star_color, :star_size, :player0_explored
        //  attr_accessor :planet_type, :ecology, :wealth, :shields, :production_bars, :prod_locked_bars
        //  attr_accessor :curr_population, :amt_waste, :amt_factories, :amt_bases, :amt_productivity, :max_productivity
        //attr_accessor :ship_build_bank, :base_build_bank, :reserve # what is reserve?  damn, i figured it out at one point, probably from flipping thru the guide
        //attr_accessor :has_stargate
        //attr_accessor :curr_ship, :reloc, :small_planet_image_idx

        private bool mIsColonized = false;
        private int mPlayerId = -1;
        private int mStarColor = 0;
        private int mStarSize = 0;
        private bool mPlayer0Explored = false;
        private PlanetTypeEnum mPlanetType = PlanetTypeEnum.Jungle;
        private EcologyTypeEnum mEcologyType = EcologyTypeEnum.Normal;
        private PlanetWealthEnum mWealth = PlanetWealthEnum.Normal;
        private PlanetaryShieldsEnum mShileds = PlanetaryShieldsEnum.None;
        private PlanetaryProduction mProduction;
        private Boolean mStargate = false;
        private int mShipBuildBank;
        private int mBaseBuildBank;
        private int mReserve; // real quick before I forget, this is when you transfer money to a colony there is a hard limit on only doubling production so the rest is stored here and used up on subsequent turns.
        private int mCurrentPopulation;
        private int mMaxPopulation;
        private int mAmtBases;
        private int mAmtWaste;
        private int mAmtFactories;
        private int mAmtProductivity;
        private int mMaxProductivity;
        private int mCurrShip;
        private int mRelocPlanet;
        private int mSmallPlanetImageIndex;
        private Game mGame;

        public int ProductionPoints { get; set; } // production points accrued in a single turn

        #region Properties
        public bool IsColonized
        {
            get
            {
                return mIsColonized;
            }
            set
            {
                mIsColonized = value;
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

        public int StarColor
        {
            get
            {
                return mStarColor;
            }
            set
            {
                mStarColor = value;
            }
        }

        public int StarSize
        {
            get
            {
                return mStarSize;
            }
            set
            {
                mStarSize = value;
            }
        }

        public bool Player0Explored
        {
            get
            {
                return mPlayer0Explored;
            }
            set
            {
                mPlayer0Explored = value;
            }
        }

        public PlanetTypeEnum PlanetType
        {
            get
            {
                return mPlanetType;
            }
            set
            {
                mPlanetType = value;
            }
        }

        public EcologyTypeEnum EcologyType
        {
            get
            {
                return mEcologyType;
            }
            set
            {
                mEcologyType = value;
            }
        }

        public PlanetWealthEnum Wealth
        {
            get
            {
                return mWealth;
            }
            set
            {
                mWealth = value;
            }
        }

        public PlanetaryShieldsEnum Shileds
        {
            get
            {
                return mShileds;
            }
            set
            {
                mShileds = value;
            }
        }

        public PlanetaryProduction Production
        {
            get
            {
                return mProduction;
            }
            set
            {
                mProduction = value;
            }
        }

        public Boolean Stargate
        {
            get
            {
                return mStargate;
            }
            set
            {
                mStargate = value;
            }
        }

        public int ShipBuildBank
        {
            get
            {
                return mShipBuildBank;
            }
            set
            {
                mShipBuildBank = value;
            }
        }

        public int BaseBuildBank
        {
            get
            {
                return mBaseBuildBank;
            }
            set
            {
                mBaseBuildBank = value;
            }
        }

        public int Reserve
        {
            get
            {
                return mReserve;
            }
            set
            {
                mReserve = value;
            }
        }

        public int CurrentPopulation
        {
            get
            {
                return mCurrentPopulation;
            }
            set
            {
                mCurrentPopulation = value;
            }
        }

        public int AmtWaste
        {
            get
            {
                return mAmtWaste;
            }
            set
            {
                mAmtWaste = value;
            }
        }

        public int AmtFactories
        {
            get
            {
                return mAmtFactories;
            }
            set
            {
                mAmtFactories = value;
            }
        }

        public int AmtBases
        {
            get
            {
                return mAmtBases;
            }
            set
            {
                mAmtBases = value;
            }
        }

        public int AmtProductivity
        {
            get
            {
                return mAmtProductivity;
            }
            set
            {
                mAmtProductivity = value;
            }
        }

        public int MaxProductivity
        {
            get
            {
                return mMaxProductivity;
            }
            set
            {
                mMaxProductivity = value;
            }
        }

        public int CurrShip
        {
            get
            {
                return mCurrShip;
            }
            set
            {
                mCurrShip = value;
            }
        }

        public int RelocPlanet
        {
            get
            {
                return mRelocPlanet;
            }
            set
            {
                mRelocPlanet = value;
            }
        }

        public int SmallPlanetImageIndex
        {
            get
            {
                return mSmallPlanetImageIndex;
            }
            set
            {
                mSmallPlanetImageIndex = value;
            }
        }

        public int MaxPopulation
        {
            get
            {
                return mMaxPopulation;
            }
            set
            {
                mMaxPopulation = value;
            }
        }


        #endregion

        public Planet(int pId, Game pGame)
            : base(pId)
        {
            mGame = pGame;
        }

        public int calcSingleYearShipProduction()
        {
            int numShips = 0;

            float shipCost = (float)mGame.Starships[mCurrShip].Cost;
            float shipProd = (float)this.AmtProductivity * ((float)this.Production.Ship.Value / 100.0f);
            //shipProd = (float)Math.Round((double)shipProd, 2);
            float numShipsF = shipProd / shipCost;
            numShips = (int)numShipsF; // if it's less than 1 i believe it will truncate to 0

            return numShips;

        }

        public int calcNumYearsToProduceShip()
        {
            float shipCost = (float)mGame.Starships[mCurrShip].Cost;
            float shipProd = (float)this.AmtProductivity * ((float)this.Production.Ship.Value / 100.0f);
            int numYears = (int)(shipCost / shipProd);
            return numYears;
        }
    }
}
