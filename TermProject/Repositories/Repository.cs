using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TermProject.Models;
using Microsoft.EntityFrameworkCore;

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
                .ThenInclude(d=>d.Players)
            .ToList());//getting all the data here
        public void AddPlayer(Duel duel,Player player)
        {
            context.Players.Add(player);
            context.SaveChanges();
        }

        public void AddPlayerToDuel(Duel duel)
        {
            Player player = Players.Find(p => p.PlayerID == duel.VoterID);//getting player from id
            duel = context.Duels.Find(Players.Count() != 2);//getting duel that needs a player
            if (duel != null)//if duel not empty add player to list
            {
                duel.Players.Add(player);
                context.Duels.Update(duel);//addplayer and update
            }
            else
            {
                Duel newDuel = new Duel();
                newDuel.Players[0] = player;//new duel add player
                context.Duels.Add(duel);
            }

            context.SaveChanges();
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
            if (duel.DuelID != 0)
            {
                int VotesP1 = duel.VotesP1;
                int VotesP2 = duel.VotesP2;
                duel = Tournaments[0].Duels.Find(d => d.DuelID == duel.DuelID);
                context.Tournaments.Update(Tournaments[0]);
                context.SaveChanges();
            }
        }
    }
}
