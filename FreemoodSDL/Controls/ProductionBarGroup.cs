using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
