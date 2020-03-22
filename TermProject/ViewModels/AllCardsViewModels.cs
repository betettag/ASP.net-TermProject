using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TermProject.Models;

namespace TermProject.ViewModels
{
    public class AllCardsViewModels
    {
        //public Player player { get; set; }
        [Required(ErrorMessage = "Selecting a card is required")]
        public int? white_card { get; set; }
        public int? black_card { get; set; }
        public List<Card> Cards { get; set; }
    }
}
