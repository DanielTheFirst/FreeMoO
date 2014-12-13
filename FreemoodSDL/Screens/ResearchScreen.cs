﻿using System;
using System.Collections.Generic;

using FreemooSDL.Service;
using SdlDotNet.Graphics;
using SdlDotNet.Core;
using SdlDotNet.Input;

namespace FreemooSDL.Screens
{
    public class ResearchScreen
        : AbstractScreen
    {
        public ResearchScreen(FreemooGame pGame)
            : base(pGame)
        {
        }

        public override void draw(FreemooTimer pTimer)
        {
            ImageService imgService = Game.Images;
            GuiService gs = Game.Screen;

            Surface techBoard = imgService.getSurface(ArchiveEnum.BACKGRND, "TECH_BRD", 0); //imgService.Images[ArchiveEnum.BACKGRND, "TECH_BRD"][0];
            gs.drawImage(techBoard, 0, 0);
        }

        public override void update(FreemooTimer pTimer)
        {
            //throw new NotImplementedException();
        }

        public override void mousePressed(MouseButtonEventArgs pMbea)
        {
            if (pMbea.Button == MouseButton.SecondaryButton)
            {
                Game.popScreen();
            }
            base.mousePressed(pMbea);
        }
    }
}