using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using FreemooSDL.Collections;
using FreemooSDL.Service;

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

        public int SaveFileIndex { get; set; }

        private FreemooImageInstance _indicatorLightGreen = null;
        private FreemooImageInstance _indicatorLightGray = null;

        private SaveFileControlState _sfcState = SaveFileControlState.Deselected;
        public SaveFileControlState ControlState 
        { 
            get
            {
                return _sfcState;
            }
            set
            {
                _sfcState = value;
                Text.HasFocus = _sfcState == SaveFileControlState.Selected;
                if (!Text.HasFocus)
                {
                    Text.FontPalette = FontPaletteEnum.LoadScreenGray;
                }
                else
                {
                    Text.FontPalette = FontPaletteEnum.LoadScreenGreen;
                }
            }
        }

        private bool _calcRect = false;

        public event OnSaveFileControlSelected OnSelected;

        public SaveFileControl()
        {
            _textBox = new TextBox();

        }

        public void InitTextBox()
        {
            // must be called after x,y,w,h is set for the save file control
            Text.X = this.X + 20;
            Text.Y = this.Y + 5;
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
                Text.FontPalette = FontPaletteEnum.LoadScreenGreen;
            }
        }

        public void InitIndicator(ImageService imgRef)
        {
            _indicatorLightGreen = new FreemooImageInstance(ArchiveEnum.VORTEX, "LOAD     green", imgRef);
            _indicatorLightGray = new FreemooImageInstance(ArchiveEnum.VORTEX, "LOAD     grey", imgRef);
        }

        public override void Draw(FreemooTimer pTimer, Service.GuiService pGuiService)
        {
            Text.Draw(pTimer, pGuiService);

            if (this.ControlState == SaveFileControlState.Selected)
            {
                pGuiService.drawImage(_indicatorLightGreen.getCurrentFrame(), this.X + 5, this.Y + 3);
            }
            else if (this.ControlState == SaveFileControlState.Deselected)
            {
                pGuiService.drawImage(_indicatorLightGray.getCurrentFrame(), this.X + 5, this.Y + 3);
            }
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Update(FreemooTimer pTimer)
        {
        }

        public override void mousePressed(SdlDotNet.Input.MouseButtonEventArgs pMbea)
        {
            if (!(this.ControlState == SaveFileControlState.Selected) && !(this.ControlState == SaveFileControlState.Disabled))
            {
                if (BoundingRect.Contains(pMbea.Position))
                {
                    this.ControlState = SaveFileControlState.Selected;
                    if (OnSelected != null)
                    {
                        OnSelected(this);
                    }
                }
                
            }
            base.mousePressed(pMbea);
        }
    }
}
