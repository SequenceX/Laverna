using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Laverna
{
    public  class INIHandler
    {    /// Create a New INI file to store or load data
        private string path;
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public INIHandler(string INIPath)
        {        /// INIFile Constructor.
            path = INIPath;
        }

        public void IniWriteValue(string Section, string Key, string Value)
        {/// Write Data to the INI File
            WritePrivateProfileString(Section, Key, Value, this.path);
        }
        public string IniReadValue(string Section, string Key)
        {// Read Data Value From the Ini File
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.path);
            return temp.ToString();
        }


     
    }
}
