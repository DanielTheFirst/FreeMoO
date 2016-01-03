using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using FreemooSDL.Collections;
using FreemooSDL.Game;
using FreemooSDL.Screens;
using FreemooSDL.Service;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Input;

namespace FreemooSDL.Controls
{
    public class FleetIntransitPanel
        : AbstractControl
    {
        private MainScreen _mainScreen = null;
        private Fleet _fleetRef = null;

        public Fleet Fleet
        {
            get
            {
                return _fleetRef;
            }
            set
            {
                _fleetRef = value;
            }
        }

        public FleetIntransitPanel(MainScreen main)
            : base()
        {
            _mainScreen = main;
            Id = "FleetIntransit";

        }

        public override void Update(FreemooTimer pTimer)
        {
            
        }

        public override void Draw(FreemooTimer pTimer, GuiService pGuiService)
        {
            if (this.Visible)
            {
                ImageService imgSvc = _mainScreen.Game.Images;
                Surface panelSurf = imgSvc.getSurface(ArchiveEnum.STARMAP, "MOVEXTR2", 0);
                pGuiService.drawRect(224, 5, panelSurf.Width, panelSurf.Height, Color.FromArgb(0x79, 0x79, 0x79));
                pGuiService.drawImage(panelSurf, 224, 5);
            }
        }
    }
}
