using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using FreeMoO;

namespace FreeMoO.Collections
{
    public class ImageCollection
    {
        private Dictionary<ArchiveEnum, Dictionary<string, Image>> mImages = new Dictionary<ArchiveEnum, Dictionary<string, Image>>();

        public ImageCollection()
        {
        }

        public void add(ArchiveEnum pArchive, string pImageName, Image pImage)
        {
            if (!mImages.ContainsKey(pArchive))
            {
                Dictionary<string, Image> tmp = new Dictionary<string, Image>();
                mImages.Add(pArchive, tmp);
            }
            while (mImages[pArchive].ContainsKey(pImageName))
            {
                pImageName = pImageName + "2";
            }
            mImages[pArchive].Add(pImageName, pImage);
        }

        public Image get(ArchiveEnum pArchive, string pImageName)
        {
            //Debug.Assert(mImages.ContainsKey(pArchive), "That archive " + pArchive.ToString() + " is not found in this collection...but it shoudl be.");
            //Debug.Assert(mImages[pArchive].ContainsKey(pImageName), "That image " + pImageName + " is not found in this collection.");
            return mImages[pArchive][pImageName];
        }

        public Image this[ArchiveEnum pArchive, string pImageName]
        {
            get
            {
                return this.get(pArchive, pImageName);
            }
        }

        public void delete(ArchiveEnum pArchive, string pImageName)
        {
            Debug.Assert(mImages.ContainsKey(pArchive), "That archive " + pArchive.ToString() + " is not found in this collection...but it shoudl be.");
            Debug.Assert(mImages[pArchive].ContainsKey(pImageName), "That image " + pImageName + " is not found in this collection.");
            mImages[pArchive].Remove(pImageName);
        }

        public void delete(ArchiveEnum pArchive)
        {
            Debug.Assert(mImages.ContainsKey(pArchive), "That archive is not found in this collection...but it shoudl be.");
            mImages[pArchive].Clear();
            mImages.Remove(pArchive);
        }
    }
}
