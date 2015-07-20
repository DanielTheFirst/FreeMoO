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
            this.Controls.add(_continueBtn);

            _loadBtn = new MainMenuButton(129, 143, "Load Game");
            _loadBtn.Id = "Load";
            _loadBtn.Enabled = true;
            _loadBtn.ParentControl = this;
            this.Controls.add(_loadBtn);

            _newBtn = new MainMenuButton(131, 159, "New Game");
            _newBtn.Id = "New Button";
            _newBtn.Enabled = true;
            _newBtn.ParentControl = this;
            this.Controls.add(_newBtn);

            _quitBtn = new MainMenuButton(125, 175, "Quit To Dos");
            _quitBtn.Id = "Quit";
            _quitBtn.Enabled = true;
            _quitBtn.ParentControl = this;
            this.Controls.add(_quitBtn);

            base.start();
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
