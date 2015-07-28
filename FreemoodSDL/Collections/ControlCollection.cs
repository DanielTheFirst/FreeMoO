using System;
using System.Collections.Generic;
using System.Linq;

using FreemooSDL.Controls;

namespace FreemooSDL.Collections
{
    public class ControlCollection
    {
        // this should be a subclass of the dictionary
        private Dictionary<string, IControl> mControls = new Dictionary<string,IControl>();
        private List<string> mKeys = new List<string>();

        public ControlCollection()
        {
            
        }
        
        public void add(IControl pCtrl)
        {
            // assert pctrl is not null and thaat id is not empty
            mKeys.Add(pCtrl.Id);
            mControls.Add(pCtrl.Id, pCtrl);
        }

        public Dictionary<string, IControl>.Enumerator GetEnumerator()
        {
            return mControls.GetEnumerator();
        }

        public IEnumerable<KeyValuePair<string, IControl>> GetSafeRemove()
        {
            var test = from c in mControls
                       where c.Value.SafeRemove == true
                       select c;
            return test;
        }

        public IControl get(string pId)
        {
            // assert that the internal dict has this key
            return mControls[pId];
        }

        public IControl get(int pIdx)
        {
            if (mControls.ContainsKey(mKeys[pIdx]))
            {
                return mControls[mKeys[pIdx]];
            }
            return null;
        }

        public void remove(string pId)
        {
            mControls.Remove(pId);
        }

        public void remove(int pIdx)
        {
            mControls.Remove(mKeys[pIdx]);
            mKeys.RemoveAt(pIdx);
        }

        public void clear()
        {
            mControls.Clear();
            mKeys.Clear();
        }

        public int count()
        {
            return mControls.Count;
        }
    }
}
