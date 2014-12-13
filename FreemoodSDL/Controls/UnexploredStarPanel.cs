using System;
using System.Collections.Generic;
using System.Drawing;

using FreemooSDL.Game;
using FreemooSDL.Screens;
using FreemooSDL.Service;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Input;

namespace FreemooSDL.Controls
{
    class UnexploredStarPanel
        : AbstractControl
    {
        private const string UNEXPLORED_TEXT = "UNEXPLORED";

        private Planet _planetRef = null;
        private MainScreen _mainScreenRef = null;

        public UnexploredStarPanel(MainScreen pScreen, Planet pPlanet)
            : base()
        {
            _planetRef = pPlanet;
            _mainScreenRef = pScreen;

            Id = "UNEXPLOREDPANEL_" + _planetRef.Name;
        }

        public override void draw(FreemooTimer pTimer, GuiService pGuiService)
        {
            ImageService imgService = _mainScreenRef.Game.Images;

            Surface pnlSurface = imgService.getSurface(ArchiveEnum.STARMAP, "UNEXPLOR", 0);
            pGuiService.drawImage(pnlSurface, 224, 5);
                                                                                                             
            // draw unex text at 240, 27
            // color 73, 207, 36
            // font 5
            Rectangle unexRect = new Rectangle(240, 27, 59, 7);
            pGuiService.drawString(UNEXPLORED_TEXT, unexRect, FontEnum.font_5, Color.FromArgb(73, 207, 36));
        }

        public override void update(FreemooTimer pTimer)
        {
            
        }
    }
}
