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
    public class RaceScreen
        : AbstractScreen
    {
        public RaceScreen(FreemooGame pGame)
            : base(pGame)
        {
        }

        public override void Draw(Timer pTimer, GuiService pGuiService)
        {
            ImageService imgService = Game.Images;
            GuiService gs = Game.Screen;

            Surface raceBoard = imgService.getSurface(ArchiveEnum.BACKGRND, "RACEBACK",0);
            gs.drawImage(raceBoard, 0, 0);
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
