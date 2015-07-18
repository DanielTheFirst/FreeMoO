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
    public class PlanetsScreen
        : AbstractScreen
    {
        public PlanetsScreen(FreemooGame pGame)
            : base (pGame)
        {
        }

        public override void Draw(FreemooTimer pTimer, GuiService pGuiService)
        {
            ImageService imgService = Game.Images;
            GuiService gs = Game.Screen;

            Surface planetBrd = imgService.getSurface(ArchiveEnum.BACKGRND, "PLANET_BRD",0);
            gs.drawImage(planetBrd, 0, 0);
        }

        public override void Update(FreemooTimer pTimer)
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
