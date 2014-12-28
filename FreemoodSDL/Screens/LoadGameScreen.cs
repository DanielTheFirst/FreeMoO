using System;

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

        public override void update(FreemooTimer pTimer)
        {
            
        }

        public override void draw(FreemooTimer pTimer)
        {
            Game.Screen.drawImage(_loadScreenImage.getCurrentFrame(), 0, 0);
        }
    }
}
