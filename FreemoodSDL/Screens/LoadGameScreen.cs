using System;

using FreemooSDL.Service;

namespace FreemooSDL.Screens
{
    class LoadGameScreen
        : AbstractScreen
    {

        FreemooImageInstance _loadScreenImage = null;

        public LoadGameScreen(FreemooGame game)
            : base (game)
        {

        }

        public override void start()
        {
            _loadScreenImage = new FreemooImageInstance(ArchiveEnum.VORTEX, "LOAD_OPT", Game.Images);
            base.start();
        }

        public override void Update(FreemooTimer pTimer)
        {
            
        }

        public override void Draw(FreemooTimer pTimer, GuiService pGuiService)
        {
            pGuiService.drawImage(_loadScreenImage.getCurrentFrame(), 0, 0);
        }
    }
}
