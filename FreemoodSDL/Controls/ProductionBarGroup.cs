using System;
using System.Collections.Generic;

using FreemooSDL.Collections;

namespace FreemooSDL.Controls
{
    public delegate void ProductionBarUpdateDelegate(ProductionBarUpdateArgs args);

    public class ProductionBarGroup
        : AbstractControl
    {

        public event ProductionBarUpdateDelegate OnProductionBarUpdate;

        private ProductionBarUpdateArgs _updateArgs = new ProductionBarUpdateArgs();
        

        public override void Update(FreemooTimer pTimer)
        {
            for (int i = 0; i < Controls.count(); i++)
            {
                Controls.get(i).Update(pTimer);
            }
        }

        public override void Draw(FreemooTimer pTimer, Service.GuiService pGuiService)
        {
            for (int i = 0; i < Controls.count(); i++)
            {
                Controls.get(i).Draw(pTimer, pGuiService);
            }
        }

        public override void Release()
        {
            for (int i = 0; i < Controls.count(); i++)
            {
                Controls.get(i).Release();
            }
        }

        public void ClickUp(string id)
        {

        }

        public void ClickDown(string id)
        {

        }

        public void ClickBar(string id, int delta)
        {

        }

        public override void mousePressed(SdlDotNet.Input.MouseButtonEventArgs pMbea)
        {
            for (int i = 0; i < Controls.count(); i++)
            {
                Controls.get(i).mousePressed(pMbea);
            }
        }

        public override void mouseReleased(SdlDotNet.Input.MouseButtonEventArgs pMbea)
        {
            for (int i = 0; i < Controls.count(); i++)
            {
                Controls.get(i).mouseReleased(pMbea);
            }
        }

        public void HandleProductionBarChange(ProductionBarEventArgs prodBarEvt)
        {
            Console.WriteLine(prodBarEvt.ID + " sent a delta of " + prodBarEvt.Delta);
            int delta = prodBarEvt.Delta * 4;
            int prodIdx = (int)prodBarEvt.ProdType;
            // there is probably a better way to do this but for now...
            ProductionBar[] prodBars = new ProductionBar[5];

            //int index = 0;
            foreach (var ctr in Controls)
            {
                if (ctr.Value is ProductionBar)
                {
                    var prodCtr = (ProductionBar)ctr.Value;
                    prodBars[(int)prodCtr.ProdType] = (ProductionBar)ctr.Value;
                    //index++;
                }
            }
            int[] order = { 4, 2, 3, 0, 1 };
            int newVal = prodBarEvt.Sender.Value - delta;
            int leftOver = newVal - prodBarEvt.Sender.Value;

            if (leftOver < 0)
            {
                foreach (int pk in order)
                {
                    if (leftOver < 0)
                    {
                        if (!prodBars[pk].Locked && pk != prodIdx)
                        {
                            if ((prodBars[pk].Value - leftOver) <= 100)
                            {
                                prodBars[pk].Value -= leftOver;
                                leftOver = 0;
                            }
                            else
                            {
                                leftOver -= (100 - prodBars[pk].Value);
                                prodBars[pk].Value = 100;
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
                        if (!prodBars[pk].Locked && pk != prodIdx)
                        {
                            if ((prodBars[pk].Value - leftOver) >= 0)
                            {
                                prodBars[pk].Value -= leftOver;
                                leftOver = 0;
                            }
                            else
                            {
                                leftOver -= (prodBars[pk].Value);
                                prodBars[pk].Value = 0;
                            }
                        }
                    }
                }
            }

            // now update the triggering bar
            prodBarEvt.Sender.Value = newVal;

            if (OnProductionBarUpdate != null)
            {
                //_updateArgs.prodValues = prodBars;
                _updateArgs.prodValues.Clear();
                foreach(var p in prodBars)
                {
                    _updateArgs.prodValues.Add(new Tuple<ProductionEnum, int, bool>(p.ProdType, p.Value, p.Locked ));
                }
                OnProductionBarUpdate(_updateArgs);
            }

            // now write the new values to the planet ref
            //PlanetaryProduction pp = mPlanetRef.Production;
            //pp.Ship.Value = mProductionBars[0].Value;
            //pp.Defense.Value = mProductionBars[1].Value;
            //pp.Industry.Value = mProductionBars[2].Value;
            //pp.Ecology.Value = mProductionBars[3].Value;
            //pp.Technology.Value = mProductionBars[4].Value;
            //mPlanetRef.Production = pp;
        }
    }

    public class ProductionBarUpdateArgs
        : EventArgs
    {
        public List<Tuple<ProductionEnum, int, bool>> prodValues = new List<Tuple<ProductionEnum,int, bool>>();
    }
}
