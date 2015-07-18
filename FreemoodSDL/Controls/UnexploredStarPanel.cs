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
    class UnexploredStarPanel
        : AbstractControl
    {
        private const string UNEXPLORED_TEXT = "UNEXPLORED";
        private const string RANGE_TEMPLATE = "Range {0} Parsecs";

        private Planet _planetRef = null;
        private MainScreen _mainScreenRef = null;
        private string _starDescText = string.Empty;
        private string[] _starDescLines;
        private string _rangeText = RANGE_TEMPLATE;


        public UnexploredStarPanel(MainScreen pScreen, Planet pPlanet)
            : base()
        {
            _planetRef = pPlanet;
            _mainScreenRef = pScreen;

            Id = "UNEXPLOREDPANEL_" + _planetRef.Name;

            _setStarDescText();
            RecalculateRange();
        }

        private void _setStarDescText()
        {
            switch (_planetRef.StarColor)
            {
                case 0:
                    _starDescText = FreemooConstants.YELLOW_STARS;
                    break;
                case 1:
                    _starDescText = FreemooConstants.RED_STARS;
                    break;
                case 2:
                    _starDescText = FreemooConstants.GREEN_STARS;
                    break;
                case 3:
                    _starDescText = FreemooConstants.BLUE_STARS;
                    break;
                case 4:
                    _starDescText = FreemooConstants.WHITE_STARS;
                    break;
                case 5:
                    _starDescText = FreemooConstants.NEUTRON_STARS;
                    break;

            }
            _starDescLines = _starDescText.Split('|');
        }

        public void RecalculateRange()
        {
            int range = _mainScreenRef.Game.OrionGame.CalcPlayer0Range(_planetRef.X, _planetRef.Y);
            _rangeText = string.Format(RANGE_TEMPLATE, range);
        }

        public override void Draw(FreemooTimer pTimer, GuiService pGuiService)
        {
            ImageService imgService = _mainScreenRef.Game.Images;
            Rectangle rect = ObjectPool.RectanglePool.GetObject();
            Point p = ObjectPool.PointObjPool.GetObject();
            Size s = ObjectPool.SizeObjPool.GetObject();

            Surface pnlSurface = imgService.getSurface(ArchiveEnum.STARMAP, "UNEXPLOR", 0);
            pGuiService.drawImage(pnlSurface, 224, 5);
                                                                                                             
            // draw unex text at 240, 27
            // color 73, 207, 36
            // font 5
            //Rectangle unexRect = new Rectangle(240, 27, 59, 7);
            s.Width = 59;
            s.Height = 7;
            rect.Size = s;
            p.X = 240;
            p.Y = 27;
            rect.Location = p;
            pGuiService.drawString(UNEXPLORED_TEXT, rect, FontEnum.font_5, Color.FromArgb(73, 207, 36));

            p.Y = 74;
            p.X = 227;
            s.Width = 84;
            s.Height = 14;
            rect.Size = s;

            for (int i = 0; i < _starDescLines.Length; i++)
            {
                rect.Location = p;
                pGuiService.drawString(_starDescLines[i], rect, FontEnum.font_5, Color.FromArgb(113, 150, 190), TextAlignEnum.Center, TextVAlignEnum.Center);

                p.Y += s.Height;
            }

            p.Y = 165;
            rect.Location = p;
            pGuiService.drawString(_rangeText, rect, FontEnum.font_0, FontPaletteEnum.UnexploredRange);

            ObjectPool.RectanglePool.PutObject(rect);
            ObjectPool.PointObjPool.PutObject(p);
            ObjectPool.SizeObjPool.PutObject(s);

        }

        public override void Update(FreemooTimer pTimer)
        {
            
        }
    }
}
