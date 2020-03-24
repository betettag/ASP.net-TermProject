using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TermProject.Models;

namespace TermProject.Repositories
{
    public class FakeRepository : IRepository
    {
        private static List<Card> cards = new List<Card>();
        private static List<Player> players = new List<Player>();
        private static List<Tournament> tournaments = new List<Tournament>();

        public FakeRepository()
        {
            tournaments.Add(new Tournament()
            {
                ExpiryTime = DateTime.Now.AddDays(7)
            });
            tournaments[0].Duels = new List<Duel>();
            
        }
        public List<Card> WhiteCards => cards.Where(c => c.IsPrompt == false).ToList();
        public List<Card> Cards => cards;
        public List<Card> Prompts => cards.Where(c => c.IsPrompt == true).ToList();
        public List<Player> Players => players;
        public List<Tournament> Tournaments => tournaments;//getting all the data here

        public void AddWhiteCard(Card whiteCard)
        {
            if (whiteCard.IsPrompt == false && whiteCard.Text != null && whiteCard.Text != "")
            {
                Cards.Add(whiteCard);//if not black or empty card then add
            }
        }
        public Task AddPlayerToDuel(int black_card, int white_card, string playerId)
        {
            ResetTournament();//reset if needed
            var player = players.FirstOrDefault(p => p.Id ==playerId);
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
            else
            {
                duel.Players.Add(player);//if it has found a spot ad another player
                duel.Player2ID = player.Id;
                player.IsDueling = true;
            }
            return Task.CompletedTask;
            //Player newPlayer = player;//store previous values if player is new
            //player = Players.Find(p => (p.UserName == player.UserName) && (p.Password == player.Password));//find user
            //if (player == null)
            //{
            //    player = newPlayer;
            //    player.Score = 1;
            //    if (Players.LastOrDefault() == null)
            //        player.PlayerID = 1;
            //    else
            //    {
            //        player.PlayerID = Players.LastOrDefault().PlayerID+1;
            //    }
            //}
            //player.PromtID = newPlayer.PromtID;
            //player.IsDueling = true;
            //player.DuelCard = Cards.Find(c => c.CardID == newPlayer.CardID);

            //int duelid = tournaments[0].Duels.FindIndex(d => d.Players.Count() != 2);//getting duel that needs a player or null
            //if (duelid >= 0)//if duel not empty add player to list
            //{
            //    Players.Add(player);
            //    tournaments[0].Duels[duelid].Players.Add(player);
            //}
            //else
            //{
            //    Duel newDuel = new Duel();
            //    newDuel.Prompt = Cards.Find(c => c.CardID == player.PromtID);
            //    newDuel.CardID = player.PromtID;
            //    newDuel.Players = new List<Player>();
            //    if(Tournaments.LastOrDefault().Duels.LastOrDefault() != null)
            //        newDuel.DuelID = Tournaments.LastOrDefault().Duels.LastOrDefault().DuelID+1;
            //    if (newDuel.DuelID <= 0)
            //        newDuel.DuelID = 1;
            //    newPlayer = player;
            //    newDuel.Players.Add(newPlayer);//new duel add new player
            //    Players.Add(newPlayer);
            //    //context.Players.Update(player); not needed
            //    Tournaments.First().Duels.Add(newDuel);
            //}
            //return player;
        }

        public Task UpdateDuelVotesAndScore(string playerId)
        {
            var player = Players.FirstOrDefault(p => p.Id == playerId);
            ++player.Score;
            return Task.CompletedTask;

        }
        public void ResetTournament()
        {
            if (Tournaments[0].ExpiryTime < DateTime.Now)
            {

                Tournaments[0].Duels.Clear();

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

                Tournaments[0].Duels.Add(guestDuel);
                Tournaments[0] = FirstTournament;
            }
            //do nothing :< because not expired
        }
    }
}
