using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VRP.Models
{
    public class AlgorithmsTestViewModel
    {
        public List<Node> GAPath { get; set; }
        public List<Node> ClarkeWrightPath { get; set; }
        public double GAPathCost { get; set; }
        public double ClarkeWrightPathCost { get; set; }

        /// <summary>
        /// In miliseconds
        /// </summary>
        public double GAExecutionTime { get; set; }

        /// <summary>
        /// In miliseconds
        /// </summary>
        public double ClarkeWrightExecutionTime { get; set; }
    }
}