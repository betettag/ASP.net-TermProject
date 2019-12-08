using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TermProject.Models;

namespace TermProject.Repositories
{
    public interface IRepository
    {
        List<Card> WhiteCards { get; }
        List<Card> Prompts { get; }
        List<Card> Cards { get; }
        List<Player> Players { get; }
        List<Tournament> Tournaments { get; }
        void AddPlayer(Duel duel, Player player);
        void AddWhiteCard(Card whiteCard);
    }
}
