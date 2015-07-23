﻿using System;

using FreemooSDL.Collections;
using FreemooSDL.Controls;
using FreemooSDL.Reverse;
using FreemooSDL.Service;

using SdlDotNet.Input;

namespace FreemooSDL.Screens
{
    class LoadGameScreen
        : AbstractScreen
    {

        private const int SFC_X_OFFSET = 129;
        private const int SFC_Y_OFFSET = 30;
        private const int SFC_WIDTH = 134;
        private const int SFC_HEIGHT = 18;

        FreemooImageInstance _loadScreenImage = null;
        ConfigMoo _saveFileNames = new ConfigMoo();
        private EmptyControl _loadBtn = null;
        private EmptyControl _cancelBtn = null;


        public LoadGameScreen(FreemooGame game)
            : base (game)
        {

        }

        public override void start()
        {
            _loadScreenImage = new FreemooImageInstance(ArchiveEnum.VORTEX, "LOAD_OPT", Game.Images);
            // cancel 273, 287
            // load 401, 287
            // height 322- 287
            // width 382 - 273

            _loadBtn = new EmptyControl(201, 144, 55, 18);
            _loadBtn.Id = "Load Button";
            _loadBtn.EmptyControlClickEvent += LoadClick;

            _cancelBtn = new EmptyControl(137, 144, 55, 18);
            _cancelBtn.Id = "Cancel Button";
            _cancelBtn.EmptyControlClickEvent += CancelClick;
            Controls.add(_cancelBtn);
            Controls.add(_loadBtn);

            InitSaveFileControls();
            base.start();
        }

        private void InitSaveFileControls()
        {
            bool firstActive = false;
            for (int i = 0; i < 6; i++)
            {
                SaveFileControl sfc = new SaveFileControl();
                if (!firstActive)
                {
                    sfc.ControlState = SaveFileControlState.Selected;
                    firstActive = true;
                }
                else
                {
                    sfc.ControlState = SaveFileControlState.Deselected;
                }
                sfc.Text.Text = _saveFileNames[i+1];
                sfc.Id = string.Format("SAVEFILE{0}", i);
                sfc.Width = SFC_WIDTH;
                sfc.Height = SFC_HEIGHT;
                sfc.X = SFC_X_OFFSET;
                sfc.Y = SFC_Y_OFFSET + (i * (SFC_HEIGHT));
                sfc.InitTextBox();
                sfc.InitIndicator(Game.Images);
                sfc.OnSelected += HandleSaveFileSelected;
                Controls.add(sfc);
            }
        }


        private void HandleSaveFileSelected(SaveFileControl sfCtrl)
        {
            foreach(var ctrl in this.Controls)
            {
                if (ctrl.Value is SaveFileControl)
                {
                    if (!ctrl.Value.Id.Equals(sfCtrl.Id))
                    {
                        var sfc = ctrl.Value as SaveFileControl;
                        sfc.ControlState = SaveFileControlState.Deselected;
                    }
                }
            }
        }

        private void LoadClick(EmptyControl sender, MouseButton args)
        {
            if (args == MouseButton.PrimaryButton)
            {

            }
        }

        private void CancelClick(EmptyControl sender, MouseButton args)
        {
            Game.popScreen();
        }

        public override void Update(FreemooTimer pTimer)
        {
            UpdateControls(pTimer);
        }

        public override void Draw(FreemooTimer pTimer, GuiService pGuiService)
        {
            pGuiService.drawImage(_loadScreenImage.getCurrentFrame(), 0, 0);

            foreach (var ctrl in Controls) ctrl.Value.Draw(pTimer, pGuiService);
        }
    }
}
