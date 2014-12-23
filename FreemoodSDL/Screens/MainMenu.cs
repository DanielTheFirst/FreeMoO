using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreemooSDL.Screens
{
    class MainMenu
        : AbstractScreen
    {
        private FreemooImageInstance _bgImage = null;
        private FreemooImageInstance _title = null;

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

            base.start();
        }

        public override void update(FreemooTimer pTimer)
        {
            _bgImage.UpdateAnimation(pTimer);
        }

        public override void draw(FreemooTimer pTimer)
        {
            Game.Screen.drawImage(_bgImage.getCurrentFrame(), 0, 0);
            Game.Screen.drawImage(_title.getCurrentFrame(), 45, 11);
            // 119, 127
            Game.Screen.drawString("Continue Game", 119, 127, FontEnum.font_4, FontPaletteEnum.MainMenuBtnDisabled);
            Game.Screen.drawString("Load Game", 129, 143, FontEnum.font_4, FontPaletteEnum.MainMenuBtnActive);
            Game.Screen.drawString("New Game", 131, 159, FontEnum.font_4, FontPaletteEnum.Font4Colors);
            Game.Screen.drawString("Quit to Dos", 125, 175, FontEnum.font_4, FontPaletteEnum.Font4Colors);
        }
    }
}
