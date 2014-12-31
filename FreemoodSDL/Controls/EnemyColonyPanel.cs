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

        private static string[] environments = { "NOPLANET", "RADIATE2", "TOXIC2", "INFERNO2", "DEAD2", 
                                                   "TUNDRA2", "BARREN2", "MINIMAL2", "DESERT2", "STEPPE2", 
                                                   "ARID2", "OCEAN2", "JUNGLE2", "TERRAN2" };

        public EnemyColonyPanel(MainScreen ms, Planet p)
        {
            _mainScreen = ms;
            _planet = p;
            Id = "EnemyColony_" + p.Name;
            BuildControls();
        }

        private void BuildControls()
        {
            _spl = new SmallPlanetLabel(_mainScreen, this, _planet);
            _spl.Id = "spl_" + _planet.Name;
            Controls.add(_spl);
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

            foreach(var ctrls in Controls)
            {
                ctrls.Value.draw(pTimer, guiService);
            }
        }
    }
}
