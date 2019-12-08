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
            Player newPlayer = player;//lets see if its not by reference
            player = Players.Find(p => (p.Username == player.Username) && (p.Password == player.Password));//find user
            if (player == null)
            {
                player = new Player();
                player.Username = newPlayer.Username;//copy prev parameters passed
                player.Password = newPlayer.Password;
            }
            player.PromtID = newPlayer.PromtID;
            player.IsDueling = true;
            player.DuelCard = Cards.Find(c => c.CardID == newPlayer.CardID);

            Duel duel = context.Duels.Find(Players.Count() != 2);//getting duel that needs a player or null
            if (duel != null)//if duel not empty add player to list
            {
                duel.Players.Add(player);
                context.Duels.Update(duel);//addplayer and update
            }
            else
            {
                Duel newDuel = new Duel();
                newDuel.Prompt = Cards.Find(c => c.CardID == player.PromtID);
                newDuel.Players.Add(player);//new duel add new player
                context.Duels.Add(newDuel);
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
        public void UpdateDuelVotes(Duel duel)
        {
            ResetTournament();//reset if needed
            if (duel.DuelID != 0)
            {
                int VotesP1 = duel.VotesP1;
                int VotesP2 = duel.VotesP2;
                duel = Tournaments[0].Duels.Find(d => d.DuelID == duel.DuelID);
                context.Tournaments.Update(Tournaments[0]);
                context.SaveChanges();
            }
        }
        public void ResetTournament()
        {
            if (Tournaments[0].ExpiryTime > DateTime.Now)
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
            //do nothing :<
        }
    }
}
