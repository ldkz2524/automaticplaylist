using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Drawing;
using Dongkeun.AutomaticPlaylist.Log;
using XnaFan.ImageComparison;
using YoutubeExtractor;
using System.IO;
using Dongkeun.AutomaticPlaylist.DataTypes;

namespace Dongkeun.AutomaticPlaylist.VideoProcessing
{

    public class VideoCapture
    {
        private const string TempPath = @"C:\Users\Dongkeun\Desktop\TempSong\";

        private const float DifferenceAllowance = 0.05f;

        public string FilePath
        {
            get;
            set;
        }

        public string Link
        {
            get;
            set;
        }

        public VideoInfo Video
        {
            get;
            set;
        }

        public VideoCapture(string link)
        {
            this.Link = link;

            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link, false);

            Video = videoInfos.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 360);

            if (Video.RequiresDecryption)
                DownloadUrlResolver.DecryptDownloadUrl(Video);

            FilePath = string.Empty;
        }

        ~VideoCapture()
        {
            System.IO.File.Delete(FilePath);
        }

        private Image<Bgr, Byte> GetVideoFrame(Capture capture, double frameNum)
        {
            capture.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_POS_FRAMES, frameNum);
            return capture.QueryFrame();
        }

        private string RemoveIllegalPathCharacters(string path)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new System.Text.RegularExpressions.Regex(string.Format("[{0}]", System.Text.RegularExpressions.Regex.Escape(regexSearch)));
            return r.Replace(path, "");
        }

        public void DownloadVideo()
        {
            FilePath = Path.Combine(TempPath, RemoveIllegalPathCharacters(this.Link.Substring(this.Link.IndexOf("?")) + Video.VideoExtension));

            VideoDownloader videoDownloader = new VideoDownloader(Video, FilePath);

            videoDownloader.Execute();
        }

        public bool IsSong(int numOfFramesToCheck)
        {
            try
            {
                using (Capture capture = new Capture(FilePath))
                {
                    if (numOfFramesToCheck <= 1)
                        return false;

                    List<Image> imageList = new List<Image>();
                    double totalFrames = capture.GetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_COUNT);

                    for (int i = 0; i < numOfFramesToCheck; i++)
                        imageList.Add(this.GetVideoFrame(capture, i * totalFrames / numOfFramesToCheck).ToBitmap());


                    for (int i = 0; i < imageList.Count; i++)
                    {
                        for (int j = i + 1; j < imageList.Count; j += 2)
                        {
                            if (imageList[i].PercentageDifference(imageList[j]) > DifferenceAllowance)
                                return false;
                        }
                    }

                    return true;
                }
            }
            catch (Exception e)
            {
                Logger.RecordError(e.ToString() + " : " + FilePath);
                return false;
            }
        }
    }
}
