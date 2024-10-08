using Microsoft.AspNetCore.Mvc;
using newEmpty.Models;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using newEmpty.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace newEmpty.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Medecin> _signInManager; // permet de gerer la connexion et la deconnexion des utilisateurs, nous est fourni par ASP.NET Core Identity


        public AccountController(SignInManager<Medecin> signInManager, UserManager<Medecin> userManager)
        {
            _signInManager = signInManager;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View(); // Affiche la vue Login
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }


        public IActionResult Register()
        {
            return View();
        }

        // [HttpPost]
        // public async Task<IActionResult> Register(RegisterViewModel model)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         var user = new Medecin { UserName = model.UserName, Email = model.Email };
        //         var result = await _userManager.CreateAsync(user, model.Password);

        //         if (result.Succeeded)
        //         {
        //             await _signInManager.SignInAsync(user, isPersistent: false);
        //             return RedirectToAction("Index", "Home");
        //         }

        //         foreach (var error in result.Errors)
        //         {
        //             ModelState.AddModelError(string.Empty, error.Description);
        //         }
        //     }

        //     return View(model);
        // }
    }
}
