using System;
using System.Collections.Generic;
using System.Text;
using iTunesMusic.Entities.EntityListItems;

namespace iTunesMusic.Entities
{
    class Album
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ImageSource ImageSource { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Genre Genre { get; set; }
        public string Price { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual List<SongListItem> Songs { get; set; }

        public Album()
        {
            this.Songs = new List<SongListItem>();
        }
    }
}
