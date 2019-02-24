using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTracking.Models
{
    public class Factory
    {
        public string Name { get; set; }
        public string ImageSource { get; set; }

        public Factory(string name, string imageSource)
        {
            Name = name;
            ImageSource = imageSource;
        }
    }
}
