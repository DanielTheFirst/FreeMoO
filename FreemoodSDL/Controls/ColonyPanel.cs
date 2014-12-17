using System;
using System.Collections.Generic;
using System.Drawing;

using FreemooSDL.Collections;
using FreemooSDL.Game;
using FreemooSDL.Screens;
using FreemooSDL.Service;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Input;

namespace FreemooSDL.Controls
{
    /*class ProductionBarEventArgs
        : EventArgs
    {
        public ProductionBarEventArgs()
            : base()
        {
        }
        public ProductionEnum ProdType;
        //public int NewValue;
        public int Delta;
    }*/

    /*class ProductionBars
        : AbstractControl
    {
        ProductionEnum mProdType;
        bool mLocked;
        int mValue;
        bool mBlockClick = false;

        const int HEIGHT_OFFSET = 84;
        const int WIDTH_OFFSET = 253;
        const int RECT_WIDTH = 26;
        const int RECT_HEIGHT = 4;
        const int HEIGHT_LEN = 11;

        public event EventHandler<ProductionBarEventArgs> ProductionBarChange;

        public override void draw(FreemooTimer pTimer, GuiService pGuiService)
        {
            // 241C14
            Color fillColor = Color.FromArgb(0x24, 0x1c, 0x14);
            int fy = HEIGHT_OFFSET + (HEIGHT_LEN * (int)mProdType);
            pGuiService.drawRect(WIDTH_OFFSET, fy, RECT_WIDTH, RECT_HEIGHT, fillColor);
            Color rectColor = mLocked ? Color.FromArgb(0x84, 0x2c, 0x00) : Color.FromArgb(0x2c, 0x88, 0x88);
            int y = (HEIGHT_OFFSET + 1) + (HEIGHT_LEN * (int)mProdType);
            pGuiService.drawRect(WIDTH_OFFSET, y, mValue, RECT_HEIGHT - 1, rectColor);
        }

        public override void update(FreemooTimer pTimer)
        {
            // don't really need to do anything here.
        }

        public int Value
        {
            get
            {
                return mValue * 4;
            }
            set
            {
                mValue = value / 4;
            }
        }

        public bool Locked
        {
            get
            {
                return mLocked;
            }
            set
            {
                mLocked = value;
            }
        }

        public ProductionEnum ProductionType
        {
            get
            {
                return mProdType;
            }
            set
            {
                mProdType = value;
            }
        }

        public override void mouseReleased(MouseButtonEventArgs pMbea)
        {
            if (pMbea.Button == MouseButton.PrimaryButton)
            {

                mBlockClick = false;
            }
        }

        public override void mousePressed(MouseButtonEventArgs pMbea)
        {
            int y = HEIGHT_OFFSET + (HEIGHT_LEN * (int)mProdType);
            //Point adjustPt = new Point(pMbea.X / 4, pMbea.Y / 4);
            Rectangle prodRect = new Rectangle(WIDTH_OFFSET, y, RECT_WIDTH, RECT_HEIGHT);
            Rectangle downRect = new Rectangle(248, y, 3, RECT_HEIGHT);
            Rectangle upRect = new Rectangle(279, y, 3, RECT_HEIGHT);
            if (pMbea.Button == MouseButton.PrimaryButton && !mBlockClick)
            {
                if (!mLocked)
                {
                    if (prodRect.Contains(pMbea.Position))
                    {
                        // at this point we make the change to the production bar value
                        // and then notify subscribers that it has changed.
                        int newX = pMbea.X - WIDTH_OFFSET;
                        int deltaX = mValue - newX; //newX - mValue;
                        ProductionBarEventArgs pbea = new ProductionBarEventArgs();
                        //pbea.NewValue = newX;
                        pbea.Delta = deltaX;
                        pbea.ProdType = mProdType;
                        if (ProductionBarChange != null)
                        {
                            ProductionBarChange(this, pbea);
                        }
                    }
                    else if (downRect.Contains(pMbea.Position) && mValue > 0)
                    {
                        ProductionBarEventArgs pbea = new ProductionBarEventArgs();
                        pbea.Delta = 1;
                        pbea.ProdType = mProdType;
                        if (ProductionBarChange != null)
                        {
                            ProductionBarChange(this, pbea);
                        }
                    }
                    else if (upRect.Contains(pMbea.Position) && mValue < 25)
                    {
                        ProductionBarEventArgs pbea = new ProductionBarEventArgs();
                        pbea.Delta = -1;
                        pbea.ProdType = mProdType;
                        if (ProductionBarChange != null)
                        {
                            ProductionBarChange(this, pbea);
                        }
                    }
                }
            }
        }
    }*/

    public class ColonyPanel 
        : AbstractControl
    {
        private Planet mPlanetRef = null;
        private MainScreen mScreenRef = null;
        //private ProductionBars[] mProductionBars = null;
        private ProductionBarGroup _productionBars = null;
        private MooButton[] mButtons = null;

        private const int PRODUCTION_PANEL_X_OFFSET = 226;
        private const int PRODUCTION_PANEL_Y_OFFSET = 81;
        private const int PRODUCTION_BAR_HEIGHT = 11;
        private const int PRODUCTION_BAR_WIDTH = 60;
        private const int PRODUCTION_BAR_LABEL_WIDTH = 20;
        
        private Color _lockedColor = Color.FromArgb(0xff, 0x86, 0x2c, 0x00); //862C00

        public ColonyPanel(MainScreen pScreen, Planet pPlanet)
            : base()
        {
            mPlanetRef = pPlanet;
            mScreenRef = pScreen;
            Id = "COLONYPANEL_" + pPlanet.Name;

            buildProductionBars();
            buildButtons();
        }

        private void buildProductionBars()
        {
            _productionBars = ObjectPool.ProductionBarGroupPool.GetObject();

            PlanetaryProduction pp = mPlanetRef.Production;
            int[] vals = pp.getArrayOfValues();
            bool[] locked = pp.getArrayOfLocked();

            _productionBars.ParentControl = this;
            _productionBars.Id = "ColonyProductionBars";

            /*mProductionBars = new ProductionBars[5];*/
            for (int i = 0; i < 5; i++)
            {
                ProductionEnum pe = (ProductionEnum)i;
                ProductionBar pb = ObjectPool.ProductionBarPool.GetObject();
                //pb = new ProductionBars();
                pb.Id = "ProductionBar_" + i;
                pb.Value = vals[i];
                pb.Locked = locked[i];
                pb.ProdType = pe;
                pb.X = PRODUCTION_PANEL_X_OFFSET;
                pb.Y = PRODUCTION_PANEL_Y_OFFSET + (PRODUCTION_BAR_HEIGHT * i);
                pb.Width = PRODUCTION_BAR_WIDTH;
                pb.Height = PRODUCTION_BAR_HEIGHT;
                pb.LabelWidth = PRODUCTION_BAR_LABEL_WIDTH;
                pb.PreCalculateRectangles();
                pb.MaxValue = 25; // 100 / 4
                pb.ProductionBarChange += _productionBars.HandleProductionBarChange;
                //pb.ProductionBarChange += new EventHandler<ProductionBarEventArgs>(this.handleProductionBarChange);
                _productionBars.Controls.add(pb);
            }
            Controls.add(_productionBars);
        }

        private void buildButtons()
        {
            mButtons = new MooButton[3];
            ImageService imgService = mScreenRef.Game.Images;
            mButtons[0] = new MooButton(ArchiveEnum.STARMAP, "COL_BUTT ship", imgService, 282, 140);
            mButtons[0].Id = "ShipButton";
            mButtons[0].Click +=new EventHandler<EventArgs>(this.ShipBtn_Click);
            mButtons[1] = new MooButton(ArchiveEnum.STARMAP, "COL_BUTT relocate", imgService, 282, 152);
            mButtons[1].Id = "RelocButton";
            mButtons[1].Click += new EventHandler<EventArgs>(this.RelocBtn_Click);
            mButtons[2] = new MooButton(ArchiveEnum.STARMAP, "COL_BUTT trans", imgService, 282, 164);
            mButtons[2].Id = "TransButton";
            mButtons[2].Click += new EventHandler<EventArgs>(this.TransBtn_Click);
            foreach (MooButton mb in mButtons)
            {
                Controls.add(mb);
            }
        }

        public override void Release()
        {
            _productionBars.Release();
            ObjectPool.ProductionBarGroupPool.PutObject(_productionBars);
            base.Release();
        }

        public override void update(FreemooTimer pTimer)
        {
            //foreach (ProductionBars pb in mProductionBars)
            //{
            //    pb.update(pTimer);
            //}

            for (int i = 0; i < Controls.count(); i++)
            {
                Controls.get(i).update(pTimer);
            }
        }

        public override void draw(FreemooTimer pTimer, GuiService pGuiService)
        {
            ImageService imgService = mScreenRef.Game.Images; //(ImageService)mScreenRef.Game.Services[ServiceEnum.ImageService];
            Rectangle poolRect = ObjectPool.RectanglePool.GetObject();
            Point poolPoint = ObjectPool.PointObjPool.GetObject();
            Size poolSize = ObjectPool.SizeObjPool.GetObject();

            for (int i = 0; i < _productionBars.Controls.count(); i++)
            {
                if (_productionBars.Controls.get(i) is ProductionBar)
                {
                    var ctr = (ProductionBar)_productionBars.Controls.get(i);

                    if (ctr.Locked)
                    {
                        pGuiService.drawRect(ctr.X, ctr.Y, ctr.LabelWidth, ctr.Height, _lockedColor);
                    }
                    else
                    {
                        pGuiService.drawRect(ctr.X, ctr.Y, ctr.LabelWidth, ctr.Height, Color.Black);
                    }
                }
            }

            // draw a box behind the production labels black for unlocked, 862C00 for locked
            //int y = 82;
            //mProductionBars[1].Locked = true;
            /*foreach (ProductionBars pb in mProductionBars)
            {
                Color fillcol = Color.Black;
                if (pb.Locked)
                {
                    fillcol = Color.FromArgb(0xff, 0x86, 0x2c, 0x00);
                }
                pGuiService.drawRect(227, y, 17, 7, fillcol);
                y += 11;
            }*/

            Surface panelSurf = imgService.getSurface(ArchiveEnum.STARMAP, "YOURPLNT", 0); //imgService.Images[ArchiveEnum.STARMAP, "YOURPLNT"][0];

            pGuiService.drawImage(panelSurf, 224, 5);

            Rectangle rect = new Rectangle(227, 8, 311 - 227, 21 - 8);
            pGuiService.drawString(mPlanetRef.Name, rect, FontEnum.font_4, FontPaletteEnum.Font4Colors);

            string smallPlanet = "PLANET" + (mPlanetRef.SmallPlanetImageIndex+1);
            Surface smallPanetSurf = imgService.getSurface(ArchiveEnum.PLANETS, smallPlanet, 0); //imgService.Images[ArchiveEnum.PLANETS, smallPlanet][0];
            // 229x26
            pGuiService.drawImage(smallPanetSurf, 229, 26);

            string planetType = mPlanetRef.PlanetType.ToString().ToUpper();
            // [0xff00ff, 0xFFDF51, 0xff88ff, 0xff88ff, 0xCB9600]
            pGuiService.drawString(planetType, new Rectangle(263, 28, 43, 5), FontEnum.font_0, FontPaletteEnum.PlanetType, TextAlignEnum.Right, TextVAlignEnum.None );

            if (mPlanetRef.Wealth != PlanetWealthEnum.Normal)
            {
                string wealth = String.Empty;

                if (mPlanetRef.Wealth == PlanetWealthEnum.UltraPoor)
                {
                    wealth = "ULTRA POOR";
                }
                else if (mPlanetRef.Wealth == PlanetWealthEnum.UltraRich)
                {
                    wealth = "ULTRA RICH";
                }
                else
                {
                    wealth = mPlanetRef.Wealth.ToString().ToUpper();
                }
                pGuiService.drawString(wealth, new Rectangle(263, 36, 43, 5), FontEnum.font_0, FontPaletteEnum.PlanetBluePal, TextAlignEnum.Right, TextVAlignEnum.None);
            }

            string popString = "POP" + mPlanetRef.MaxPopulation.ToString().PadLeft(3, ' ') + " MAX";
            pGuiService.drawString(popString, new Rectangle(263, 45, 43, 5), FontEnum.font_2, FontPaletteEnum.PopulationGreen, TextAlignEnum.Right, TextVAlignEnum.None);

            string shipString = string.Empty;
            if (mPlanetRef.Production.Ship.Value > 0)
            {
                
                int shipsPerYear = mPlanetRef.calcSingleYearShipProduction();
                if (shipsPerYear == 0)
                {
                    int yearsPerShip = mPlanetRef.calcNumYearsToProduceShip();
                    shipString = yearsPerShip + " Y";
                }
                else
                {
                    shipString = "1 Y";
                }

            }
            else
            {
                shipString = "NONE";
            }
            pGuiService.drawString(shipString, new Rectangle(288, 83, 24, 5), FontEnum.font_2, Color.Black, TextAlignEnum.Right, TextVAlignEnum.None);

            for (int i = 0; i < Controls.count(); i++)
            {
                Controls.get(i).draw(pTimer, pGuiService);
            }

            ObjectPool.RectanglePool.PutObject(poolRect);
            ObjectPool.SizeObjPool.PutObject(poolSize);
            ObjectPool.PointObjPool.PutObject(poolPoint);
        }

        public override void mousePressed(MouseButtonEventArgs pMbea)
        {
            //base.mousePressed(pMbea);
            /*foreach (ProductionBars pb in mProductionBars)
            {
                pb.mousePressed(pMbea);
            }*/
            /*foreach (MooButton btn in mButtons)
            {
                btn.mousePressed(pMbea);
            }*/
            for (int i = 0; i < Controls.count(); i++)
            {
                Controls.get(i).mousePressed(pMbea);
            }
        }

        public override void mouseReleased(MouseButtonEventArgs pMbea)
        {
            /*foreach (ProductionBars pb in mProductionBars)
            {
                pb.mousePressed(pMbea);
            }*/
            /*foreach (MooButton btn in mButtons)
            {
                btn.mouseReleased(pMbea);
            }*/
            for (int i = 0; i < Controls.count(); i++)
            {
                Controls.get(i).mouseReleased(pMbea);
            }
        }

        public override void mouseMoved(MouseMotionEventArgs pMbea)
        {
            foreach (MooButton btn in mButtons)
            {
                btn.mouseMoved(pMbea);
            }
            base.mouseMoved(pMbea);
        }

        private void handleProductionBarChange(object sender, ProductionBarEventArgs pArgs)
        {
            //Console.Write(pArgs.ProdType.ToString());
            // recalculate the production bar
            /*int delta = pArgs.Delta * 4;
            int prodIdx = (int)pArgs.ProdType;
            int[] order = { 4, 2, 3, 0, 1 };
            int newVal = mProductionBars[prodIdx].Value - delta;
            int leftOver = newVal - mProductionBars[prodIdx].Value;
            if (leftOver < 0)
            {
                foreach (int pk in order)
                {
                    if (leftOver < 0)
                    {
                        if (!mProductionBars[pk].Locked && pk != prodIdx)
                        {
                            if ((mProductionBars[pk].Value - leftOver) <= 100)
                            {
                                mProductionBars[pk].Value -= leftOver;
                                leftOver = 0;
                            }
                            else
                            {
                                leftOver -= (100 - mProductionBars[pk].Value);
                                mProductionBars[pk].Value = 100;
                            }

                        }
                    }
                }
            }
            else if (leftOver > 0)
            {
                foreach (int pk in order)
                {
                    if (leftOver > 0)
                    {
                        if (!mProductionBars[pk].Locked && pk != prodIdx)
                        {
                            if ((mProductionBars[pk].Value - leftOver) >= 0)
                            {
                                mProductionBars[pk].Value -= leftOver;
                                leftOver = 0;
                            }
                            else
                            {
                                leftOver -= (mProductionBars[pk].Value);
                                mProductionBars[pk].Value = 0;
                            }
                        }
                    }
                }
            }

            // now update the triggering bar
            mProductionBars[prodIdx].Value = newVal;

            // now write the new values to the planet ref
            PlanetaryProduction pp = mPlanetRef.Production;
            pp.Ship.Value = mProductionBars[0].Value;
            pp.Defense.Value = mProductionBars[1].Value;
            pp.Industry.Value = mProductionBars[2].Value;
            pp.Ecology.Value = mProductionBars[3].Value;
            pp.Technology.Value = mProductionBars[4].Value;
            mPlanetRef.Production = pp;*/
        }

        private void ShipBtn_Click(object pSender, EventArgs pArgs)
        {
            
        }

        private void TransBtn_Click(object pSender, EventArgs pArgs)
        {
        }

        private void RelocBtn_Click(object pSender, EventArgs pArgs)
        {
        }
    }
}
