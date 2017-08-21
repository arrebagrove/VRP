using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VRP.Models
{
    public class ShortestPathAlgorithm
    {
        public List<Node> Dijkstra(List<Tuple<int, double>>[] adjacencList, List<Node> nodeList, Node source, Node destination)
        {
            const int INF = 1000000;

            int nodesNumber = adjacencList.Length;

            List<int> S = new List<int>();
            List<int> Q = new List<int>();
            double[] d = new double[nodesNumber];
            int[] p = new int[nodesNumber];

            for (int i = 0; i < nodesNumber; i++)
            {
                d[i] = INF;
                p[i] = -1;
                Q.Add(i);
            }
            d[source.ArrayIndex] = 0;

            while (Q.Count != 0)
            {
                int nodeWithMinDisctance = FindNodeWithLowestCost(d, Q);
                //double minDistance = d[nodeWithMinDisctance];
                S.Add(nodeWithMinDisctance);
                Q.Remove(nodeWithMinDisctance);
                
                for (int i = 0; i < adjacencList[nodeWithMinDisctance].Count(); i++)
                {
                    int index = adjacencList[nodeWithMinDisctance][i].Item1;
                    if (d[index] > d[nodeWithMinDisctance] + adjacencList[nodeWithMinDisctance][i].Item2)
                    {
                        d[index] = d[nodeWithMinDisctance] + adjacencList[nodeWithMinDisctance][i].Item2;
                        p[index] = nodeWithMinDisctance;
                    }
                }
            }

            int[] path = ReconstructPath(p, source.ArrayIndex, destination.ArrayIndex);

            List<Node> result = new List<Node>();
            foreach (var index in path)
            {
                result.Add(nodeList[index]);
            }

            return result;
        }

        public int FindNodeWithLowestCost(double[] d, List<int> Q)
        {
            int nodeWithMinDistance = Q[0];
            
            for (int i = 1; i < Q.Count(); i++)
            {
                if (d[nodeWithMinDistance] > d[Q[i]])
                {
                    nodeWithMinDistance = Q[i];
                }
            }

            return nodeWithMinDistance;
        }

        public int[] ReconstructPath(int[] prev, int SRC, int DEST)
        {
            int[] ret = new int[prev.Length];
            int currentNode = 0;
            ret[currentNode] = DEST;
            while (ret[currentNode] != 1000000 && ret[currentNode] != SRC)
            {
                ret[currentNode + 1] = prev[ret[currentNode]];
                currentNode++;
            }
            if (ret[currentNode] != SRC)
                return null;
            int[] reversed = new int[currentNode + 1];
            for (int i = currentNode; i >= 0; i--)
                reversed[currentNode - i] = ret[i];
            return reversed;
        }

        public List<Node> AStar(List<Tuple<int, double>>[] adjacencList, List<Node> nodeList, Node source, Node destination, double speed)
        {
            FindRouteBusinessLayer findRouteBL = new FindRouteBusinessLayer();
            List<int> closetSet = new List<int>();
            List<int> openSet = new List<int>();
            int nodesNumber = adjacencList.Length;
            double[] gScore = new double[nodesNumber];
            double[] fScore = new double[nodesNumber];
            int[] cameFrom = new int[nodesNumber];

            for (int i = 0; i < nodesNumber; i++)
            {
                gScore[i] = double.MaxValue;
                fScore[i] = double.MaxValue;
            }

            gScore[source.ArrayIndex] = 0;
            //fScore[source.ArrayIndex] = findRouteBL.GetEstimateRouteCost(source, destination) * speed;
            fScore[source.ArrayIndex] = GetDistanceInKM(source, destination) * speed;


            for (int i = 0; i < adjacencList.Length; i++)
            {
                openSet.Add(i);
            }

            while (openSet.Count() != 0)
            {
                int current = FindNodeWithLowestCost(fScore, openSet);

                if (current == destination.ArrayIndex)
                {
                    int[] path = ReconstructPath(cameFrom, source.ArrayIndex, destination.ArrayIndex);

                    List<Node> result = new List<Node>();
                    foreach (var index in path)
                    {
                        result.Add(nodeList[index]);
                    }

                    return result;
                }

                openSet.RemoveAt(openSet.FindIndex(x => x == current));
                closetSet.Add(current);

                for (int i = 0; i < adjacencList[current].Count(); i++)
                {
                    if (closetSet.Contains(adjacencList[current][i].Item1))
                    {
                        continue;
                    }

                    double tentative_gScore = gScore[current] + adjacencList[current][i].Item2;

                    if (openSet.Contains(adjacencList[current][i].Item1) == false)
                    {
                        openSet.Add(adjacencList[current][i].Item1);
                    }
                    if (tentative_gScore >= gScore[adjacencList[current][i].Item1])
                        continue;
                    cameFrom[adjacencList[current][i].Item1] = current;
                    gScore[adjacencList[current][i].Item1] = tentative_gScore;
                    //fScore[adjacencList[current][i].Item1] = gScore[adjacencList[current][i].Item1]
                    //  + findRouteBL.GetEstimateRouteCost(nodeList.Where(x => x.ArrayIndex == adjacencList[current][i].Item1).First(), destination) * speed;
                    fScore[adjacencList[current][i].Item1] = gScore[adjacencList[current][i].Item1]
                        + GetDistanceInKM(nodeList.Where(x => x.ArrayIndex == adjacencList[current][i].Item1).First(), destination) * speed;
                }
            }

            return null;
        }

        private double GetDistanceInKM(Node source, Node destination)
        {
            //source: http://andrew.hedges.name/experiments/haversine/
            double odleglosc;//in kilometers
            double lon1 = source.Longitude, lon2 = destination.Longitude, lat1 = source.Latitude, lat2 = destination.Latitude;
            double dlon = lon2 - lon1;
            double dlat = lat2 - lat1;
            double a = Math.Pow((Math.Sin(((dlat / 2) * 2 * 3.14) / 360)), 2) + Math.Cos((lat1 * 3.14 * 2) / 360) * Math.Cos((lat2 * 3.14 * 2) / 360) * Math.Pow((Math.Sin(((dlon / 2) * 3.14 * 2) / 360)), 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            odleglosc = 6373 * c;

            return odleglosc;
        }
    }
}