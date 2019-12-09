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
            tournaments[0].Duels = new List<Duel>();
            tournaments[0].Duels.ForEach(d => d.Players = new List<Player>());
        }
        public List<Card> WhiteCards => cards.Where(c => c.IsPrompt == false).ToList();
        public List<Card> Cards => cards.ToList();
        public List<Card> Prompts => cards.Where(c => c.IsPrompt == true).ToList();
        public List<Player> Players => players.ToList();
        public List<Tournament> Tournaments => tournaments;//getting all the data here
        public void AddPlayer(Duel duel, Player player)
        {
            Players.Add(player);
        }

        public Player AddPlayerToDuel(Player player)
        {
            return player;
        }
        public void AddWhiteCard(Card whiteCard)
        {
            if (whiteCard.IsPrompt == false)
            {
                Cards.Add(whiteCard);//if not black card then add
            }
        }
        public void AddPrompt(Card prompt)
        {
            if (prompt.IsPrompt == true)
            {
                Cards.Add(prompt);
            }
        }
        public Player UpdateDuelVotesAndScore(Duel duel)
        {
            return new Player();
        }
        public void ResetTournament()
        {
            //do nothing :< because not expired
        }
    }
}
