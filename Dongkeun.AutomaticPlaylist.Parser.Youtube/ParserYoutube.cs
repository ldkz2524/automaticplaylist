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

            string channelContextStartKey = "<ul class=\"yt-lockup-meta-info\"><li>";
            string channelContextEndKey = "</li>";

            string channelStartKey = ">";
            string channelEndKey = "</a>";

            string viewCountStartKey = "ago</li><li>";
            string viewCountEndKey = "views</li>";

            string officialBadgeKey = "<span class=\"yt-badge \" >Official</span>";

            inputHtml = HttpUtility.HtmlDecode(inputHtml);

            string resultContext = HelperParser.RetrieveParsedString(inputHtml, resultContextStartKey, resultContextEndKey);

            while (resultContext.IndexOf(titleContextStartKey) != -1)
            {
                string titleContext = HelperParser.RetrieveParsedString(ref resultContext, titleContextStartKey, titleContextEndKey);

                string url = "https://www.youtube.com" + HelperParser.RetrieveParsedString(ref titleContext, urlStartKey, urlEndKey);

                string title = HelperParser.RetrieveParsedString(titleContext, titleStartKey, titleEndKey);

                string channelContext = HelperParser.RetrieveParsedString(ref resultContext, channelContextStartKey, channelContextEndKey);

                string channel = HelperParser.RetrieveParsedString(channelContext, channelStartKey, channelEndKey);

                string viewCount = HelperParser.RetrieveParsedString(ref resultContext, viewCountStartKey, viewCountEndKey);

                if (viewCount == "No")
                    viewCount = "0";

                Boolean isOfficial = false;

                if (resultContext.IndexOf(titleContextStartKey) == -1)
                {
                    if (resultContext.IndexOf(officialBadgeKey) != -1)
                        isOfficial = true;
                }
                else 
                {
                    if (resultContext.IndexOf(titleContextStartKey) > resultContext.IndexOf(officialBadgeKey) && resultContext.IndexOf(officialBadgeKey) != -1)
                        isOfficial = true;
                }

                try
                {
                    newVideoList.Add(new YoutubeVideoInformation(title, channel, int.Parse(viewCount,System.Globalization.NumberStyles.AllowThousands), isOfficial, url));
                }
                catch (Exception e)
                {
                    Logger.RecordError(e.ToString() + " : " + title + " : " + channel + " : " + viewCount + " : " + isOfficial + " : " + url);
                }
            }

            return newVideoList;
        }

    }
}
