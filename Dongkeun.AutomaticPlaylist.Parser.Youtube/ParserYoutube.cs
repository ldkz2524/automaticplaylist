using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dongkeun.AutomaticPlaylist.Log;
using Dongkeun.AutomaticPlaylist.DataTypes;
using Dongkeun.AutomaticPlaylist.Helper;
using System.Web;

namespace Dongkeun.AutomaticPlaylist.Parser.Youtube
{
    public static class ParserYoutube
    {
        public static List<YoutubeVideoInformation> RetrieveVideoList(string inputHtml)
        {
            List<YoutubeVideoInformation> newVideoList = new List<YoutubeVideoInformation>();

            string resultContextStartKey = "<ol class=\"item-section\">";
            string resultContextEndKey = "</ol>";

            string titleContextStartKey = "<h3 class=\"yt-lockup-title\">";
            string titleContextEndKey = "</h3>";

            string urlStartKey = "href=\"";
            string urlEndKey = "\"";

            string titleStartKey = "title=\"";
            string titleEndKey = "\"";

            string officialBadgeKey = "<span class=\"yt-badge \" >Official</span>";

            inputHtml = HttpUtility.HtmlDecode(inputHtml);

            string resultContext = HelperParser.RetrieveParsedString(inputHtml, resultContextStartKey, resultContextEndKey);

            while (resultContext.IndexOf(titleContextStartKey) != -1)
            {
                string titleContext = HelperParser.RetrieveParsedString(ref resultContext, titleContextStartKey, titleContextEndKey);

                string url = "https://www.youtube.com" + HelperParser.RetrieveParsedString(ref titleContext, urlStartKey, urlEndKey);

                string title = HelperParser.RetrieveParsedString(titleContext, titleStartKey, titleEndKey);

                try
                {
                    newVideoList.Add(new YoutubeVideoInformation(title, url));
                }
                catch (Exception e)
                {
                    Logger.RecordError(e.ToString() + " : " + title + " : "  + url);
                }
            }

            return newVideoList;
        }

    }
}
