using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FreemooSDL.Controls;
using FreemooSDL.Game;
using FreemooSDL.Service;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Input;
using SdlDotNet.Audio;

namespace FreemooSDL.Screens
{
    public class OpeningMovie
        : AbstractScreen
    {
        private FreemooImageInstance _introMovie1 = null;

        private List<FreemooImageInstance> _introMovieSequence = null;
        private string[] _intro1Movies = { "TITLE", "BOMBNEW", "LAUNCHER", "CITYBOMB" };
        private string[] _intro2Movies = { "EXPLODE","WARP", "CREDITS" };
        private int _currPicIdx = 0;


        public OpeningMovie(FreemooGame game)
            : base(game)
        {
            

        }

        public override void start()
        {
            //if (_introMovie1 == null)
            //{
            //    //_introMovie1 = new FreemooImageInstance(ArchiveEnum.INTRO, "TITLE", Game.Images);
            //    _introMovie1 = new FreemooImageInstance(ArchiveEnum.INTRO, "LAUNCHER", Game.Images);
            //}
            //_introMovie1.Animate = true;
            //_introMovie1.AnimationTimer = 0;
            //_introMovie1.ResetAnimation();
            if (_introMovieSequence == null)
            {
                BuildMovie();
                _currPicIdx = 0;
            }
            base.start();
        }

        public override void stop()
        {
            Game.Images.ResetImageCache();
            base.stop();
        }



        private void BuildMovie()
        {
            _introMovieSequence = new List<FreemooImageInstance>();
            foreach (string m in _intro1Movies)
            {
                FreemooImageInstance fii = new FreemooImageInstance(ArchiveEnum.INTRO, m, Game.Images);
                fii.Animate = true;
                fii.AnimationTimer = 0;
                fii.ResetAnimation();
                _introMovieSequence.Add(fii);
            }
            foreach (string m in _intro2Movies)
            {
                FreemooImageInstance fii = new FreemooImageInstance(ArchiveEnum.INTRO2, m, Game.Images);
                fii.Animate = true;
                fii.AnimationTimer = 0;
                fii.ResetAnimation();
                _introMovieSequence.Add(fii);
            }
        }

        public override void Draw(FreemooTimer pTimer, GuiService guiSvc)
        {
            ImageService imgSvc = Game.Images;
            //GuiService guiSvc = Game.Screen;

            //Surface surf = imgSvc.getSurface(ArchiveEnum.LANDING, "LANDINF1", 0);
            //guiSvc.drawImage(surf, 0, 0);
            //Surface surf = _introMovie1.getCurrentFrame();
            Surface surf = _introMovieSequence[_currPicIdx].getCurrentFrame();
            guiSvc.drawImage(surf, 0, 0);
        }

        public override void Update(FreemooTimer pTimer)
        {
            //_introMovie1.AnimationTimer  += (long)pTimer.MillisecondsElapsed;
            //if (_introMovie1.AnimationTimer > _introMovie1.FrameRate * FreemooConstants.FRAMERATE_ADJUST)
            //{
            //    _introMovie1.gotoNextFrame();
            //    _introMovie1.AnimationTimer = 0;
            //}
            _introMovieSequence[_currPicIdx].AnimationTimer += (long)pTimer.MillisecondsElapsed;
            if (_introMovieSequence[_currPicIdx].AnimationTimer > _introMovieSequence[_currPicIdx].FrameRate * FreemooConstants.FRAMERATE_ADJUST)
            {
                if (_introMovieSequence[_currPicIdx].CurrentFrameNum >= _introMovieSequence[_currPicIdx].FrameCount-1)
                {
                    _currPicIdx = _currPicIdx < _introMovieSequence.Count - 1 ? _currPicIdx + 1 : 0;
                }
                else
                {
                    _introMovieSequence[_currPicIdx].gotoNextFrame();
                }
                _introMovieSequence[_currPicIdx].AnimationTimer = 0;
            }
        }
    }
}
