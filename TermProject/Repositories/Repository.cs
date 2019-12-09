using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TermProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace TermProject.Repositories
{
    public class Repository : IRepository
    {
        private AppDbContext context;
        public Repository(AppDbContext appDbContext)
        {
            context = appDbContext;
        }
        public List<Card> WhiteCards => context.Cards.Where(c => c.IsPrompt == false).ToList();
        public List<Card> Cards => context.Cards.ToList();
        public List<Card> Prompts => context.Cards.Where(c => c.IsPrompt == true).ToList();
        public List<Player> Players => context.Players.ToList();
        public List <Tournament> Tournaments => (context.Tournaments
            .Include(t=>t.Duels)
                .ThenInclude(d =>d.Prompt)//the duel bone is connected to the prompt bone
            .Include(t => t.Duels)
                .ThenInclude(d =>d.Players)
                    .ThenInclude(c => c.DuelCard)
            .ToList());//getting all the data here
        public void AddPlayer(Duel duel,Player player)
        {
            context.Players.Add(player);
            context.SaveChanges();
        }

        public Player AddPlayerToDuel(Player player)
        {
            ResetTournament();//reset if needed
            Player newPlayer = player;//store previous values if player is new
            player = Players.Find(p => (p.Username == player.Username) && (p.Password == player.Password));//find user
            if (player == null)
            {
                player = newPlayer;
                player.PlayerID = 0;
                player.Score = 1;
            }
            player.PromtID = newPlayer.PromtID;
            player.IsDueling = true;
            player.DuelCard = Cards.Find(c => c.CardID == newPlayer.CardID);

            Duel duel = context.Duels.Where(d =>d.Players.Count() != 2).FirstOrDefault();//getting duel that needs a player or null
            if (duel != null)//if duel not empty add player to list
            {
                duel.Players.Add(player);
                context.Players.Add(player);
                context.Duels.Update(duel);//addplayer and update
                context.Tournaments.Update(context.Tournaments.First());
            }
            else
            {
                Duel newDuel = new Duel();
                newDuel.Prompt = Cards.Find(c => c.CardID == player.PromtID);
                newDuel.Players = new List<Player>();
                newPlayer = player;
                newDuel.Players.Add(newPlayer);//new duel add new player
                //context.Players.Update(player); not needed
                context.Tournaments.First().Duels.Add(newDuel);
            }

            context.SaveChanges();
            return player;
        }
        public void AddWhiteCard(Card whiteCard)
        {
            if (whiteCard.IsPrompt == false)
            {
                context.Cards.Add(whiteCard);//if not black card then add
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
        public Player UpdateDuelVotesAndScore(Duel duel)
        {            
            bool flag;
            int Votes;
            int PlayerID;
            if (duel.VotesP1 == 0) {//modifying only one field and capturing variables
                Votes = duel.VotesP2;
                flag = false;
                PlayerID = duel.Players[1].PlayerID;
            }
            else
            {
                Votes = duel.VotesP1;
                flag = true;
                PlayerID = duel.Players[0].PlayerID;
            }
            Player player = Players.Find(p => p.PlayerID == duel.VoterID);
            Player player2 = Players.Find(p => p.PlayerID == Tournaments[0].Duels.Find(d =>d.DuelID == duel.DuelID).Players.Find(p2 => p2.PlayerID == PlayerID).PlayerID);
            if (player.Equals(player2))
                return player; //cant vote for yourself try again
            ++player2.Score;
            ResetTournament();//reset if needed
            player.Voted = true;
            context.Players.Update(player);//getting player from player id and updating it
            context.Players.Update(player2);//adding points

            duel = Tournaments[0].Duels.Find(d => d.DuelID == duel.DuelID);
            if (flag)
            {
                duel.VotesP2 = Votes;
            }
            else
            {
                duel.VotesP1 = Votes;
            }
            
            
            context.Duels.Update(duel);//updating duel votes

            context.SaveChanges();
            return player;
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
