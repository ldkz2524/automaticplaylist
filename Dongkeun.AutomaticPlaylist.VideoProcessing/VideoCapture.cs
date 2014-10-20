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
            try
            {
                IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link, false);

                Video = videoInfos.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 360);

                if (Video.RequiresDecryption)
                    DownloadUrlResolver.DecryptDownloadUrl(Video);
            }
            catch (Exception e)
            {
                this.Link = string.Empty;
                Logger.RecordError(e.ToString() + " : " + link);
            }

            FilePath = string.Empty;
        }

        ~VideoCapture()
        {
            try
            {
                if (FilePath != string.Empty)
                    System.IO.File.Delete(FilePath);
            }
            catch { }
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
            if (this.Link == string.Empty)
                return;

            try
            {
                this.FilePath = Path.Combine(TempPath, RemoveIllegalPathCharacters(this.Link.Substring(this.Link.IndexOf("?")) + Video.VideoExtension));

                VideoDownloader videoDownloader = new VideoDownloader(Video, this.FilePath);

                videoDownloader.Execute();
            }
            catch (Exception e)
            {
                Logger.RecordError(e.ToString() + " : " + this.FilePath + " : " + this.Link);
                this.FilePath = string.Empty;
                this.Link = string.Empty;
            }
        }

        public bool IsSong(int numOfFramesToCheck)
        {
            if (numOfFramesToCheck <= 1)
                return false;

            if (FilePath == string.Empty)
                return false;

            try
            {
                using (Capture capture = new Capture(FilePath))
                {
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
