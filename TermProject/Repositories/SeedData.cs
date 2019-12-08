using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TermProject.Models;
using System.Globalization;

namespace TermProject.Repositories
{
    public class SeedData
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Tournaments.Any())
            {
                Card whitecard = new Card();
                whitecard.Text = "Undoubtedly White Card";
                whitecard.IsPrompt = false;

                Card whitecard2 = new Card();
                whitecard2.Text = "Undeniably White Card";
                whitecard2.IsPrompt = false;

                Card blackcard = new Card();
                blackcard.Text = "Pick The Whiter Card. (There is a right answer)...";
                blackcard.IsPrompt = true;

                Player guestPlayer = new Player()
                {
                    Username = "Guest",
                    Score = 0,
                    IsDueling = false,
                    Voted = false,
                    DuelCard = whitecard2
                };
                Player seedPlayer = new Player()
                {
                    Username = "Gino :<",
                    Score = 1,
                    IsDueling = true,
                    Voted = false,
                    DuelCard = whitecard
                };

                Duel guestDuel = new Duel();
                guestDuel.Players = new List<Player>();
                guestDuel.Players.Add(guestPlayer);
                guestDuel.Prompt = blackcard;
                guestDuel.Players.Add(seedPlayer);

                Tournament FirstTournament = new Tournament();
                FirstTournament.Duels = new List<Duel>();
                FirstTournament.Duels.Add(guestDuel);
                FirstTournament.ExpiryTime = DateTime.Now.AddDays(7);

                context.Cards.Add(whitecard);
                context.Cards.Add(blackcard);
                context.Cards.Add(blackcard);
                context.Players.Add(guestPlayer);
                context.Duels.Add(guestDuel);
                context.Tournaments.Add(FirstTournament);
                
                
                context.SaveChanges(); // save all the data
            }
        }
    }
}
