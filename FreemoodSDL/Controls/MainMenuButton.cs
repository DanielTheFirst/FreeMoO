using System;

using SdlDotNet.Input;

using FreemooSDL.Collections;
using FreemooSDL.Service;

namespace FreemooSDL.Controls
{
    class MainMenuButton
        : MooButton
    {

        private const int WIDTH = 83;
        private const int HEIGHT = 16;

        public bool Enabled { get; set; }

        public string Text { get; set; }

        // 83 x 10

        public MainMenuButton(int x, int y, string btnText)
        {
            X = x;
            Y = y;
            mBoundingRect = ObjectPool.RectanglePool.GetObject();
            mBoundingRect.X = X;
            mBoundingRect.Y = Y;
            mBoundingRect.Width = WIDTH;
            mBoundingRect.Height = HEIGHT;
            Text = btnText;
        }

        public override void Draw(FreemooTimer pTimer, Service.GuiService guiSvc)
        {
            FontPaletteEnum fpe = FontPaletteEnum.Font4Colors;
            if (Enabled && MouseOver)
            {
                fpe = FontPaletteEnum.MainMenuBtnActive;
            }
            else if (!Enabled)
            {
                fpe = FontPaletteEnum.MainMenuBtnDisabled;
            }

            guiSvc.drawString(Text, X, Y, FontEnum.font_4, fpe);
        }
    }
}
