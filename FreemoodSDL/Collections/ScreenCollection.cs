using System;
using System.Collections.Generic;

using FreemooSDL.Screens;

namespace FreemooSDL.Collections
{
    public class ScreenCollection 
        : Dictionary<ScreenEnum, IScreen>
    {
        // todo: add asserts to override the existing add, etc functions.
    }
}
