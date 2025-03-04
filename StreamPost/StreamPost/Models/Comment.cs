using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StreamPost.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentID { get; set; }
        [Required]
        public string CommentText { get; set; }
        public string Id { get; set; }
        public int PostID { get; set; }

        [ForeignKey("Id")]              
        public User User { get; set; }

        [ForeignKey("PostID")]
        public Post Post { get; set; }
    }
}
