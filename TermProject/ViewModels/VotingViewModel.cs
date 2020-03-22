using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TermProject.Models;

namespace TermProject.ViewModels
{
    public class VotingViewModel
    {
        public List<Duel> Duels { get; set; }
        //public Duel duel { get; set; }
        [Required(ErrorMessage = "selecting a card is required")]
        public string VotesPlayerId { get; set; }
        [Required]
        public string PlayerId1 { get; set; }
        [Required]
        public string PlayerId2 { get; set; }
    }
}
