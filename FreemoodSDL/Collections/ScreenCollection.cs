using System;
using System.Collections.Generic;

using FreeMoO.Screens;

namespace FreeMoO.Collections
{
    public class ScreenCollection 
        : Dictionary<ScreenEnum, IScreen>
    {
        // todo: add asserts to override the existing add, etc functions.
    }
}
