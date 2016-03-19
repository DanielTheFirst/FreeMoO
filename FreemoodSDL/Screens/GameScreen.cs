﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FreeMoO.Service;
using SdlDotNet.Graphics;
using SdlDotNet.Core;
using SdlDotNet.Input;

namespace FreeMoO.Screens
{
    public class GameScreen
        : AbstractScreen
    {
        public GameScreen(FreemooGame pGame)
            : base(pGame)
        {
        }

        public override void Draw(Timer pTimer, GuiService pGuiService)
        {
            ImageService imgService = Game.Images;
            //GuiService gs = Game.Screen;

            Surface gameBack = imgService.getSurface(ArchiveEnum.VORTEX, "GAME",0);
            pGuiService.drawImage(gameBack, 0, 0);
        }

        public override void Update(Timer pTimer)
        {
            //throw new NotImplementedException();
        }

        public override void mousePressed(MouseButtonEventArgs pMbea)
        {
            if (pMbea.Button == MouseButton.SecondaryButton)
            {
                //Game.popScreen();
                _screenAction.ScreenAction = ScreenActionEnum.Pop;
                Game.QueueScreenAction(_screenAction);
            }
            base.mousePressed(pMbea);
        }
    }
}
