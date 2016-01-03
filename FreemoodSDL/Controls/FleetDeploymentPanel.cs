﻿using System;
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
    public class FleetDeploymentPanel
        : AbstractControl
    {
        private MainScreen _mainScreen = null;
        private Fleet _fleetRef = null;
        private MooButton _cancelBtn = null;
        private MooButton _acceptBtn = null;

        public FleetDeploymentPanel(MainScreen screen)
            :base()
        {
            _mainScreen = screen;

            Id = "FleetDeploy";
            BuildButtons();
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
                        pGuiService.drawRect(227, ycoord, 32, 24, Color.Black);
                        pGuiService.drawImage(imgSvc.getSurface(shipArc, colors[playerColor] + shipSizes[shipSize], 0, offset), 227, ycoord);

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
    }
}
