using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VRP.Models
{
    public class FindRouteViewModel
    {
        public List<Node> GARoute { get; set; }
        public List<Node> CWRoute { get; set; }
        public double GARouteCost { get; set; }
        public double CWRouteCost { get; set; }
        public List<Address> AddressList { get; set; }
    }
}