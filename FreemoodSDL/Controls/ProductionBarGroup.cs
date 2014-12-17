using System;

using FreemooSDL.Collections;

namespace FreemooSDL.Controls
{
    public class ProductionBarGroup
        : AbstractControl
    {

        public override void update(FreemooTimer pTimer)
        {
            for (int i = 0; i < Controls.count(); i++)
            {
                Controls.get(i).update(pTimer);
            }
        }

        public override void draw(FreemooTimer pTimer, Service.GuiService pGuiService)
        {
            for (int i = 0; i < Controls.count(); i++)
            {
                Controls.get(i).draw(pTimer, pGuiService);
            }
        }

        public override void Release()
        {
            for (int i = 0; i < Controls.count(); i++)
            {
                Controls.get(i).Release();
            }
        }

        // who needs events? um, I do because it's probably cheaper than casting things all the fucking time
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
}
