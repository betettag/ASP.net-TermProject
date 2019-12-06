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

        public void AddDuel(Duel duel, Player player)
        {
            if (duel.Players.Any())//if duel not empty add player to list
            {
                context.Duels.Where(d => (d.DuelID == duel.DuelID))
                    .FirstOrDefault();
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
                context.Cards.Add(whiteCard);
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
    }
}
