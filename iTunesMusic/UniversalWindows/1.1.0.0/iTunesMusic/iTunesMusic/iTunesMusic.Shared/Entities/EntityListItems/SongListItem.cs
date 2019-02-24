using System;
using System.Collections.Generic;
using System.Text;

namespace iTunesMusic.Entities.EntityListItems
{
    class SongListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ArtistName { get; set; }
        public DateTime ReleaseDate { get; set; }
        public ImageSource ImageSource { get; set; }
        public int Sort { get; set; }
    }
}
