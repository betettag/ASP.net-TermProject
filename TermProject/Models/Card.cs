using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TermProject.Models
{
    public class Card
    {
        public int CardID { get; set; }
        public bool IsPrompt { get; set; }//black cards
        [Required]
        public string Text { get; set; }
        public int PlayedCount { get; set; }
        [Required]
        public int CreatorID { get; set; }
    }
}
