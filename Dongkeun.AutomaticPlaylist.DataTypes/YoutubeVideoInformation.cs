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

        public string Url
        {
            get;
            set;
        }

        public YoutubeVideoInformation()
        {
            this.Title = string.Empty;
            this.Url = string.Empty;
        }

        public YoutubeVideoInformation(string title, string url)
        {
            this.Title = title;
            this.Url = url;
        }

    }
}
