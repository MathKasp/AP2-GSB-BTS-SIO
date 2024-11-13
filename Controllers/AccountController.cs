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
        private readonly UserManager<Medecin> _userManager;

        public AccountController(SignInManager<Medecin> signInManager, UserManager<Medecin> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        #region LOGIN
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
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Tentative erronée");
            }
            return View(model);
        }
        #endregion

        #region LOGOUT
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region REGISTER
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            DateTime Min = DateTime.Now.AddYears(-18);
            if (ModelState.IsValid)
            {
                if (model.Date >= Min)
                {
                    ModelState.AddModelError("", "Le médecin doit être majeur (18 ans)");
                }
                else
                {
                    var medecin = new Medecin
                    {
                        UserName = model.UserName,
                        Date_naissance_m = model.Date,
                        Nom_m = model.Nom_m,
                        Prenom_m = model.Prenom_m,
                        Identifiant_m = model.Identifiant_m,
                    };

                    var result = await _userManager.CreateAsync(medecin, model.Password);

                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(medecin, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        #endregion
    }
}
