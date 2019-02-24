using System;
using System.Collections.Generic;
using System.Text;

namespace iTunesMusic.Entities
{
    class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public virtual List<Playlist> Playlists { get; set; }
    }
}
