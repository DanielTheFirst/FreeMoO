using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FreemooSDL.Collections
{
    public class ServiceCollection
    {
        Dictionary<ServiceEnum, object> mServices = new Dictionary<ServiceEnum, object>();

        public ServiceCollection()
        {
        }

        public void add(object pObject, ServiceEnum pKey)
        {
            //Debug.Assert(!mServices.ContainsKey(typeof(T)), "Can only have one instance of each service"); // yes, it's a backdoor singleton
            mServices.Add(pKey, pObject);
        }

        public object get(ServiceEnum pKey)
        {
            Debug.Assert(mServices.ContainsKey(pKey), "You did not add tthis service to the collection yet.");
            return mServices[pKey];
        }

        public object this[ServiceEnum pKey]
        {
            get
            {
                return this.get(pKey);
            }
        }

        public void delete(ServiceEnum pKey)
        {
            Debug.Assert(mServices.ContainsKey(pKey), "Trying to delete it when it isn't there.  Smells funky.");
            mServices.Remove(pKey);
        }
    }
}
