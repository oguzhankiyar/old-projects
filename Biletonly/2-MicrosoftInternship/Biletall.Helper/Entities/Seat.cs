using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biletall.Helper.Enums;

namespace Biletall.Helper.Entities
{
    public class Seat
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public SeatState State { get; set; }
        public SeatState NextState { get; set; }
        public Price Price { get; set; }
    }
}
