using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VRP.Models
{
    public class AlgorithmsTestBusinessLayer
    {
        /// <summary>
        /// [dla każdego ustawienia GA][dla każdegj próby (wywołania GA)][wynik dla każdej generacji GA]
        /// </summary>
        List<List<List<Chromosome>>> gaResultsList = new List<List<List<Chromosome>>>();
        List<GASettings> gaSettingsList = new List<GASettings>();
        List<Chromosome> bestGASolution = new List<Chromosome>();
        List<List<double>> gaCostSumList = new List<List<double>>();
        List<int> cwResultNodeIndexList = new List<int>();
        Graph vrpGraph = new Graph();
        ManageFile mf = new ManageFile();
        List<Route> pathMemoryList;
        List<double> gaExecutionTimeList = new List<double>();
        double gaExecutionTime, cwExecutionTime;
        

        public AlgorithmsTestBusinessLayer(int testNumber)
        {
            #region test1
            if (testNumber == 1)
            {
                gaSettingsList.Add(new GASettings(100, 20, 20, 0.8, 0.3, 1));
                gaSettingsList.Add(new GASettings(100, 50, 50, 0.8, 0.3, 1));
                gaSettingsList.Add(new GASettings(100, 100, 100, 0.8, 0.3, 1));

                gaSettingsList.Add(new GASettings(100, 100, 100, 0.5, 0.1, 1));
                gaSettingsList.Add(new GASettings(100, 100, 100, 0.5, 0.5, 1));
                gaSettingsList.Add(new GASettings(100, 100, 100, 0.9, 0.3, 1));
                gaSettingsList.Add(new GASettings(100, 100, 100, 0.9, 0.9, 1));

                gaSettingsList.Add(new GASettings(100, 100, 100, 0.9, 0.9, 2));
                gaSettingsList.Add(new GASettings(100, 100, 100, 0.5, 0.5, 2));


                vrpGraph.Edges = mf.GetAdjacencyList(HttpContext.Current.Server.MapPath("~/Files/Test/Rzeszow_Edges_Test1.json"));
                vrpGraph.NodeList = mf.GetNodes(HttpContext.Current.Server.MapPath("~/Files/Test/Rzeszow_Nodes_Test1.json"));
                pathMemoryList = mf.GetPathMemory(HttpContext.Current.Server.MapPath("~/Files/Test/Rzeszow_PathMemory_Test1.json"));
            }
            #endregion

            #region test2
            else if (testNumber == 2)
            {
                gaSettingsList.Add(new GASettings(200, 100, 100, 0.8, 0.3, 1));
                gaSettingsList.Add(new GASettings(200, 100, 100, 0.9, 0.9, 1));
                gaSettingsList.Add(new GASettings(200, 100, 100, 0.9, 0.9, 2));

                vrpGraph.Edges = mf.GetAdjacencyList(HttpContext.Current.Server.MapPath("~/Files/Test/Rzeszow_Edges_Test2.json"));
                vrpGraph.NodeList = mf.GetNodes(HttpContext.Current.Server.MapPath("~/Files/Test/Rzeszow_Nodes_Test2.json"));
                pathMemoryList = mf.GetPathMemory(HttpContext.Current.Server.MapPath("~/Files/Test/Rzeszow_PathMemory_Test2.json"));
            }
            #endregion

            #region test3
            else if (testNumber == 3)
            {
                gaSettingsList.Add(new GASettings(100, 100, 100, 0.8, 0.3, 1));
                gaSettingsList.Add(new GASettings(100, 100, 100, 0.9, 0.9, 1));
                gaSettingsList.Add(new GASettings(100, 100, 100, 0.9, 0.9, 2));

                vrpGraph.Edges = mf.GetAdjacencyList(HttpContext.Current.Server.MapPath("~/Files/Test/Rzeszow_Edges_Test3.json"));
                vrpGraph.NodeList = mf.GetNodes(HttpContext.Current.Server.MapPath("~/Files/Test/Rzeszow_Nodes_Test3.json"));
                pathMemoryList = mf.GetPathMemory(HttpContext.Current.Server.MapPath("~/Files/Test/Rzeszow_PathMemory_Test3.json"));
            }
            #endregion

            #region test4
            else if (testNumber == 4)
            {
                gaSettingsList.Add(new GASettings(100, 20, 20, 0.8, 0.3, 1));
                gaSettingsList.Add(new GASettings(100, 50, 50, 0.8, 0.3, 1));
                gaSettingsList.Add(new GASettings(100, 100, 100, 0.8, 0.3, 1));

                gaSettingsList.Add(new GASettings(100, 100, 100, 0.5, 0.1, 1));
                gaSettingsList.Add(new GASettings(100, 100, 100, 0.5, 0.5, 1));
                gaSettingsList.Add(new GASettings(100, 100, 100, 0.9, 0.3, 1));
                gaSettingsList.Add(new GASettings(100, 100, 100, 0.9, 0.9, 1));

                gaSettingsList.Add(new GASettings(100, 100, 100, 0.9, 0.9, 2));
                gaSettingsList.Add(new GASettings(100, 100, 100, 0.5, 0.5, 2));


                vrpGraph.Edges = mf.GetAdjacencyList(HttpContext.Current.Server.MapPath("~/Files/Test/Warszawa_Edges_Test1.json"));
                vrpGraph.NodeList = mf.GetNodes(HttpContext.Current.Server.MapPath("~/Files/Test/Warszawa_Nodes_Test1.json"));
                pathMemoryList = mf.GetPathMemory(HttpContext.Current.Server.MapPath("~/Files/Test/Warszawa_PathMemory_Test1.json"));
            }
            #endregion

            #region test5
            else if (testNumber == 5)
            {
                gaSettingsList.Add(new GASettings(200, 100, 100, 0.8, 0.3, 1));
                gaSettingsList.Add(new GASettings(200, 100, 100, 0.9, 0.9, 1));
                gaSettingsList.Add(new GASettings(200, 100, 100, 0.9, 0.9, 2));

                vrpGraph.Edges = mf.GetAdjacencyList(HttpContext.Current.Server.MapPath("~/Files/Test/Warszawa_Edges_Test2.json"));
                vrpGraph.NodeList = mf.GetNodes(HttpContext.Current.Server.MapPath("~/Files/Test/Warszawa_Nodes_Test2.json"));
                pathMemoryList = mf.GetPathMemory(HttpContext.Current.Server.MapPath("~/Files/Test/Warszawa_PathMemory_Test2.json"));
            }
            #endregion

            #region test6
            else if (testNumber == 6)
            {
                gaSettingsList.Add(new GASettings(100, 100, 100, 0.8, 0.3, 1));
                gaSettingsList.Add(new GASettings(100, 100, 100, 0.9, 0.9, 1));
                gaSettingsList.Add(new GASettings(100, 100, 100, 0.9, 0.9, 2));

                vrpGraph.Edges = mf.GetAdjacencyList(HttpContext.Current.Server.MapPath("~/Files/Test/Warszawa_Edges_Test3.json"));
                vrpGraph.NodeList = mf.GetNodes(HttpContext.Current.Server.MapPath("~/Files/Test/Warszawa_Nodes_Test3.json"));
                pathMemoryList = mf.GetPathMemory(HttpContext.Current.Server.MapPath("~/Files/Test/Warszawa_PathMemory_Test3.json"));
            }
            #endregion
        }

        public AlgorithmsTestViewModel Main()
        {
            FindRouteBusinessLayer findRouteBL = new FindRouteBusinessLayer();
            AlgorithmsTestViewModel algorithmsTestVM = new AlgorithmsTestViewModel();
            Route fullGAPathNodeList = new Route();
            Route fullCWPathNodeList = new Route();

            GAResults();
            SumGACosts();
            BestGASolution();
            ClarkeWrightResult();
            InsertResultsToExcelFile();

            algorithmsTestVM.GAPath = findRouteBL.ConvertSolution(bestGASolution.Last().NodeIndexList, vrpGraph);
            algorithmsTestVM.GAPath.Add(algorithmsTestVM.GAPath[0]);
            fullGAPathNodeList = findRouteBL.CreateFullRoute(algorithmsTestVM.GAPath, pathMemoryList);
            algorithmsTestVM.GAPath = fullGAPathNodeList.Path;
            algorithmsTestVM.GAPathCost = bestGASolution.Last().FitnessValue;
            algorithmsTestVM.GAPathCost = Math.Round(algorithmsTestVM.GAPathCost, 3);

            algorithmsTestVM.ClarkeWrightPath = findRouteBL.ConvertSolution(cwResultNodeIndexList, vrpGraph);
            fullCWPathNodeList = findRouteBL.CreateFullRoute(algorithmsTestVM.ClarkeWrightPath, pathMemoryList);
            algorithmsTestVM.ClarkeWrightPath = fullCWPathNodeList.Path;
            algorithmsTestVM.ClarkeWrightPathCost = fullCWPathNodeList.Cost;
            algorithmsTestVM.ClarkeWrightPathCost = Math.Round(algorithmsTestVM.ClarkeWrightPathCost, 3);

            algorithmsTestVM.GAExecutionTime = gaExecutionTime;
            algorithmsTestVM.ClarkeWrightExecutionTime = cwExecutionTime;

            return algorithmsTestVM;
        }

        private void BestGASolution()
        {
            List<List<Chromosome>> allSolutionsList = new List<List<Chromosome>>();
            FindRouteBusinessLayer findRouteBL = new FindRouteBusinessLayer();
            
            for (int i = 0; i < gaResultsList.Count(); i++)
            {
                for (int j = 0; j < gaResultsList[i].Count(); j++)
                {
                    allSolutionsList.Add(gaResultsList[i][j]);
                }
            }

            List<Chromosome> best = allSolutionsList[0];
            int bestIndex = 0;
            for (int i = 1; i < allSolutionsList.Count(); i++)
            {
                if (best.Last().FitnessValue > allSolutionsList[i].Last().FitnessValue)
                {
                    best = allSolutionsList[i];
                    bestIndex = i;
                }
            }

            gaExecutionTime = gaExecutionTimeList[bestIndex];
            bestGASolution = best;
        }

        private void SumGACosts()
        {
            for (int i = 0; i < gaResultsList.Count(); i++)
            {
                gaCostSumList.Add(new List<double>());

                for (int k = 0; k < gaSettingsList[i].MaxNumberOfGeneration; k++)
                {
                    gaCostSumList[i].Add(0);
                }

                for (int j = 0; j < gaResultsList[i].Count(); j++)
                {                    
                    for (int k = 0; k < gaResultsList[i][j].Count(); k++)
                    {
                        gaCostSumList[i][k] += gaResultsList[i][j][k].FitnessValue;
                    }
                }
            }
        }

        private void GAResults()
        {
            for (int i = 0; i < gaSettingsList.Count(); i++)
            {
                GeneticAlgorithm ga = new GeneticAlgorithm(vrpGraph, gaSettingsList[i]);

                gaResultsList.Add(new List<List<Chromosome>>());

                for (int j = 0; j < 50; j++)
                {
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    List<Chromosome> gaResult = ga.Main();
                    watch.Stop();

                    gaExecutionTimeList.Add(watch.ElapsedMilliseconds);
                    gaResultsList[i].Add(gaResult);
                }
            }
        }

        private void ClarkeWrightResult()
        {
            ClarkeAndWrightAlgorithm clarkeWright = new ClarkeAndWrightAlgorithm(vrpGraph);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            cwResultNodeIndexList = clarkeWright.Main();
            watch.Stop();
            cwExecutionTime = watch.ElapsedMilliseconds;
        }

        private void InsertResultsToExcelFile()
        {
            Application app = null;
            Workbook workbook = null;
            Worksheet worksheet1 = null;

            try
            {
                app = new Application();
                app.Visible = true;
                workbook = app.Workbooks.Add(Type.Missing);
                worksheet1 = (Worksheet)workbook.Sheets[1];
            }
            catch
            {
                Console.Write("Error");
            }

            //fill file
            //GA
            for (int i = 0; i < gaCostSumList.Count; i++)
            {
                worksheet1.Cells[1, (i + 1)] = "gaCostSumList #" + (i + 1);

                for (int j = 0; j < gaCostSumList[i].Count; j++)
                {
                    worksheet1.Cells[j + 2, i + 1] = (gaCostSumList[i][j]);
                }
            }

            worksheet1.Cells[1, gaCostSumList.Count + 2] = "bestGASolution";

            for (int i = 0; i < bestGASolution.Count(); i++)
            {
                worksheet1.Cells[i + 2, gaCostSumList.Count + 2] = (bestGASolution[i].FitnessValue);
            }
        }

    }
}