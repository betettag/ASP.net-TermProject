using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TermProject.Models;
using TermProject.Repositories;
using TermProject.ViewModels;
using Microsoft.EntityFrameworkCore;

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
            if (p.PlayerID == 0 || p.Username == null)
            {
                p = Repository.Players[0];
            }
            return View(p);
        }
        [HttpPost]
        public IActionResult LoginValidation(Player p)
        {//getting right user
            p = Repository.Players.Find(player => player.Username == p.Username && player.Password == p.Password && player.PlayerID != 1);
            if(p == null){
                p = Repository.Players[0];
                RedirectToAction("Login");//Invalid therefore redirect to login again
            }
            return View("Index", p);//returns home for a valid user
        }
        public IActionResult Voting(Player p)
        {
            return View(p);
        }
        public IActionResult AddCard(Player p)
        {
            if (p.PlayerID == 0 || p.Username == null)//if its not passed a player it sets to guest
            {
                p = Repository.Players[0];
                return View("PlayerError",p);//error view to login or create duel
            }else if (p.PlayerID == 1 || p.Username == "Guest" || p.Score == 0)//if guest then still error
            {
                return View("PlayerError", p);
            }
            return View(p);
        }
        [HttpPost]
        public IActionResult AddCard(Card c)
        {
            Player player = Repository.Players.Find(p => p.PlayerID == c.CreatorID);//getting player from player id

            if (c.Text != "" || c.Text == null || c.CreatorID == 0 
                && Repository.WhiteCards.Contains(c) == false)//do not write duplicate or bad card
            {
                const string CensoredText = "[Censored]";
                const string PatternTemplate = @"\b({0})(s?)\b";//censoring words
                const RegexOptions Options = RegexOptions.IgnoreCase;//pattern from the insternet

                string[] badWords = new[] { "fuck", "bitch", "ass", "fucker" };

                IEnumerable<Regex> badWordMatchers = badWords.
                    Select(x => new Regex(string.Format(PatternTemplate, x), Options));

                string input = c.Text;

                string output = badWordMatchers.//matching
                Aggregate(input, (current, matcher) => matcher.Replace(current, CensoredText));

                c.Text = output;//replacing text
                c.IsPrompt = false;
                Repository.AddWhiteCard(c);//adding white card
                return View("AllCards", player);
            }
            return View("AddCard", player);
        }
        public IActionResult AllCards(Player p)
        {
            if (p.PlayerID == 0 || p.Username == null)
            {
                p = Repository.Players[0];
            }
            var viewModel = new AllCardsViewModels() {
                Cards = Repository.Cards, //passing the player and cards
                player = p
            };
            return View(viewModel);
        }

        public IActionResult NewDuel(Player p)
        {
            if (p.PlayerID == 0 || p.Username == null)
            {
                p = Repository.Players[0];
            }
            var viewModel = new AllCardsViewModels()
            {
                Cards = Repository.Cards, //passing the player and cards
                player = p
            };
            return View(viewModel);
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
