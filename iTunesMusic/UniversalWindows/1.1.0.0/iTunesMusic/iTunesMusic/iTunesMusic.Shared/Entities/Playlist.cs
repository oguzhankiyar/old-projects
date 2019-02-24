using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using iTunesMusic.Tools;
using iTunesMusic.Entities.EntityListItems;

namespace iTunesMusic.Entities
{
    class Playlist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ImageSource ImageSource { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual List<SongListItem> Songs { get; set; }

        public Playlist()
        {
            this.Id = Database.Playlists.Any() ? Database.Playlists.Last().Id + 1 : 1;
        }
    }
}
