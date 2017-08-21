using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VRP.Models
{
    public class FindRouteBusinessLayer
    {
        public class CompleteGraphAndDijPaths
        {
            public Graph Graph { get; set; }
            public List<Route> PathMemoryList { get; set; }
        }


        public CompleteGraphAndDijPaths CreateCompleteGraph(Graph basedGraph, List<int> targetNodeIndexList, double aStarSpeed)
        {
            CompleteGraphAndDijPaths result = new CompleteGraphAndDijPaths();
            Graph completeGraph = new Graph();
            List<Route> pathMemoryList = new List<Route>();

            completeGraph.Edges = new List<Tuple<int, double>>[targetNodeIndexList.Count()];
            completeGraph.NodeList = new List<Node>();

            for (int i = 0; i < completeGraph.Edges.Length; i++)
            {
                completeGraph.Edges[i] = new List<Tuple<int, double>>();
            }

            for (int i = 0; i < targetNodeIndexList.Count() - 1; i++)
            {
                Node source = basedGraph.NodeList[targetNodeIndexList[i]];

                for (int j = i + 1; j < targetNodeIndexList.Count(); j++)
                {
                    Route pathMemory = new Route();
                    ShortestPathAlgorithm dij = new ShortestPathAlgorithm();
                    Node destination = basedGraph.NodeList[targetNodeIndexList[j]];
                    Node sourceCopy, destCopy;

                    pathMemory.Path = dij.AStar(basedGraph.Edges, basedGraph.NodeList, source, destination, aStarSpeed); //dij.Dijkstra(basedGraph.Edges, basedGraph.NodeList, source, destination);
                    pathMemory.Cost = GetRouteCost(pathMemory.Path, basedGraph.Edges);
                    pathMemoryList.Add(pathMemory);

                    completeGraph.Edges[i].Add(new Tuple<int, double>(j, pathMemory.Cost));
                    completeGraph.Edges[j].Add(new Tuple<int, double>(i, pathMemory.Cost));

                    sourceCopy = source.DeepthCopy();
                    destCopy = destination.DeepthCopy();

                    sourceCopy.ArrayIndex = i;
                    destCopy.ArrayIndex = j;
                    completeGraph.NodeList.Add(sourceCopy);
                    completeGraph.NodeList.Add(destCopy);
                }
            }

            completeGraph.NodeList = completeGraph.NodeList.GroupBy(x => x.Id).Select(g => g.First()).ToList();

            result.Graph = completeGraph;
            result.PathMemoryList = pathMemoryList;

            return result;
        }

        public double GetRouteCost(List<Node> route, List<Tuple<int, double>>[] edges)
        {
            double cost = 0;

            for (int i = 0; i < route.Count() - 1; i++)
            {
                cost += edges[route[i].ArrayIndex].Where(x => x.Item1 == route[i + 1].ArrayIndex).First().Item2;
            }

            return cost;
        }

        /// <summary>
        /// Create route running through real streets
        /// </summary>
        public Route CreateFullRoute(List<Node> nodeList, List<Route> pathMemoryList)
        {
            Route solution = new Route();
            solution.Path = new List<Node>();

            for (int i = 0; i < nodeList.Count() - 1; i++)
            {
                Route routeFragment;

                if (pathMemoryList.Exists(x => x.Path.First().Id.Equals(nodeList[i].Id) && x.Path.Last().Id.Equals(nodeList[i + 1].Id)))
                {
                    routeFragment = pathMemoryList.Where(x => x.Path.First().Id.Equals(nodeList[i].Id) && x.Path.Last().Id.Equals(nodeList[i + 1].Id)).First();

                    for (int j = 0; j < routeFragment.Path.Count() - 1; j++)
                    {
                        solution.Path.Add(routeFragment.Path[j]);
                    }
                }
                else if (pathMemoryList.Exists(x => x.Path.Last().Id.Equals(nodeList[i].Id) && x.Path.First().Id.Equals(nodeList[i + 1].Id)))
                {
                    routeFragment = pathMemoryList.Where(x => x.Path.Last().Id.Equals(nodeList[i].Id) && x.Path.First().Id.Equals(nodeList[i + 1].Id)).First();

                    for (int j = routeFragment.Path.Count() -1; j >=1 ; j--)
                    {
                        solution.Path.Add(routeFragment.Path[j]);
                    }
                }
                else
                {
                    return null;
                }
                
                solution.Cost += routeFragment.Cost;
                routeFragment.Path.First().IsMarked = true;
                routeFragment.Path.Last().IsMarked = true;
            }

            solution.Path.Add(solution.Path.First());

            return solution;
        }

        /// <summary>
        /// Convert solution from List of int to list of nodes
        /// </summary>
        public List<Node> ConvertSolution(List<int> nodeIndexList, Graph graph)
        {
            List<Node> nodeList = new List<Node>();

            for (int i = 0; i < nodeIndexList.Count(); i++)
            {
                Node node = new Node();

                node = graph.NodeList.Where(x => x.ArrayIndex == nodeIndexList[i]).First();
                node.IsMarked = true;
                nodeList.Add(node);
            }

            return nodeList;
        }

        public bool ValidAddress(string street, string houseNumber, List<Node> allNodes)
        {
            if (allNodes.Exists(x => x.dict.ContainsKey("addr:street") && x.dict.ContainsKey("addr:housenumber")
                && x.dict["addr:street"] == street && x.dict["addr:housenumber"] == houseNumber))
            {
                return true;
            }

            return false;
        }

        /// <returns>Distance between nodes in kilometers</returns>
        static public double CalculateDistance(Node from, Node to)
        {
            //source: http://andrew.hedges.name/experiments/haversine/
            double lon1 = from.Longitude, lon2 = to.Longitude, lat1 = from.Latitude, lat2 = to.Latitude;
            double dlon = lon2 - lon1;
            double dlat = lat2 - lat1;
            double a = Math.Pow((Math.Sin(((dlat / 2) * 2 * 3.14) / 360)), 2) + Math.Cos((lat1 * 3.14 * 2) / 360) * Math.Cos((lat2 * 3.14 * 2) / 360) * Math.Pow((Math.Sin(((dlon / 2) * 3.14 * 2) / 360)), 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = 6373 * c;

            return distance;
        }
    }
}