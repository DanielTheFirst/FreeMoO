using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FreemooSDL.Controls
{
    public delegate void OnSaveFileControlSelected(SaveFileControl ctrl);

    public enum SaveFileControlState
    {
        Disabled,
        Selected,
        Deselected
    }
    public class SaveFileControl
        : AbstractControl
    {
        // logic for the display of the save files on both load and save game screens
        // consists of the text box and the little button image at the left
        private TextBox _textBox = null;
        public TextBox Text
        {
            get
            {
                return _textBox;
            }
        }

        public SaveFileControlState ControlState { get; set; }

        public event OnSaveFileControlSelected OnSelected;

        public SaveFileControl()
        {
            _textBox = new TextBox();
        }

        public void InitTextBox()
        {
            // must be called after x,y,w,h is set for the save file control
            Text.X = this.X + 17;
            Text.Y = this.Y + 4;
            Text.Width = 109;
            Text.Height = 6;
            Text.Font = FontEnum.font_0;
            Text.UsePalette = true;
            /*Text.FontColor = Color.LightGray;
            if (this.ControlState == SaveFileControlState.Selected)
            {
                Text.FontColor = Color.LimeGreen;
            }
            else if (this.ControlState == SaveFileControlState.Disabled)
            {
                Text.FontColor = Color.Red;
            }*/
            Text.FontPalette = FontPaletteEnum.LoadScreenGray;
            Text.TextAlign = TextAlignEnum.Left;
            Text.TextValign = TextVAlignEnum.Bottom;
            Text.ReadOnly = true;
            Text.ParentControl = this;
            Text.HasFocus = false;
            if (this.ControlState == SaveFileControlState.Selected)
            {
                Text.HasFocus = true;
            }
        }

        public override void Draw(FreemooTimer pTimer, Service.GuiService pGuiService)
        {
            Text.Draw(pTimer, pGuiService);
        }

        public override void Update(FreemooTimer pTimer)
        {
            
        }
    }
}
