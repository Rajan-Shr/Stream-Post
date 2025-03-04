using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreamPost.DataAccessLayer;
using StreamPost.Models;
using StreamPost.ViewModels;

namespace StreamPost.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly StreamPostDataAccess _dataAccess;
        public CategoryController(StreamPostDataAccess dataAccess) {
            _dataAccess = dataAccess;
        }
        public IActionResult Category(int id)
        {
            var Categories = _dataAccess.Categories.ToList();
            var Posts = _dataAccess.Posts
                        .Include(p => p.User)
                        .Where(p => p.CategoryID == id)
                        .Take(6)
                        .ToList();
            var CategoryName = Categories.FirstOrDefault(c => c.CategoryID == id) ?.CategoryName;
            var categoryModel = new CategoryViewModel
            {
                categories = Categories,
                posts = Posts,
                categoryName = CategoryName
            };
            return View(categoryModel);
        }

        public IActionResult CustomCategory(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return View(new SearchViewModel
                {
                    categories = _dataAccess.Categories.ToList(),
                    posts = new List<Post>() 
                });
            }
                
            var Categories = _dataAccess.Categories.ToList();
            var filteredPosts = _dataAccess.Posts
                                     .Where(p => EF.Functions.Like(p.Title, $"%{searchText}%"))
                                     .Take(5)
                                     .Include(p => p.User)
                                     .ToList();

            var model = new SearchViewModel
            {
                categories = Categories,
                posts = filteredPosts
            };
            return View(model);
        }

    }
}
