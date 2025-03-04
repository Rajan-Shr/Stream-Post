using StreamPost.Models;

namespace StreamPost.ViewModels
{
    public class SearchViewModel
    {
        public List<Post> posts { get; set; }
        public List<Category> categories { get; set; }
    }
}
