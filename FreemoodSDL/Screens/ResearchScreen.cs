using System;
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

        public override void Draw(FreemooTimer pTimer, GuiService pGuiService)
        {
            ImageService imgService = Game.Images;
            //GuiService gs = Game.Screen;

            Surface techBoard = imgService.getSurface(ArchiveEnum.BACKGRND, "TECH_BRD", 0); //imgService.Images[ArchiveEnum.BACKGRND, "TECH_BRD"][0];
            pGuiService.drawImage(techBoard, 0, 0);
        }

        public override void Update(FreemooTimer pTimer)
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
