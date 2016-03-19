using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace FreeMoO.Game
{
    public class LookupVals
    {
        // just a collection of static values that are hard coded in the original (or appear to be) but 
        // I am storing them in xml.

        private static string mDataFolder = "C:\\Users\\Daniel\\Documents\\Visual Studio 2010\\Projects\\FreemoodSDL\\data";

        private static Dictionary<RacialEnum, Dictionary<TechTypeEnum, float>> mRacialTechModifiers = null;

        public static float getTechModifier(RacialEnum pRace, TechTypeEnum pTech)
        {
            if (mRacialTechModifiers == null)
            {
                string fn = mDataFolder + "\\freemoo.xml";
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(fn);
                XmlNodeList nodes = xdoc.SelectNodes("/Freemoo/RacialTechModifiers/race");
                mRacialTechModifiers = new Dictionary<RacialEnum, Dictionary<TechTypeEnum, float>>();
                foreach (XmlNode node in nodes)
                {
                    string raceName = string.Empty;
                    Dictionary<TechTypeEnum, float> traits = new Dictionary<TechTypeEnum, float>();
                    foreach (XmlAttribute a in node.Attributes)
                    {
                        if (a.Name.Equals("name"))
                        {
                            raceName = a.Value;
                        }
                        else
                        {
                            TechTypeEnum t = (TechTypeEnum)Enum.Parse(typeof(TechTypeEnum), a.Name);
                            float f = float.Parse(a.Value);
                            traits.Add(t, f);
                        }
                    }
                    RacialEnum r = (RacialEnum)Enum.Parse(typeof(RacialEnum), raceName);
                    mRacialTechModifiers.Add(r, traits);
                }
            }

            return mRacialTechModifiers[pRace][pTech];
        }
    }
}
