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
            if (player.PlayerID == 0 || player.Username == null)
            {
                player = Repository.Players[0];
            }
            ViewBag.playerCount = Repository.Players.Count()-1;
            ViewBag.playerNew = Repository.Players.Last().Username;
            ViewBag.duelsCount = Repository.Tournaments[0].Duels.Count()-1;
            return View(player);
        }
        public IActionResult Login(Player p)
        {
            return View();
        }
        public IActionResult Voting(Player p)
        {
            return View();
        }
        public IActionResult AddCard(Player p)
        {
            return View();
        }
        public IActionResult AllCards(Player p)
        {
            var viewModel = new AllCardsViewModels() {
                Cards = Repository.Cards,
                player = p
            };
            return View(viewModel);
        }

        public IActionResult NewDuel(Player p)
        {
            return View();
        }

        public ActionResult HighScores(Player p)
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
