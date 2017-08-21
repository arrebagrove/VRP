using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace VRP.Models
{
    public class ManageFile
    {
        public List<Tuple<int, double>>[] GetAdjacencyList(string pathToFile)
        {
            JsonSerializer serializer = new JsonSerializer();
            List<Tuple<int, double>>[] adjacencyList;
            StreamReader file = File.OpenText(pathToFile);

            adjacencyList = (List<Tuple<int, double>>[])serializer.Deserialize(file, typeof(List<Tuple<int, double>>[]));

            return adjacencyList;
        }

        public List<Node> GetNodes(string pathToFile)
        {
            JsonSerializer serializer = new JsonSerializer();
            List<Node> nodeList = new List<Node>();
            StreamReader file = File.OpenText(pathToFile);

            nodeList = (List<Node>)serializer.Deserialize(file, typeof(List<Node>));

            return nodeList;
        }

        public List<Route> GetPathMemory(string pathToFile)
        {
            JsonSerializer serializer = new JsonSerializer();
            List<Route> pathMemoryList = new List<Route>();
            StreamReader file = File.OpenText(pathToFile);

            pathMemoryList = (List<Route>)serializer.Deserialize(file, typeof(List<Route>));

            return pathMemoryList;
        }

        public void SaveAdjacencyList(List<Tuple<int, double>>[] edges)
        {
            using (StreamWriter file = File.CreateText(@"D:\Warszawa_Edges_Test2.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, edges);
            }
        }

        public void SaveNodes(List<Node> nodeList)
        {
            using (StreamWriter file = File.CreateText(@"D:\Warszawa_Nodes_Test2.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, nodeList);
            }
        }

        public void SavePathMemory(List<Route> pathMemoryList)
        {
            using (StreamWriter file = File.CreateText(@"D:\Warszawa_PathMemory_Test2.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, pathMemoryList);
            }
        }

        public double[,] GetAdjacencyMatrix(string pathToFile)
        {
            JsonSerializer serializer = new JsonSerializer();
            double[,] matrix;
            StreamReader file = File.OpenText(pathToFile);

            matrix = (double[,])serializer.Deserialize(file, typeof(double[,]));

            return matrix;
        }
    }
}