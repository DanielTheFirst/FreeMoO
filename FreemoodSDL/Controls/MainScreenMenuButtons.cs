using System;
using System.Collections.Generic;
using System.Drawing;
using FreemooSDL.Screens;
using FreemooSDL.Service;
using FreemooSDL;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Input;

namespace FreemooSDL.Controls
{
    public class MainScreenMenuButtons
        : AbstractControl
    {
        public event EventHandler<EventArgs> Click;

        private Rectangle mRect;
        private bool mHover = false;
        private bool mClick = false;
        private MainScreen mParentScreen = null;
        private MenuButtonEnum mButtonEnum;

        public MenuButtonEnum ButtonType
        {
            get
            {
                return mButtonEnum;
            }
        }

        public MainScreenMenuButtons(MainScreen pParent, string pId, Rectangle pRect, MenuButtonEnum pType)
            : base()
        {
            mParentScreen = pParent;
            this.Id = pId;  // in this class, Id is the text to display
            mRect = pRect;
            mButtonEnum = pType;
        }

        public override void Draw(FreemooTimer pTimer, GuiService pGuiService)
        {
            Color drawColor = mHover ? Color.FromArgb(0xefefef) : Color.FromArgb(0xa6a6a6);
            mParentScreen.Game.Screen.drawString(Id, mRect, FontEnum.font_5, drawColor);
        }

        public override void Update(FreemooTimer pTimer)
        {
            //throw new NotImplementedException();
        }

        public override void mouseMoved(MouseMotionEventArgs pMbea)
        {
            if (mRect.Contains(pMbea.Position))
            {
                mHover = true;
            }
            else
            {
                mHover = false;
            }
            base.mouseMoved(pMbea);
        }

        public override void mousePressed(MouseButtonEventArgs pMbea)
        {
            if (mRect.Contains(pMbea.Position))
            {
                mClick = true;
            }
            base.mousePressed(pMbea);
        }

        public override void mouseReleased(MouseButtonEventArgs pMbea)
        {
            bool saveClick = mClick;
            mClick = false;
            if (mHover && saveClick)
            {
                if (Click != null)
                {
                    Click(this, new EventArgs());
                }
            }
            base.mouseReleased(pMbea);
        }
    }
}
