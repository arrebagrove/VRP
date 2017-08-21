using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VRP.Models
{
    public class GeneticAlgorithm
    {
        public Graph Graph { get; set; }

        Random rand = new Random();
        int generationNumber, maxNumberOfGeneration, populationSize, offsprnigNumber, mutationType;
        double crossoverProbability, mutationProbability;
        
        public GeneticAlgorithm(Graph g)
        {
            Graph = g;
            generationNumber = 1;
            maxNumberOfGeneration = 100;
            populationSize = 100;
            offsprnigNumber = populationSize;
            crossoverProbability = 0.8;
            mutationProbability = 0.2;
            mutationType = 1;
        }

        public GeneticAlgorithm(Graph g, GASettings settings)
        {
            Graph = g;
            generationNumber = 1;
            maxNumberOfGeneration = settings.MaxNumberOfGeneration;
            populationSize = settings.PopulationSize;
            offsprnigNumber = settings.SelectedIndividualsNumber;
            crossoverProbability = settings.CrossoverProbability;
            mutationProbability = settings.MutationProbability;
            mutationType = settings.MutationType;
        }

        public List<Chromosome> Main()
        {
            List<Chromosome> bestChromosomeInGeneration = new List<Chromosome>();    //najlepszy chromosom w i-tej generacji
            List<Chromosome> population;
            
            population = CreatePopulation();
            Evaluation(population);
            bestChromosomeInGeneration.Add(population.OrderBy(x => x.FitnessValue).First());
            generationNumber = 1;

            while (generationNumber < maxNumberOfGeneration)
            {
                List<Chromosome> selectedChromosomeList;
                List<Chromosome> offspringList = new List<Chromosome>();
                
                selectedChromosomeList = TournamentSelection(population, 2);

                //Crossover
                for (int i = 0; i < selectedChromosomeList.Count - 1; i += 2)
                {
                    if (rand.NextDouble() < crossoverProbability)
                    {
                        Chromosome[] crossoverChromosome = new Chromosome[2];
                        
                        crossoverChromosome = OXCrossover(selectedChromosomeList[i], selectedChromosomeList[i + 1]);
                        offspringList.Add(crossoverChromosome[0]);
                        offspringList.Add(crossoverChromosome[1]);
                    }
                }

                //Mutation
                for (int i = 0; i < offspringList.Count(); i++)
                {
                    if (rand.NextDouble() < mutationProbability)
                    {
                        if (mutationType == 1)
                            Mutation(offspringList[i]);
                        else if (mutationType == 2)
                            OwnMutation(offspringList[i]);
                    }
                }

                Evaluation(offspringList);
                population = Replacement(population, offspringList);
                bestChromosomeInGeneration.Add(population.First());
                generationNumber++;
            }

            return bestChromosomeInGeneration;
        }

        public void OwnMutation(Chromosome offspring)
        {
            int completed = (100 * generationNumber) / maxNumberOfGeneration;
            int chromosomeLength = offspring.NodeIndexList.Count();
            double probabilityOfChange = Math.Exp(-1 * (1 + (Math.Pow(chromosomeLength, 1.0 / (56 - completed / 2.0)))));

            for (int i = 1; i < offspring.NodeIndexList.Count - 2; i++)
            {
                if (rand.NextDouble() < probabilityOfChange)
                {
                    int tmp = offspring.NodeIndexList[i];
                    offspring.NodeIndexList[i] = offspring.NodeIndexList[i + 1];
                    offspring.NodeIndexList[i + 1] = tmp;
                }
            }
        }

        public List<Chromosome> Replacement(List<Chromosome> population, List<Chromosome> offspringList)
        {
            List<Chromosome> result = population.Concat(offspringList).OrderBy(x => x.FitnessValue).Take(populationSize).ToList();

            return result;
        }

        /// <summary>
        /// Pozycje dwóch losowych genów są zamieniane
        /// </summary>
        public void Mutation(Chromosome offspring)
        {
            int position1, position2;

            do
            {
                position1 = rand.Next(1, offspring.NodeIndexList.Count());
                position2 = rand.Next(1, offspring.NodeIndexList.Count());
            } while (position1 == position2);

            int tmp = offspring.NodeIndexList[position1];
            offspring.NodeIndexList[position1] = offspring.NodeIndexList[position2];
            offspring.NodeIndexList[position2] = tmp;
        }

        public Chromosome[] OXCrossover(Chromosome parent0, Chromosome parent1)
        {
            Chromosome[] offspring = new Chromosome[2];
            int chromosomeLength = parent0.NodeIndexList.Count();
            int[] cutPoint = GetCutPoint(chromosomeLength);
            int inheritedGenIndex0 = cutPoint[1], inheritedGenIndex1 = cutPoint[1];

            offspring[0] = new Chromosome();
            offspring[1] = new Chromosome();

            for (int i = 0; i < parent0.NodeIndexList.Count(); i++)
            {
                if (i == 0 || (i >= cutPoint[0] && i < cutPoint[1]))
                {
                    offspring[0].NodeIndexList.Add(parent0.NodeIndexList[i]);
                    offspring[1].NodeIndexList.Add(parent1.NodeIndexList[i]);
                }
                else
                {
                    offspring[0].NodeIndexList.Add(-1);
                    offspring[1].NodeIndexList.Add(-1);
                }
            }

            for (int j = 0; j < 2; j++)
            {
                int i = 0, length;

                if (j == 0)
                {
                    i = cutPoint[1];
                    length = chromosomeLength;
                }
                else
                {
                    i = 1;
                    length = cutPoint[0];
                }

                for (; i < length; i++)
                {
                    while (offspring[0].NodeIndexList.Contains(parent1.NodeIndexList[inheritedGenIndex1]))
                    {
                        inheritedGenIndex1++;

                        if (inheritedGenIndex1 == chromosomeLength)
                            inheritedGenIndex1 = 1;
                    }
                    while (offspring[1].NodeIndexList.Contains(parent0.NodeIndexList[inheritedGenIndex0]))
                    {
                        inheritedGenIndex0++;

                        if (inheritedGenIndex0 == chromosomeLength)
                            inheritedGenIndex0 = 1;
                    }

                    offspring[0].NodeIndexList[i] = parent1.NodeIndexList[inheritedGenIndex1];
                    offspring[1].NodeIndexList[i] = parent0.NodeIndexList[inheritedGenIndex0];
                }
            }

            return offspring;
        }

        public int[] GetCutPoint(int chromoseomeLength)
        {
            int[] cutPoint = new int[2];

            do
            {
                cutPoint[0] = rand.Next(1, chromoseomeLength);
                cutPoint[1] = rand.Next(1, chromoseomeLength);
            } while (cutPoint[0] == cutPoint[1] 
                || (cutPoint[0] == 1 && cutPoint[1] == chromoseomeLength) 
                || (cutPoint[0] == chromoseomeLength && cutPoint[1] == 1));

            if (cutPoint[0] > cutPoint[1])
            {
                int tmp = cutPoint[0];
                cutPoint[0] = cutPoint[1];
                cutPoint[1] = tmp;
            }

            return cutPoint;
        }

        /// <param name="k">Liczba osobników rywalizujących ze sobą</param>
        public List<Chromosome> TournamentSelection(List<Chromosome> population, int k)
        {
            List<Chromosome> selectedChromosomeList = new List<Chromosome>();

            while (selectedChromosomeList.Count() < offsprnigNumber)
            {
                List<Chromosome> candidateList = new List<Chromosome>();
                List<int> candidateIndexList = new List<int>();
                int randomNumber;

                while (candidateIndexList.Count() < k)
                {
                    randomNumber = rand.Next(population.Count());

                    if (!candidateIndexList.Contains(randomNumber))
                        candidateIndexList.Add(randomNumber);
                }

                foreach (var item in candidateIndexList)
                {
                    candidateList.Add(population[item]);
                }

                selectedChromosomeList.Add(candidateList.OrderBy(x => x.FitnessValue).First());
            }

            return selectedChromosomeList;
        }

        public List<Chromosome> CreatePopulation()
        {
            List<Chromosome> population = new List<Chromosome>();
            List<int> clientList = new List<int>();

            for (int i = 0; i < populationSize; i++)
            {
                Chromosome solution = new Chromosome();
                solution.NodeIndexList.Add(0);  //depot

                for (int j = 1; j < Graph.NodeList.Count(); j++)
                {
                    clientList.Add(Graph.NodeList[j].ArrayIndex);
                }

                while (clientList.Count() != 0)
                {
                    int index = rand.Next(clientList.Count());
                    solution.NodeIndexList.Add(clientList[index]);
                    clientList.RemoveAt(index);
                }

                population.Add(solution);
            }

            return population;
        }

        public void Evaluation(List<Chromosome> population)
        {
            FindRouteBusinessLayer findRouteBL = new FindRouteBusinessLayer();

            for (int i = 0; i < population.Count(); i++)
            {
                population[i].FitnessValue = 0;

                for (int j = 0; j < population[i].NodeIndexList.Count() - 1; j++)
                {
                    population[i].FitnessValue += Graph.Edges[population[i].NodeIndexList[j]]
                        .Where(x => x.Item1 == population[i].NodeIndexList[j + 1]).First().Item2;
                }
                population[i].FitnessValue+= Graph.Edges[population[i].NodeIndexList.Last()]
                    .Where(x => x.Item1 == population[i].NodeIndexList[0]).First().Item2;
            }
        }
    }
}