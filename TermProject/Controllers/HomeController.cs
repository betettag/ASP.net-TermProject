using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TermProject.Models;
using TermProject.Repositories;
using TermProject.ViewModels;

namespace TermProject.Controllers
{
    public class HomeController : Controller
    {
        IRepository Repository;
        public HomeController(IRepository r)
        {
            Repository = r;
        }
        public IActionResult Index(Player player)
        {
            if (player == null)
            {
                player = Repository.Players[0];
            }
            ViewBag.playerCount = Repository.Players.Count()-1;
            ViewBag.playerNew = Repository.Players.Last().Username;
            ViewBag.duelsCount = Repository.Tournaments[0].Duels.Count()-1;
            return View(player);
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult HighScores(Player p)
        {
            var viewModel = new HighScoresViewModel();
            viewModel.player = p;
            if (Repository.Players.Count() > 1)
            {
                viewModel.Players = Repository.Players;
            }
            else { viewModel.Players = new List<Player>(); }
             
            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
