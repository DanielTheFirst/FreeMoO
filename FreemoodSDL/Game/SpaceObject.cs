using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeMoO.Game
{
    public abstract class SpaceObject
        : GameObject
    {
        private int mX;
        private int mY;

        public int X
        {
            get
            {
                return mX;
            }
        }

        public int Y
        {
            get
            {
                return mY;
            }
        }

        public SpaceObject(int pId)
            : base(pId)
        {
        }

        public virtual void relocate(int px, int py)
        {
            mX = px;
            mY = py;
        }
    }
}
