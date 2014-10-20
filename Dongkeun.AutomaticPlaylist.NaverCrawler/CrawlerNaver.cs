using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Dongkeun.AutomaticPlaylist.Log;

namespace Dongkeun.AutomaticPlaylist.Crawler.Naver
{
    public class CrawlerNaver
    {
        public int TimeOut
        {
            get;
            set;
        }

        public CrawlerNaver(int timeOut = 30000)
        {
            this.TimeOut = timeOut;
        }

        public bool IsWebpageAvailable(string url, ref string output)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ReadWriteTimeout = TimeOut;
                request.Timeout = TimeOut;
                request.Method = "GET";
                request.ContentLength = 0;
                request.AutomaticDecompression = DecompressionMethods.GZip;
                request.KeepAlive = true;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream resStream = response.GetResponseStream())
                        {
                            using (StreamReader readStream = new StreamReader(resStream, Encoding.UTF8))
                            {
                                output = readStream.ReadToEnd();
                            }
                        }
                    }
                    else
                    {
                        output = string.Empty;
                        Logger.RecordError("Naver Music Webpage Http Status Not 200");
                        return false;
                    }
                }

                return true;
            }
            catch (System.Net.WebException e)
            {
                output = string.Empty;
                Logger.RecordError(url + " : " + e.ToString());
                return false;
            }
        }
    }
}
