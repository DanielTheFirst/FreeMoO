using System;

namespace FreemooSDL
{
    public class ScreenActionEventArgs
    {
        public ScreenEnum NextScreen { get; set; }
        public ScreenActionEnum ScreenAction { get; set; }

        public ScreenActionEventArgs()
        {
            NextScreen = ScreenEnum.None;
            ScreenAction = ScreenActionEnum.None;
        }

        public ScreenActionEventArgs(ScreenEnum scr, ScreenActionEnum act)
        {
            NextScreen = scr;
            ScreenAction = act;
        }
    }
}
