﻿using System;
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

        public NoColonyPanel(MainScreen ms, Planet p)
            : base()
        {
            _mainScreen = ms;
            _planet = p;
            Id = "NoColony_" + p.Name;
            //_ownerRace = _mainScreen.Game.OrionGame.Players[p.PlayerId].Name

            RecalculateRange();
            BuildControls();
        }

        private void BuildControls()
        {
            _spl = new SmallPlanetLabel(_mainScreen, this, _planet);
            _spl.Id = "spl_" + _planet.Name;
            Controls.add(_spl);
        }

        public void RecalculateRange()
        {
            int range = _mainScreen.Game.OrionGame.CalcPlayer0Range(_planet.X, _planet.Y);
            _rangeText = string.Format(RANGE_TEMPLATE, range);
        }

        public override void update(FreemooTimer pTimer)
        {
            foreach (var ctrls in Controls)
            {
                ctrls.Value.update(pTimer);
            }
        }

        public override void draw(FreemooTimer timer, GuiService guiService)
        {
            Surface panelSurf = _mainScreen.Game.Images.getSurface(ArchiveEnum.STARMAP, "NO_COLNY", 0);
            guiService.drawImage(panelSurf, 224, 5);

            Surface colonySurf = _mainScreen.Game.Images.getSurface(ArchiveEnum.COLONIES, environments[(int)_planet.PlanetType], 0);
            guiService.drawImage(colonySurf, 227, 73);

            Rectangle rect = new Rectangle(227, 8, 84, 13);
            guiService.drawString(_planet.Name, rect, FontEnum.font_4, FontPaletteEnum.Font4Colors);

            Rectangle rangeRect = new Rectangle(237, 84, 63, 8);
            guiService.drawString(_rangeText, rangeRect, FontEnum.font_0, FontPaletteEnum.UnexploredRange);

            foreach (var ctrls in Controls)
            {
                ctrls.Value.draw(timer, guiService);
            }
        }
    }
}