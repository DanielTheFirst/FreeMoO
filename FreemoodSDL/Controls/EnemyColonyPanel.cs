using System;
using System.Collections.Generic;
using System.Drawing;

using FreeMoO.Collections;
using FreeMoO.Game;
using FreeMoO.Screens;
using FreeMoO.Service;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Input;

namespace FreeMoO.Controls
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

        public EnemyColonyPanel(MainScreen ms)
            :base()
        {
            _mainScreen = ms;
            //_planet = p;
            Id = "EnemyColony";
            //_ownerRace = _mainScreen.Game.OrionGame.Players[p.PlayerId].Race.ToString() + " Colony";

            //RecalculateRange();
            BuildControls();
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
                _ownerRace = _mainScreen.Game.OrionGame.Players[_planet.PlayerId].Race.ToString() + " Colony";
                _spl.SetPlanet(_planet);
            }
        }

        private void BuildControls()
        {
            _spl = new SmallPlanetLabel(_mainScreen, this);
            _spl.Id = "smallplanetlabel1";// +_planet.Name;
            Controls.add(_spl);
        }

        public void RecalculateRange()
        {
            int range = _mainScreen.Game.OrionGame.CalcPlayer0Range(_planet.X, _planet.Y);
            _rangeText = string.Format(RANGE_TEMPLATE, range);
        }

        public override void Update(Timer pTimer)
        {
            if (this.Enabled)
            {
                foreach (var ctrls in Controls)
                {
                    ctrls.Value.Update(pTimer);
                }
            }
        }

        public override void Draw(Timer pTimer, GuiService guiService)
        {
            if (this.Visible)
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


                foreach (var ctrls in Controls)
                {
                    ctrls.Value.Draw(pTimer, guiService);
                }
            }
        }
    }
}
