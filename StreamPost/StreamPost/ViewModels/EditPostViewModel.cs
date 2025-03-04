using StreamPost.Models;

namespace StreamPost.ViewModels
{
    public class EditPostViewModel
    {
        public int PostID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string FeaturedImage { get; set; }
        public int CategoryID { get; set; }
        public List<Category> categories { get; set; }
        public Post Post { get; set; }
    }
}