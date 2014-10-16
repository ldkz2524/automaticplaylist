using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dongkeun.AutomaticPlaylist.Crawler.Naver;
using Dongkeun.AutomaticPlaylist.Log;
using System.IO;
using Dongkeun.AutomaticPlaylist.Parser.Naver;
using Dongkeun.AutomaticPlaylist.Parser.Youtube;
using Dongkeun.AutomaticPlaylist.DataTypes;
using Dongkeun.AutomaticPlaylist.Retriever.Naver;
using Dongkeun.AutomaticPlaylist.Retriever.Youtube;
using Dongkeun.AutomaticPlaylist.Crawler.Youtube;

namespace Dongkeun.AutomaticPlaylist.TestBench
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = @"http://music.naver.com/listen/top100.nhn?domain=TOTAL&duration=1h";
            string youtubeUrl = @"http://www.youtube.com/results?search_query=%EB%82%B4+%EB%A7%88%EC%9D%8C%EC%9D%B4+%EB%AD%90%EA%B0%80+%EB%8F%BC";

            List<SongInformation> naverSongList = new RetrieverNaver().RetrieveSongList(url);

            List<YoutubeVideoInformation> youtubeVideoList = new RetrieverYoutube().RetrieveVideoList(youtubeUrl);

            foreach (SongInformation song in naverSongList)
            {
                Console.WriteLine(song.Rank + " : " + song.Title + " : " + song.Artist);
            }

            //foreach (YoutubeVideoInformation video in youtubeVideoList)
            //{
            //    Console.WriteLine(video.Title + " : " + video.Channel + " : " + video.ViewCount + " : " + video.IsOfficial + " : " + video.Url);
            //}

            Console.Read();
        }
    }
}
