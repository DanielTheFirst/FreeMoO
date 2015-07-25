using System;
using System.Collections.Generic;

using FreemooSDL.Service;
using FreemooSDL.Collections;
using FreemooSDL.Controls;

using SdlDotNet.Input;

namespace FreemooSDL.Screens
{
    public abstract class AbstractScreen : IScreen , IControl
    {
        private FreemooGame mGame;
        private ControlCollection mControls = new ControlCollection();
        protected ScreenActionEventArgs _screenAction = new ScreenActionEventArgs();

        public FreemooGame Game
        {
            get
            {
                return mGame;
            }
        }

        public ControlCollection Controls
        {
            get
            {
                return mControls;
            }
        }

        public AbstractScreen(FreemooGame pGame)
        {
            mGame = pGame;
        }

        public virtual void start() { }

        public virtual void stop() { }

        public virtual void pause() { }

        public virtual void resume() { }

        public abstract void Update(FreemooTimer pTimer);

        public abstract void Draw(FreemooTimer pTimer, GuiService pGuiService);

        public virtual void keyPressed(KeyboardEventArgs pKea)
        {
            if (keyPressedEvent != null)
            {
                keyPressedEvent(this, pKea);
            }
            foreach (var ctrl in Controls)
            {
                ctrl.Value.keyPressed(pKea);
            }
        }

        public virtual void keyReleased(KeyboardEventArgs pKea)
        {
            if (keyReleasedEvent != null)
            {
                keyReleasedEvent(this, pKea);
            }
            foreach (var ctrl in Controls)
            {
                ctrl.Value.keyReleased(pKea);
            }
        }

        public virtual void mousePressed(MouseButtonEventArgs pMbea)
        {
            if (mousePressedEvent != null)
            {
                mousePressedEvent(this, pMbea);
            }
            foreach (var ctrl in Controls)
            {
                ctrl.Value.mousePressed(pMbea);
            }
        }

        public virtual void mouseReleased(MouseButtonEventArgs pMbea)
        {
            if (mouseReleasedEvent != null)
            {
                mouseReleasedEvent(this, pMbea);
            }
            foreach (var ctrl in Controls)
            {
                ctrl.Value.mouseReleased(pMbea);
            }
        }

        public virtual void mouseMoved(MouseMotionEventArgs pMbea)
        {
            if (mouseMovedEvent != null)
            {
                mouseMovedEvent(this, pMbea);
            }
            foreach (var ctrl in Controls)
            {
                ctrl.Value.mouseMoved(pMbea);
            }
        }

        public event EventHandler<MouseButtonEventArgs> mouseReleasedEvent;

        public event EventHandler<MouseButtonEventArgs> mousePressedEvent;

        public event EventHandler<MouseMotionEventArgs> mouseMovedEvent;

        public event EventHandler<KeyboardEventArgs> keyPressedEvent;

        public event EventHandler<KeyboardEventArgs> keyReleasedEvent;

        protected void UpdateControls(FreemooTimer timer)
        {
            List<int> toRemove = new List<int>();
            for (int i = 0; i < Controls.count(); i++)
            {
                if (Controls.get(i) != null)
                {
                    Controls.get(i).Update(timer);
                }
                else
                {
                    toRemove.Add(i);
                }
            }
            foreach (int i in toRemove)
            {
                Controls.remove(i);
            }
        }


        //public void draw(FreemooTimer pTimer, GuiService pGuiService)
        //{
        //    throw new NotImplementedException();
        //}

        public void Release()
        {
            throw new NotImplementedException();
        }

        public string Id
        {
            get { throw new NotImplementedException(); }
        }

        public IControl ParentControl
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int X
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Y
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Width
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Height
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
