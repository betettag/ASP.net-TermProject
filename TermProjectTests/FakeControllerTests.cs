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
    public class FakeControllerTests
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
        }
        [Fact]
        public void Cards()
        {
            Card whitecard = new Card();
            whitecard.Text = "Undoubtedly White Card";//arrange
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

            repo.Cards.Add(whitecard);//test
            repo.Cards.Add(whitecard2);
            repo.Cards.Add(blackcard);

            //assert
            Assert.True(repo.WhiteCards.Count() == 2);
            Assert.True(repo.Prompts.Count() == 1);
            Assert.True(repo.Cards.Count() == 3);

        }
        [Fact]
        public void AddPlayerToDuel()
        {
            Arrange();
            Player guestPlayer = new Player()
            {
                UserName = "Guest",
                Score = 0,
                IsDueling = false,
                Voted = true,
                DuelCard = repo.Cards[0],
                PlayerID = 1,
                CardID = repo.Cards[0].CardID,
                PromtID = repo.Cards[2].CardID
            };
            Player seedPlayer = new Player()
            {
                UserName = "Gino :<",
                Score = 1,
                IsDueling = true,
                Voted = false,
                DuelCard = repo.Cards[1],
                Password = "pass",
                PlayerID = 2,
                CardID = repo.Cards[1].CardID,
                PromtID = repo.Cards[2].CardID
            };
            int previousPlayers = repo.Players.Count();
            int previousDuels = repo.Tournaments[0].Duels.Count();
            repo.AddPlayerToDuel(guestPlayer);//test

            //assert
            Assert.True(repo.Tournaments[0].Duels.Count() != previousDuels);
            Assert.True(repo.Players.Count() != previousPlayers);

            //Arrange
            previousPlayers = repo.Players.Count();
            previousDuels = repo.Tournaments[0].Duels.Count();

            repo.AddPlayerToDuel(seedPlayer);//test2

            //assert
            Assert.True(repo.Tournaments[0].Duels.Count() == previousDuels);
            Assert.True(repo.Players.Count() != previousPlayers);
        }
        [Fact]
        public void UpdateDuelVotesAndScore()
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
                UserName = "Guest",
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
                UserName = "Gino :<",
                Score = 1,
                IsDueling = true,
                Voted = false,
                DuelCard = whitecard2,
                Password = "pass",
                PlayerID = 2,
                CardID = whitecard.CardID,
                PromtID = blackcard.CardID
            };

            //arrange
            repo.AddPlayerToDuel(guestPlayer);
            repo.AddPlayerToDuel(seedPlayer);
            Duel duel = repo.Tournaments[0].Duels[0];
            duel.VotesP2 = 1;//voting for only one
            duel.VotesP1 = 0;
            duel.VoterID = 2;//invalid voter

            repo.UpdateDuelVotesAndScore(duel);//test

            Assert.False(repo.Players[1].Voted);//cant vote for yourself
            Assert.True(repo.Players[0].Score == 1);//scores unchanged
            Assert.True(repo.Players[1].Score == 1);

            duel.VotesP1 = 1;//voting for only one
            duel.VotesP2 = 0;
            duel.VoterID = 2;//valid

            repo.UpdateDuelVotesAndScore(duel);//test

            //assert
            Assert.True(repo.Players[0].Voted);
            Assert.True(repo.Players[0].Score == 2);
        }
        [Fact]
        public void ResetTournament()
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
                UserName = "Guest",
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
                UserName = "Gino :<",
                Score = 1,
                IsDueling = true,
                Voted = false,
                DuelCard = whitecard2,
                Password = "pass",
                PlayerID = 2,
                CardID = whitecard.CardID,
                PromtID = blackcard.CardID
            };

            //arrange
            repo.AddPlayerToDuel(guestPlayer);
            repo.AddPlayerToDuel(seedPlayer);
            repo.Tournaments[0].Duels.Add(new Duel() { });
            int prevValue = repo.Tournaments[0].Duels.Count();
            repo.Tournaments[0].ExpiryTime =(DateTime.Now.AddDays(-8));
            //expired
            repo.ResetTournament();

            Assert.True(repo.Players.Count() ==2);
            Assert.True(repo.Tournaments[0].Duels.Count() != prevValue);
            Assert.True(repo.Tournaments[0].ExpiryTime > DateTime.Now);

            repo.Tournaments[0].Duels.Add(new Duel() { });//arrange
            int prevValue2 = repo.Tournaments[0].Duels.Count();

            repo.ResetTournament();//test

            Assert.True(repo.Tournaments[0].Duels.Count() == prevValue2);
        }
    }
}
