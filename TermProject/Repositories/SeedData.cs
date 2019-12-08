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
                whitecard.Text = "Extremely White Card";
                whitecard.IsPrompt = false;

                Card whitecard2 = new Card();
                whitecard2.Text = "Undenialbly White Card";
                whitecard2.IsPrompt = false;

                Card blackcard = new Card();
                blackcard.Text = "Pick The Whitest Card...";
                blackcard.IsPrompt = true;

                Player guestPlayer = new Player()
                {
                    Username = "Guest",
                    Score = 0,
                    IsDueling = false,
                    Voted = false,
                    
                };

                Duel guestDuel = new Duel();
                guestDuel.Players = new List<Player>();
                guestDuel.Players.Add(guestPlayer);
                guestDuel.Prompt = blackcard;
                guestDuel.Cards = new List<Card>();
                guestDuel.Cards.Add(whitecard);
                guestDuel.Cards.Add(whitecard2);

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
