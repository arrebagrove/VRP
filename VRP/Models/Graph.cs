using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VRP.Models
{
    public class Graph
    {
        /// <summary>
        /// Krawędzie grafu zapisane w postaci listy sąsiedztwa.
        /// </summary>
        public List<Tuple<int, double>>[] Edges { get; set; }
        public List<Node> NodeList { get; set; }

        public Graph() { }

        public Graph(string pathToConnections, string pathToNodes)
        {
            ManageFile mf = new ManageFile();

            Edges = mf.GetAdjacencyList(pathToConnections);
            NodeList = mf.GetNodes(pathToNodes);
        }

        public Graph DeepthCopy()
        {
            Graph copy = new Graph();

            copy.Edges = new List<Tuple<int, double>>[Edges.Length];
            copy.NodeList = new List<Node>();

            //copy edges
            for (int i = 0; i < Edges.Length; i++)
            {
                copy.Edges[i] = new List<Tuple<int, double>>();

                for (int j = 0; j < Edges[i].Count(); j++)
                {
                    int item1 = Edges[i][j].Item1;
                    double item2 = Edges[i][j].Item2;

                    copy.Edges[i].Add(new Tuple<int, double>(item1, item2));
                }
            }

            //copy nodes
            for (int i = 0; i < NodeList.Count(); i++)
            {
                copy.NodeList.Add(NodeList[i].DeepthCopy());
            }

            return copy;
        }

        public void ConnectAddressesToGraph(List<Node> addressList)
        {
            for (int i = 0; i < addressList.Count(); i++)
            {
                //item1: wierzchołek, item2: odległośc do tego wierzchołka
                List<Tuple<Node, double>> nodeBelongToStreetList = new List<Tuple<Node, double>>();
                string streetName = addressList[i].dict["addr:street"];

                for (int j = 0; j < NodeList.Count(); j++)
                {
                    if (NodeList[j].dict.ContainsKey("addr:street") && NodeList[j].dict["addr:street"].Equals(streetName))
                    {
                        double distance = FindRouteBusinessLayer.CalculateDistance(addressList[i], NodeList[j]);

                        nodeBelongToStreetList.Add(new Tuple<Node, double>(NodeList[j], distance));
                    }
                }

                nodeBelongToStreetList = nodeBelongToStreetList.OrderBy(x => x.Item2).ToList();

                if (nodeBelongToStreetList.Count > 0)
                {
                    //add node
                    addressList[i].ArrayIndex = Edges.Length;
                    NodeList.Add(addressList[i]);

                    //add edges
                    int newArraySize = Edges.Length + 1;
                    List<Tuple<int, double>>[] tmpEdges = Edges;

                    Array.Resize<List<Tuple<int, double>>>(ref tmpEdges, newArraySize);
                    Edges = new List<Tuple<int, double>>[newArraySize];
                    Edges = tmpEdges;
                    Edges[newArraySize - 1] = new List<Tuple<int, double>>();
                    Edges[newArraySize - 1].Add(new Tuple<int, double>(nodeBelongToStreetList[0].Item1.ArrayIndex, nodeBelongToStreetList[0].Item2));
                    Edges[nodeBelongToStreetList[0].Item1.ArrayIndex].Add(new Tuple<int, double>(addressList[i].ArrayIndex, nodeBelongToStreetList[0].Item2));
                }
            }
        }
    }
}