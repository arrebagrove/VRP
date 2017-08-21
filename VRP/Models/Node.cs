using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VRP.Models
{
    public class Node
    {
        public string Id { get; set; }
        public int ArrayIndex { get; set; }
        public double Latitude { get; set; } //szerokość geograficzna
        public double Longitude { get; set; } //długość geograficzna
        public Dictionary<string, string> dict { get; set; }
        public bool IsMarked { get; set; }

        public Node() { }

        public Node DeepthCopy()
        {
            Node copy = new Node();
            Dictionary<string, string> copyDict = new Dictionary<string, string>();

            copy.Id = Id;
            copy.ArrayIndex = ArrayIndex;
            copy.Latitude = Latitude;
            copy.Longitude = Longitude;
            copy.IsMarked = IsMarked;

            foreach (var item in dict)
            {
                copyDict[item.Key] = item.Value;
            }

            copy.dict = copyDict;

            return copy;
        }
    }
}