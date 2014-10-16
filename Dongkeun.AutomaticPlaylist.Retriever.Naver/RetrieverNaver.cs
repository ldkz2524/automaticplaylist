using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dongkeun.AutomaticPlaylist.DataTypes;
using Dongkeun.AutomaticPlaylist.Crawler.Naver;
using Dongkeun.AutomaticPlaylist.Log;
using Dongkeun.AutomaticPlaylist.Parser.Naver;

namespace Dongkeun.AutomaticPlaylist.Retriever.Naver
{
    public class RetrieverNaver
    {
        public List<SongInformation> RetrieveSongList (string url)
        {
            string output = string.Empty;

            if (new CrawlerNaver().IsWebpageAvailable(url, ref output) == false)
                return new List<SongInformation>();
            else
                return ParserNaver.RetrieveSongList(output);

        }
    }
}
