using System;
using System.Drawing;

using FreemooSDL.Collections;
using FreemooSDL.Service;
using SdlDotNet.Graphics;
using SdlDotNet.Core;
using SdlDotNet.Input;

namespace FreemooSDL.Screens
{
    class LoadingScreen
        : AbstractScreen
    {
        string LoadingText = "Loading Master Of Orion...";
        int textR = 0x00;
        int textG = 0x21;
        int textB = 0x45;
        int textMaxR = 0x65;
        double fadeTimer = 0;

        public LoadingScreen(FreemooGame game)
            : base(game)
        {

        }

        public override void update(FreemooTimer pTimer)
        {
            //throw new NotImplementedException();
        }

        public override void draw(FreemooTimer pTimer)
        {
            ImageService img = Game.Images;
            GuiService gui = Game.Screen;

            fadeTimer += pTimer.MillisecondsElapsed;

            if (textR < textMaxR && fadeTimer >= 12)
            {
                textR++;
                textG++;
                textB++;
                fadeTimer = 0;
            }

            Color currTextColor = Color.FromArgb(textR, textG, textB);

            gui.drawString(LoadingText, 0, 0, FontEnum.font_1, currTextColor);

        }
    }
}
