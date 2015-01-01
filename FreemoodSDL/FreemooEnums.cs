
namespace FreemooSDL
{
    // service enums
    public enum ServiceEnum
    {
        ImageService,
        GuiService,
        ConfigService,
        StateManager,
        InputService
    }

    public enum ArchiveEnum
    {
        BACKGRND,
        COLONIES,
        COUNCIL,
        DESIGN,
        EMBASSY,
        INTRO,
        INTRO2,
        LANDING,
        MISSILE,
        NEBULA,
        NEWSCAST,
        PLANETS,
        SCREENS,
        SHIPS,
        SHIPS2,
        SPACE,
        SPIES,
        STARMAP,
        STARVIEW,
        TECHNO,
        V11,
        VORTEX,
        WINLOSE 
    }

    public enum ScreenEnum
    {
        MainScreen,
        ResearchScreen,
        PlanetScreen,
        RaceScreen,
        MapScreen,
        FleetScreen,
        DesignScreen,
        GameScreen,
        OpeningMovie,
        LoadingScreen,
        MainMenu,
        LoadGameScreen
    }

    public enum PlanetTypeEnum
    {
        None = 0,
        Radiated = 1,
        Toxic = 2,
        Inferno = 3,
        Dead = 4,
        Tundra = 5,
        Barren = 6, 
        Minimal = 7,
        Desert = 8,
        Steppe = 9,
        Arid = 10,
        Ocean = 11,
        Jungle = 12,
        Terran = 13
    }

    public enum EcologyTypeEnum
    {
        Hostile = 0,
        Normal = 1,
        Fertile = 2,
        Gaia = 3
    }

    public enum PlanetWealthEnum
    {
        UltraPoor = 0,
        Poor = 1,
        Normal = 2,
        Artifacts = 3,
        Rich = 4,
        UltraRich = 5,
        Orion = 6
    }

    public enum PlanetaryShieldsEnum
    {
        None = 0,
        V = 1,
        X = 2,
        XV = 3,
        XX = 4
    }

    public enum ProductionEnum
    {
        Ship = 0, Def = 1, Ind = 2, Eco = 3, Tech = 4, None = 99
    }

    public enum FontEnum
    {
        font_0, font_1, font_2, font_3, font_4, font_5
    }

    public enum FontPaletteEnum
    {
        SingleColor,
        Font4Colors,
        PlanetType,
        PlanetBluePal,
        PopulationGreen,
        UnexploredRange,
        MainMenuBtnDisabled,
        MainMenuBtnEnabled,
        MainMenuBtnActive
    }

    public enum TextAlignEnum
    {
        Left, Center, Right, None
    }

    public enum TextVAlignEnum
    {
        Top, Center, Bottom, None
    }

    public enum SoundFXEnum
    {
        BOOMBOOMPOW
    }

    public enum TechTypeEnum
    {
        Computer, Construction, ForceFields, Planetology, Propulsion, Weapons
    }

    public enum RacialEnum
    {
        //Alkari, Bulrathi, Darlok, Human, Klackon, Meklar, Mrrshan, Psilon, Sakkra, Silicoid
        Human, Mrrshan, Silicoid, Sakkra, Psilon, Alkari, Klackon, Bulrathi, Meklar, Darlock
    }

    public enum ShipComponentType
    {
        BattleComputer, ECM, Shield, Armor, Engine, Maneuver
    }

    public enum MenuButtonEnum
    {
        Game, Design, Fleet, Map, Races, Planets, Tech, NextTurn
    }

    public enum ShipSizeEnum
    {
        Small, Medium, Large, Huge
    }
}