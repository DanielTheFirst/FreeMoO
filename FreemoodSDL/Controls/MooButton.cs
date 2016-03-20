using System;
using System.Collections.Generic;
using System.Drawing;

using FreeMoO.Service;

using SdlDotNet.Graphics;

namespace FreeMoO.Controls
{
    class MooButton 
        : AbstractControl
    {
        private bool _mouseDown = false;
        private bool _mouseOver = false;

        //protected Rectangle mBoundingRect;
        //private FreemooImage mButtonImage;
        private ImageInstance _buttonImage;
        private ImageService mImgServiceRef = null;

        public event EventHandler<EventArgs> Click;

        protected bool MouseOver
        {
            get
            {
                return _mouseOver;
            }
        }

        public MooButton()
            : base()
        {
            Visible = true;
            Enabled = true;
        }

        public MooButton(ArchiveEnum pButtonImageArchive, string pButtonImageIndex, ImageService pImgService, int px, int py)
            : base()
        {
            //mButtonImage = pImgService.getImage(pButtonImageArchive, pButtonImageIndex); //pImgService.Images[pButtonImageArchive, pButtonImageIndex];
            _buttonImage = new ImageInstance(pButtonImageArchive, pButtonImageIndex, pImgService);
            Surface s = _buttonImage.getCurrentFrame();
            //mBoundingRect = new Rectangle(px, py, s.Width,s.Height);
            X = px;
            Y = py;
            Width = s.Width;
            Height = s.Height;
            mImgServiceRef = pImgService;
            Visible = true;
            Enabled = true;
        }

        public override void Draw(Timer pTimer, Service.GuiService pGuiService)
        {
            if (this.Visible)
            {
                int frame = 0;
                if (_mouseDown && _mouseOver)
                {
                    //pGuiService.drawImage(mButtonImage[1], mBoundingRect.X, mBoundingRect.Y);
                    frame = 1;
                }
                else if (!Enabled)
                {
                    //pGuiService.drawImage(mButtonImage[0], mBoundingRect.X, mBoundingRect.Y);
                    frame = 1;
                }
                _buttonImage.CurrentFrameNum = frame;
                Surface surf = _buttonImage.getCurrentFrame(); //mImgServiceRef.getSurface(mButtonImage.Archive, mButtonImage.ImageIndex, frame);
                pGuiService.drawImage(surf, BoundingRect.X, BoundingRect.Y);
            }
        }

        public override void Update(Timer pTimer)
        {
        }

        public override void mouseMoved(SdlDotNet.Input.MouseMotionEventArgs pMbea)
        {
            if (this.Enabled)
            {
                if (BoundingRect.Contains(pMbea.Position))
                {
                    _mouseOver = true;
                }
                else
                {
                    _mouseOver = false;
                }
                base.mouseMoved(pMbea);
            }
        }

        public override void mousePressed(SdlDotNet.Input.MouseButtonEventArgs pMbea)
        {
            if (this.Enabled)
            {
                if (BoundingRect.Contains(pMbea.Position) && pMbea.Button == SdlDotNet.Input.MouseButton.PrimaryButton)
                {
                    _mouseDown = true;
                }
                base.mousePressed(pMbea);
            }
        }

        public override void mouseReleased(SdlDotNet.Input.MouseButtonEventArgs pMbea)
        {
            if (this.Enabled)
            {
                _mouseDown = false;
                if (BoundingRect.Contains(pMbea.Position) && pMbea.Button == SdlDotNet.Input.MouseButton.PrimaryButton)
                {
                    //mMouseDown = false;
                    // bubble up a click event
                    if (Click != null)
                    {
                        Click(this, new EventArgs());
                    }
                }
                base.mouseReleased(pMbea);
            }
        }
    }
}
