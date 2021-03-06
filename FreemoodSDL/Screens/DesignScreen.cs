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
    public class DesignScreen
        : AbstractScreen
    {

        public DesignScreen(FreemooGame pGame)
            : base (pGame)
        {
        }

        public override void Draw(Timer pTimer, GuiService pGuiService)
        {
            ImageService imgService = Game.Images;
            // gs = Game.Screen;

            Surface designBack = imgService.getSurface(ArchiveEnum.DESIGN, "DESIGN_BACK",0);
            pGuiService.drawImage(designBack, 0, 0);
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
