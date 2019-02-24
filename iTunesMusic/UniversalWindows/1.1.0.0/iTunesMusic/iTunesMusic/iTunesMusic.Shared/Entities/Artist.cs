using System;
using System.Collections.Generic;
using System.Text;
using iTunesMusic.Entities.EntityListItems;

namespace iTunesMusic.Entities
{
    class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ImageSource ImageSource { get; set; }
        public Genre Genre { get; set; }

        public virtual List<AlbumListItem> Albums { get; set; }
        public virtual List<SongListItem> Songs { get; set; }

        public Artist()
        {
            this.Albums = new List<AlbumListItem>();
            this.Songs = new List<SongListItem>();
        }
    }
}
