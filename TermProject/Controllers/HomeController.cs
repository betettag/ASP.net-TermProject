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
                player = Repository.Players[0];//first time loading gets guest
            }
            ViewBag.playerCount = Repository.Players.Count()-1;
            ViewBag.playerNew = Repository.Players.Last().Username;//random stats
            ViewBag.duelsCount = Repository.Tournaments[0].Duels.Count()-1;//guest doesnt count
            return View(player);
        }
        public IActionResult Login(Player p)
        {
            if (p.PlayerID == 0 || p.Username == null)
            {
                p = Repository.Players[0];
            }
            if (p.Username != "Guest" && p.PlayerID != 1 && p.Score != 0)
                return View("Index", p);//not going to let a user login again
            return View(p);
        }
        //public IActionResult Logout(Player p)
        //{
        //        p = Repository.Players[0];
        //        return View("Index", p);//logout is user=guest not sure if im going to implement this
        //}
        [HttpPost]
        public IActionResult LoginValidation(Player p)
        {//getting right user except guest
            p = Repository.Players.Find(player => player.Username == p.Username && player.Password == p.Password && player.PlayerID != 1);
            if(p == null){
                p = Repository.Players[0];
                ModelState.AddModelError("Validation", "Wrong username or pass");
                RedirectToAction("Login");//Invalid therefore redirect to login again
            }
            return View("Index", p);//returns home for a valid user
        }
        public IActionResult Voting(Player p)//later tm
        {
            Repository.ResetTournament();
            p = Repository.Players.Find(player => player.PlayerID == p.PlayerID);

            if (p == null || p.PlayerID == 0 || p.Username == null)//if its not passed a player it sets to guest
            {
                p = Repository.Players[0];
                return View("PlayerError", p);//error view to login or create duel
            }
            else if (p.PlayerID == 1 || p.Username == "Guest" || p.Score == 0)//if guest then still error
            {
                return View("PlayerError", p);
            }
            var viewModel = new VotingViewModel();
            viewModel.player = p;//passing player and list of duels
            viewModel.Duels = Repository.Tournaments[0].Duels;

            return View(viewModel);
        }
        [HttpGet]
        public IActionResult Voted(Duel d)//post for after voting
        {

            Player player = Repository.Players.Find(p => p.PlayerID == d.VoterID);//getting player from player id
            player.Voted = true; //player has voted
            Repository.UpdateDuelVotes(d);
            return View("Index",player);
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
            Card card = new Card()
            {
                CreatorID = p.PlayerID,
                IsPrompt = false
            };
            p.DuelCard = card;//setting card to be passed
            return View(p);
        }
        [HttpPost]
        public IActionResult AddCardValidation(Player player)
        {
            //Player player = Repository.Players.Find(p => p.PlayerID == p.DuelCard.CreatorID);//getting player from player id

            if (player.DuelCard.Text != "" || player.DuelCard.Text != null || player.DuelCard.CreatorID != 0 
                && Repository.WhiteCards.Where(c => c.Text == player.DuelCard.Text) != null)//do not write duplicate or bad card
            {
                const string CensoredText = "[Censored]";
                const string PatternTemplate = @"\b({0})(s?)\b";//censoring words
                const RegexOptions Options = RegexOptions.IgnoreCase;//pattern from the insternet

                string[] badWords = new[] { "fuck", "bitch", "ass", "fucker","fucking", "flipping", "flippin", "damm" };

                IEnumerable<Regex> badWordMatchers = badWords.
                    Select(x => new Regex(string.Format(PatternTemplate, x), Options));

                string input = player.DuelCard.Text;

                string output = badWordMatchers.//matching
                Aggregate(input, (current, matcher) => matcher.Replace(current, CensoredText));

                player.DuelCard.Text = output;//replacing text
                player.DuelCard.IsPrompt = false;
                Repository.AddWhiteCard(player.DuelCard);//adding white card
                player = Repository.Players.Find(p => p.PlayerID == player.DuelCard.CreatorID);
                ModelState.AddModelError("Validation", "Card Added ");
                var viewModel = new AllCardsViewModels()
                {
                    Cards = Repository.Cards, //passing the player and cards
                    player = player
                };
                return View("AllCards", viewModel);
            }
            ModelState.AddModelError("Validation", "Card Error ");
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
            Repository.ResetTournament();
            p = Repository.Players.Find(player => player.PlayerID == p.PlayerID);

            if (p == null || p.PlayerID == 0 || p.Username == null)
            {
                p = Repository.Players[0];
            }

            var viewModel = new AllCardsViewModels()
            {
                Cards = Repository.Cards, //passing the player and cards
                player = p
            };
            
            Repository.ResetTournament();
            if (p.IsDueling == false)//not going to let a user duel twice
                return View(viewModel);
            return View("Index", p);
        }
        [HttpPost]
        public IActionResult NewDuelPost(Player player) 
        {
            if (Repository.Players.Where(p => p.Username == player.Username) != null)
            {
                var viewModel = new AllCardsViewModels()
                {
                    Cards = Repository.Cards, //passing the player and cards
                    player = player
                };
                return View(viewModel);//return view again if player has been found to avoid duplicates
            }
            Repository.ResetTournament();//reset tournament if a week has passed. keeps players
            player = Repository.AddPlayerToDuel(player);//reusing variable and adding player to duel
            return View("Index", player);
        }

        public ActionResult HighScores(Player p)
        {
            if (p.PlayerID == 0 || p.Username == null)
            {
                p = Repository.Players[0];
            }
            var viewModel = new HighScoresViewModel();
            viewModel.player = p;
            viewModel.Players = Repository.Players;//passing player and list of players
            viewModel.Players.RemoveAt(0); //not passing guest :<


            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
