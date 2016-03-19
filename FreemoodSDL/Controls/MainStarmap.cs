using System;
using System.Collections.Generic;
using System.Drawing;

using FreeMoO.Collections;
using FreeMoO.Screens;
using FreeMoO.Game;
using FreeMoO.Service;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Input;

namespace FreeMoO.Controls
{

    class StarImage
        : AbstractControl
    {
        private FreemooImageInstance mStarImage;
        private FreemooImageInstance mPlanetBorder;

        private MainStarmap mParent;
        private Planet mPlanetRef;

        private bool mTwinkle = false;
        private int mSinceLastTwinkle = 0;

        public event EventHandler<EventArgs> StarImageClickEvent;

        public StarImage(MainStarmap pParent, Planet pPlanet)
        {
            mParent = pParent;
            mPlanetRef = pPlanet;
            string[] colorRef = { "yellow", "red", "green", "blue", "white", "purple" };

            string imageIndex = (mPlanetRef.StarSize == 0 ? "SMSTARS  " : "MEDSTARS ") + colorRef[mPlanetRef.StarColor];
            ImageService imgService = mParent.Screen.Game.Images; //(ImageService)mParent.Screen.Game.Services[ServiceEnum.ImageService];
            mStarImage = new FreemooImageInstance(ArchiveEnum.STARMAP, imageIndex, imgService);

            mPlanetBorder = new FreemooImageInstance(ArchiveEnum.STARMAP, "PLANBORD", imgService);
        }

        public override void Update(Timer pTimer)
        {
            //checkAnimation(pTimer);
        }

        private void checkAnimation(Timer pTimer)
        {
            if (mTwinkle)
            {
                mSinceLastTwinkle += (int)pTimer.MillisecondsElapsed;
                if (mSinceLastTwinkle >= 10000)
                {
                    mStarImage.gotoNextFrame();
                    if (mStarImage.CurrentFrameNum == 0)
                    {
                        mTwinkle = false;
                        mSinceLastTwinkle = 0;
                    }
                }
            }
            else
            {

                mSinceLastTwinkle += (int)pTimer.MillisecondsElapsed;
                if (mSinceLastTwinkle >= 5000)
                {
                    mTwinkle = true;
                    mSinceLastTwinkle = 0;
                }
            }
        }

        private bool testClick(Point pMousePt)
        {
            int x = mPlanetRef.X - mParent.View_X;
            int y = mPlanetRef.Y - mParent.View_Y;
            int w = mPlanetRef.StarSize == 6 ? 13 : 7;
            int h = mPlanetRef.StarSize == 6 ? 11 : 7;
            Rectangle rec = ObjectPool.GetRectangle(x, y, w, h); // new Rectangle(x, y, w, h);
            bool ret = false;
            pMousePt = new Point(pMousePt.X, pMousePt.Y); 
            if (rec.Contains(pMousePt))
            {
                ret = true;
            }
            ObjectPool.RectanglePool.PutObject(rec);
            return ret;
        }

        public override void Draw(Timer pTimer, GuiService pGui)
        {
            int maxViewX = mParent.View_X + MainStarmap.WIDTH;
            int maxViewY = mParent.View_Y + MainStarmap.HEIGHT;
            if (mPlanetRef.X > mParent.View_X && mPlanetRef.X < maxViewX)
            {
                if (mPlanetRef.Y > mParent.View_Y && mPlanetRef.Y < maxViewY)
                {
                    int x = mPlanetRef.X - mParent.View_X;
                    int y = mPlanetRef.Y - mParent.View_Y;
                    pGui.drawImage(mStarImage.getCurrentFrame(), x, y);

                    // there is a good amount of logic to determine this but for now
                    // i'm just going to put it out there
                    string drawName = mPlanetRef.Name.ToUpper();
                    int heightAdjust = mPlanetRef.StarSize == 0 ? 8 : 13;
                    pGui.drawString(drawName, x - 8, y + heightAdjust, FontEnum.font_2, Color.LightYellow);

                    // now check to see if I am focused on
                    if (mPlanetRef.ID == mParent.Screen.Game.OrionGame.GalaxyData.PlanetFocus)
                    {
                        // draw the planet border
                        // border is 25x25
                        // small stars are 7x7 
                        // med stars are 13x11
                        int xAdjust = mPlanetRef.StarSize == 0 ? -3 : 0;
                        int yAdjust = mPlanetRef.StarSize == 0 ? -3 : -1;
                        pGui.drawImage(mPlanetBorder.getCurrentFrame(), x  + xAdjust, y + yAdjust);
                    }
                }
            }
        }

        public override void mouseReleased(MouseButtonEventArgs pMbea)
        {
            if (testClick(pMbea.Position) && pMbea.Button == MouseButton.PrimaryButton)
            {
                if (StarImageClickEvent != null)
                {
                    StarImageClickEvent(mPlanetRef, new EventArgs());
                }
            }
            base.mouseReleased(pMbea);
        }

        public Planet PlanetRef
        {
            get
            {
                return mPlanetRef;
            }
        }
    }

    class FleetImage
        : AbstractControl
    {
        const int WIDTH = 7;
        const int HEIGHT = 3;
        const int ORBIT_X_OFFSET = 17;
        const int ORBIT_Y_OFFSET = -1;

        MainStarmap mParent;
        private Fleet mFleetRef;
        FreemooImageInstance mImage;

        public event EventHandler<EventArgs> FleetClickEvent;

        public Fleet FleetRef
        {
            get
            {
                return mFleetRef;
            }
        }

        public FleetImage(MainStarmap pParent, Fleet pFleet)
        {
            mParent = pParent;
            mFleetRef = pFleet;
            ImageService imgService = mParent.Screen.Game.Images;
            int colorId = this.mParent.Screen.Game.OrionGame.Players[mFleetRef.PlayerId].ColorId;
            mImage = new FreemooImageInstance(ArchiveEnum.STARMAP, "SMALSHIP" + colorId, imgService);
        }

        public override void  Update(Timer pTimer)
        {
        }

        public override void Draw(Timer pTimer, GuiService pGui)
        {
            if (mFleetRef.InTransit == true)
            {
                int maxViewX = mParent.View_X + MainStarmap.WIDTH;
                int maxViewY = mParent.View_Y + MainStarmap.HEIGHT;
                if (mFleetRef.X > mParent.View_X && mFleetRef.X < maxViewX)
                {
                    if (mFleetRef.Y > mParent.View_Y && mFleetRef.Y < maxViewY)
                    {
                        int x = mFleetRef.X - mParent.View_X;
                        int y = mFleetRef.Y - mParent.View_Y;
                        pGui.drawImage(mImage.getCurrentFrame(), x, y);
                    }
                }
            }
            else
            {
                Planet planetRef = mParent.Screen.Game.OrionGame.Planets[mFleetRef.PlanetId];
                int maxViewX = mParent.View_X + MainStarmap.WIDTH;
                int maxViewY = mParent.View_Y + MainStarmap.HEIGHT;
                if (planetRef.X > mParent.View_X && planetRef.X < maxViewX)
                {
                    if (planetRef.Y > mParent.View_Y && planetRef.Y < maxViewY)
                    {
                        int x = planetRef.X - mParent.View_X;
                        int y = planetRef.Y - mParent.View_Y;
                        x += ORBIT_X_OFFSET;
                        y += ORBIT_Y_OFFSET;
                        pGui.drawImage(mImage.getCurrentFrame(), x, y);
                    }
                }
            }
        }

        private bool testClick(Point pMousePt)
        {
            //int x = mFleetRef.X - mParent.View_X;
            //int y = mFleetRef.Y - mParent.View_Y;
            int x = 0;
            int y = 0;
            if (mFleetRef.InTransit)
            {
                x = mFleetRef.X - mParent.View_X;
                y = mFleetRef.Y - mParent.View_Y;
            }
            else
            {
                var planetRef = mParent.Screen.Game.OrionGame.Planets[mFleetRef.PlanetId];
                x = (planetRef.X - mParent.View_X) + ORBIT_X_OFFSET;
                y = (planetRef.Y - mParent.View_Y) + ORBIT_Y_OFFSET;
            }
            Rectangle rect = ObjectPool.GetRectangle(x, y, WIDTH, HEIGHT);// new Rectangle(x, y, WIDTH, HEIGHT);
            bool ret = false;
            if (rect.Contains(pMousePt))
            {
                ret = true;
            }
            ObjectPool.RectanglePool.PutObject(rect);
            return ret;
        }

        public override void mouseReleased(MouseButtonEventArgs pMbea)
        {
            //Point mousePt = new Point(pMbea.X / 4, pMbea.Y / 4);
            if (pMbea.Button == MouseButton.PrimaryButton && testClick(pMbea.Position))
            {
                FireClickEvent();
            }
            base.mouseReleased(pMbea);
        }

        private void FireClickEvent()
        {
            if (FleetClickEvent != null)
            {
                FleetClickEvent(mFleetRef, new EventArgs());
            }
        }
    }

    public class MainStarmap
        : AbstractControl
    {
        private List<StarImage> mStarControls;
        private List<FleetImage> mFleetControls;
        private MainScreen mScreenRef;
        private int mViewableX;
        private int mViewableY;
        public const int WIDTH = 217;
        public const int HEIGHT = 173;
        private Rectangle mBoundingRect = new Rectangle(6, 6, WIDTH, HEIGHT);
        private int mSelectedPlanet = -1;
        private int mSelectedFleet = -1;
        private bool mControlClicked = false;

        public event EventHandler<EventArgs> PlanetClickEvent;
        public event EventHandler<EventArgs> FleetClickEvent;

        public int View_X
        {
            get
            {
                return mViewableX;
            }
        }

        public int View_Y
        {
            get
            {
                return mViewableY;
            }
        }

        public MainStarmap(MainScreen pScreen)
        {
            mScreenRef = pScreen;
            mStarControls = new List<StarImage>();
            mFleetControls = new List<FleetImage>();
            //mSelectedPlanet = mScreenRef.Game.OrionGame.GalaxyData.PlanetFocus;
            //int x = mScreenRef.Game.OrionGame.Planets[mSelectedPlanet].X;
            //int y = mScreenRef.Game.OrionGame.Planets[mSelectedPlanet].Y;
            //recenterMap(new Point(x, y));
        }

        public void OnScreenStart()
        {
            mSelectedPlanet = mScreenRef.Game.OrionGame.GalaxyData.PlanetFocus;
            int x = mScreenRef.Game.OrionGame.Planets[mSelectedPlanet].X;
            int y = mScreenRef.Game.OrionGame.Planets[mSelectedPlanet].Y;
            recenterMap(new Point(x, y));
        }

        private void recenterMap(Point pMousePt)
        {
            int deltaX = pMousePt.X - (mBoundingRect.Right / 2);
            int deltaY = pMousePt.Y - (mBoundingRect.Bottom / 2);
            mViewableX += deltaX;
            mViewableY += deltaY;
            // crap, this next part relies on the test save being a huge map
            // eventually we need to find a way to check the map size but i'm not sure the savefile is even reading that yet
            if (mViewableX < 0) mViewableX = 0;
            if (mViewableX > 565) mViewableX = 565;
            if (mViewableY < 0) mViewableY = 0;
            if (mViewableY > 509) mViewableY = 509;

        }

        public override void Update(Timer pTimer)
        {
            
            rebuildFleets(); // really don't need too rebuild the fleets every turn though maybe do bounds checking an
            // only build the ones on screen
            foreach (StarImage si in mStarControls)
            {
                si.Update(pTimer);
            }
        }

        public override void Draw(Timer pTimer, GuiService pGui)
        {
            foreach (StarImage si in mStarControls)
            {
                si.Draw(pTimer, pGui);
            }
            foreach (FleetImage fi in mFleetControls)
            {
                fi.Draw(pTimer, pGui);
            }
        }

        private void addPlanet(Planet pPlanet)
        {
            StarImage si = new StarImage(this, pPlanet);
            si.StarImageClickEvent += new EventHandler<EventArgs>(this.handleStarClick);
            mStarControls.Add(si);
        }

        public void addPlanets(List<Planet> pPlanets)
        {
            foreach (Planet p in pPlanets)
            {
                addPlanet(p);
            }
        }

        private void rebuildFleets()
        {
            // it's gonna change every turn
            // and here I am rebuilding it every frame...need to do bounds checking
            List<Fleet> fleets = this.Screen.Game.OrionGame.Fleets;
            mFleetControls.Clear();
            foreach (Fleet f in fleets)
            {
                FleetImage fi = new FleetImage(this, f);
                fi.FleetClickEvent += new EventHandler<EventArgs>(this.handleFleetClick);
                mFleetControls.Add(fi);
            }
        }

        public MainScreen Screen
        {
            get
            {
                return mScreenRef;
            }
        }

        public override void mouseReleased(MouseButtonEventArgs pMbea)
        {
            mControlClicked = false;
            // should be checking fleets first
            if (mBoundingRect.Contains(new Point(pMbea.X, pMbea.Y)))
            {
                foreach (StarImage si in mStarControls)
                {
                    si.mouseReleased(pMbea);
                }
                if (mControlClicked == false)
                {
                    foreach (FleetImage fi in mFleetControls)
                    {
                        fi.mouseReleased(pMbea);
                    }
                }
                if (mControlClicked == false && pMbea.Button == MouseButton.PrimaryButton)
                {
                    //Point mousePt = new Point(pMbea.X, pMbea.Y);
                    //if (mBoundingRect.Contains(mousePt) && pMbea.Button == MouseButton.PrimaryButton)
                    //{
                    //    recenterMap(mousePt);
                    //}
                    recenterMap(new Point(pMbea.X, pMbea.Y));
                }
            }

            base.mouseReleased(pMbea);
        }

        private void handleStarClick(object sender, EventArgs ea)
        {
            Planet p = (Planet)sender;
            Console.WriteLine("Star " + p.ID + " was clicked.");
            mControlClicked = true;
            mSelectedPlanet = p.ID;
            if (PlanetClickEvent != null)
            {
                PlanetClickEvent(p, ea);
            }
        }

        private void handleFleetClick(object sender, EventArgs ea)
        {
            Fleet fi = (Fleet)sender;
            Console.WriteLine("Fleet " + fi.ID + " was clicked");
            mControlClicked = true;
            mSelectedFleet = fi.ID;
            if (FleetClickEvent != null)
            {
                FleetClickEvent(fi, ea);
            }
        }
    }
}
