using System;
using System.Drawing;

using FreeMoO.Collections;
using FreeMoO.Controls;
using FreeMoO.Service;
using SdlDotNet.Graphics;
using SdlDotNet.Core;
using SdlDotNet.Input;

namespace FreeMoO.Screens
{
    class LoadingScreen
        : AbstractScreen
    {
        string LoadingText = "Loading Master Of Orion...";
        int textR = 0x00;
        int textG = 0x21;
        int textB = 0x45;
        int textMaxR = 0x65;
        double _fadeTimer = 0;
        bool _fadeIn = true;
        int _fadeMode = 0;
        private EmptyControl _mouseEvtControl = null;

        const int FADE_RATE = 30;
        const int FADE_HOLD_LENGTH = 2000;
        const int FADE_MODE_IN = 0;
        const int FADE_MODE_HIGH = 1;
        const int FADE_MODE_OUT = 2;
        const int FADE_MODE_LOW = 3;

        public LoadingScreen(FreemooGame game)
            : base(game)
        {

        }

        public override void start()
        {
            _mouseEvtControl = new EmptyControl(0, 0, 320, 200);
            _mouseEvtControl.EmptyControlClickEvent += _mouseEvtControl_EmptyControlClickEvent;
            _mouseEvtControl.Id = "MouseEventControl";
            Controls.add(_mouseEvtControl);
            base.start();
        }

        void _mouseEvtControl_EmptyControlClickEvent(EmptyControl sender, MouseButton btn)
        {
            if (btn == MouseButton.PrimaryButton)
            {
                _fadeTimer = 0;
                _fadeMode = FADE_MODE_OUT;
            }
        }

        //  I'm starting to think that the abstract control shouldn't just have empty functions and
        // instead should pump input events down whether anyone uses them or not.
        public override void mousePressed(MouseButtonEventArgs pMbea)
        {
            foreach (var ctrl in Controls)
            {
                ctrl.Value.mousePressed(pMbea);
            }
            base.mousePressed(pMbea);
        }

        public override void mouseMoved(MouseMotionEventArgs pMbea)
        {
            foreach (var ctrl in Controls)
            {
                ctrl.Value.mouseMoved(pMbea);
            }
            base.mouseMoved(pMbea);
        }

        public override void mouseReleased(MouseButtonEventArgs pMbea)
        {
            foreach (var ctrl in Controls)
            {
                ctrl.Value.mouseReleased(pMbea);
            }
            base.mouseReleased(pMbea);
        }

        public override void Update(FreemooTimer pTimer)
        {
            //throw new NotImplementedException();
            _fadeTimer += pTimer.MillisecondsElapsed;

            switch (_fadeMode)
            {
                case FADE_MODE_IN:
                    if (_fadeTimer >= FADE_RATE)
                    {
                        textR++;
                        textG++;
                        textB++;
                        _fadeTimer = 0;
                        if (textR >= textMaxR)
                        {
                            _fadeMode = FADE_MODE_HIGH;
                        }
                    }
                    break;
                case FADE_MODE_HIGH:
                    if (_fadeTimer > FADE_HOLD_LENGTH)
                    {
                        _fadeMode = FADE_MODE_OUT;
                        _fadeTimer = 0;
                    }
                    break;
                case FADE_MODE_OUT:
                    if (_fadeTimer >= FADE_RATE)
                    {
                        textR--;
                        textG--;
                        textB--;
                        _fadeTimer = 0;
                        if (textR <= 0 || textG <= 0 || textB <= 0)
                        {
                            _fadeMode = FADE_MODE_LOW;
                        }
                    }
                    break;
                case FADE_MODE_LOW:
                    if (_fadeTimer > FADE_HOLD_LENGTH)
                    {
                        //Game.changeScreen(ScreenEnum.OpeningMovie);
                        _screenAction.ScreenAction = ScreenActionEnum.Change;
                        _screenAction.NextScreen = ScreenEnum.OpeningMovie;
                        Game.QueueScreenAction(_screenAction);
                    }
                    break;
            }
        }

        public override void Draw(FreemooTimer pTimer, GuiService pGuiService)
        {
            ImageService img = Game.Images;
            GuiService gui = Game.Screen;

            

            /*if (_fadeMode == 0 && textR < textMaxR && _fadeTimer >= FADE_RATE)
            {
                textR++;
                textG++;
                textB++;
                _fadeTimer = 0;
                if (textR >= textMaxR)
                {
                    _fadeMode = FADE_MODE_HIGH
                }
            }
            else if (!_fadeIn && textR > 0 && _fadeTimer >= FADE_RATE)
            {
                textR--;
                textG--;
                textB--;
                _fadeTimer = 0;
                if (textR <= 0)
                {
                    Game.changeScreen(ScreenEnum.OpeningMovie);
                }
            }*/

            Color currTextColor = Color.FromArgb(textR, textG, textB);

            pGuiService.drawString(LoadingText, 0, 0, FontEnum.font_1, currTextColor);

        }
    }
}
