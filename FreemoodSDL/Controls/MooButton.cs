using System;
using System.Collections.Generic;
using System.Drawing;

using FreemooSDL.Service;

using SdlDotNet.Graphics;

namespace FreemooSDL.Controls
{
    class MooButton 
        : AbstractControl
    {
        private bool mMouseDown = false;
        private bool mMouseOver = false;

        private Rectangle mBoundingRect;
        private FreemooImage mButtonImage;
        private ImageService mImgServiceRef = null;

        public event EventHandler<EventArgs> Click;

        public MooButton(ArchiveEnum pButtonImageArchive, string pButtonImageIndex, ImageService pImgService, int px, int py)
            : base()
        {
            mButtonImage = pImgService.getImage(pButtonImageArchive, pButtonImageIndex); //pImgService.Images[pButtonImageArchive, pButtonImageIndex];
            Surface s = mButtonImage[0];
            mBoundingRect = new Rectangle(px, py, s.Width,s.Height);
            mImgServiceRef = pImgService;
        }

        public override void draw(FreemooTimer pTimer, Service.GuiService pGuiService)
        {
            int frame = 0;
            if (mMouseDown && mMouseOver)
            {
                //pGuiService.drawImage(mButtonImage[1], mBoundingRect.X, mBoundingRect.Y);
                frame = 1;
            }
            else
            {
                //pGuiService.drawImage(mButtonImage[0], mBoundingRect.X, mBoundingRect.Y);
            }
            Surface surf = mImgServiceRef.getSurface(mButtonImage.Archive, mButtonImage.ImageIndex, frame);
            pGuiService.drawImage(surf, mBoundingRect.X, mBoundingRect.Y);
        }

        public override void update(FreemooTimer pTimer)
        {
        }

        public override void mouseMoved(SdlDotNet.Input.MouseMotionEventArgs pMbea)
        {
            if (mBoundingRect.Contains(pMbea.Position))
            {
                mMouseOver = true;
            }
            else
            {
                mMouseOver = false;
            }
            base.mouseMoved(pMbea);
        }

        public override void mousePressed(SdlDotNet.Input.MouseButtonEventArgs pMbea)
        {
            if (mBoundingRect.Contains(pMbea.Position) && pMbea.Button == SdlDotNet.Input.MouseButton.PrimaryButton)
            {
                mMouseDown = true;
            }
            base.mousePressed(pMbea);
        }

        public override void mouseReleased(SdlDotNet.Input.MouseButtonEventArgs pMbea)
        {
            mMouseDown = false;
            if (mBoundingRect.Contains(pMbea.Position) && pMbea.Button == SdlDotNet.Input.MouseButton.PrimaryButton)
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
