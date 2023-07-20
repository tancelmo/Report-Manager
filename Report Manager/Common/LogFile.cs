using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report_Manager.Common
{
    internal class LogFile
    {
        public static void Write(string errMessage, string localExeption)
        {
            using (StreamWriter writer = new StreamWriter(Globals.ConfigPath + "\\Log.txt", append: true))
            {
                writer.Write("[" + DateTime.Now + "]: " + errMessage + " " + localExeption + "\n");
            }
        }
    }
}
