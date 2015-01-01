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
    public class EnemyColonyPanel
        : AbstractControl
    {

        private MainScreen _mainScreen = null;
        private Planet _planet = null;
        private SmallPlanetLabel _spl = null;
        private string _ownerRace = string.Empty;  // not pulling this value in savegame.cs yet.
        private string _rangeText = string.Empty;

        private static string[] environments = { "NOPLANET", "RADIATE2", "TOXIC2", "INFERNO2", "DEAD2", 
                                                   "TUNDRA2", "BARREN2", "MINIMAL2", "DESERT2", "STEPPE2", 
                                                   "ARID2", "OCEAN2", "JUNGLE2", "TERRAN2" };

        private const string RANGE_TEMPLATE = "Range {0} Parsecs";

        public EnemyColonyPanel(MainScreen ms, Planet p)
            :base()
        {
            _mainScreen = ms;
            _planet = p;
            Id = "EnemyColony_" + p.Name;
            _ownerRace = _mainScreen.Game.OrionGame.Players[p.PlayerId].Race.ToString() + " Colony";

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
            // nothing
            foreach(var ctrls in Controls)
            {
                ctrls.Value.update(pTimer);
            }
        }

        public override void draw(FreemooTimer pTimer, GuiService guiService)
        {

            Surface panelSurf = _mainScreen.Game.Images.getSurface(ArchiveEnum.STARMAP, "EN_COLNY", 0);
            guiService.drawImage(panelSurf, 224, 5);

            Surface colonySurf = _mainScreen.Game.Images.getSurface(ArchiveEnum.COLONIES, environments[(int)_planet.PlanetType], 0);
            guiService.drawImage(colonySurf, 227, 73);

            Rectangle rect = new Rectangle(227, 8, 84, 13);
            guiService.drawString(_planet.Name, rect, FontEnum.font_4, FontPaletteEnum.Font4Colors);

            Rectangle ownerRect = new Rectangle(237, 83, 63, 9);
            //string ownerStr = _mainScreen.Game.OrionGame.Players[_planet.PlayerId].Race.ToString() + " Colony";
            guiService.drawString(_ownerRace, ownerRect, FontEnum.font_1, Color.Magenta, TextAlignEnum.Center, TextVAlignEnum.Center);

            Rectangle rangeRect = new Rectangle(237, 94, 63, 8);
            guiService.drawString(_rangeText, rangeRect, FontEnum.font_0, FontPaletteEnum.UnexploredRange);

            string currentPopulation = _planet.CurrentPopulation.ToString().PadLeft(3, ' ');
            guiService.drawString(currentPopulation, new Rectangle(259, 61, 8, 5), FontEnum.font_2, FontPaletteEnum.PlanetType, TextAlignEnum.Right, TextVAlignEnum.None);

            string currentBases = _planet.AmtBases.ToString().PadLeft(3, ' ');
            //guiService.drawString(currentBases, new Rectangle(304, 61, 8, 5), FontEnum.font_2, FontPaletteEnum.PlanetType, TextAlignEnum.Right, TextVAlignEnum.None);


            foreach(var ctrls in Controls)
            {
                ctrls.Value.draw(pTimer, guiService);
            }
        }
    }
}
