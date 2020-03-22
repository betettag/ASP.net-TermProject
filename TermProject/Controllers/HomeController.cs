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
        public IActionResult Index(string message)
        {
            if (message != null)
                ModelState.AddModelError("Message", message);

            ViewBag.playerCount = Repository.Players.Count();
            ViewBag.playerNew = Repository.Players.Last().UserName;//random stats
            ViewBag.duelsCount = Repository.Tournaments[0].Duels.Count();
            return View("Index");
        }
                                                        //this is now done with identity
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
        [Authorize(Roles = "Member, Admins")]
        public async Task<IActionResult> Voting()//later tm
        {
            Repository.ResetTournament();
            Player player = await userManager.GetUserAsync(HttpContext.User);
            if (player.Voted != true)
            {
                var viewModel = new VotingViewModel();
                viewModel.Duels = Repository.Tournaments[0].Duels;
                //viewModel.duel = new Duel();
                //viewModel.duel.Players = new List<Player>();
                //viewModel.duel.Players.Add(new Player());
                //viewModel.duel.Players.Add(new Player());
                return View(viewModel);
            }
            return Index("Already voted");
        }
        [HttpPost]
        [Authorize(Roles = "Member, Admins")]
        public async Task<IActionResult> Voted(VotingViewModel v)//post for after voting
        {
            Player player = await userManager.GetUserAsync(HttpContext.User);
            if (player.Id == v.VotesPlayerId)
                ModelState.AddModelError("Validation", "You cant vote for yourself :P");
            if (ModelState.IsValid)
            {
                player.Voted = true;
                if(v.VotesPlayerId == v.PlayerId1)
                    await Repository.UpdateDuelVotesAndScore(v.PlayerId1);
                else
                    await Repository.UpdateDuelVotesAndScore(v.PlayerId2);
                return Index("Successfully voted");
            }
            return await Voting();
        }
        [Authorize(Roles = "Member, Admins")]
        public IActionResult AddCard()
        {
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Member, Admins")]
        public async Task<IActionResult> AddCardValidation(Card card)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            card.CreatorID = user.UserName;
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
            Repository.AddWhiteCard(card);//adding white card
            ModelState.AddModelError("Validation", "Card Added");
            return View("AllCards", Repository.Cards);
        }
        [AllowAnonymous]
        public IActionResult AllCards()
        {
            return View(Repository.Cards);
        }
        [Authorize(Roles = "Member, Admins")]
        public async Task<IActionResult> NewDuel()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            Repository.ResetTournament();//reset if expired

            if (user.IsDueling == false)
            {//not going to let a user duel twice
                return View("NewDuel",new AllCardsViewModels() { Cards = Repository.Cards,
                    white_card = null, black_card = null });
            }
            return Index("Already in a Duel");
        }
        
        [Authorize(Roles = "Member, Admins")]
        [HttpGet]
        public async Task<IActionResult> NewDuelValidation(AllCardsViewModels v) 
        {
            var player = await userManager.GetUserAsync(HttpContext.User);
            //if (v.white_card == null)//some validation
            //    ModelState.AddModelError("Message", "please select a white card");

            if (ModelState.IsValid) {
                //find first open duel or create one

                player.DuelCard = Repository.Cards.FirstOrDefault(c => c.CardID == v.white_card);
                await Repository.AddPlayerToDuel((int)v.black_card, (int)v.white_card, player.Id);
                return Index("You have been Added to a duel. dont forget to vote");
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            return await NewDuel();//return view again if errors happen
        }
        [AllowAnonymous]
        public ActionResult HighScores()
        {
            //passing player and list of players
            var players = Repository.Players;

            return View(players);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
