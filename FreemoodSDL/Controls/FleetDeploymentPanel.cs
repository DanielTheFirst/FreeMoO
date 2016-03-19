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
    public class FleetDeploymentPanel
        : AbstractControl
    {
        const string COUNT_STR = "{0}";

        private MainScreen _mainScreen = null;
        private Fleet _fleetRef = null;
        private int[] _internalFleetCount = new int[6];
        private MooButton _cancelBtn = null;
        private MooButton _acceptBtn = null;
        private List<FreemooImageInstance> _images = null;
        private List<FleetDeployButtonSet> _moveButtons = null;

        public FleetDeploymentPanel(MainScreen screen)
            :base()
        {
            _mainScreen = screen;

            Id = "FleetDeploy";
            BuildButtons();
            _images = new List<FreemooImageInstance>();
            for (int i = 0; i < 6; i++)
            {
                FreemooImageInstance fii = new FreemooImageInstance(ArchiveEnum.SHIPS, "RSMALL", 0, _mainScreen.Game.Images);
                _images.Add(fii);
            }
        }

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
                UpdateMoveButtons();
                UpdateInternalFleetCount();
            }
        }

        private void BuildButtons()
        {
            ImageService imgService = _mainScreen.Game.Images;
            _cancelBtn = new MooButton(ArchiveEnum.STARMAP, "RELOC_BU cancel", imgService, 227, 180);
            _cancelBtn.Id = "FleetDeployCancel";
            this.Controls.add(_cancelBtn);
            _acceptBtn = new MooButton(ArchiveEnum.STARMAP, "RELOC_BU accept", imgService, 271, 180);
            _acceptBtn.Id = "FleetDeployAccept";
            _acceptBtn.Enabled = false;
            this.Controls.add(_acceptBtn);

            _moveButtons = new List<FleetDeployButtonSet>();
            for(int i = 0; i < 6; i++)
            {
                FleetDeployButtonSet btnSet = new FleetDeployButtonSet(_mainScreen, i);
                btnSet.Id = "MOVEBUTTONSET_" + i;
                btnSet.Visible = true;
                btnSet.Enabled = true;
                _moveButtons.Add(btnSet);
                Controls.add(btnSet);
            }
        }

        public override void Update(FreemooTimer pTimer)
        {
            
        }

        public override void Draw(FreemooTimer pTimer, GuiService pGuiService)
        {
            if (this.Visible)
            {
                ImageService imgSvc = _mainScreen.Game.Images;
                Surface panelSurf = imgSvc.getSurface(ArchiveEnum.STARMAP, "MOVE_SHI", 0);
                pGuiService.drawRect(224, 5, panelSurf.Width, panelSurf.Height, Color.FromArgb(0x79, 0x79, 0x79));
                pGuiService.drawImage(panelSurf, 224, 5);

                int idx = 0;
                var ships = _mainScreen.Game.OrionGame.Starships.Where(s => s.PlayerID == 0).ToList();
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
                        int playerColor = _mainScreen.Game.OrionGame.Players[0].ColorId;
                        int ycoord = 22 + (idx * 27);
                        int xCoord = 227;
                        pGuiService.drawRect(227, ycoord, 32, 24, Color.Black);
                        //pGuiService.drawImage(imgSvc.getSurface(shipArc, colors[playerColor] + shipSizes[shipSize], 0, offset), 227, ycoord);
                        pGuiService.drawImage(_images[idx].getCurrentFrame(), xCoord, ycoord);
                        int numDigits = Util.CountDigits(_fleetRef[i]);
                        
                        // numbers are magically delicious
                        Rectangle rect = ObjectPool.GetRectangle(258 - (numDigits * 5), 40 + (idx * 27), (5 * numDigits), 4);
                        pGuiService.drawString(string.Format(COUNT_STR, _fleetRef[i]), rect, FontEnum.font_0, FontPaletteEnum.FleetPanelYellow);
                        ObjectPool.RectanglePool.PutObject(rect);
                        idx++;
                    }
                }

                for (int i = 0; i < Controls.count(); i++)
                {
                    Controls.get(i).Draw(pTimer, pGuiService);
                }
            }
        }

        public override void mouseMoved(MouseMotionEventArgs pMbea)
        {
            for (int i = 0; i < Controls.count(); i++)
            {
                Controls.get(i).mouseMoved(pMbea);
            }
           
        }

        public override void mousePressed(MouseButtonEventArgs pMbea)
        {
            for (int i = 0; i < Controls.count(); i++)
            {
                Controls.get(i).mousePressed(pMbea);
            }
        }


        public override void mouseReleased(MouseButtonEventArgs pMbea)
        {
            for (int i = 0; i < Controls.count(); i++)
            {
                Controls.get(i).mouseReleased(pMbea);
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

        private void UpdateMoveButtons()
        {
            int count = 0;
            for(int i = 0; i < 6; i++)
            {
                if (_fleetRef[i] > 0)
                {
                    count++;
                }
            }
            for(int i = 0; i < 6; i++)
            {
                if (i < count)
                {
                    _moveButtons[i].Visible = true;
                    _moveButtons[i].Enabled = true;
                }
                else
                {
                    _moveButtons[i].Visible = false;
                    _moveButtons[i].Enabled = false;
                }
            }
        }

        private void UpdateInternalFleetCount()
        {
            for (int i = 0; i < 6; i++) _internalFleetCount[i] = _fleetRef[i];
        }
    }
}
