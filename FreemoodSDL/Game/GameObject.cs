using System;


namespace FreemooSDL.Game
{
    public abstract class GameObject
    {
        private int mId;
        private string mName;

        public string Name
        {
            get
            {
                return mName;
            }
            set
            {
                mName = value;
            }
        }

        public int ID
        {
            get
            {
                return mId;
            }
        }

        public bool CanDelete { get; set; } // helper field if when looping through a set of objects you want to delete some of them post loop

        public GameObject(int pId, string pName)
        {
            mId = pId;
            mName = pName;
            CanDelete = false;
        }

        public GameObject(int pId)
            : this(pId, string.Empty)
        {
            
        }
    }
}
