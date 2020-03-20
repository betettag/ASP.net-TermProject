
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TermProject.Models
{
    public class CreateUserViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public class LoginModel
    {
        [Required] 
        [UIHint("Email")] 
        [EmailAddress] 
        public string Email { get; set; }
        [Required] 
        [UIHint("password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
    public class RoleEditModel
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<Player> Members { get; set; }
        public IEnumerable<Player> NonMembers { get; set; }
    }
    public class RoleModificationModel
    {
        [Required]
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }
}
