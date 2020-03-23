using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TermProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Identity;

namespace TermProject.Repositories
{
    public class Repository : IRepository
    {
        private AppDbContext context;
        private UserManager<Player> userManager;
        public Repository(AppDbContext appDbContext, UserManager<Player> usrMgr)
        {
            context = appDbContext;
            userManager = usrMgr;
        }
        public List<Card> WhiteCards => context.Cards.Where(c => c.IsPrompt == false).ToList();
        public List<Card> Cards => context.Cards.ToList();
        public List<Card> Prompts => context.Cards.Where(c => c.IsPrompt == true).ToList();
        public List<Player> Players => userManager.Users.ToList();
        public List <Tournament> Tournaments => (context.Tournaments
            .Include(t=>t.Duels)
                .ThenInclude(d =>d.Prompt)//the duel bone is connected to the prompt bone
            .Include(t => t.Duels)
                .ThenInclude(d =>d.Player1)
                    .ThenInclude(c => c.DuelCard)
            .Include(t => t.Duels)
                .ThenInclude(d => d.Player2)
                    .ThenInclude(c => c.DuelCard)
            .ToList());//getting all the data here

        public async Task AddPlayerToDuel(int black_card, int white_card, string playerId)
        {
            ResetTournament();//reset if needed
            var player = await userManager.FindByIdAsync(playerId);
            var duel = Tournaments[0].Duels.Where(d => d.Players.Count() == 1 || d.Player2ID == null).FirstOrDefault();
            if (duel == null)
            {
                duel = new Duel()
                {
                    Player1ID = playerId,
                    Prompt = Cards.FirstOrDefault(c => c.CardID == black_card),
                    CardID = black_card,//create new duel
                    Players = new List<Player>()
                };
                duel.Players.Add(player);
                Tournaments[0].Duels.Add(duel);
            }
            else {
                duel.Players.Add(player);//if it has found a spot ad another player
                duel.Player2ID = player.Id;
                player.IsDueling = true;
            }
            
            //return;


            //Player newPlayer = player;//store previous values if player is new
            //player = Players.Find(p => (p.UserName == player.UserName) && (p.Password == player.Password));//find user
            //if (player == null)
            //{
            //    player = newPlayer;
            //    player.PlayerID = 0;
            //    player.Score = 1;
            //}
            //player.PromtID = newPlayer.PromtID;
            //player.IsDueling = true;
            //player.DuelCard = Cards.Find(c => c.CardID == newPlayer.CardID);

            //Duel duel = context.Duels.Where(d =>d.Players.Count() != 2).FirstOrDefault();//getting duel that needs a player or null
            //if (duel != null)//if duel not empty add player to list
            //{
            //    duel.Players.Add(player);
            //    //context.Players.Add(player);
            //    context.Duels.Update(duel);//addplayer and update
            //    context.Tournaments.Update(context.Tournaments.First());
            //}
            //else
            //{
            //    Duel newDuel = new Duel();
            //    newDuel.Prompt = Cards.Find(c => c.CardID == player.PromtID);
            //    newDuel.Players = new List<Player>();
            //    newPlayer = player;
            //    newDuel.Players.Add(newPlayer);//new duel add new player
            //    //context.Players.Update(player); not needed
            //    context.Tournaments.First().Duels.Add(newDuel);
            //}

            context.SaveChanges();
        }
        public void AddWhiteCard(Card whiteCard)
        {
            if (whiteCard.IsPrompt == false && whiteCard.Text != null && whiteCard.Text != "")
            {
                context.Cards.Add(whiteCard);//if not black or empty card then add
                context.SaveChanges();
            }
        }
        public void AddPrompt(Card prompt)
        {
            if (prompt.IsPrompt == true)
            {
                context.Cards.Add(prompt);
                context.SaveChanges();
            }
        }
        public async Task UpdateDuelVotesAndScore(string playerId)
        {           
            var player = await userManager.FindByIdAsync(playerId);
            ++player.Score;

            context.SaveChanges();
        }
        public void ResetTournament()
        {
            if (Tournaments[0].ExpiryTime < DateTime.Now)
            {
                //List<Player> players = context.Players.ToList();
                foreach (var tournament in context.Tournaments)//remove if expired tournament
                {
                    foreach (var duel in tournament.Duels)
                    {
                        foreach (var player in duel.Players)
                        {
                            player.IsDueling = false;
                            player.PromtID = 0;
                            player.Voted = false;
                        }
                        context.Duels.Remove(duel);
                    }
                    context.Tournaments.Remove(tournament);
                }
                //foreach (var player in players)
                //context.Players.Add(player);

                //add seed data
                Duel guestDuel = new Duel();
                guestDuel.Players = new List<Player>();
                guestDuel.Players.Add(Players[0]);
                guestDuel.Prompt = Prompts[0];
                guestDuel.Players.Add(Players[1]);
                guestDuel.Player1ID = Players[0].Id;
                guestDuel.Player2ID = Players[1].Id;

                Tournament FirstTournament = new Tournament();
                FirstTournament.Duels = new List<Duel>();
                FirstTournament.Duels.Add(guestDuel);
                FirstTournament.ExpiryTime = DateTime.Now.AddDays(7);

                context.Duels.Add(guestDuel);
                context.Tournaments.Add(FirstTournament);
                context.SaveChanges();
            }
            //do nothing :< because not expired
        }
    }
}
