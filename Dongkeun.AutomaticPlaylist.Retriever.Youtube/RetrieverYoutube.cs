using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dongkeun.AutomaticPlaylist.DataTypes;
using Dongkeun.AutomaticPlaylist.Crawler.Youtube;
using Dongkeun.AutomaticPlaylist.Parser.Youtube;
using Dongkeun.AutomaticPlaylist.Log;
using System.Web;
using Dongkeun.AutomaticPlaylist.VideoProcessing;

namespace Dongkeun.AutomaticPlaylist.Retriever.Youtube
{
    public class RetrieverYoutube
    {
        public const int NumberOfFramesToCheck = 5;

        public const int NumberOfYoutubeVideosToCheck = 10;

        private const string youtubeSearchBaseUrl = @"http://www.youtube.com/results?search_query=";

        /// <summary>
        /// Return list of video information gathered from the given url
        /// </summary>
        /// <param name="url">url of Youtube search result</param>
        /// <returns></returns>
        public List<YoutubeVideoInformation> RetrieveVideoList(string searchTerm)
        {
            string output = string.Empty;

            if (new CrawlerYoutube().IsWebpageAvailable(youtubeSearchBaseUrl + HttpUtility.UrlEncode(searchTerm), ref output) == false)
                return new List<YoutubeVideoInformation>();
            else
                return ParserYoutube.RetrieveVideoList(output);
        }

        /// <summary>
        /// Receive list of videos returned from Youtuve search result and return list of 'pure' music
        /// </summary>
        /// <param name="candidateVideoList">list of videos to search music from</param>
        /// <returns></returns>
        public List<YoutubeVideoInformation> RetrieveSongList(List<YoutubeVideoInformation> candidateVideoList)
        {
            List<YoutubeVideoInformation> newVideoList = new List<YoutubeVideoInformation>();

            for (int i = 0; i < candidateVideoList.Count; i++)
            {
                VideoCapture video = new VideoCapture(candidateVideoList[i].Url);
                video.DownloadVideo();
                if (video.IsSong(NumberOfFramesToCheck) == true)
                    newVideoList.Add(candidateVideoList[i]);

                if (i == NumberOfYoutubeVideosToCheck)
                    break;
            }

            return newVideoList;

        }
    }
}
