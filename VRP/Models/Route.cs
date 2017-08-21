using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VRP.Models
{
    public class Route
    {
        public List<Node> Path { get; set; }
        public double Cost { get; set; }
    }
}