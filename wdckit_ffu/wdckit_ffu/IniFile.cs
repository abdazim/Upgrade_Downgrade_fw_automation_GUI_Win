using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace wdckit_ffu
{
    class IniFile
    {
        public string path;
        string Path = System.Environment.CurrentDirectory + "\\" + "Config" + "\\" + "ConfigFile.ini";

        //write to ini file
        //static extern used on the method you're importing from Kernel32.dll. 
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
        [DllImport("kernel32")]
        //DLLImport can be used to call any exported function in a native Windows DLL, DllImportAttribute to import kernel32

        private static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
            int size, string filePath);


        /// <summary>
        /// return the path of the ini file 
        /// </summary>
        /// <param name="Path"></param>
        public IniFile(string Path)
        {
            path = Path;
        }

        /// <summary>
        /// if ini file not exist create one and write the values inside (Section1", "Device", "'disk2'");
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }


        /// <summary>
        /// Read from ini file ,by section and key retrive value
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp,
                                            255, this.path);
            return temp.ToString();
        }
    }
}
