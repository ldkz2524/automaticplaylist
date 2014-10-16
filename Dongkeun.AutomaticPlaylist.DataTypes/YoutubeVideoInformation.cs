using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dongkeun.AutomaticPlaylist.DataTypes
{
    public class YoutubeVideoInformation
    {
        public string Title
        {
            get;
            set;
        }

        public string Channel
        {
            get;
            set;
        }

        public int ViewCount
        {
            get;
            set;
        }

        public bool IsOfficial
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public YoutubeVideoInformation()
        {
            this.Title = string.Empty;
            this.Channel = string.Empty;
            this.ViewCount = 0;
            this.IsOfficial = false;
            this.Url = string.Empty;
        }

        public YoutubeVideoInformation(string title, string channel, int viewCount, bool isOfficial, string url)
        {
            this.Title = title;
            this.Channel = channel;
            this.ViewCount = viewCount;
            this.IsOfficial = isOfficial;
            this.Url = url;
        }

    }
}
