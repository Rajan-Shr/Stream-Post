using StreamPost.Models;

namespace StreamPost.ViewModels
{
    public class CategoryViewModel
    {
        public List<Category> categories { get; set; }
        public List<Post> posts { get; set; }
        public string categoryName { get; set; }
    }
}
