using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace FreeMoO.Reverse
{
    /*
     * The file Config.MOO contains a list of the names of the save games.  it might contain other stuff, not sure yet
     * 00 - 21 : ???
     * 22 - 35 : Save Game 1 name, zero terminated string
     * 36 - ## : Save game names are max 0x14 characters wide, probably limited to 0x13 so the last character can be zero
     */
    public class ConfigMoo
    {
        private string _dataFolder = string.Empty;
        private string _configFileName = string.Empty;
        private string[] _fileNameList = new string[6];

        private const string CONFIG_FILE = "CONFIG.MOO";
        private const int FILE_NAME_LENGTH = 0x14;
        private const int FILE_NAME_OFFSET = 0x22;

        public ConfigMoo()
        {
            _configFileName = Config.DataFolder.PathCmb(CONFIG_FILE); // string.Format("{0}\\{1}", Config.DataFolder, CONFIG_FILE);
            ReadFile();
        }

        public void ReadFile()
        {
            Debug.Assert(File.Exists(_configFileName), "Config.Moo is missing.");
            using (BinaryReader br = new BinaryReader(new FileStream(_configFileName, FileMode.Open, FileAccess.Read)))
            {
                br.BaseStream.Seek(FILE_NAME_OFFSET, SeekOrigin.Begin);
                for (int i = 0; i < 6; i++)
                {
                    _fileNameList[i] = string.Empty;
                    byte[] raw = new byte[0x14];
                    int bytesRead = br.Read(raw, 0, FILE_NAME_LENGTH);
                    var fn = raw.GetZString();
                    _fileNameList[i] = fn;
                }
            }
        }

        public string this[int idx]
        {
            get
            {
                Debug.Assert(idx > 0 && idx <= 6);
                return _fileNameList[idx - 1];
            }
        }

        public int Count
        {
            get
            {
                return 6;
            }
        }
    }
}
