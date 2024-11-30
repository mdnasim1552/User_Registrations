using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.Data;
using WebApplication1.IRepositories;
using WebApplication1.UserDefineModels;
using WebApplication1.Views.Account;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register() => View();
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _userRepository.IsEmailRegistered(model.Email))
                {
                    ModelState.AddModelError("", "Email is already registered.");
                    return View(model);
                }

                bool isRegistered =await _userRepository.RegisterUser(model.Username, model.Email, model.Password);
                if (isRegistered)
                    return RedirectToAction("Login");
            }
            return View(model);
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login() => View();
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                bool isValid = await _userRepository.ValidateUser(model.Email, model.Password);
                if (isValid)
                {
                    // Set authentication cookie or session
                    Session["Email"] = model.Email;
                    var ticket = new FormsAuthenticationTicket(
                        1,
                        model.Email,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(30),
                        model.RememberMe,
                        model.Email,
                        FormsAuthentication.FormsCookiePath
                    );
                    var encryptedTicket = FormsAuthentication.Encrypt(ticket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
                    {
                        Expires = ticket.Expiration
                    };
                    if (!model.RememberMe)
                    {
                        authCookie.Expires = DateTime.MinValue;
                    }
                    Response.Cookies.Add(authCookie);
                    TempData.Remove("Message");
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid email or password.");
            }
            return View(model);
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
        //[AllowAnonymous]
        //public ActionResult ForgotPassword()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userRepository.GetUserByEmailAsync(model.Email);
        //        if (user != null)
        //        {
        //            // Generate a password reset token (in a real scenario, use a GUID or secure method)
        //            string resetToken = Guid.NewGuid().ToString();

        //            // Save the token and expiration in the database
        //            await _userRepository.SavePasswordResetTokenAsync(user.Id, resetToken);

        //            // Create a reset link
        //            string resetLink = Url.Action("ResetPassword", "Account", new { token = resetToken }, Request.Url.Scheme);

        //            // Send the email with the reset link (using a mailing service)
        //            await _emailService.SendEmailAsync(model.Email, "Reset Password",
        //                $"Please reset your password by clicking <a href='{resetLink}'>here</a>.");

        //            TempData["Message"] = "Reset link has been sent to your email.";
        //        }
        //        else
        //        {
        //            TempData["Message"] = "If this email is registered, a reset link has been sent.";
        //        }

        //        return RedirectToAction("ForgotPasswordConfirmation");
        //    }

        //    return View(model);
        //}

        //[AllowAnonymous]
        //public ActionResult ResetPassword(string token)
        //{
        //    if (string.IsNullOrEmpty(token))
        //    {
        //        return RedirectToAction("Error", "Home");
        //    }

        //    return View(new ResetPasswordViewModel { Token = token });
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userRepository.GetUserByPasswordResetTokenAsync(model.Token);
        //        if (user != null)
        //        {
        //            // Update the password
        //            await _userRepository.UpdatePasswordAsync(user.Id, model.NewPassword);

        //            // Clear the reset token
        //            await _userRepository.ClearPasswordResetTokenAsync(user.Id);

        //            TempData["Message"] = "Password has been reset successfully.";
        //            return RedirectToAction("Login");
        //        }

        //        ModelState.AddModelError("", "Invalid token or the token has expired.");
        //    }

        //    return View(model);
        //}
    }
}