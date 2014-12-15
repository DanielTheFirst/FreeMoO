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
        }
    }
}
