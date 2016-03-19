using System;
using System.Diagnostics;

using SdlDotNet.Audio;
using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Input;

using FreeMoO.Collections;
using FreeMoO.Service;
using FreeMoO.Game;
using FreeMoO.Screens;
using FreeMoO.Controls;

namespace FreeMoO
{
    public class FreemooGame : IDisposable
    {
        private ScreenCollection _screenCollection = null;
        private ScreenStack _screenStack = null;
        private bool _quit = false;
        private FreemooTimer _timer = null;
        private Game.Game _orionGame = null;
        //private IScreen mCurrentScreen = null;
        private SoundFXService _soundFxService = null;
        private ConfigService _configService = null;
        private ImageService _imageService = null;
        private GuiService _guiService = null;
        private bool _screenshot = false;
        private string _dispFps = "{0} FPS";
        private ScreenActionEventArgs _queuedScreenAction = new ScreenActionEventArgs();

        // properties
        public Game.Game OrionGame
        {
            get
            {
                return _orionGame;
            }
        }
        public SoundFXService SoundFX
        {
            get
            {
                return _soundFxService;
            }
        }
        public ConfigService Config
        {
            get
            {
                return _configService;
            }
        }
        public GuiService Screen
        {
            get
            {
                return _guiService;
            }
        }
        public ImageService Images
        {
            get
            {
                return _imageService;
            }
        }

        public void Dispose()
        {
            Mixer.Close();
        }

        public void Run()
        {
            Initialize();
            loadContent();

            changeScreen(ScreenEnum.MainScreen);
            //changeScreen(ScreenEnum.LoadingScreen);
            //changeScreen(ScreenEnum.MainMenu);

            int framesElapsed = 0;
            double currMillis = _timer.TotalMilliseconds;
            string fpsString = "{0} FPS";
            while (!_quit)
            {
                double timeGoneBy = _timer.TotalMilliseconds - currMillis;
                if (timeGoneBy > 1000)
                {
                    double fps = (double)framesElapsed / (timeGoneBy / 1000D);
                    Console.Write("FPS = " + Math.Round(fps, 2) + Environment.NewLine);
                    currMillis = _timer.TotalMilliseconds;
                    framesElapsed = 0;
                    //_dispFps = string.Format(fpsString, Math.Round(fps, 2));
                    var test = _dispFps.Fmt(Math.Round(fps, 2));
                }
                update();
                Draw();
                framesElapsed++;

                if (_queuedScreenAction.ScreenAction != ScreenActionEnum.None)
                {
                    ProcessScreenAction();
                    _queuedScreenAction.ScreenAction = ScreenActionEnum.None;
                    _queuedScreenAction.NextScreen = ScreenEnum.None;
                }
            }

            unloadContent();
        }

        private void Initialize()
        {
            ObjectPool.Initialize();

            buildServices();

            this.Images.loadImages();

            buildEventHandlers();
            buildScreensCollection();

            

            //Mixer.Initialize();

            _screenStack = new ScreenStack();

            this.Screen.initializeVideo();

            _timer = new FreemooTimer();

            _orionGame = new Game.Game();
            _orionGame.loadGame(1);

            
        }

        private void loadContent()
        {
            //this.Images.loadImages();
            //this.SoundFX.loadSoundFX();


        }

        private void unloadContent()
        {
        }

        private void update()
        {
            _timer.update();
            Events.Poll();
            ScreenControl.Update(_timer);
        }

        private void Draw()
        {
            this.Screen.blank();

            ScreenControl.Draw(_timer, this.Screen);

            if (_screenshot)
            {
                Screen.takescreenshot();
                _screenshot = false;
            }
            //Screen.drawString(_dispFps, 0, 0, FontEnum.font_2, FontPaletteEnum.PopulationGreen);
            this.Screen.flip();
        }

        private void buildServices()
        {

            _configService = new ConfigService(this);
            _imageService = new ImageService(this);
            _guiService = new GuiService(this);

            _soundFxService = new SoundFXService(this);
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
            _screenCollection = new ScreenCollection();

            _screenCollection.Add(ScreenEnum.MainScreen, new MainScreen(this));
            _screenCollection.Add(ScreenEnum.ResearchScreen, new ResearchScreen(this));
            _screenCollection.Add(ScreenEnum.PlanetScreen, new PlanetsScreen(this));
            _screenCollection.Add(ScreenEnum.RaceScreen, new RaceScreen(this));
            _screenCollection.Add(ScreenEnum.MapScreen, new MapScreen(this));
            _screenCollection.Add(ScreenEnum.FleetScreen, new FleetScreen(this));
            _screenCollection.Add(ScreenEnum.DesignScreen, new DesignScreen(this));
            _screenCollection.Add(ScreenEnum.GameScreen, new GameScreen(this));
            _screenCollection.Add(ScreenEnum.OpeningMovie, new OpeningMovie(this));
            _screenCollection.Add(ScreenEnum.LoadingScreen, new LoadingScreen(this));
            _screenCollection.Add(ScreenEnum.MainMenu, new MainMenu(this));
            _screenCollection.Add(ScreenEnum.LoadGameScreen, new LoadGameScreen(this));
        }

        public void quitAction(object sender, QuitEventArgs qea)
        {
            _quit = true;
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
            ScreenControl.keyPressed(pKea);
        }

        public void keyReleased(object sender, KeyboardEventArgs pKea)
        {
#if DEBUG
            if (pKea.Key == Key.Escape)
            {
                _quit = true;
            }
            if (pKea.Key == Key.F2)
            {
                _screenshot = true;
            }
#endif
            ScreenControl.keyReleased(pKea);
        }

        public void mousePressed(object sender, MouseButtonEventArgs pMbea)
        {
            // need to scale the click position
            //pMbea.Position = new System.Drawing.Point(pMbea.Position.X / 4, pMbea.Position.Y / 4);
            ScreenControl.mousePressed(scaleMouseBtnPos(pMbea));
        }

        public void mouseReleased(object sender, MouseButtonEventArgs pMbea)
        {

            ScreenControl.mouseReleased(scaleMouseBtnPos(pMbea));
        }

        public void mouseMoved(object sender, MouseMotionEventArgs pMbea)
        {
            ScreenControl.mouseMoved(scaleMouseMovedPos(pMbea));
        }

        // screen manager functions
        public void changeScreen(ScreenEnum pNewScreen)
        {
            while (_screenStack.Count > 0)
            {
                //popScreen();
                IScreen tmp = _screenStack.Pop();
                tmp.stop();
            }
            _screenStack.Push(_screenCollection[pNewScreen]);
            _screenStack.Peek().start();
        }

        public void pushScreen(ScreenEnum pNextScreen)
        {
            _screenStack.Peek().pause();
            _screenStack.Push(_screenCollection[pNextScreen]);
            _screenStack.Peek().start();
        }

        public void popScreen()
        {
            Debug.Assert(_screenStack.Count > 0);
            _screenStack.Peek().stop();
            _screenStack.Pop();
            _screenStack.Peek().resume();
        }

        public void QueueScreenAction(ScreenActionEventArgs scr)
        {
            Debug.Assert(_queuedScreenAction.ScreenAction == ScreenActionEnum.None && _queuedScreenAction.NextScreen == ScreenEnum.None, "Really bad idea to let an action be queued once one already has.");
            _queuedScreenAction.NextScreen = scr.NextScreen;
            _queuedScreenAction.ScreenAction = scr.ScreenAction;
        }

        private void ProcessScreenAction()
        {
            Debug.Assert(_queuedScreenAction.ScreenAction != ScreenActionEnum.None, "Cannot process a NONE action.");
            switch(_queuedScreenAction.ScreenAction)
            {
                case ScreenActionEnum.Change:
                    changeScreen(_queuedScreenAction.NextScreen);
                    break;
                case ScreenActionEnum.Push:
                    pushScreen(_queuedScreenAction.NextScreen);
                    break;
                case ScreenActionEnum.Pop:
                    popScreen();
                    break;
                case ScreenActionEnum.Quit:
                    _quit = true;
                    while (_screenStack.Count > 0) _screenStack.Pop();
                    break;
            }
        }

        public IScreen CurrentScreen
        {
            get
            {
                return _screenStack.Peek();
            }
        }

        public IControl ScreenControl
        {
            get
            {
                return (IControl)_screenStack.Peek();
            }
        }
    }
}
