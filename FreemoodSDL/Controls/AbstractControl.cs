using System;

using FreemooSDL.Collections;

using SdlDotNet.Input;

namespace FreemooSDL.Controls
{
    public abstract class AbstractControl : IControl
    {
        private string mId;
        private ControlCollection mControls = new ControlCollection();

        public string Id
        {
            get
            {
                return mId;
            }
            set
            {
                mId = value;
            }
        }

        public ControlCollection Controls
        {
            get
            {
                return mControls;
            }
        }

        public IControl ParentControl { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public abstract void update(FreemooTimer pTimer);
        public abstract void draw(FreemooTimer pTimer, FreemooSDL.Service.GuiService pGuiService);
        public void Release() { }

        public virtual void keyPressed(KeyboardEventArgs pKea) { }

        public virtual void keyReleased(KeyboardEventArgs pKea) { }

        public virtual void mousePressed(MouseButtonEventArgs pMbea) { }

        public virtual void mouseReleased(MouseButtonEventArgs pMbea) { }

        public virtual void mouseMoved(MouseMotionEventArgs pMbea) { }

        public event EventHandler<MouseButtonEventArgs> mouseReleasedEvent;

        public event EventHandler<MouseButtonEventArgs> mousePressedEvent;

        public event EventHandler<MouseMotionEventArgs> mouseMovedEvent;

        public event EventHandler<KeyboardEventArgs> keyPressedEvent;

        public event EventHandler<KeyboardEventArgs> keyReleasedEvent;

    }
}
