using System;
using System.Drawing;

using FreemooSDL.Collections;

namespace FreemooSDL.Controls
{

    public delegate void ProductionBarChangeDelegate(ProductionBarEventArgs arg);

    public class ProductionBar
        : AbstractControl
    {

        private const int PROD_BAR_OFFSET_X = 7;
        private const int PROD_BAR_OFFSET_Y = 3;

        public int LabelWidth { get; set; }
        public int MaxValue { get; set; }
        public int ArrowButtonWidth { get; set; }

        private int _value;
        public Boolean Locked { get; set; }

        private Color _lockedColor = Color.FromArgb(0xff, 0x86, 0x2c, 0x00); //862C00
        private Color _unlockedColor = Color.FromArgb(0xff, 0x2c, 0x88, 0x88);
        private Color _bgFillColor = Color.FromArgb(0x24, 0x1c, 0x14);


        private Rectangle upRect;
        private Rectangle downRect;
        private Rectangle prodRect;
        private Rectangle labelRect;

        ProductionBarEventArgs ProdEventArgs = new ProductionBarEventArgs();


        public event ProductionBarChangeDelegate ProductionBarChange;
        
        public int Value
        {
            get
            {
                return _value * 4;
            }
            set
            {
                _value = value / 4;
            }
        }
        public ProductionEnum ProdType { get; set; }


        public override void update(FreemooTimer pTimer)
        {
            
        }

        public override void draw(FreemooTimer pTimer, Service.GuiService pGuiService)
        {
            pGuiService.drawRect(X + LabelWidth + PROD_BAR_OFFSET_X, Y + PROD_BAR_OFFSET_Y, Width - LabelWidth - PROD_BAR_OFFSET_X - 7, 4, _bgFillColor);
            pGuiService.drawRect(X + LabelWidth + PROD_BAR_OFFSET_X, Y + PROD_BAR_OFFSET_Y + 1, _value, 3, Locked ? _lockedColor : _unlockedColor);
        }

        public override void mouseReleased(SdlDotNet.Input.MouseButtonEventArgs pMbea)
        {
 	        
        }

        public override void mousePressed(SdlDotNet.Input.MouseButtonEventArgs pMbea)
        {
 	         if (labelRect.Contains(pMbea.Position))
             {
                 this.Locked = !Locked;
             }

            if (upRect.Contains(pMbea.Position))
            {
                if (!this.Locked && _value < MaxValue)
                {
                    if (ProductionBarChange != null)
                    {
                        ProdEventArgs.ProdType = ProdType;
                        ProdEventArgs.ID = Id;
                        ProdEventArgs.Delta = 1;
                        ProdEventArgs.Sender = this;
                        ProductionBarChange(ProdEventArgs);
                    }
                    else
                    {
                        // assume not part of a group and just make the change as is
                        Value += 1;
                    }
                }
            }

            if (downRect.Contains(pMbea.Position))
            {
                if (!this.Locked && _value > 0)
                {
                    if (ProductionBarChange != null)
                    {
                        ProdEventArgs.ProdType = ProdType;
                        ProdEventArgs.ID = Id;
                        ProdEventArgs.Delta = -1;
                        ProdEventArgs.Sender = this;
                        ProductionBarChange(ProdEventArgs);
                    }
                    else
                    {
                        Value += 1;
                    }
                }
            }
        }

        public void PreCalculateRectangles()
        {
            Size s = ObjectPool.SizeObjPool.GetObject();
            Point p = ObjectPool.PointObjPool.GetObject();

            // only actually works if you call this after setting the positions
            prodRect = ObjectPool.RectanglePool.GetObject();
            p.X = X + LabelWidth + PROD_BAR_OFFSET_X;
            p.Y = Y + 3;
            prodRect.Location = p;
            s.Width = Width - LabelWidth - PROD_BAR_OFFSET_X - 7;
            s.Height = 4;
            prodRect.Size = s;

            downRect = ObjectPool.RectanglePool.GetObject();
            p.X = X + LabelWidth;
            p.Y = Y;
            downRect.Location = p;
            s.Width = 7;
            s.Height = Height;
            downRect.Size = s;


            upRect = ObjectPool.RectanglePool.GetObject();
            p.X = X + Width - 7;
            p.Y = Y;
            upRect.Location = p;
            upRect.Size = s; // same size as upRect

            labelRect = ObjectPool.RectanglePool.GetObject();
            p.X = X;
            p.Y = Y;
            labelRect.Location = p;
            s.Width = LabelWidth;
            s.Height = Height;
            labelRect.Size = s;

            ObjectPool.SizeObjPool.PutObject(s);
            ObjectPool.PointObjPool.PutObject(p);
        }

        public override void Release()
        {
            ObjectPool.RectanglePool.PutObject(prodRect);
            ObjectPool.RectanglePool.PutObject(upRect);
            ObjectPool.RectanglePool.PutObject(downRect);
            ObjectPool.RectanglePool.PutObject(labelRect);
            ObjectPool.ProductionBarPool.PutObject(this);
        }
    }

    public class ProductionBarEventArgs
        : EventArgs
    {
        public ProductionBarEventArgs()
            : base()
        {
        }
        public ProductionEnum ProdType;
        //public int NewValue;
        public int Delta;
        public string ID;
        public ProductionBar Sender;
    }
}
