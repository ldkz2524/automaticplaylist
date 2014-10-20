using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dongkeun.AutomaticPlaylist.DataTypes
{
    public class SongInformation
    {
        public int Rank
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Artist
        {
            get;
            set;
        }

        public SongInformation()
        {
            this.Rank = 0;
            this.Title = string.Empty;
            this.Artist = string.Empty;
        }

        public SongInformation(int rank, string title, string artist)
        {
            this.Rank = rank;
            this.Title = title;
            this.Artist = artist;
        }
    }
}
