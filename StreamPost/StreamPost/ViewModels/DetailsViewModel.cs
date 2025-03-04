using StreamPost.Models;

namespace StreamPost.ViewModels
{
    public class DetailsViewModel
    {
        public List<Category> categories { get; set; }
        public Post post { get; set; }
        public List<Post>? relatedPosts{ get; set; }
        public List<Comment> comments { get; set; }
        public bool hasLiked { get; set; }
        public string userid { get; set; }
    }
}
