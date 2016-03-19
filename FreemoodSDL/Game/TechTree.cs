using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace FreeMoO.Game
{
    // not really a tree but it is a dictionary of all technologies
    public class TechTree
    {
        private List<Technology> mTechList = new List<Technology>();

        public TechTree()
        {
            loadXml();
        }

        // lookup by either name or level/type
        public Technology getByName(string pName)
        {
            Technology ret = (Technology)mTechList.FirstOrDefault(x => x.Name == pName);
            return ret;
        }

        public Technology getByLevelAndType(TechTypeEnum pType, int pLevel)
        {
            return (Technology)mTechList.FirstOrDefault(x => x.Level == pLevel && x.TechType == pType);
        }

        private void loadXml()
        {
            // I  think the config is going to  have to be static rather  than an instancee in the game class
            //string fn = "C:\\Users\\Daniel\\Documents\\Visual Studio 2010\\Projects\\FreemoodSDL\\data\\freemoo.xml";
            string fn = Config.DataFolder + "\\rules\\tech.xml";

            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(fn);
            mTechList = new List<Technology>();
            XmlNodeList nodes = xdoc.SelectNodes("/Freemoo/technology/tech");
            int currId = 0;
            foreach (XmlNode node in nodes)
            {

                Technology t = new Technology(currId++);
                int level = int.Parse(node.Attributes["level"].Value);
                TechTypeEnum ty = decodeType(node.Attributes["type"].Value);
                t.TechType = ty;
                t.Level = level;
                t.Name = node.Attributes["name"].Value;
                if (node.Attributes["description"] != null)
                {
                    t.Description = node.Attributes["description"].Value;
                }
                mTechList.Add(t);
            }
        }

        private TechTypeEnum decodeType(string pType)
        {
            TechTypeEnum retval = TechTypeEnum.Construction;
            switch (pType)
            {
                case "computer":
                    retval = TechTypeEnum.Computer;
                    break;
                case "construction":
                    retval  = TechTypeEnum.Construction;
                    break;
                case  "shield":
                    retval =TechTypeEnum.ForceFields;
                    break;
                case "planetology":
                    retval = TechTypeEnum.Planetology;
                    break;
                case "propulsion":
                    retval =TechTypeEnum.Propulsion;
                    break;
                case "weapons":
                    retval = TechTypeEnum.Weapons;
                    break;
            }
            return retval;
        }

        public List<int> GetRoboticTechs()
        {
            return (from t in mTechList
                   where t.Description.Contains("Improved Robotic Controls")
                   select t.Level).ToList();
        }
    }
}
