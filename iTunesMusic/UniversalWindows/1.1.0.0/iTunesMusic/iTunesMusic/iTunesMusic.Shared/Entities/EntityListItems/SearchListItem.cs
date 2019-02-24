using System;
using System.Collections.Generic;
using System.Text;

namespace iTunesMusic.Entities.EntityListItems
{
    class SearchListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public string Type { get; set; }
        public ImageSource ImageSource { get; set; }
    }
}
