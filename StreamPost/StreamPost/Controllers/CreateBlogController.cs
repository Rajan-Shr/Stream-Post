using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreamPost.DataAccessLayer;
using StreamPost.Models;
using StreamPost.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace StreamPost.Controllers
{
    [Authorize]
    public class CreateBlogController : Controller
    {
        private readonly StreamPostDataAccess _dataAccess;
        private readonly SignInManager<User> _signInManager;
        public CreateBlogController(StreamPostDataAccess dataAccess, SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
            _dataAccess = dataAccess;
        }
        public List<Category> Categories { get; set; }
        public IActionResult CreateBlog()
        {
            Categories = _dataAccess.Categories.ToList();

            var model = new HomeViewModel
            {
                user = _signInManager.UserManager.GetUserAsync(User).Result,
                categories = Categories
            };

            if(_signInManager.IsSignedIn(User))
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBlog(BlogPostViewModel model)
        {
            var blog = new Post
            { 
               Title = model.Title,
               FeaturedImage = string.IsNullOrEmpty(model.FeaturedImage) ? null : model.FeaturedImage, 
               Description = model.Content,
               PublishedDate = DateTime.Now.Date.ToString("yyyy-MM-dd"),
               LikeNumber = 0,
               CommentNumber = 0,
               Id = _signInManager.UserManager.GetUserId(User),
               CategoryID = model.Category
            };
            if (ModelState.IsValid)
            {
                _dataAccess.Posts.Add(blog);
                await _dataAccess.SaveChangesAsync();
                return RedirectToAction("Index" , "Home");
            }

            return View(model);
        }

    }
}
