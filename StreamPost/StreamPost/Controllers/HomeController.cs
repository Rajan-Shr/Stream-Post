using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using StreamPost.DataAccessLayer;
using StreamPost.Models;
using StreamPost.ViewModels;

namespace StreamPost.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly StreamPostDataAccess _dataAccess;

        public HomeController(ILogger<HomeController> logger, SignInManager<User> signInManager, StreamPostDataAccess dataAccess)
        {
            _logger = logger;
            _signInManager = signInManager;
            _dataAccess = dataAccess;
        }

        public List<Category> Categories { get; set; }
        public Post LatestPost { get; set; }
        public List<Post> Posts { get; set; }
        public IActionResult Index()
        {
            var Categories = _dataAccess.Categories.ToList();

            // Get the latest post with a featured image
            var LatestPost = _dataAccess.Posts
                .OrderByDescending(p => p.PostID)
                .Include(p => p.User)
                .Include(p => p.Category)
                .FirstOrDefault(p => !string.IsNullOrEmpty(p.FeaturedImage))
                ?? _dataAccess.Posts
                    .Where(p => !string.IsNullOrEmpty(p.FeaturedImage))
                    .OrderByDescending(p => p.PublishedDate)
                    .Include(p => p.User)
                    .Include(p => p.Category)
                    .FirstOrDefault();

            var Posts = _dataAccess.Posts
                    .OrderByDescending(p => p.PublishedDate)
                    .Include(p => p.User)
                    .Take(5)
                    .ToList();

            var TechPosts = _dataAccess.Posts
                            .Where(p => p.CategoryID == 1 && !string.IsNullOrEmpty(p.FeaturedImage))
                            .OrderByDescending(p => p.PublishedDate)
                            .Include(p => p.User)
                            .Take(4)
                            .ToList();

            var EntertainmentPosts = _dataAccess.Posts
                            .Where(p => p.CategoryID == 2 && !string.IsNullOrEmpty(p.FeaturedImage))
                            .OrderByDescending(p => p.PublishedDate)
                            .Include(p => p.User)
                            .Take(1)
                            .ToList();

            var SportsPosts = _dataAccess.Posts
                            .Where(p => p.CategoryID == 3 && !string.IsNullOrEmpty(p.FeaturedImage)) 
                            .OrderByDescending(p => p.PublishedDate)
                            .Include(p => p.User)
                            .Take(1)
                            .ToList();

            var PoliticsPosts = _dataAccess.Posts
                            .Where(p => p.CategoryID == 4 && !string.IsNullOrEmpty(p.FeaturedImage)) 
                            .OrderByDescending(p => p.PublishedDate)
                            .Include(p => p.User)
                            .Take(3)
                            .ToList();

            var SocialPosts = _dataAccess.Posts
                            .Where(p => p.CategoryID == 5 && !string.IsNullOrEmpty(p.FeaturedImage))
                            .OrderByDescending(p => p.PublishedDate)
                            .Include(p => p.User)
                            .Take(1)
                            .ToList();

            var HealthPosts = _dataAccess.Posts
                            .Where(p => p.CategoryID == 6 && !string.IsNullOrEmpty(p.FeaturedImage)) 
                            .OrderByDescending(p => p.PublishedDate)
                            .Include(p => p.User)
                            .Take(1)
                            .ToList();

            var MostLikesPost = _dataAccess.Posts
                                .Where(p => !string.IsNullOrEmpty(p.FeaturedImage))
                                .OrderByDescending(p => p.LikeNumber)
                                .Take(2)
                                .ToList();

            var user = _signInManager.UserManager.GetUserAsync(User).Result;

            var homeModel = new HomeViewModel
            {
                user = user,
                categories = Categories,
                latestpost = LatestPost,
                posts = Posts,
                techPosts = TechPosts,
                entertainmentPosts = EntertainmentPosts,
                sportsPosts = SportsPosts,
                politicsPosts = PoliticsPosts,
                socialPosts = SocialPosts,
                healthPosts = HealthPosts,
                mostLikesPosts = MostLikesPost
            };

            if (_signInManager.IsSignedIn(User) && user != null)
            {
                return View(homeModel);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public IActionResult Details(int id)
        {
            var Categories = _dataAccess.Categories.ToList();
            var Post = _dataAccess.Posts
            .Include(p => p.User)
            .FirstOrDefault(p => p.PostID == id);
            if (Post == null)
            {
                return NotFound();
            }
            var RelatedPosts = _dataAccess.Posts
                                .Include(p => p.User)
                                .Where(p => p.CategoryID == Post.CategoryID && p.PostID != id)
                                .OrderByDescending(p => p.LikeNumber)
                                .Take(3)
                                .ToList();
            var Comments = _dataAccess.Comments
                                      .Include(p => p.User)
                                      .Where(p => p.PostID == id)
                                      .ToList();

            var userid = _signInManager.UserManager.GetUserId(User);
            var hasLiked = _dataAccess.Likes.Any(l => l.PostID == id && l.Id == userid);

            var detailsModel = new DetailsViewModel
            {
                categories = Categories,
                post = Post,
                relatedPosts = RelatedPosts,
                comments = Comments,
                hasLiked = hasLiked,
                userid = userid
            };
            return View(detailsModel);
        }

        [HttpPost]
        public async Task<IActionResult> Comment(int PostID,string CommentText)
        {
            var post = _dataAccess.Posts.Find(PostID);
            var user = _signInManager.UserManager.GetUserAsync(User).Result;
            var commentModel = new Comment
            {
                Id = _signInManager.UserManager.GetUserId(User),
                PostID = PostID,
                CommentText = CommentText
            };

            _dataAccess.Comments.Add(commentModel);
            post.CommentNumber += 1;
            _dataAccess.Posts.Update(post);
            await _dataAccess.SaveChangesAsync();
            return new JsonResult(new
            {
                success = true,
                message = "Commented!",
                commentCount = post.CommentNumber,
                username = user.UserName,
                commentText = commentModel.CommentText
            });
        }

        [HttpPost]
        public async Task<IActionResult> Like(int postid)
        {
            var post = _dataAccess.Posts.Find(postid);
            if (post == null)
            {
                return Json(new { success = false, message = "Post not found" });
            }
            var userid = _signInManager.UserManager.GetUserId(User);
            var hasLiked = _dataAccess.Likes.FirstOrDefault(l => l.PostID == postid && l.Id == userid);

            if(hasLiked != null)
            {
                _dataAccess.Likes.Remove(hasLiked);
                post.LikeNumber -= 1;
                _dataAccess.Posts.Update(post);
                await _dataAccess.SaveChangesAsync();

                return new JsonResult(new
                {
                    success = true,
                    message = "DisLiked",
                    isLiked = hasLiked,
                    likeCount = post.LikeNumber
                });
            }
            else
            {
                var likeModel = new Like
                {
                    Id = userid,
                    PostID = postid,
                };

                _dataAccess.Likes.Add(likeModel);
                post.LikeNumber += 1;
                _dataAccess.Posts.Update(post);
                await _dataAccess.SaveChangesAsync();

                return new JsonResult(new
                {
                    success = true,
                    message = "Liked",
                    isLiked = hasLiked,
                    likeCount = post.LikeNumber
                });
            }

        }

        public IActionResult SearchByTitle(string searchText)
        {

            var filteredPosts = _dataAccess.Posts
                                    .Where(p => EF.Functions.Like(p.Title, $"%{searchText}%"))
                                    .Take(4)
                                    .ToList();

            return PartialView("_SearchResults", filteredPosts);
        }
        [HttpGet]
        public IActionResult EditPost(int id)
        {
            var Categories = _dataAccess.Categories.ToList();
            var Post = _dataAccess.Posts.FirstOrDefault(p => p.PostID == id);
            if(Post == null)
            {
                return NotFound();
            }
            var model = new EditPostViewModel
            {
                categories = Categories,
                Post = Post
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(EditPostViewModel model)
        {
            var Post = _dataAccess.Posts.FirstOrDefault(p => p.PostID == model.PostID);

            if (Post == null)
            {
                return NotFound();
            }

            Post.Title = model.Title;
            Post.Description = model.Content;
            Post.FeaturedImage = model.FeaturedImage ?? Post.FeaturedImage;
            Post.CategoryID = model.CategoryID;

            _dataAccess.Update(Post);
            await _dataAccess.SaveChangesAsync();

            return RedirectToAction("Details", "Home", new { id = Post.PostID });
        }

        public IActionResult DeletePost(int id)
        {
            var deletePost = _dataAccess.Posts.FirstOrDefault(p => p.PostID == id);
            if(deletePost == null)
            {
                return NotFound();
            }
            _dataAccess.Posts.Remove(deletePost);
            _dataAccess.SaveChangesAsync();
            return RedirectToAction("UserProfile","UserProfile");
        }

        public IActionResult About()
        {
            var Categories = _dataAccess.Categories.ToList();
            var aboutModel = new AboutViewModel
            {
                categories = Categories
            };
            return View(aboutModel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
