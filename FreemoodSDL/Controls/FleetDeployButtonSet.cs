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
    public class FleetDeployButtonSet
        : AbstractControl
    {
        private MooButton _removeAllBtn = null;
        private MooButton _removeOneBtn = null;
        private MooButton _addOneBtn = null;
        private MooButton _addAllBtn = null;
        private MainScreen _parentScreen = null;
        private int _index = 0;

        public FleetDeployButtonSet(MainScreen parent, int index)
            : base()
        {
            _parentScreen = parent;
            _index = index;
            BuildButtons();
        }

        private void BuildButtons()
        {
            int ycoord = 35 + (_index * 26);
            _removeAllBtn = new MooButton(ArchiveEnum.STARMAP, "MOVE_BUT none", _parentScreen.Game.Images , 265, ycoord);
            _removeAllBtn.Id = Id + "_none";
            Controls.add(_removeAllBtn);
            _removeOneBtn = new MooButton(ArchiveEnum.STARMAP, "MOVE_BUT minus", _parentScreen.Game.Images, 277, ycoord);
            _removeOneBtn.Id = Id + "_minus";
            Controls.add(_removeOneBtn);
            _addOneBtn = new MooButton(ArchiveEnum.STARMAP, "MOVE_BUT plus", _parentScreen.Game.Images, 288, ycoord);
            _addOneBtn.Id = Id + "_plus";
            Controls.add(_addOneBtn);
            _addAllBtn = new MooButton(ArchiveEnum.STARMAP, "MOVE_BUT all", _parentScreen.Game.Images, 299, ycoord);
            _addAllBtn.Id = Id + "_all";
            Controls.add(_addAllBtn);
        }

        public override void Update(FreemooTimer pTimer)
        {
            
        }

        public override void Draw(FreemooTimer pTimer, GuiService pGuiService)
        {
            if (this.Visible)
            {
                for (int i = 0; i < Controls.count(); i++)
                {
                    Controls.get(i).Draw(pTimer, pGuiService);
                }
            }
        }

        public override void mouseMoved(MouseMotionEventArgs pMbea)
        {
            for(int i = 0; this.Enabled && i < Controls.count(); i++)
            {
                Controls.get(i).mouseMoved(pMbea);
            }
        }

        public override void mouseReleased(MouseButtonEventArgs pMbea)
        {
            for (int i = 0; this.Enabled && i < Controls.count(); i++) Controls.get(i).mouseReleased(pMbea);
        }

        public override void mousePressed(MouseButtonEventArgs pMbea)
        {
            for (int i = 0; this.Enabled && i < Controls.count(); i++) Controls.get(i).mousePressed(pMbea);
        }
    }
}
