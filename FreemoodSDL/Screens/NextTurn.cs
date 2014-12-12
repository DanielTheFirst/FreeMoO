using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreemooSDL.Screens
{
    // the star map background is starmap_47.
    public class NextTurn
        : GameScreen
    {
        public NextTurn(FreemooGame game)
            : base(game)
        {
            // in moo, the next turn sequence appears to last for multiple ticks.  most likely the next turn process
            // was broken up due to limitations of cpu speed in 1994.  there are a lot of steps.  i'm not sure all of 
            // them can be processed in one tick on modern computers but even ifthey can, a lot happens, especially late game
            // and the animations are nice.  plus there are all kinds of interruptions.  


        }
    }
}
