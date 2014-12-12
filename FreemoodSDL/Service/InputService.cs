using System;
using System.Collections.Generic;
using System.Drawing;

using SdlDotNet.Input;
using SdlDotNet.Core;
using Tao.Sdl;

namespace FreemooSDL.Service
{

    public struct ButtonState
    {
        public bool previouseState;
        public bool currState;
    }

    // polls the sdl input events
    // and records the state of the keyboard and mouse at the beginning of each frame
    public class InputService
    {
        private FreemooGame mGame = null;
        private KeyboardState mKeyState = new KeyboardState();
        private KeyboardState mPreviousKeystate = new KeyboardState();
        private Point mMousePosition = new Point(0, 0);
        private Point mPrevMousePosition = new Point(0, 0);
        private bool mQuit = false;
        private int mScaleRatio = 4;

        

        Dictionary<MouseButton, ButtonState> mMouseButtonState = new Dictionary<MouseButton, ButtonState>();

        public bool Quit
        {
            get
            {
                return mQuit;
            }
        }
        

        public InputService(FreemooGame pGame)
        {
            mGame = pGame;

            mMouseButtonState.Add(MouseButton.PrimaryButton, new ButtonState());
            mMouseButtonState.Add(MouseButton.SecondaryButton, new ButtonState());
        }

        public void update()
        {
            //Console.WriteLine("mouse pos = " + Mouse.MousePosition);

            // need to pull the stretching ratio from the config so we know what to divide the coords by
            // hardcode to 4 for now cause that's what I'm using in dev
            mScaleRatio = 4;

            mPreviousKeystate = mKeyState;
            mKeyState.Update();

            int x = 0, y = 0;
            int mouseVal = (int)Sdl.SDL_GetMouseState(out x, out y);
            int leftmouse = mouseVal & (int)Sdl.SDL_BUTTON(Sdl.SDL_BUTTON_LEFT);
            setMouseState(MouseButton.PrimaryButton, leftmouse != 0);
            int rightmouse = mouseVal & (int)Sdl.SDL_BUTTON(Sdl.SDL_BUTTON_RIGHT);
            setMouseState(MouseButton.SecondaryButton, rightmouse != 0);
            mPrevMousePosition.X = mMousePosition.X;
            mPrevMousePosition.Y = mMousePosition.Y;
            mMousePosition = new Point(x, y);
            mQuit = Sdl.SDL_QuitRequested() != 0;
            Sdl.SDL_PumpEvents();
        }

        private void setMouseState(MouseButton pBtn, bool newCurrState)
        {
            ButtonState bs = mMouseButtonState[pBtn];
            bs.previouseState = bs.currState;
            bs.currState = newCurrState;
            mMouseButtonState[pBtn] = bs;
        }

        public bool this[Key pKey]
        {
            get
            {
                return mKeyState.IsKeyPressed(pKey);
            }
        }

        public bool released(Key pKey)
        {
            return mPreviousKeystate[pKey] == true && mKeyState[pKey] == false;
        }

        public int MouseX
        {
            get
            {
                return mMousePosition.X / 4;
            }
        }

        public int MouseY
        {
            get
            {
                return mMousePosition.Y / 4;
            }
        }

        public Point MousePos
        {
            get
            {
                return new Point(this.MouseX, this.MouseY);
            }
        }

        public ButtonState this[MouseButton pMb]
        {
            get
            {
                return mMouseButtonState[pMb];
            }
        }

        public bool isClick(MouseButton pMb)
        {
            return mMouseButtonState[pMb].previouseState && !mMouseButtonState[pMb].currState;
        }

    }
}
