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
    public class SmallPlanetLabel
        : AbstractControl
    {

        private Planet _planetRef = null;
        private MainScreen _screenRef = null;

        public SmallPlanetLabel(MainScreen screen, IControl parentControl)
            : base()
        {
            ParentControl = parentControl;
            //_planetRef = planet;
            _screenRef = screen;
        }

        public void SetPlanet(Planet p)
        {
            _planetRef = p;
        }

        public override void Update(Timer pTimer)
        {
            
        }

        public override void Draw(Timer pTimer, GuiService guiService)
        {
            //if (this.Visible)
            //{
                ImageService imgService = _screenRef.Game.Images;
                string smallPlanet = "PLANET" + (_planetRef.SmallPlanetImageIndex + 1);
                Surface smallPanetSurf = imgService.getSurface(ArchiveEnum.PLANETS, smallPlanet, 0); //imgService.Images[ArchiveEnum.PLANETS, smallPlanet][0];
                // 229x26
                guiService.drawImage(smallPanetSurf, 229, 26);

                string planetType = _planetRef.PlanetType.ToString().ToUpper();
                // [0xff00ff, 0xFFDF51, 0xff88ff, 0xff88ff, 0xCB9600]
                guiService.drawString(planetType, new Rectangle(263, 28, 43, 5), FontEnum.font_0, FontPaletteEnum.PlanetType, TextAlignEnum.Right, TextVAlignEnum.None);

                if (_planetRef.Wealth != PlanetWealthEnum.Normal)
                {
                    string wealth = String.Empty;

                    if (_planetRef.Wealth == PlanetWealthEnum.UltraPoor)
                    {
                        wealth = "ULTRA POOR";
                    }
                    else if (_planetRef.Wealth == PlanetWealthEnum.UltraRich)
                    {
                        wealth = "ULTRA RICH";
                    }
                    else
                    {
                        wealth = _planetRef.Wealth.ToString().ToUpper();
                    }
                    guiService.drawString(wealth, new Rectangle(263, 36, 43, 5), FontEnum.font_0, FontPaletteEnum.PlanetBluePal, TextAlignEnum.Right, TextVAlignEnum.None);
                }

                string popString = "POP" + _planetRef.MaxPopulation.ToString().PadLeft(3, ' ') + " MAX";
                guiService.drawString(popString, new Rectangle(263, 45, 43, 5), FontEnum.font_2, FontPaletteEnum.PopulationGreen, TextAlignEnum.Right, TextVAlignEnum.None);
            //}

        }
    }
}
