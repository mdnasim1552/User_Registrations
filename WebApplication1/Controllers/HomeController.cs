using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.IRepositories;
using WebApplication1.Views.Account;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepository;
        public HomeController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ActionResult> Index()
        {
            var userEmail = User.Identity.Name; // Assuming the username or email is used as the identifier
            var user = await _userRepository.GetUserByEmailAsync(userEmail);

            if (user == null)
            {
                return HttpNotFound();
            }

            var model = new ProfileViewModel
            {
                Email = user.Email,
                FullName = user.Username,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateProfile(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userEmail = User.Identity.Name; // Assuming the username or email is used as the identifier
                var user = await _userRepository.GetUserByEmailAsync(userEmail);

                if (user == null)
                {
                    return HttpNotFound();
                }

                user.Username = model.FullName;
                user.PhoneNumber = model.PhoneNumber;
                user.Address = model.Address;

                var result=await _userRepository.UpdateUserAsync(user);

                TempData["Message"] = "Profile updated successfully.";
                return RedirectToAction("Index");
            }

            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteProfile()
        {
            var userEmail = User.Identity.Name; // Assuming the username or email is used as the identifier
            var user = await _userRepository.GetUserByEmailAsync(userEmail);

            if (user == null)
            {
                return HttpNotFound();
            }

            var result=await _userRepository.DeleteUserAsync(user.UserId);

            // Sign out the user after deleting the account
            FormsAuthentication.SignOut();

            TempData["Message"] = "Your account has been deleted.";

            return RedirectToAction("Login", "Account");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}