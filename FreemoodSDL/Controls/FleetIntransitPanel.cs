using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using FreeMoO.Collections;
using FreeMoO.Game;
using FreeMoO.Screens;
using FreeMoO.Service;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Input;

namespace FreeMoO.Controls
{
    public class FleetIntransitPanel
        : AbstractControl
    {
        private MainScreen _mainScreen = null;
        private Fleet _fleetRef = null;
        private List<FreemooImageInstance> _images = null;
        private FreemooImageInstance _scannerImage = null;

        public Fleet Fleet
        {
            get
            {
                return _fleetRef;
            }
            set
            {
                _fleetRef = value;
                UpdateImageInstances();
            }
        }

        public FleetIntransitPanel(MainScreen main)
            : base()
        {
            _mainScreen = main;
            Id = "FleetIntransit";
            _images = new List<FreemooImageInstance>();
            for(int i = 0; i < 6; i++)
            {
                FreemooImageInstance fii = new FreemooImageInstance(ArchiveEnum.SHIPS, "RSMALL", 0, _mainScreen.Game.Images);
                fii.Animate = true; fii.AnimateLoop = true;
                _images.Add(fii);
            }
            _scannerImage = new FreemooImageInstance(ArchiveEnum.STARMAP, "SCANNER", _mainScreen.Game.Images);
            _scannerImage.Animate = true;
            _scannerImage.AnimateLoop = true;

        }

        public override void Update(FreemooTimer timer)
        {
            foreach(var fi in _images)
            {
                fi.UpdateAnimation(timer);
            }
            // animated images are not working right yet if there are a lot of frames.
            // note to self: intro movie works because I implemented the animation 
            // custom rather than using my FreemooImageInstance updateanimation code
            //_scannerImage.UpdateAnimation(timer);
        }

        public override void Draw(FreemooTimer pTimer, GuiService pGuiService)
        {
            if (this.Visible)
            {
                ImageService imgSvc = _mainScreen.Game.Images;
                Surface panelSurf = imgSvc.getSurface(ArchiveEnum.STARMAP, "MOVEXTR2", 0);
                pGuiService.drawRect(224, 5, panelSurf.Width, panelSurf.Height, Color.FromArgb(0x79, 0x79, 0x79));
                pGuiService.drawImage(panelSurf, 224, 5);

                var playerId = _fleetRef.PlayerId;
                int idx = 0;
                var ships = _mainScreen.Game.OrionGame.Starships.Where(s => s.PlayerID == playerId).ToList();
                string[] shipSizes = { "SMALL", "MEDIUM", "LARGE", "HUGE" };
                string[] colors = { "B", "G", "P", "R", "W", "Y" };
                pGuiService.drawImage(_scannerImage.getCurrentFrame(), 227, 8);
                for (int i = 0; i < 6; i++)
                {
                    if (_fleetRef[i] > 0)
                    {
                        //var starshipImgIdx = ships[i].ImageIdx;
                        //ArchiveEnum shipArc = ArchiveEnum.SHIPS;
                        //if (starshipImgIdx < 72)
                        //{
                        //    shipArc = ArchiveEnum.SHIPS2;
                        //}
                        //else
                        //{
                        //    starshipImgIdx -= 72;
                        //}
                        //int offset = starshipImgIdx % 6;
                        //int shipSize = starshipImgIdx / 6 % 4;
                        //int playerColor = _mainScreen.Game.OrionGame.Players[playerId].ColorId;
                        int ycoord = 45 + (((idx - (idx & 0x01)) / 2) * 40);
                        int xCoord = (idx & 0x01) == 0 ? 228 : 271;
                        pGuiService.drawRect(xCoord, ycoord, 39, 25, Color.Black);
                        //pGuiService.drawImage(imgSvc.getSurface(shipArc, colors[playerColor] + shipSizes[shipSize], 0, offset), xCoord, ycoord);
                        pGuiService.drawImage(_images[idx].getCurrentFrame(), xCoord, ycoord);

                        idx++;
                    }
                }
            }
        }

        private void UpdateImageInstances()
        {
            var playerId = _fleetRef.PlayerId;
            int idx = 0;
            var ships = _mainScreen.Game.OrionGame.Starships.Where(s => s.PlayerID == playerId).ToList();
            string[] shipSizes = { "SMALL", "MEDIUM", "LARGE", "HUGE" };
            string[] colors = { "B", "G", "P", "R", "W", "Y" };
            for (int i = 0; i < 6; i++)
            {
                if (_fleetRef[i] > 0)
                {
                    var starshipImgIdx = ships[i].ImageIdx;
                    ArchiveEnum shipArc = ArchiveEnum.SHIPS;
                    if (starshipImgIdx < 72)
                    {
                        shipArc = ArchiveEnum.SHIPS2;
                    }
                    else
                    {
                        starshipImgIdx -= 72;
                    }
                    int offset = starshipImgIdx % 6;
                    int shipSize = starshipImgIdx / 6 % 4;
                    int playerColor = _mainScreen.Game.OrionGame.Players[playerId].ColorId;
                    _images[idx].ChangeImageReference(shipArc, colors[playerColor] + shipSizes[shipSize], offset);
                    _images[idx].Offset = offset;
                    idx++;
                }
            }
        }
    }
}
