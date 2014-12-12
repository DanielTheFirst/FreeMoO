using System;
using System.Collections.Generic;
using System.Drawing;

using FreemooSDL;

namespace FreemooSDL.Service
{
    public class ConfigService
    {
        // eventually this class will read from a real config file

        private FreemooGame mGame;

        public ConfigService(FreemooGame pGame)
        {
            mGame = pGame;
        }

        public string DataFolder
        {
            get
            {
                return "C:\\Users\\Daniel\\Documents\\Visual Studio 2013\\Projects\\FreeMoO Data\\data";
            }
        }

        public int StretchRatio
        {
            get
            {
                return 4; /// blow out the original 320x200 to 1280x800.  looks pretty good.
            }
        }

        public bool Fullscreen
        {
            get
            {
                return false;
            }
        }

    }
}
