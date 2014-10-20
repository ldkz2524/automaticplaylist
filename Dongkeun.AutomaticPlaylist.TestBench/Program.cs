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
using Dongkeun.AutomaticPlaylist.VideoProcessing;

namespace Dongkeun.AutomaticPlaylist.TestBench
{
    class Program
    {
        static void Main(string[] args)
        {
            string naverMusicUrl = @"http://music.naver.com/listen/top100.nhn?domain=TOTAL&duration=1h";

            List<SongInformation> naverSongList = new RetrieverNaver().RetrieveSongList(naverMusicUrl);

            foreach (SongInformation song in naverSongList)
            {
                List<YoutubeVideoInformation> youtubeVideoList = new RetrieverYoutube().RetrieveVideoList(song.Title + " - " + song.Artist);

                YoutubeVideoInformation youtubeSong = new RetrieverYoutube().RetrieveSongList(youtubeVideoList);

                Console.WriteLine(youtubeSong.Title + " : " + youtubeSong.Url);
            }

            Console.Read();
        }
    }
}
