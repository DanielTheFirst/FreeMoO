using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreemooSDL
{
    // maintain this and the original configservice class for now just to keep backwards compatibility.
    public static class Config
    {
        public static  string DataFolder
        {
            get
            {
                return "C:\\Users\\Daniel\\Documents\\Visual Studio 2013\\FreeMoO Data\\data";
            }
        }

        public static int StretchRatio
        {
            get
            {
                return 4; /// blow out the original 320x200 to 1280x800.  looks pretty good.
            }
        }

        public static bool Fullscreen
        {
            get
            {
                return false;
            }
        }
    }
}
