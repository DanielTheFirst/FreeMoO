using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using FreeMoO.Collections;

namespace FreeMoO.Controls
{
    public delegate void OnTextBoxFocus (TextBox txtBox);
    public delegate void OnTextBoxBlur (TextBox txtBox);

    public class TextBox
        : AbstractControl
    {
        public string Text { get; set; }
        public FontEnum Font { get; set; }
        public FontPaletteEnum FontPalette { get; set; }
        public Color FontColor { get; set; }
        public bool UsePalette { get; set; }
        public TextAlignEnum TextAlign { get; set; }
        public TextVAlignEnum TextValign { get; set; }

        public bool ReadOnly { get; set; }
        public bool HasFocus { get; set; }

        public event OnTextBoxBlur OnBlur;
        public event OnTextBoxFocus OnFocus;

        private Rectangle _boundRect;

        public override void Draw(Timer pTimer, Service.GuiService pGuiService)
        {
            /*Color c = Color.Red;
            if (HasFocus)
            {
                c = Color.Green;
            }*/
            
            RecalcBoundingRect();

            //pGuiService.drawString(Text, _boundRect, Font, c, TextAlign, TextValign);
            if (UsePalette)
            {
                pGuiService.drawString(Text, _boundRect, Font, FontPalette, TextAlign, TextValign);
            }
            else
            {
                pGuiService.drawString(Text, _boundRect, Font, FontColor, TextAlign, TextValign);
            }
        }

        public override void Update(Timer pTimer)
        {
            
        }

        public override void Release()
        {
            if (_boundRect != null)
            {
                ObjectPool.RectanglePool.PutObject(_boundRect);
            }
            base.Release();
        }

        private void RecalcBoundingRect()
        {
            if (_boundRect == null)
            {
                _boundRect = ObjectPool.RectanglePool.GetObject();
            }
            _boundRect.X = this.X;
            _boundRect.Y = this.Y;
            _boundRect.Width = this.Width;
            _boundRect.Height = this.Height;
        }
    }
}
