using StreamPost.Models;

namespace StreamPost.ViewModels
{
    public class UserProfileViewModel
    {
        public List<Category> categories { get; set; }
        public User user { get; set; }
        public List<Post> userPosts { get; set; }
    }
}
