using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FreemooSDL.Service;
using SdlDotNet.Graphics;
using SdlDotNet.Core;
using SdlDotNet.Input;

namespace FreemooSDL.Screens
{
    public class FleetScreen
        : AbstractScreen
    {
        public FleetScreen(FreemooGame pGame)
            : base(pGame)
        {
        }

        public override void draw(FreemooTimer pTimer)
        {
            ImageService imgService = Game.Images;
            GuiService gs = Game.Screen;

            Surface fleetBack = imgService.getSurface(ArchiveEnum.BACKGRND, "FLEETBRB",0);
            gs.drawImage(fleetBack, 0, 0);
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
