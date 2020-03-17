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

        public Player AddPlayerToDuel(Player player)
        {
            ResetTournament();//reset if needed
            Player newPlayer = player;//store previous values if player is new
            player = Players.Find(p => (p.UserName == player.UserName) && (p.Password == player.Password));//find user
            if (player == null)
            {
                player = newPlayer;
                player.Score = 1;
                if (Players.LastOrDefault() == null)
                    player.PlayerID = 1;
                else
                {
                    player.PlayerID = Players.LastOrDefault().PlayerID+1;
                }
            }
            player.PromtID = newPlayer.PromtID;
            player.IsDueling = true;
            player.DuelCard = Cards.Find(c => c.CardID == newPlayer.CardID);

            int duelid = tournaments[0].Duels.FindIndex(d => d.Players.Count() != 2);//getting duel that needs a player or null
            if (duelid >= 0)//if duel not empty add player to list
            {
                Players.Add(player);
                tournaments[0].Duels[duelid].Players.Add(player);
            }
            else
            {
                Duel newDuel = new Duel();
                newDuel.Prompt = Cards.Find(c => c.CardID == player.PromtID);
                newDuel.CardID = player.PromtID;
                newDuel.Players = new List<Player>();
                if(Tournaments.LastOrDefault().Duels.LastOrDefault() != null)
                    newDuel.DuelID = Tournaments.LastOrDefault().Duels.LastOrDefault().DuelID+1;
                if (newDuel.DuelID <= 0)
                    newDuel.DuelID = 1;
                newPlayer = player;
                newDuel.Players.Add(newPlayer);//new duel add new player
                Players.Add(newPlayer);
                //context.Players.Update(player); not needed
                Tournaments.First().Duels.Add(newDuel);
            }
            return player;
        }

        public Player UpdateDuelVotesAndScore(Duel duel)
        {
            bool flag;
            int Votes;
            int PlayerID;
            if (duel.VotesP1 == 0)
            {//modifying only one field and capturing variables
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
            Player player2 = Players.Find(p => p.PlayerID == Tournaments[0].Duels.Find(d => d.DuelID == duel.DuelID).Players.Find(p2 => p2.PlayerID == PlayerID).PlayerID);
            if (player.Equals(player2))
                return player; //cant vote for yourself try again
            ++player2.Score;
            ResetTournament();//reset if needed
            player.Voted = true;


            duel = Tournaments[0].Duels.Find(d => d.DuelID == duel.DuelID);
            if (flag)
            {
                duel.VotesP2 = Votes;
            }
            else
            {
                duel.VotesP1 = Votes;
            }
            return player;
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
