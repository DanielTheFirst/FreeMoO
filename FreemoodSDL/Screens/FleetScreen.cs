using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FreeMoO.Service;
using SdlDotNet.Graphics;
using SdlDotNet.Core;
using SdlDotNet.Input;

namespace FreeMoO.Screens
{
    public class FleetScreen
        : AbstractScreen
    {
        public FleetScreen(FreemooGame pGame)
            : base(pGame)
        {
        }

        public override void Draw(FreemooTimer pTimer, GuiService pGuiService)
        {
            ImageService imgService = Game.Images;
            //GuiService gs = Game.Screen;

            Surface fleetBack = imgService.getSurface(ArchiveEnum.BACKGRND, "FLEETBRB",0);
            pGuiService.drawImage(fleetBack, 0, 0);
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
