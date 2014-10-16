using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Dongkeun.AutomaticPlaylist.Log
{
    public class Logger
    {
        private static string m_LogFileName = string.Empty;

        public static string LogFileName
        {
            get { return m_LogFileName; }
            set { m_LogFileName = value; }
        }

        static Logger()
        {
            LogFileName = @"C:\Users\Dongkeun\Desktop\AutomaticPlaylistLog.txt";
        }

        private static void Record(string type, string log)
        {
            //try
            //{
            //    using (StreamWriter sw = File.AppendText(LogFileName))
            //    {
            //        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + " : " + type + " : " + log);
            //    }
            //}
            //catch (Exception)
            //{
            //    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + " : Writing To Log Failed");
            //}
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + " : " + type + " : " + log);
        }

        public static void RecordError(string log)
        {
            Record("ERROR", log);
        }

        public static void RecordMessage(string log)
        {
            Record("MESSAGE", log);
        }

        public static void RecordDebug(string log)
        {
            Record("DEBUG", log);
        }
    }
}
