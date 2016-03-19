using System;
using System.Drawing;

using FreeMoO.Collections;

using SdlDotNet.Input;

namespace FreeMoO.Controls
{
    public abstract class AbstractControl : IControl
    {
        private string mId;
        private ControlCollection mControls = new ControlCollection();

        public AbstractControl()
        {
            _boundingRect = ObjectPool.RectanglePool.GetObject();
            SafeRemove = false;
        }

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

        public bool Enabled { get; set; }

        public bool Visible { get; set; }

        private Rectangle _boundingRect;
        public Rectangle BoundingRect
        {
            get
            {
                return _boundingRect;
            }
        }
        private int _x;
        private int _y;
        private int _width;
        private int _height;
        public int X 
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
                _boundingRect.X = _x;
            }
        }
        public int Y 
        { 
            get
            {
                return _y;
            }
            set
            {
                _y = value;
                _boundingRect.Y = _y;
            }
        }
        public int Width 
        { 
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                _boundingRect.Width = _width;
            }
        }
        public int Height 
        { 
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                _boundingRect.Height = _height;
            }
        }

        public bool SafeRemove { get; set; }

        public abstract void Update(Timer pTimer);
        public abstract void Draw(Timer pTimer, FreeMoO.Service.GuiService pGuiService);
        public virtual void Release() 
        {
            ObjectPool.RectanglePool.PutObject(_boundingRect);
        }

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
