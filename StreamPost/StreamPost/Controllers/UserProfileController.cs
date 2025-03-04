using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StreamPost.DataAccessLayer;
using StreamPost.Models;
using StreamPost.ViewModels;

namespace StreamPost.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly StreamPostDataAccess _dataAccess;
        private readonly SignInManager<User> _signInManager;
        public UserProfileController(StreamPostDataAccess dataAccess,SignInManager<User> signInManager)
        {
            _dataAccess = dataAccess;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> UserProfile()
        {
            var categories = _dataAccess.Categories.ToList();
            var userid = _signInManager.UserManager.GetUserId(User);
            if (userid == null)
            {
                return NotFound();
            }
            var user = await _signInManager.UserManager.FindByIdAsync(userid);
            if (user == null)
            {
                return NotFound();
            }
            var posts = _dataAccess.Posts.Where(p => p.Id == userid).ToList();

            if(posts.Count == 0)
            {
                posts = new List<Post>();
            }
            var model = new UserProfileViewModel
            {
                categories = categories,
                user = user,
                userPosts = posts
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUserProfile(EditUserProfileViewModel model)
        {
            var userId = _signInManager.UserManager.GetUserId(User);

            if (userId == null)
            {
                return NotFound();
            }

            var user = await _signInManager.UserManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

             user.UserName = model.UserName;
             user.Email = model.Email;
             user.PhoneNumber = model.PhoneNumber;
             user.DateOfBirth = model.DateOfBirth;
            

            var result = await _signInManager.UserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("UserProfile");
            }

            ModelState.AddModelError("", "Failed to update profile");
            return View("UserProfile", model);
        } 
        [HttpPost]
        public async Task<IActionResult> UpdateUserPassword(EditUserProfileViewModel model)
        {
            var userId = _signInManager.UserManager.GetUserId(User);

            if (userId == null)
            {
                return NotFound();
            }

            var user = await _signInManager.UserManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

        
            var result = await _signInManager.UserManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("UserProfile");
            }

            return RedirectToAction("UserProfile");
        }

    }
}
