using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VRP.Models
{
    public class GASettings
    {
        public int MaxNumberOfGeneration { get; set; }
        public int PopulationSize { get; set; }
        public int SelectedIndividualsNumber { get; set; }
        public double CrossoverProbability { get; set; }
        public double MutationProbability { get; set; }
        public int MutationType { get; set; }

        public GASettings(int maxNumberOfGeneration, int populationSize, int selectedIndividualsNumber, double crossoverProbability, double mutationProbability, int mutationType)
        {
            MaxNumberOfGeneration = maxNumberOfGeneration;
            PopulationSize = populationSize;
            SelectedIndividualsNumber = selectedIndividualsNumber;
            CrossoverProbability = crossoverProbability;
            MutationProbability = mutationProbability;
            MutationType = mutationType;
        }
    }
}