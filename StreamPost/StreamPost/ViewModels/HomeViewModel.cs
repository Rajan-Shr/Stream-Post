using StreamPost.Models;

namespace StreamPost.ViewModels
{
    public class HomeViewModel
    {
        public User user { get; set; }
        public List<Category> categories { get; set; }
        public Post latestpost { get; set; }
        public List<Post> posts { get; set; }
        public List<Post> techPosts { get; set; }
        public List<Post> entertainmentPosts { get; set; }
        public List<Post> sportsPosts { get; set; }
        public List<Post> socialPosts { get; set; }
        public List<Post> healthPosts { get; set; }
        public List<Post> politicsPosts { get; set; }
        public List<Post> mostLikesPosts { get; set; }
    }
}
