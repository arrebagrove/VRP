using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VRP.Models;

namespace VRP.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult AlgorithmsTest()
        {
            AlgorithmsTestViewModel emptyModel = new AlgorithmsTestViewModel();
            emptyModel.ClarkeWrightPath = new List<Node>();
            emptyModel.GAPath = new List<Node>();

            return View(emptyModel);
        }

        [HttpPost]
        public ActionResult AlgorithmsTest(string testBtn)
        {
            int testNumber = Int32.Parse(testBtn.Remove(0, 6));
            AlgorithmsTestBusinessLayer test = new AlgorithmsTestBusinessLayer(testNumber);
            AlgorithmsTestViewModel algorithmsTestVM = new AlgorithmsTestViewModel();

            algorithmsTestVM = test.Main();

            return View(algorithmsTestVM);
        }

        [HttpGet]
        public ActionResult FindRoute()
        {
            FindRouteViewModel emptyModel = new FindRouteViewModel();
            emptyModel.GARoute = new List<Node>();
            emptyModel.CWRoute = new List<Node>();

            ManageFile mf = new ManageFile();
            List<Node> allAddressList = mf.GetNodes(Server.MapPath("~/Files/Rzeszow_Addresses.json"));
            TempData["AllAddresses"] = allAddressList;

            return View(emptyModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FindRoute(FindRouteViewModel findRouteVM, string speed)
        {
            Graph basedGraph = new Graph(Server.MapPath("~/Files/Rzeszow_AdjacencyList.json"), Server.MapPath("~/Files/Rzeszow_Nodes.json"));

            Graph vrpGraph = new Graph();
            FindRouteBusinessLayer findRouteBL = new FindRouteBusinessLayer();
            List<int> addressArrayIndexList = new List<int>();  //indeksy adresów w grafie bazowym
            FindRouteBusinessLayer.CompleteGraphAndDijPaths completeGraphAndDijPaths = new FindRouteBusinessLayer.CompleteGraphAndDijPaths();
            List<Route> vrpPathMemoryList = new List<Route>();
            GeneticAlgorithm ga;
            ClarkeAndWrightAlgorithm clarkeWright;
            Chromosome gaResult;
            List<int> clarkeWrightResult;
            List<Node> gaResultNodeList, cwResultNodeList;
            Route gaFullResult, cwFullResult;
            ManageFile mf = new ManageFile();
            List<Node> allAddressList = mf.GetNodes(Server.MapPath("~/Files/Rzeszow_Addresses.json"));
            List<Node> destinationList = new List<Node>();

            //validation
            for (int i = 0; i < findRouteVM.AddressList.Count(); i++)
            {
                if(!findRouteBL.ValidAddress(findRouteVM.AddressList[i].StreetName, findRouteVM.AddressList[i].HouseNumber, allAddressList))
                {
                    ModelState.AddModelError(string.Empty, "Adres " + findRouteVM.AddressList[i].StreetName + " "
                        + findRouteVM.AddressList[i].HouseNumber + " nie został odnaleziony");
                    return View(findRouteVM);
                }
                else
                {
                    Node destination = allAddressList.Where(x => x.dict["addr:street"].Equals(findRouteVM.AddressList[i].StreetName)
                      && x.dict["addr:housenumber"].Equals(findRouteVM.AddressList[i].HouseNumber)).First();
                    destinationList.Add(destination);
                }
            }
            basedGraph.ConnectAddressesToGraph(destinationList);
            
            for (int i = 0; i < findRouteVM.AddressList.Count(); i++)
            {
                addressArrayIndexList.Add(basedGraph.NodeList.Where(n => n.dict.ContainsKey("addr:housenumber")
                    && n.dict["addr:street"] == findRouteVM.AddressList[i].StreetName
                    && n.dict["addr:housenumber"] == findRouteVM.AddressList[i].HouseNumber).First().ArrayIndex);
            }


            completeGraphAndDijPaths = findRouteBL.CreateCompleteGraph(basedGraph, addressArrayIndexList, double.Parse(speed.Replace(".", ",")));
            vrpGraph = completeGraphAndDijPaths.Graph;
            vrpPathMemoryList = completeGraphAndDijPaths.PathMemoryList;

            //mf.SaveAdjacencyList(completeGraphAndDijPaths.Graph.Edges);
            //mf.SaveNodes(completeGraphAndDijPaths.Graph.NodeList);
            //mf.SavePathMemory(completeGraphAndDijPaths.PathMemoryList);

            ga = new GeneticAlgorithm(vrpGraph);
            clarkeWright = new ClarkeAndWrightAlgorithm(vrpGraph);

            gaResult = ga.Main().Last();
            gaResult.NodeIndexList.Add(gaResult.NodeIndexList[0]);
            clarkeWrightResult = clarkeWright.Main();

            gaResultNodeList = findRouteBL.ConvertSolution(gaResult.NodeIndexList, vrpGraph);
            cwResultNodeList = findRouteBL.ConvertSolution(clarkeWrightResult, vrpGraph);

            gaFullResult = findRouteBL.CreateFullRoute(gaResultNodeList, vrpPathMemoryList);
            cwFullResult = findRouteBL.CreateFullRoute(cwResultNodeList, vrpPathMemoryList);

            findRouteVM.GARouteCost = gaFullResult.Cost;
            findRouteVM.CWRouteCost = cwFullResult.Cost;
            findRouteVM.GARouteCost = Math.Round(findRouteVM.GARouteCost, 3);
            findRouteVM.CWRouteCost = Math.Round(findRouteVM.CWRouteCost, 3);

            //display full result
            findRouteVM.GARoute = gaFullResult.Path;
            findRouteVM.CWRoute = cwFullResult.Path;

            return View(findRouteVM);
        }

        
        public JsonResult GetStreetsNames(string partName, string houseNumber)
        {
            List<string> streetNameList = new List<string>();
            List<Node> addressList = new List<Node>();
            List<Node> basedGraph = (List<Node>)TempData.Peek("AllAddresses");

            if (String.IsNullOrEmpty(houseNumber))
            {
                addressList = basedGraph
                    .Where(x => x.dict.ContainsKey("addr:housenumber")
                        && x.dict["addr:street"].IndexOf(partName, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }
            else
            {
                addressList = basedGraph
                    .Where(x => x.dict.ContainsKey("addr:housenumber")
                        && x.dict["addr:housenumber"].Equals(houseNumber, StringComparison.OrdinalIgnoreCase)
                        && x.dict["addr:street"].IndexOf(partName, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }

            addressList = addressList.GroupBy(x => x.dict["addr:street"]).Select(x => x.First()).ToList();

            foreach (var item in addressList)
            {
                streetNameList.Add(item.dict["addr:street"]);
            }

            return Json(streetNameList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHousesNumbers(string partNumber, string streetName)
        {
            List<string> houseNumberList = new List<string>();
            List<Node> addressList = new List<Node>();
            List<Node> allAddressesList = (List<Node>)TempData.Peek("AllAddresses");

            if (String.IsNullOrEmpty(streetName))
            {
                addressList = allAddressesList
                    .Where(x => x.dict.ContainsKey("addr:housenumber")
                        && x.dict["addr:housenumber"].IndexOf(partNumber, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }
            else
            {
                addressList = allAddressesList
                    .Where(x => x.dict.ContainsKey("addr:housenumber")
                        && x.dict["addr:housenumber"].IndexOf(partNumber, StringComparison.OrdinalIgnoreCase) >= 0
                        && x.dict["addr:street"].Equals(streetName, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            addressList = addressList.GroupBy(x => x.dict["addr:housenumber"]).Select(x => x.First()).ToList();

            foreach (var item in addressList)
            {
                houseNumberList.Add(item.dict["addr:housenumber"]);
            }

            return Json(houseNumberList, JsonRequestBehavior.AllowGet);
        }
    }
}