using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VRP.Models
{
    public class Chromosome
    {
        public List<int> NodeIndexList { get; set; }
        public double FitnessValue { get; set; }

        public Chromosome()
        {
            NodeIndexList = new List<int>();
            FitnessValue = 1000000;
        }

        public Chromosome DeepthCopy()
        {
            Chromosome copy = new Chromosome();

            copy.FitnessValue = FitnessValue;

            foreach (var item in NodeIndexList)
            {
                copy.NodeIndexList.Add(item);
            }

            return copy;
        }
    }
}