using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FreemooSDL.Controls;
using FreemooSDL.Service;

namespace FreemooSDL.Screens
{
    class MainMenu
        : AbstractScreen
    {
        private FreemooImageInstance _bgImage = null;
        private FreemooImageInstance _title = null;

        private MainMenuButton _continueBtn = null;
        private MainMenuButton _newBtn = null;
        private MainMenuButton _loadBtn = null;
        private MainMenuButton _quitBtn = null;

        public MainMenu(FreemooGame game)
            : base(game)
        {
            
        }

        public override void start()
        {
            _bgImage = new FreemooImageInstance(ArchiveEnum.VORTEX, "VORTEX", Game.Images);
            _bgImage.ResetAnimation();
            _bgImage.Animate = true;
            _bgImage.AnimateLoop = true;

            _title = new FreemooImageInstance(ArchiveEnum.VORTEX, "STARLORD", Game.Images);

            _continueBtn = new MainMenuButton(119, 127, "Continue Game");
            _continueBtn.Id = "Continue";
            _continueBtn.Enabled = false;
            _continueBtn.ParentControl = this;
            _continueBtn.Click += ContinueButtonClick;
            this.Controls.add(_continueBtn);

            _loadBtn = new MainMenuButton(129, 143, "Load Game");
            _loadBtn.Id = "Load";
            _loadBtn.Enabled = true;
            _loadBtn.ParentControl = this;
            _loadBtn.Click += LoadButtonClick;
            this.Controls.add(_loadBtn);

            _newBtn = new MainMenuButton(131, 159, "New Game");
            _newBtn.Id = "New Button";
            _newBtn.Enabled = true;
            _newBtn.ParentControl = this;
            _newBtn.Click += NewButtonClick;
            this.Controls.add(_newBtn);

            _quitBtn = new MainMenuButton(125, 175, "Quit To Dos");
            _quitBtn.Id = "Quit";
            _quitBtn.Enabled = true;
            _quitBtn.ParentControl = this;
            _quitBtn.Click += QuitButtonClick;
            this.Controls.add(_quitBtn);

            base.start();
        }

        private void LoadButtonClick(object sender, EventArgs args)
        {
            if (_loadBtn.Enabled)
            {
                //this.Game.pushScreen(ScreenEnum.LoadGameScreen);
                _screenAction.ScreenAction = ScreenActionEnum.Push;
                _screenAction.NextScreen = ScreenEnum.LoadGameScreen;
                Game.QueueScreenAction(_screenAction);
            }
        }

        private void NewButtonClick(object sender, EventArgs args)
        {
            if (_newBtn.Enabled)
            {
                //this.Game.changeScreen(ScreenEnum.)
            }
        }

        private void QuitButtonClick(object sender, EventArgs args)
        {
            // provably going to need to write into FreemooGame a quit function, or have it subscribe to this event or something
            //Game.
            _screenAction.NextScreen = ScreenEnum.None;
            _screenAction.ScreenAction = ScreenActionEnum.Quit;
            Game.QueueScreenAction(_screenAction);
        }

        private void ContinueButtonClick(object sender, EventArgs args)
        {

        }

        public override void Update(FreemooTimer pTimer)
        {
            _bgImage.UpdateAnimation(pTimer);
        }

        public override void Draw(FreemooTimer pTimer, GuiService pGuiService)
        {
            Game.Screen.drawImage(_bgImage.getCurrentFrame(), 0, 0);
            Game.Screen.drawImage(_title.getCurrentFrame(), 45, 11);
            // 119, 127
            /*Game.Screen.drawString("Continue Game", 119, 127, FontEnum.font_4, FontPaletteEnum.MainMenuBtnDisabled);
            Game.Screen.drawString("Load Game", 129, 143, FontEnum.font_4, FontPaletteEnum.MainMenuBtnActive);
            Game.Screen.drawString("New Game", 131, 159, FontEnum.font_4, FontPaletteEnum.Font4Colors);
            Game.Screen.drawString("Quit to Dos", 125, 175, FontEnum.font_4, FontPaletteEnum.Font4Colors);*/
            
            foreach(var ctrl in this.Controls)
            {
                ctrl.Value.Draw(pTimer, pGuiService);
            }
        }
    }
}
