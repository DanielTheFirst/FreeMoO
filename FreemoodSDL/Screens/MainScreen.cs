using System;
using System.Collections.Generic;
using System.Drawing;

using FreemooSDL.Service;
using FreemooSDL.Controls;
using FreemooSDL.Game;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Input;
using SdlDotNet.Audio;

namespace FreemooSDL.Screens
{
    public class MainScreen : AbstractScreen
    {
        private MainStarmap mStarmap = null;
        private ColonyPanel mColonyPanel = null;
        private UnexploredStarPanel _unexploredPanel = null;
        private EnemyColonyPanel _enemyColPanel = null;
        private NoColonyPanel _noColPanel = null;
        private MainScreenMenuButtons[] mMenuButtons = new MainScreenMenuButtons[8];

        public MainScreen(FreemooGame pGame)
            : base(pGame)
        {
            InitializeControls();
        }

        private void InitializeControls()
        {
            _unexploredPanel = new UnexploredStarPanel(this);
            _unexploredPanel.Enabled = false;
            _unexploredPanel.Visible = false;
            Controls.add(_unexploredPanel);

            mColonyPanel = new ColonyPanel(this);
            mColonyPanel.Enabled = false;
            mColonyPanel.Visible = false;
            Controls.add(mColonyPanel);

            _enemyColPanel = new EnemyColonyPanel(this);
            _enemyColPanel.Enabled = false;
            _enemyColPanel.Visible = false;
            Controls.add(_enemyColPanel);

            _noColPanel = new NoColonyPanel(this);
            _noColPanel.Enabled = false;
            _noColPanel.Visible = false;
            Controls.add(_noColPanel);

        }

        public override void Update(FreemooTimer pTimer)
        {
            //mStarmap.update(pTimer);
            //if (mColonyPanel != null) mColonyPanel.update(pTimer);
            //List<int> toRemove = new List<int>();
            //for (int i = 0; i < Controls.count(); i++)
            //{
            //    if (Controls.get(i) != null)
            //    {
            //        Controls.get(i).update(pTimer);
            //    }
            //    else
            //    {
            //        toRemove.Add(i);
            //    }
            //}
            //foreach (int i in toRemove)
            //{
            //    Controls.remove(i);
            //}
            UpdateControls(pTimer);
        }



        public override void start()
        {
            mStarmap = new MainStarmap(this);
            mStarmap.Id = "Main Starmap";
            mStarmap.addPlanets(Game.OrionGame.Planets);
            Controls.add(mStarmap);
            mStarmap.PlanetClickEvent += new EventHandler<EventArgs>(this.handlePlanetClick);

            handlePlanetClick(Game.OrionGame.Planets[Game.OrionGame.GalaxyData.PlanetFocus], null); // ugly way to handle this  but it works

            //Music m = Game.SoundFX.GetMusic();
            //m.Play(true);

            initMenu();
        }

        private void initMenu()
        {
            mMenuButtons[0] = new MainScreenMenuButtons(this, "Game", new Rectangle(5, 181, 32, 14), MenuButtonEnum.Game);
            mMenuButtons[1] = new MainScreenMenuButtons(this, "Design", new Rectangle(40, 181, 36, 14), MenuButtonEnum.Design);
            mMenuButtons[2] = new MainScreenMenuButtons(this, "Fleet", new Rectangle(79, 181, 33, 14), MenuButtonEnum.Fleet);
            mMenuButtons[3] = new MainScreenMenuButtons(this, "Map", new Rectangle(115, 181, 25, 14), MenuButtonEnum.Map);
            mMenuButtons[4] = new MainScreenMenuButtons(this, "Races", new Rectangle(143, 181, 34, 14), MenuButtonEnum.Races);
            mMenuButtons[5] = new MainScreenMenuButtons(this, "Planets", new Rectangle(180, 181, 42, 14), MenuButtonEnum.Planets);
            mMenuButtons[6] = new MainScreenMenuButtons(this, "Tech", new Rectangle(225, 181, 30, 14), MenuButtonEnum.Tech);
            mMenuButtons[7] = new MainScreenMenuButtons(this, "Next Turn", new Rectangle(258, 181, 57, 14), MenuButtonEnum.NextTurn);

            foreach (MainScreenMenuButtons mm in mMenuButtons)
            {
                Controls.add(mm);
                mm.Click += new EventHandler<EventArgs>(this.handleMenuClick);
            }
        }

        public override void Draw(FreemooTimer pTimer, GuiService pGuiService)
        {
            ImageService imgService = Game.Images; //(ImageService)Game.Services.get(ServiceEnum.ImageService);
            //GuiService gs = Game.Screen; //(GuiService)Game.Services.get(ServiceEnum.GuiService);

            //Surface starBack = imgService.Images[ArchiveEnum.STARMAP, "STARBAK2"][0];
            Surface starBack = imgService.getSurface(ArchiveEnum.STARMAP, "STARBAK2", 0);
            //Surface mainInterface = imgService.Images[ArchiveEnum.STARMAP, "MAINVIEW"][0];
            Surface mainInterface = imgService.getSurface(ArchiveEnum.STARMAP, "MAINVIEW", 0);

            pGuiService.drawImage(starBack, 0, 0);
            pGuiService.drawImage(mainInterface, 0, 0);

            //mStarmap.draw(pTimer, gs);
            //if (mColonyPanel != null) mColonyPanel.draw(pTimer, gs);

            for (int i = 0; i < Controls.count(); i++)
            {
                Controls.get(i).Draw(pTimer, pGuiService);
            }
       }

        public override void mouseReleased(SdlDotNet.Input.MouseButtonEventArgs pMbea)
        {
            //mStarmap.mouseReleased(pMbea);
            //if (mColonyPanel != null) mColonyPanel.mouseReleased(pMbea);
            //Rectangle tmprect = new Rectangle(225, 181, 30, 14);
            //if (tmprect.Contains(pMbea.Position))
            //{
            //    Game.changeScreen(ScreenEnum.ResearchScreen);
            //}
            /*for (int i = 0; i < Controls.count(); i++)
            {
                if (Controls.get(i) != null)
                {
                    Controls.get(i).mouseReleased(pMbea);
                }
            }*/
            base.mouseReleased(pMbea);
        }

        public override void mouseMoved(SdlDotNet.Input.MouseMotionEventArgs pMbea)
        {
            //if (mColonyPanel != null) mColonyPanel.mouseMoved(pMbea);
            //for (int i = 0; i < Controls.count(); i++)
            //{
            //    Controls.get(i).mouseMoved(pMbea);
            //}
            base.mouseMoved(pMbea);
        }

        public override void mousePressed(SdlDotNet.Input.MouseButtonEventArgs pMbea)
        {
            //if (mColonyPanel != null) mColonyPanel.mousePressed(pMbea);
            //for (int i = 0; i < Controls.count(); i++)
            //{
            //    Controls.get(i).mousePressed(pMbea);
            //}
            base.mousePressed(pMbea);
        }

        public override void keyPressed(SdlDotNet.Input.KeyboardEventArgs pKea)
        {

            base.keyPressed(pKea);
        }

        private void SetVisibleAndEnabled(AbstractControl ctrl, bool val)
        {
            ctrl.Visible = val;
            ctrl.Enabled = val;
        }

        private void handlePlanetClick(object Sender, EventArgs ea)
        {
            Planet p = (Planet)Sender;
            SetVisibleAndEnabled(mColonyPanel, false);
            SetVisibleAndEnabled(_unexploredPanel, false);
            SetVisibleAndEnabled(_enemyColPanel, false);
            SetVisibleAndEnabled(_noColPanel, false);
            // this should probably be strongly typed using delegates....eventually
            // also, should not be nulling and newing here. 
            /*if (mColonyPanel != null)
            {
                //Controls.remove(mColonyPanel.Id);
                mColonyPanel.SafeRemove = true;
                mColonyPanel = null;
            }
            if (_unexploredPanel != null)
            {
                //Controls.remove(_unexploredPanel.Id);
                _unexploredPanel.SafeRemove = true;
                _unexploredPanel = null;
            }
            if (_enemyColPanel != null)
            {
                //Controls.remove(_enemyColPanel.Id);
                _enemyColPanel.SafeRemove = true;
                _enemyColPanel = null;
            }
            if (_noColPanel != null)
            {
                // seriously, rewriting this should be quite high on your to do list
                //Controls.remove(_noColPanel.Id);
                _noColPanel.SafeRemove = true;
                _noColPanel = null;
            }*/
            
            if (p.PlayerId == 0)
            {
                //mColonyPanel = new ColonyPanel(this, p);
                //mColonyPanel.ShipProductionUpdateEvent += this.HandleColonyShipProdChange;
                //Controls.add(mColonyPanel);
                mColonyPanel.Planet = p;
                SetVisibleAndEnabled(mColonyPanel, true);
            }
            else if (!p.Player0Explored)
            {
                //_unexploredPanel = new UnexploredStarPanel(this, p);
                //Controls.add(_unexploredPanel);
                SetVisibleAndEnabled(_unexploredPanel, true);
                _unexploredPanel.Planet = p;
            }
            else if (p.Player0Explored && p.IsColonized)
            {
                //_enemyColPanel = new EnemyColonyPanel(this, p);
                //Controls.add(_enemyColPanel);
                SetVisibleAndEnabled(_enemyColPanel, true);
                _enemyColPanel.Planet = p;
            }
            else if (p.Player0Explored && !p.IsColonized)
            {
                //_noColPanel = new NoColonyPanel(this, p);
                //Controls.add(_noColPanel);
                SetVisibleAndEnabled(_noColPanel, true);
                _noColPanel.Planet = p;
            }

            Game.OrionGame.UpdatePlanetFocus(p.ID);
        }

        private void handleMenuClick(object Sender, EventArgs ea)
        {
            MainScreenMenuButtons m = (MainScreenMenuButtons)Sender;
            // ok, now I need to stub in all the other screens
            _screenAction.ScreenAction = ScreenActionEnum.Push;
            switch (m.ButtonType)
            {
                case MenuButtonEnum.Tech:
                    //Game.pushScreen(ScreenEnum.ResearchScreen);
                    _screenAction.NextScreen = ScreenEnum.ResearchScreen;
                    break;
                case MenuButtonEnum.Planets:
                    _screenAction.NextScreen = ScreenEnum.PlanetScreen;
                    break;
                case MenuButtonEnum.Races:
                    _screenAction.NextScreen = ScreenEnum.RaceScreen;
                    break;
                case MenuButtonEnum.Map:
                    _screenAction.NextScreen = ScreenEnum.MapScreen;
                    break;
                case MenuButtonEnum.Fleet:
                    _screenAction.NextScreen = ScreenEnum.FleetScreen;
                    break;
                case MenuButtonEnum.Design:
                    _screenAction.NextScreen = ScreenEnum.DesignScreen;
                    break;
                case MenuButtonEnum.Game:
                    _screenAction.NextScreen = ScreenEnum.GameScreen;
                    break;
                case MenuButtonEnum.NextTurn:
                    _screenAction.ScreenAction = ScreenActionEnum.None;
                    CalculateNextTurnTemp();
                    break;
            }
            if (_screenAction.ScreenAction != ScreenActionEnum.None)
            {
                Game.QueueScreenAction(_screenAction);
            }
            
        }

        private void HandleColonyShipProdChange(int planetId)
        {
            Game.OrionGame.Planets[planetId].UpdateShipProduction();
        }

        private void CalculateNextTurnTemp()
        {
            // just do all the next turn calculations so I can
            // do them without worrying about the interface for now
            // some "next turn" functions involve interaction but i'll
            // deal with it when I get to it.
        }
    }
}
