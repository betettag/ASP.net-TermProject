using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TermProject.Models;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace TermProject.Repositories
{
    public class SeedData
    {
        public static async Task SeedAsync(AppDbContext context, UserManager<Player> usrMgr, RoleManager<IdentityRole> roleMgr, IConfiguration configuration)
        {
            if (!context.Tournaments.Any())
            {

                UserManager<Player> userManager = usrMgr;
                RoleManager<IdentityRole> roleManager = roleMgr;

                Card whitecard = new Card();
                whitecard.Text = "Undoubtedly White Card";
                whitecard.IsPrompt = false;
                whitecard.CreatorID = "seeded";
                context.Cards.Add(whitecard);

                Card whitecard2 = new Card()
                {
                    Text = "Undeniably White Card",
                    IsPrompt = false,
                    CreatorID = "seeded"
                };
                context.Cards.Add(whitecard2);
                context.SaveChanges();

                Player seedPlayer = new Player()
                {
                    UserName = "SeedPlayer",
                    Email = "seedemail@email.com",
                    Score = 1,
                    IsDueling = true,
                    Voted = false,
                    DuelCard = whitecard2,
                    Password = "Secret2!",
                    ProfilePicture = "profile.png"
                };
                // Getting user info out of appsettings.json   
                string username = configuration["Data:AdminUser:Name"];
                string email = configuration["Data:AdminUser:Email"];
                string password = configuration["Data:AdminUser:Password"];
                string role = configuration["Data:AdminUser:Role"];

                if (await userManager.FindByNameAsync(username) == null)
                {
                    if (await roleManager.FindByNameAsync(role) == null)
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                        await roleManager.CreateAsync(new IdentityRole("Member"));
                    }

                    Player user = new Player
                    {
                        UserName = username,
                        Email = email,
                        Score = 1,
                        Voted = false,
                        IsDueling = true,
                        DuelCard = whitecard,
                        ProfilePicture = "profile.png",
                        Password = "Secret1!"
                    };

                    var result2 = await userManager.CreateAsync(seedPlayer, password);
                    IdentityResult result = await userManager
                    .CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, role);
                    }
                    if (result2.Succeeded)
                    {
                        await userManager.AddToRoleAsync(seedPlayer, "Member");
                    }
                }



                Card blackcard = new Card();
                blackcard.Text = "Pick The Whiter Card. (There is a right answer)...";
                blackcard.IsPrompt = true;
                blackcard.CreatorID = "seeded";
                context.Cards.Add(blackcard);

                Card blackcard2 = new Card();
                blackcard2.Text = "Im going to cleanse this week, nothing but kale and ...";
                blackcard2.IsPrompt = true;
                blackcard2.CreatorID = "seeded";
                context.Cards.Add(blackcard2);

                Card blackcard3 = new Card();
                blackcard3.Text = "Believe it or not, Jim Carrey can do a dead-on impression of ...";
                blackcard3.IsPrompt = true;
                blackcard3.CreatorID = "seeded";
                context.Cards.Add(blackcard3);
                Card blackcard4 = new Card();
                blackcard4.Text = "It’s Morphin’ Time! Mastodon! Pterodactyl! Triceratops! Sabertooth Tiger!...!";
                blackcard4.IsPrompt = true;
                blackcard4.CreatorID = "seeded";
                context.Cards.Add(blackcard4);
                Card blackcard5 = new Card();
                blackcard5.Text = "I’m not going to lie. I despise ... There, I said it.";
                blackcard5.IsPrompt = true;
                blackcard5.CreatorID = "seeded";
                context.Cards.Add(blackcard5);
                Card blackcard6 = new Card();
                blackcard6.Text = "Sir, we found you passed out naked on the side of the road. What’s the last thing you remember?";
                blackcard6.IsPrompt = true;
                blackcard6.CreatorID = "seeded";
                context.Cards.Add(blackcard6);
                Card blackcard7 = new Card();
                blackcard7.Text = "My name is Inigo Montoya. You killed my father. Prepare for ....";
                blackcard7.IsPrompt = true;
                blackcard7.CreatorID = "seeded";
                context.Cards.Add(blackcard7);
                Card blackcard8 = new Card();
                blackcard8.Text = "A study published in Nature this week found that ... is good for you in small doses.";
                blackcard8.IsPrompt = true;
                blackcard8.CreatorID = "seeded";
                context.Cards.Add(blackcard8);
                Card blackcard9 = new Card();
                blackcard9.Text = "What really killed the dinosaurs?";
                blackcard9.IsPrompt = true;
                blackcard9.CreatorID = "seeded";
                context.Cards.Add(blackcard9);

                List<string> blackcards = new List<string>();
                blackcards.Add("Hey, you guys want to try this awesome new game? It’s called ...");
                blackcards.Add("It’s not delivery. It’s ...");
                foreach (string card in blackcards)
                {
                    context.Cards.Add(new Card() { Text = card, IsPrompt = true, CreatorID = "seeded" });
                }


                
                //Player seedPlayer = new Player()
                //{
                //    UserName = "SeedPlayer2",
                //    Score = 1,
                //    IsDueling = true,
                //    Voted = false,
                //    DuelCard = whitecard,
                //    Password = "pass"
                //};
                var admin = await userManager.FindByNameAsync("Admin");
                Duel seedDuel = new Duel();
                seedDuel.Players = new List<Player>();
                seedDuel.Players.Add(admin);
                seedDuel.Prompt = blackcard;
                seedDuel.Players.Add(seedPlayer);
                seedDuel.Player1ID = admin.Id;
                seedDuel.Player2ID = seedPlayer.Id;

                Tournament FirstTournament = new Tournament();
                FirstTournament.Duels = new List<Duel>();
                FirstTournament.Duels.Add(seedDuel);
                FirstTournament.ExpiryTime = DateTime.Now.AddDays(7);




                context.Duels.Add(seedDuel);
                context.Tournaments.Add(FirstTournament);

                Card whitecard3 = new Card();
                whitecard3.Text = "Projectile Fart";
                whitecard3.IsPrompt = false;
                whitecard3.CreatorID = "seeded";
                context.Cards.Add(whitecard3);

                Card whitecard4 = new Card();
                whitecard4.Text = "Free Coronavirus";
                whitecard4.IsPrompt = false;
                whitecard4.CreatorID = "seeded";
                context.Cards.Add(whitecard4);

                Card whitecard5 = new Card();
                whitecard5.Text = "A bag of magic beans";
                whitecard5.IsPrompt = false;
                whitecard5.CreatorID = "seeded";
                context.Cards.Add(whitecard5);

                context.SaveChanges(); // save all the data
            }
        }
    }
}
