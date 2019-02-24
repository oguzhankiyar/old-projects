using System;
using System.Collections.Generic;
using System.Text;

namespace iTunesMusic.Entities
{
    class Song
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ImageSource ImageSource { get; set; }
        public string PreviewSource { get; set; }
        public Genre Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Price { get; set; }
        public List<SongSource> Sources { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual Album Album { get; set; }
    }
}
