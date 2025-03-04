using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StreamPost.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string DateOfBirth { get; set; }
        [Required]
        public char Gender {  get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
