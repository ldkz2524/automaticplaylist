using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dongkeun.AutomaticPlaylist.Log;

namespace Dongkeun.AutomaticPlaylist.Helper
{
    public static class HelperParser
    {
        public static string RetrieveParsedString(ref string text, string startKey, string endKey)
        {
            try
            {
                text = text.Substring(text.IndexOf(startKey) + startKey.Length);
                return text.Substring(0, text.IndexOf(endKey)).Trim();
            }
            catch (Exception e)
            {
                Logger.RecordError(e.ToString() + " : " + startKey + " : " + endKey + " : " + text);
                return string.Empty;
            }
        }

        public static string RetrieveParsedString(string text, string startKey, string endKey)
        {
            try
            {
                text = text.Substring(text.IndexOf(startKey) + startKey.Length);
                return text.Substring(0, text.IndexOf(endKey)).Trim();
            }
            catch (Exception e)
            {
                Logger.RecordError(e.ToString() + " : " + startKey + " : " + endKey + " : " + text);
                return string.Empty;
            }
        }
    }
}
