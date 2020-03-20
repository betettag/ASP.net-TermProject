using System;
using Xunit;
using TermProject.Models;
using TermProject.Repositories;
using TermProject.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TermProject.ViewModels;
using System.Linq;

namespace TermProjectTests
{
    public class HomeControllerTests
    {
        IRepository repo = new FakeRepository();
        public int lastcardid = 0;
        public int lastplayerid = 0;
        public int lastduelid = 0;
        private void Arrange()
        {
            repo.Players.Clear();
            repo.Cards.Clear();
            repo.Tournaments[0].Duels.Clear();
            repo.Tournaments.Clear();
            Tournament t = new Tournament();
            repo.Tournaments.Add(t);
            Card whitecard = new Card();
                whitecard.Text = "Undoubtedly White Card";
                whitecard.IsPrompt = false;
                whitecard.CreatorID = 1;
                whitecard.CardID = ++lastcardid;

            Card whitecard2 = new Card();
            whitecard2.Text = "Undeniably White Card";
            whitecard2.IsPrompt = false;
            whitecard2.CreatorID = 1;
            whitecard2.CardID = ++lastcardid;

            Card blackcard = new Card();
            blackcard.Text = "Pick The Whiter Card. (There is a right answer)...";
            blackcard.IsPrompt = true;
            blackcard.CreatorID = 1;
            blackcard.CardID = ++lastcardid;
            repo.Cards.Add(whitecard);
            repo.Cards.Add(whitecard2);
            repo.Cards.Add(blackcard);

            Player guestPlayer = new Player()
            {
                Username = "Guest",
                Score = 0,
                IsDueling = false,
                Voted = true,
                DuelCard = whitecard,
                PlayerID = 1,
                CardID = whitecard.CardID,
                PromtID = blackcard.CardID
            };
            Player seedPlayer = new Player()
            {
                Username = "Gino :<",
                Score = 1,
                IsDueling = true,
                Voted = false,
                DuelCard = whitecard2,
                Password = "pass",
                PlayerID = 2,
                CardID = whitecard.CardID,
                PromtID = blackcard.CardID
            };


            repo.AddPlayerToDuel(guestPlayer);
            repo.AddPlayerToDuel(seedPlayer);
                
        }
        
        [Fact]
        public void Index()
        {
            Arrange();

            Player player = new Player();//test
            var controller = new HomeController(repo);
            var index = controller.Index(player) as ViewResult;

            //assert
            Assert.Equal(1, index.ViewData["playerCount"]);
            Assert.Equal(repo.Players[1].Username, index.ViewData["playerNew"].ToString());
            Assert.Equal(0, index.ViewData["duelsCount"]);
            Assert.Equal(repo.Players[0], index.Model);
        }

        [Fact]
        public void Login()
        {
            Arrange();

            Player player = new Player();//test
            var controller = new HomeController(repo);
            var login = controller.Login(player) as ViewResult;

            //assert
            Assert.Equal(repo.Players[0], login.Model);
        }

        [Fact]
        public void LoginValidation()
        {
            Arrange();
            Player player = new Player();
            Player validPlayer = repo.Players[1];

            var controller = new HomeController(repo);//test
            var loginval = controller.LoginValidation(player) as ViewResult;//invalid player

            //assert
            Assert.Null(loginval.ViewData["playerCount"]);
            Assert.Null(loginval.ViewData["playerNew"]);
            Assert.Null(loginval.ViewData["duelsCount"]);
            Assert.NotEqual(validPlayer, loginval.Model);
            Assert.Equal(repo.Players[0], loginval.Model);

            var loginval2 = controller.LoginValidation(validPlayer) as ViewResult;//test valid
            //assert
            Assert.Equal(1, loginval2.ViewData["playerCount"]);
            Assert.Equal(validPlayer.Username, loginval2.ViewData["playerNew"].ToString());
            Assert.Equal(0, loginval2.ViewData["duelsCount"]);
            Assert.Equal(validPlayer, loginval2.Model);
        }
        [Fact]
        public void Voting()
        {
            Arrange();
            Player player = new Player();
            Player guest = repo.Players[0];
            Player validPlayer = repo.Players[1];

            var controller = new HomeController(repo);//test
            var voting = controller.Voting(player) as ViewResult;//invalid player

            //assert
            Assert.NotEqual(new VotingViewModel().GetType(), voting.Model.GetType());
            Assert.NotEqual(validPlayer, voting.Model);
            Assert.NotEqual(player, voting.Model);
            Assert.Equal(guest, voting.Model);

            var voting2 = controller.Voting(guest) as ViewResult;//invalid player2
            Assert.NotEqual(new VotingViewModel().GetType(), voting2.Model.GetType());
            Assert.Equal(guest, voting2.Model);

            var voting3 = controller.Voting(validPlayer) as ViewResult;//invalid player2
            //assert

            Assert.Equal(new VotingViewModel().GetType(), voting3.Model.GetType());
            Assert.NotEqual(validPlayer, voting3.Model);
            Assert.NotEqual(player, voting3.Model);
        }
        [Fact]
        public void Voted()
        {
            Arrange();
            VotingViewModel v = new VotingViewModel()
            {
                duel = repo.Tournaments[0].Duels[0],
                player = repo.Tournaments[0].Duels[0].Players[1]
            };
            v.duel.VotesP2 = 1;//voting for only one
            v.duel.VotesP1 = 0;
            v.duel.VoterID = 2;//invalid voter

            var controller = new HomeController(repo);//test
            var voteVal = controller.Voted(v) as ViewResult;//invalid player

            //assert
            Assert.False(repo.Players[1].Voted);//cant vote for yourself
            Assert.True(repo.Players[0].Score == 1);
            Assert.True(repo.Players[1].Score == 1);
            Assert.Equal(1, voteVal.ViewData["playerCount"]);
            Assert.Equal(v.player.Username, voteVal.ViewData["playerNew"].ToString());
            Assert.Equal(0, voteVal.ViewData["duelsCount"]);
            Assert.Equal(v.player, voteVal.Model);
            Assert.NotEqual(repo.Players[0], voteVal.Model);

            VotingViewModel v2 = new VotingViewModel() //arrange valid
            {
                duel = repo.Tournaments[0].Duels[0],
                player = repo.Tournaments[0].Duels[0].Players[1]
            };
            v2.duel.VotesP1 = 1;//voting for only one
            v2.duel.VotesP2 = 0;
            v2.duel.VoterID = 2;//valid

            var voteval2 = controller.Voted(v2) as ViewResult;//test valid
            //assert
            Assert.True(repo.Players[0].Voted);
            Assert.True(repo.Players[0].Score == 2);
            Assert.Equal(1, voteVal.ViewData["playerCount"]);
            Assert.Equal(v.player.Username, voteVal.ViewData["playerNew"].ToString());
            Assert.Equal(0, voteVal.ViewData["duelsCount"]);
            Assert.Equal(v.player, voteVal.Model);
            Assert.NotEqual(repo.Players[0], voteVal.Model);
        }
        [Fact]
        public void AddCard()
        {
            Arrange();
            Player player = new Player();
            repo.Players.Add(new Player() { PlayerID = 3, Username = "valid player", 
                DuelCard = new Card() { CreatorID = 3 } });
            Player invalidPlayer = repo.Players[1];
            Player validPlayer = repo.Players[2];
            //test
            var controller = new HomeController(repo);
            var addcard = controller.AddCardAsync(player) as ViewResult;//error

            //assert
            Assert.Equal(repo.Players[0], addcard.Model);

            //test
            var addcard2 = controller.AddCardAsync(validPlayer) as ViewResult;//valid

            //assert
            Assert.Equal(validPlayer, addcard2.Model);
            Assert.NotEqual(invalidPlayer, addcard2.Model);
        }
        [Fact]
        public void AddCardValidation()
        {
            Arrange();
            repo.Players.Add(new Player()
            {
                PlayerID = 3,
                Username = "test player",
                DuelCard = new Card() { CreatorID = 3, Text = null }
            });
            Player player = repo.Players[2];
            int prevValue = repo.Cards.Count();
            //test
            var controller = new HomeController(repo);
            var addcardval = controller.AddCardValidation(player) as ViewResult;//error

            //assert
            Assert.Equal(player, addcardval.Model);
            Assert.True(repo.Cards.Count() == prevValue);

            //arrange
            player = repo.Players[2];
            player.DuelCard.Text = "";

            var addcardval2 = controller.AddCardValidation(player) as ViewResult;//error

            //assert
            Assert.Equal(repo.Players[2], addcardval2.Model);
            Assert.True(repo.Cards.Count() == prevValue);

            //arrange
            player = repo.Players[2];
            player.DuelCard.CreatorID = 0;

            var addcardval3 = controller.AddCardValidation(player) as ViewResult;//error

            //assert
            Assert.Equal(repo.Players[2], addcardval3.Model);
            Assert.True(repo.Cards.Count() == prevValue);

            //arrange
            player = repo.Players[2];
            player.DuelCard.CreatorID = 3;
            player.DuelCard = repo.Cards[0];

            var addcardval4 = controller.AddCardValidation(player) as ViewResult;//error

            //assert
            Assert.Equal(repo.Players[2], addcardval4.Model);
            Assert.True(repo.Cards.Count() == prevValue);

            //arrange
            player = repo.Players[2];
            player.DuelCard.CreatorID = 3;
            player.DuelCard = new Card() { CreatorID = 3, Text = "validCard" };

            var addcardval5 = controller.AddCardValidation(player) as ViewResult;//valid

            //assert
            Assert.Equal(new AllCardsViewModels().GetType(), addcardval5.Model.GetType());
            Assert.True(repo.Cards.Count() != prevValue);
        }
        [Fact]
        public void AllCards()
        {
            Arrange();
            Player player = repo.Players[0];

            var controller = new HomeController(repo);
            var allcards = controller.AllCards(player) as ViewResult;//test

            //assert
            Assert.Equal(new AllCardsViewModels().GetType(), allcards.Model.GetType());
        }
        [Fact]
        public void Highscores()
        {
            Arrange();
            Player player = repo.Players[0];

            var controller = new HomeController(repo);
            var highscores = controller.HighScores(player) as ViewResult;//test

            //assert
            Assert.Equal(new HighScoresViewModel().GetType(), highscores.Model.GetType());
        }
        [Fact]
        public void NewDuel()
        {
            Arrange();
            Player validPlayer = repo.Players[0];
            validPlayer.IsDueling = false;

            var controller = new HomeController(repo);
            var newduel = controller.NewDuel(validPlayer) as ViewResult;//test valid

            //assert
            Assert.Equal(new AllCardsViewModels().GetType(), newduel.Model.GetType());


            Player invalidPlayer = repo.Players[1];//arrange

            var newduel2 = controller.NewDuel(validPlayer) as ViewResult;//test invalid

            //assert
            Assert.Equal(new AllCardsViewModels().GetType(), newduel2.Model.GetType());
        }
        [Fact]
        public void NewDuelValidation()
        {
            Arrange();
            Player player = new Player()
            {
                PlayerID = 3,
                Username = "test player",
                DuelCard = new Card() { CreatorID = 3, Text = null }
            };
            AllCardsViewModels v = new AllCardsViewModels()
            {
                player = player,
                Cards = repo.Cards
            };
            int prevValue = repo.Players.Count();
            //test
            var controller = new HomeController(repo);
            var newduelval = controller.NewDuelValidation(v) as ViewResult;//error

            //assert
            Assert.Equal(new AllCardsViewModels().GetType(), newduelval.Model.GetType());
            Assert.True(repo.Players.Count() == prevValue);

            //arrange
            Player player2 = new Player()
            {
                PlayerID = 3,
                Username = "test",
                Password = "test",
                DuelCard = repo.Cards[0]
            };
            v.player = new Player()
            {
                PlayerID = 3,
                Username = "test",
                Password = "not right",
                DuelCard = repo.Cards[0]
            };
            repo.Players.Add(player2);
            prevValue = repo.Players.Count();

            var newduelval2 = controller.NewDuelValidation(v) as ViewResult;//error

            //assert
            Assert.Equal(new AllCardsViewModels().GetType(), newduelval2.Model.GetType());
            Assert.True(repo.Players.Count() == prevValue);
            Assert.False(repo.Players[2].IsDueling);

            //arrange
            v.player = new Player()
            {
                PlayerID = 3,
                Username = "test",
                Password = null,
                DuelCard = repo.Cards[0]
            };

            var newduelval3 = controller.NewDuelValidation(v) as ViewResult;//error
            
            //assert
            Assert.Equal(new AllCardsViewModels().GetType(), newduelval3.Model.GetType());
            Assert.True(repo.Players.Count() == prevValue);
            Assert.False(repo.Players[2].IsDueling);

            //arrange
            v.player = new Player()
            {
                PlayerID = 3,
                Username = null,
                Password = "test",
                DuelCard = repo.Cards[0]
            };
            
            var newduelval4 = controller.NewDuelValidation(v) as ViewResult;//error

            //assert
            Assert.Equal(new AllCardsViewModels().GetType(), newduelval4.Model.GetType());
            Assert.True(repo.Players.Count() == prevValue);

            v.player = player2;
            repo.Players.Remove(player2);
            prevValue = repo.Players.Count();
            int prevValueDuels = repo.Tournaments[0].Duels.Count();

            var newduelval5 = controller.NewDuelValidation(v) as ViewResult;//error

            //assert
            Assert.Equal(repo.Players[2], newduelval5.Model);
            Assert.True(repo.Players[2].IsDueling);
            Assert.True(repo.Players.Count() != prevValue);
            Assert.True(repo.Tournaments[0].Duels.Count() != prevValueDuels);
        }
    }
}
