using System;
using System.Drawing;

namespace FreemooSDL.Controls
{
    public class ProductionBar
        : AbstractControl
    {

        private const int PROD_BAR_OFFSET_X = 7;
        private const int PROD_BAR_OFFSET_Y = 3;

        public int LabelWidth { get; set; }
        public int MaxValue { get; set; }

        private int _value;
        public Boolean Locked { get; set; }

        private Color _lockedColor = Color.FromArgb(0xff, 0x86, 0x2c, 0x00); //862C00
        private Color _unlockedColor = Color.FromArgb(0xff, 0x2c, 0x88, 0x88);
        private Color _bgFillColor = Color.FromArgb(0x24, 0x1c, 0x14);

        
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
 	            base.mouseReleased(pMbea);
        }

        public override void mousePressed(SdlDotNet.Input.MouseButtonEventArgs pMbea)
        {
 	         base.mousePressed(pMbea);
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
    }
}
