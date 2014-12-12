using System;
using System.Collections.Generic;

using FreemooSDL.Service;
using FreemooSDL.Collections;

using SdlDotNet.Input;

namespace FreemooSDL.Screens
{
    public abstract class AbstractScreen : IScreen 
    {
        private FreemooGame mGame;
        private ControlCollection mControls = new ControlCollection();

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

        public abstract void update(FreemooTimer pTimer);

        public abstract void draw(FreemooTimer pTimer);

        public virtual void keyPressed(KeyboardEventArgs pKea)
        {
            if (keyPressedEvent != null)
            {
                keyPressedEvent(this, pKea);
            }
        }

        public virtual void keyReleased(KeyboardEventArgs pKea)
        {
            if (keyReleasedEvent != null)
            {
                keyReleasedEvent(this, pKea);
            }
        }

        public virtual void mousePressed(MouseButtonEventArgs pMbea)
        {
            if (mousePressedEvent != null)
            {
                mousePressedEvent(this, pMbea);
            }
        }

        public virtual void mouseReleased(MouseButtonEventArgs pMbea)
        {
            if (mouseReleasedEvent != null)
            {
                mouseReleasedEvent(this, pMbea);
            }
        }

        public virtual void mouseMoved(MouseMotionEventArgs pMbea)
        {
            if (mouseMovedEvent != null)
            {
                mouseMovedEvent(this, pMbea);
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
                    Controls.get(i).update(timer);
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
    }
}
