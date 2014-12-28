using System;
using System.Diagnostics;

using SdlDotNet.Audio;
using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Input;

using FreemooSDL.Collections;
using FreemooSDL.Service;
using FreemooSDL.Game;
using FreemooSDL.Screens;

namespace FreemooSDL
{
    public class FreemooGame : IDisposable
    {
        private ScreenCollection mScreenCollection = null;
        private ScreenStack mScreenStack = null;
        private bool mQuit = false;
        //private DateTime mTimer = DateTime.Now;
        private FreemooTimer mTimer = null;
        private Game.Game mOrionGame = null;
        //private IScreen mCurrentScreen = null;
        private SoundFXService mSoundFxService = null;
        private ConfigService mConfigService = null;
        private ImageService mImageService = null;
        private GuiService mGuiService = null;
        private bool mScreenshot = false;

        // properties
        public Game.Game OrionGame
        {
            get
            {
                return mOrionGame;
            }
        }
        public SoundFXService SoundFX
        {
            get
            {
                return mSoundFxService;
            }
        }
        public ConfigService Config
        {
            get
            {
                return mConfigService;
            }
        }
        public GuiService Screen
        {
            get
            {
                return mGuiService;
            }
        }
        public ImageService Images
        {
            get
            {
                return mImageService;
            }
        }

        public void Dispose()
        {
            Mixer.Close();
        }

        public void run()
        {
            initialize();
            loadContent();

            //StateManager sm = (StateManager)Services.get(ServiceEnum.StateManager);
            //sm.changeState("");
            //mCurrentScreen = new MainScreen(this);
            //mCurrentScreen.start(); // refactor into changestate function
            changeScreen(ScreenEnum.MainScreen);
            //changeScreen(ScreenEnum.LoadGameScreen);

            int framesElapsed = 0;
            double currMillis = mTimer.TotalMilliseconds;
            while (!mQuit)
            {
                update();
                draw();
                framesElapsed++;
                double timeGoneBy = mTimer.TotalMilliseconds - currMillis;
                if (timeGoneBy > 1000)
                {
                    double fps = (double)framesElapsed / (timeGoneBy / 1000D);
                    Console.Write("FPS = " + Math.Round(fps, 2) + Environment.NewLine);
                    currMillis = mTimer.TotalMilliseconds;
                    framesElapsed = 0;
                }
            }

            unloadContent();
        }

        private void initialize()
        {
            buildServices();
            buildEventHandlers();
            buildScreensCollection();

            ObjectPool.Initialize();

            //Mixer.Initialize();

            mScreenStack = new ScreenStack();

            this.Screen.initializeVideo();

            mTimer = new FreemooTimer();

            //StateManager sm = (StateManager)Services.get(ServiceEnum.StateManager);
            //sm.initalize();

            mOrionGame = new Game.Game();
            mOrionGame.loadGame(1);

            
        }

        private void loadContent()
        {
            this.Images.loadImages();
            this.SoundFX.loadSoundFX();


        }

        private void unloadContent()
        {
        }

        private void update()
        {
            mTimer.update();
            Events.Poll();
            CurrentScreen.update(mTimer);
        }

        private void draw()
        {
            //ImageService imgs = (ImageService)Services[ServiceEnum.ImageService];
            //GuiService gs = (GuiService)Services.get(ServiceEnum.GuiService);
            //FreemooImage mooImg = imgs.Images[ArchiveEnum.STARMAP, "MAINVIEW"];
            //Surface surf = mooImg.getCurrentFrame();
            this.Screen.blank();

            //((StateManager)Services.get(ServiceEnum.StateManager)).draw(mTimer);
            CurrentScreen.draw(mTimer);

            if (mScreenshot)
            {
                Screen.takescreenshot();
                mScreenshot = false;
            }

            this.Screen.flip();
        }

        private void buildServices()
        {

            mConfigService = new ConfigService(this);
            mImageService = new ImageService(this);
            mGuiService = new GuiService(this);

            mSoundFxService = new SoundFXService(this);
        }

        private void buildEventHandlers()
        {
            Events.KeyboardDown += new EventHandler<KeyboardEventArgs>(this.keyPressed);
            Events.KeyboardUp += new EventHandler<KeyboardEventArgs>(this.keyReleased);
            Events.MouseButtonDown += new EventHandler<MouseButtonEventArgs>(this.mousePressed);
            Events.MouseButtonUp += new EventHandler<MouseButtonEventArgs>(this.mouseReleased);
            Events.MouseMotion +=new EventHandler<MouseMotionEventArgs>(this.mouseMoved);
            Events.Quit += new EventHandler<QuitEventArgs>(this.quitAction);
        }

        private void buildScreensCollection()
        {
            mScreenCollection = new ScreenCollection();

            mScreenCollection.Add(ScreenEnum.MainScreen, new MainScreen(this));
            mScreenCollection.Add(ScreenEnum.ResearchScreen, new ResearchScreen(this));
            mScreenCollection.Add(ScreenEnum.PlanetScreen, new PlanetsScreen(this));
            mScreenCollection.Add(ScreenEnum.RaceScreen, new RaceScreen(this));
            mScreenCollection.Add(ScreenEnum.MapScreen, new MapScreen(this));
            mScreenCollection.Add(ScreenEnum.FleetScreen, new FleetScreen(this));
            mScreenCollection.Add(ScreenEnum.DesignScreen, new DesignScreen(this));
            mScreenCollection.Add(ScreenEnum.GameScreen, new GameScreen(this));
            mScreenCollection.Add(ScreenEnum.OpeningMovie, new OpeningMovie(this));
            mScreenCollection.Add(ScreenEnum.LoadingScreen, new LoadingScreen(this));
            mScreenCollection.Add(ScreenEnum.MainMenu, new MainMenu(this));
            mScreenCollection.Add(ScreenEnum.LoadGameScreen, new LoadGameScreen(this));
        }

        public void quitAction(object sender, QuitEventArgs qea)
        {
            mQuit = true;
        }

        private MouseButtonEventArgs scaleMouseBtnPos(MouseButtonEventArgs pBtnArgs)
        {
            short scale = (short)this.Config.StretchRatio;
            MouseButton btn = pBtnArgs.Button;
            bool pressed = pBtnArgs.ButtonPressed;
            short x = (short)(pBtnArgs.X / scale);
            short y = (short)(pBtnArgs.Y / scale);
            MouseButtonEventArgs mb = new MouseButtonEventArgs(btn, pressed, x, y);
            return mb;
        }

        private MouseMotionEventArgs scaleMouseMovedPos(MouseMotionEventArgs pMouseArgs)
        {
            short scale = (short)this.Config.StretchRatio;
            MouseButton btn = pMouseArgs.Button;
            short x = (short)(pMouseArgs.X / scale);
            short y = (short)(pMouseArgs.Y / scale);
            short deltax = (short)(pMouseArgs.RelativeX / scale);
            short deltay = (short)(pMouseArgs.RelativeY / scale);
            MouseMotionEventArgs mm = new MouseMotionEventArgs(pMouseArgs.ButtonPressed, btn, x, y, deltax, deltay);
            return mm;
        }

        public void keyPressed(object sender, KeyboardEventArgs pKea)
        {
            CurrentScreen.keyPressed(pKea);
        }

        public void keyReleased(object sender, KeyboardEventArgs pKea)
        {
#if DEBUG
            if (pKea.Key == Key.Escape)
            {
                mQuit = true;
            }
            if (pKea.Key == Key.F2)
            {
                mScreenshot = true;
            }
#endif
            CurrentScreen.keyReleased(pKea);
        }

        public void mousePressed(object sender, MouseButtonEventArgs pMbea)
        {
            // need to scale the click position
            //pMbea.Position = new System.Drawing.Point(pMbea.Position.X / 4, pMbea.Position.Y / 4);
            CurrentScreen.mousePressed(scaleMouseBtnPos(pMbea));
        }

        public void mouseReleased(object sender, MouseButtonEventArgs pMbea)
        {

            CurrentScreen.mouseReleased(scaleMouseBtnPos(pMbea));
        }

        public void mouseMoved(object sender, MouseMotionEventArgs pMbea)
        {
            CurrentScreen.mouseMoved(scaleMouseMovedPos(pMbea));
        }

        // screen manager functions
        public void changeScreen(ScreenEnum pNewScreen)
        {
            while (mScreenStack.Count > 0)
            {
                //popScreen();
                IScreen tmp = mScreenStack.Pop();
                tmp.stop();
            }
            mScreenStack.Push(mScreenCollection[pNewScreen]);
            mScreenStack.Peek().start();
        }

        public void pushScreen(ScreenEnum pNextScreen)
        {
            mScreenStack.Peek().pause();
            mScreenStack.Push(mScreenCollection[pNextScreen]);
            mScreenStack.Peek().start();
        }

        public void popScreen()
        {
            Debug.Assert(mScreenStack.Count > 0);
            mScreenStack.Peek().stop();
            mScreenStack.Pop();
            mScreenStack.Peek().resume();
        }

        public IScreen CurrentScreen
        {
            get
            {
                return mScreenStack.Peek();
            }
        }
    }
}
