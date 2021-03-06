﻿using System;

using SdlDotNet.Input;

using FreeMoO.Collections;
using FreeMoO.Service;

namespace FreeMoO.Controls
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
            Width = WIDTH;
            Height = HEIGHT;
            Text = btnText;
        }

        public override void Draw(Timer pTimer, Service.GuiService guiSvc)
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
