using System;
using System.Collections.Generic;
using System.Drawing;

using FreemooSDL.Collections;
using FreemooSDL.Game;
using FreemooSDL.Screens;
using FreemooSDL.Service;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Input;

namespace FreemooSDL.Controls
{
    class NoColonyPanel
        : AbstractControl
    {
        private MainScreen _mainScreen = null;
        private Planet _planet = null;
        private SmallPlanetLabel _spl = null;
        private string _ownerRace = string.Empty;  // not pulling this value in savegame.cs yet.
        private string _rangeText = string.Empty;

        private static string[] environments = { "NOPLANET", "RADIATE1", "TOXIC1", "INFERNO1", "DEAD1", 
                                                   "TUNDRA1", "BARREN1", "MINIMAL1", "DESERT1", "STEPPE1", 
                                                   "ARID1", "OCEAN1", "JUNGLE1", "TERRAN1" };

        private const string RANGE_TEMPLATE = "Range {0} Parsecs";

        public NoColonyPanel(MainScreen ms)
            : base()
        {
            _mainScreen = ms;
            //_planet = p;
            Id = "NoColonyPanel";
            //_ownerRace = _mainScreen.Game.OrionGame.Players[p.PlayerId].Name

            //RecalculateRange();
            BuildControls();
        }

        private void BuildControls()
        {
            _spl = new SmallPlanetLabel(_mainScreen, this);
            _spl.Id = "smallplanetlabel1";
            Controls.add(_spl);
        }

        public Planet Planet
        {
            get
            {
                return _planet;
            }
            set
            {
                _planet = value;
                RecalculateRange();
                _ownerRace = _mainScreen.Game.OrionGame.Players[Planet.PlayerId].Name;
            }
        }

        public void RecalculateRange()
        {
            int range = _mainScreen.Game.OrionGame.CalcPlayer0Range(_planet.X, _planet.Y);
            _rangeText = string.Format(RANGE_TEMPLATE, range);
        }

        public override void Update(FreemooTimer pTimer)
        {
            if (this.Enabled)
            {
                foreach (var ctrls in Controls)
                {
                    ctrls.Value.Update(pTimer);
                }
            }
        }

        public override void Draw(FreemooTimer timer, GuiService guiService)
        {
            if (this.Visible)
            {
                Surface panelSurf = _mainScreen.Game.Images.getSurface(ArchiveEnum.STARMAP, "NO_COLNY", 0);
                guiService.drawImage(panelSurf, 224, 5);

                Surface colonySurf = _mainScreen.Game.Images.getSurface(ArchiveEnum.COLONIES, environments[(int)_planet.PlanetType], 0);
                guiService.drawImage(colonySurf, 227, 73);

                //Rectangle rect = new Rectangle(227, 8, 84, 13);
                var rect = ObjectPool.GetRectangle(227, 8, 84, 13);
                guiService.drawString(_planet.Name, rect, FontEnum.font_4, FontPaletteEnum.Font4Colors);
                ObjectPool.RectanglePool.PutObject(rect);

                //Rectangle rangeRect = new Rectangle(237, 84, 63, 8);
                var rangeRect = ObjectPool.GetRectangle(237, 84, 63, 8);
                guiService.drawString(_rangeText, rangeRect, FontEnum.font_0, FontPaletteEnum.UnexploredRange);
                ObjectPool.RectanglePool.PutObject(rangeRect);

                foreach (var ctrls in Controls)
                {
                    ctrls.Value.Draw(timer, guiService);
                }
            }
        }
    }
}
