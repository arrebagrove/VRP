using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VRP.Models
{
    public class ClarkeAndWrightAlgorithm
    {
        /// <summary>
        /// Item1: indeks klienta i, Item2: indeks klienta j, item3: oszczędność
        /// </summary>
        List<Tuple<int, int, double>> saveingsList = new List<Tuple<int, int, double>>();
        Graph graph;
        List<List<int>> routeList = new List<List<int>>();
        List<int> nodeIndexList = new List<int>();

        public ClarkeAndWrightAlgorithm(Graph g)
        {
            graph = g;
        }

        public List<int> Main()
        {
            //List<int> result = new List<int>();
            int saveingsIndex = 0;

            CreateRoutes();
            CalculateSaveings();
            saveingsList = saveingsList.OrderByDescending(x => x.Item3).ToList();
            
            while (routeList.Count != 1)
            {
                if (ConnectRoutesIsValid(saveingsList[saveingsIndex].Item1, saveingsList[saveingsIndex].Item2))
                {
                    int iRouteIndex = routeList.FindIndex(x => x.Contains(saveingsList[saveingsIndex].Item1));
                    int jRouteIndex = routeList.FindIndex(x => x.Contains(saveingsList[saveingsIndex].Item2));

                    routeList[iRouteIndex].RemoveAt(routeList[iRouteIndex].Count() - 1);
                    routeList[jRouteIndex].RemoveAt(0);
                    routeList[iRouteIndex] = routeList[iRouteIndex].Concat(routeList[jRouteIndex]).ToList();
                    routeList.RemoveAt(jRouteIndex);
                }
                
                saveingsIndex++;
            }

            return routeList[0];
        }

        public bool ConnectRoutesIsValid(int iIndex_NodeListTarget, int jIndex_NodeListTarget)
        {
            List<int> iClientRoute = routeList.Where(x => x.Contains(graph.NodeList[iIndex_NodeListTarget].ArrayIndex)).First();
            List<int> jClientRoute = routeList.Where(x => x.Contains(graph.NodeList[jIndex_NodeListTarget].ArrayIndex)).First();
            int iClientIndex = iClientRoute.FindIndex(x => x == graph.NodeList[iIndex_NodeListTarget].ArrayIndex);
            int jClientIndex = jClientRoute.FindIndex(x => x == graph.NodeList[jIndex_NodeListTarget].ArrayIndex);

            if (Object.ReferenceEquals(iClientRoute, jClientRoute) == false && iClientRoute[iClientIndex + 1] == 0 & jClientRoute[jClientIndex - 1] == 0)
            {
                return true;
            }

            return false;
        }

        
        public void CalculateSaveings()
        {
            for (int i = 1; i < graph.NodeList.Count(); i++)
            {
                for (int j = 1; j < graph.NodeList.Count(); j++)
                {
                    if (i != j)
                    {
                        List<int> iClientRoute = routeList.Where(x => x.Contains(graph.NodeList[i].ArrayIndex)).First();
                        List<int> jClientRoute = routeList.Where(x => x.Contains(graph.NodeList[j].ArrayIndex)).First();
                        int iClientIndex = iClientRoute.FindIndex(x => x == graph.NodeList[i].ArrayIndex);
                        int jClientIndex = jClientRoute.FindIndex(x => x == graph.NodeList[j].ArrayIndex);

                        if (Object.ReferenceEquals(iClientRoute, jClientRoute) == false
                            && iClientRoute[iClientIndex + 1] == 0 & jClientRoute[jClientIndex - 1] == 0)
                        {
                            double saveing = graph.Edges[i].Where(x => x.Item1 == 0).First().Item2
                                + graph.Edges[0].Where(x => x.Item1 == j).First().Item2
                                - graph.Edges[i].Where(x => x.Item1 == j).First().Item2;

                            saveingsList.Add(new Tuple<int, int, double>(i, j, saveing));
                        }
                    }
                }
            }
        }

        public void CreateRoutes()
        {
            for (int i = 1; i < graph.NodeList.Count(); i++)
            {
                List<int> route = new List<int>();

                route.Add(0);
                route.Add(i);
                route.Add(0);

                routeList.Add(route);
            }
        }
    }
}