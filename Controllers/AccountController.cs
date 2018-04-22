using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EmpApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace EmpApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager =signInManager;
        }
        //##########################    Goto Login Page
        [AllowAnonymous]
        public ViewResult Login(string returnUrl)
        {
            return View(new LoginViewModel()
            {
                ReturnUrl = returnUrl
            });
        }

        //##########################    Action Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)return View(loginViewModel);

            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    user, loginViewModel.Password, false, false);
                if (result.Succeeded){
                    if (string.IsNullOrEmpty(loginViewModel.ReturnUrl))
                        return RedirectToAction("Index","Home");
                    return RedirectToAction(loginViewModel.ReturnUrl);
                }
            }
            ModelState.AddModelError("","Invalid UserName/Password");
            return View(loginViewModel);
        }

        //##########################    Goto Register
        public ActionResult Register()
        {
            return View();
        }

        //##########################    Action Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(LoginViewModel loginViewModel)
        {

            if (!ModelState.IsValid)
                return View(loginViewModel);
            var user =new IdentityUser() { UserName = loginViewModel.UserName };
            var result= await _userManager.CreateAsync(user,loginViewModel.Password);
           
                if (result.Succeeded)
                {
                    return RedirectToAction(loginViewModel.ReturnUrl);
                }
            return View(loginViewModel);
        }

        //##########################    Action Logout
        public async Task<RedirectResult> Logout(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
    }
}