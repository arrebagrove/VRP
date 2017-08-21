using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VRP.Models
{
    public class Edge
    {
        public int Id { get; set; }
        public string StartNodeId { get; set; }
        public string EndNodeId { get; set; }
        public Dictionary<string, string> Dict { get; set; }

        public Edge() { }

        public Edge(string startNodeId, string endNodeId)
        {
            StartNodeId = startNodeId;
            EndNodeId = endNodeId;
        }
    }
}