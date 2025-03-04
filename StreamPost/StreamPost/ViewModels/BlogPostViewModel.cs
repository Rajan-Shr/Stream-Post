using StreamPost.Models;

namespace StreamPost.ViewModels
{
    public class BlogPostViewModel
    {
        public string Title { get; set; }
        public string? FeaturedImage { get; set; }
        public string Content { get; set; }
        public int Category { get; set; }
    }
}
