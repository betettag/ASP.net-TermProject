using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TermProject.Models;

namespace TermProject.Repositories
{
    public class SeedData
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Tournaments.Any())
            {
                Card whitecard = new Card();
                whitecard.Text = "testwhiteCard";
                whitecard.IsPrompt = false;
                Card blackcard = new Card();
                blackcard.Text = "testblackCard";
                blackcard.IsPrompt = true;
                Player guestPlayer = new Player()
                {
                    Username = "Guest",
                    Score = 0,
                    IsDueling = false,
                    Voted = false,
                    CardChosen = whitecard
                };

                Duel guestDuel = new Duel();
                guestDuel.Players = new List<Player>();
                guestDuel.Players.Add(guestPlayer);
                guestDuel.Prompt = blackcard;
                Tournament FirstTournament = new Tournament();
                FirstTournament.Duels = new List<Duel>();
                FirstTournament.Duels.Add(guestDuel);

                context.Cards.Add(whitecard);
                context.Cards.Add(blackcard);
                context.Players.Add(guestPlayer);
                context.Duels.Add(guestDuel);
                context.Tournaments.Add(FirstTournament);
                
                
                context.SaveChanges(); // save all the data
            }
        }
    }
}
