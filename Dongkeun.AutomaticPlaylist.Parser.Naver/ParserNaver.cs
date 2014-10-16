using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dongkeun.AutomaticPlaylist.Log;
using Dongkeun.AutomaticPlaylist.DataTypes;
using Dongkeun.AutomaticPlaylist.Helper;
using System.Web;

namespace Dongkeun.AutomaticPlaylist.Parser.Naver
{
    public static class ParserNaver
    {
        public static List<SongInformation> RetrieveSongList(string inputHtml)
        {
            List<SongInformation> newSongList = new List<SongInformation>();

            string songStartKey = "<tr class=\"_tracklist_move data";

            string rankContextStartKey = "<td class=\"ranking\">";
            string rankContextEndKey = "</td>";

            string rankDigitStartKey = "<span class=\"num";
            string rankDigitEndKey = "\">";

            string titleContextStartKey = "_title title";
            string titleContextEndKey = "</td>";

            string titleStartKey = "title=\"";
            string titleEndKey = "\" >";

            string artistContextStartKey = "_artist artist";
            string artistContextEndKey = "</td>";

            string artistStartKey1 = "title=\"";
            string artistEndKey1 = "\">";

            string artistStartKey2 = "\">";
            string artistEndKey2 = "/a";

            inputHtml = HttpUtility.HtmlDecode(inputHtml);

            while (inputHtml.IndexOf(songStartKey) != -1)
            {
                string rankContext = HelperParser.RetrieveParsedString(ref inputHtml, rankContextStartKey, rankContextEndKey);
                string rank = string.Empty;
                while (rankContext.IndexOf(rankDigitStartKey) != -1)
                {
                    rank += HelperParser.RetrieveParsedString(ref rankContext, rankDigitStartKey, rankDigitEndKey);
                }

                string title = HelperParser.RetrieveParsedString(HelperParser.RetrieveParsedString(ref inputHtml, titleContextStartKey, titleContextEndKey), titleStartKey, titleEndKey);

                string artistContext = HelperParser.RetrieveParsedString(ref inputHtml, artistContextStartKey, artistContextEndKey);

                string artist = HelperParser.RetrieveParsedString(artistContext, artistStartKey1, artistEndKey1);

                //in case multiple artists exists, retrieve the first one
                if (artist.IndexOf("layerbtn") != -1)
                {
                    artist = HelperParser.RetrieveParsedString(HelperParser.RetrieveParsedString(artistContext, artistStartKey2, artistEndKey2), ">", "<");
                }
                try
                {
                    newSongList.Add(new SongInformation(Convert.ToInt32(rank), title, artist));
                }
                catch(Exception e)
                {
                    Logger.RecordError(e.ToString() + " : " + rank + " : " + title + " : " + artist);
                }

            }

            return newSongList;
        }
    }
}
