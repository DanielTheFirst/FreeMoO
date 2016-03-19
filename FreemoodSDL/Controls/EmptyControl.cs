using System;
using System.Drawing;

using SdlDotNet.Input;

namespace FreeMoO.Controls
{
    public delegate void OnEmptyControlClick(EmptyControl sender, MouseButton btn);

    public class EmptyControl
        : AbstractControl
    {
        // just a rectangle on the screen.  nothing gets drawn or updated
        // but it does field mouse events.  possibly key events down the road
        public override void Update(FreemooTimer pTimer) { }
        public override void Draw(FreemooTimer pTimer, Service.GuiService pGuiService) { }

        private bool _mouseOver = false;
        private bool _mouseDown = false;
        //private Rectangle _rect;

        public event OnEmptyControlClick EmptyControlClickEvent;

        public EmptyControl(int x, int y, int w, int h)
        {
            this.X = x;
            this.Y = y;
            this.Width = w;
            Height = h;
            //_rect = new Rectangle(x, y, w, h);
        }

        public override void mouseMoved(SdlDotNet.Input.MouseMotionEventArgs pMbea)
        {
            //base.mouseMoved(pMbea);
            if (!BoundingRect.Contains(pMbea.Position))
            {
                _mouseOver = false;
            }
            else
            {
                _mouseOver = true;
            }
            
        }

        public override void mouseReleased(SdlDotNet.Input.MouseButtonEventArgs pMbea)
        {
            if (_mouseOver && _mouseDown)
            {
                if (EmptyControlClickEvent != null)
                {
                    EmptyControlClickEvent(this, pMbea.Button);
                }
            }
            _mouseDown = false;
        }

        public override void mousePressed(SdlDotNet.Input.MouseButtonEventArgs pMbea)
        {
            if (BoundingRect.Contains(pMbea.Position))
            {
                _mouseDown = true;
            }
        }
    }
}
