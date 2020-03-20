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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace TermProject.Controllers
{
    public class HomeController : Controller
    {
        IRepository Repository;
        UserManager<Player> userManager;
        public HomeController(IRepository r, UserManager<Player> usrMgr)
        {
            Repository = r;
            userManager = usrMgr;
        }
        public IActionResult Index()
        {

            ViewBag.playerCount = Repository.Players.Count()-1;
            ViewBag.playerNew = Repository.Players.Last().UserName;//random stats
            ViewBag.duelsCount = Repository.Tournaments[0].Duels.Count()-1;//guest doesnt count
            return View("Index");
        }
        //public IActionResult Logout(Player p)
        //{
        //        p = Repository.Players[0];
        //        return View("Index", p);//logout is user=guest not sure if im going to implement this
        //}
        //[HttpGet]
        //public IActionResult LoginValidation(Player p)
        //{//getting right user except guest
        //    p = Repository.Players.Find(player => player.UserName == p.UserName && player.Password == p.Password && player.PlayerID != 1);
        //    if(p == null){
        //        p = Repository.Players[0];
        //        ModelState.AddModelError("Validation", "Wrong username or pass");
        //        return View ("Login",p);//Invalid therefore redirect to login again
        //    }
        //    ViewBag.playerCount = Repository.Players.Count() - 1;
        //    ViewBag.playerNew = Repository.Players.Last().UserName;//random stats
        //    ViewBag.duelsCount = Repository.Tournaments[0].Duels.Count() - 1;//guest doesnt count
        //    return View ("Index",p);//returns home for a valid user
        //}
        public IActionResult Voting(Player p)//later tm
        {
            Repository.ResetTournament();

            if(p.Voted != true)
            {
                var viewModel = new VotingViewModel();
                viewModel.Duels = Repository.Tournaments[0].Duels;
                viewModel.duel = new Duel();
                viewModel.duel.Players = new List<Player>();
                viewModel.duel.Players.Add(new Player());
                viewModel.duel.Players.Add(new Player());
                return View(viewModel);
            }
            return Index();
        }
        [HttpGet]
        public IActionResult Voted(VotingViewModel v)//post for after voting
        {
            Player player = Repository.UpdateDuelVotesAndScore(v.duel);

            return Index();
        }
        [Authorize(Roles = "Member, Admins")]
        public IActionResult AddCard()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AddCardValidation(Card card)
        {
            //Player player = Repository.Players.Find(p => p.PlayerID == p.DuelCard.CreatorID);//getting player from player id

            if (!ModelState.IsValid)//do not write duplicate or bad card
            {
                ModelState.AddModelError("Validation", "Card Error ");
                return View("AddCard");
            }

            const string CensoredText = "[Censored]";
            const string PatternTemplate = @"\b({0})(s?)\b";//censoring words
            const RegexOptions Options = RegexOptions.IgnoreCase;//pattern from the internet

            string[] badWords = new[] { "fuck", "bitch", "ass", "fucker", "fucking", "flipping", "flippin", "damm" };

            IEnumerable<Regex> badWordMatchers = badWords.
                Select(x => new Regex(string.Format(PatternTemplate, x), Options));

            string input = card.Text;

            string output = badWordMatchers.//matching
            Aggregate(input, (current, matcher) => matcher.Replace(current, CensoredText));

            card.Text = output;//replacing text
            card.IsPrompt = false;
            Repository.Cards.Add(card);//adding white card
            ModelState.AddModelError("Validation", "Card Added ");
            return View("AllCards", Repository.Cards);
        }
        [AllowAnonymous]
        public IActionResult AllCards()
        {
            return View(Repository.Cards);
        }
        [Authorize(Roles = "Members, Admins")]
        public async Task<IActionResult> NewDuel()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            Repository.ResetTournament();//reset if expired
            
            if (user.IsDueling == false)//not going to let a user duel twice
                return View(Repository.Cards);
            return Index();
        }
        [HttpGet]
        public IActionResult NewDuelValidation(AllCardsViewModels v) 
        {
            Player player;
            if (Repository.Players.Find(p => p.UserName == v.player.UserName && p.Password != v.player.Password) != null 
                || v.player.UserName == null || v.player.Password == null || v.player.UserName == " " || v.player.Password ==" ") 
            {//basic validation
                player = Repository.Players.Find(p => p.PlayerID == v.player.PlayerID);
                var viewModel = new AllCardsViewModels()
                {
                    Cards = Repository.Cards, //passing the player and cards
                    player = player
                };
                return View(viewModel);//return view again if player has been found to avoid duplicates
            }
            Repository.Tournaments[0].Duels.First();//testing
            //Repository.ResetTournament();//reset tournament if a week has passed. keeps players
            player = Repository.AddPlayerToDuel(v.player);//reusing variable and adding player to duel
            //Repository.Tournaments[0].Duels.First();//testing
            ViewBag.playerCount = Repository.Players.Count() - 1;
            ViewBag.playerNew = Repository.Players.Last().UserName;//random stats
            ViewBag.duelsCount = Repository.Tournaments[0].Duels.Count() - 1;//guest doesnt count
            return View("Index", player);
        }
        [AllowAnonymous]
        public ActionResult HighScores()
        {
            //passing player and list of players
            var players = Repository.Players; //not passing Admin :<
            players.RemoveAt(0);

            return View(players);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
