using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using NotaEstudiantes.Models;

namespace NotaEstudiantes.Controllers
{
    public class AccessController : Controller
    {
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if(claimUser.Identity.IsAuthenticated)
                RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]

        public async Task <IActionResult> Login(VMLogin modelLogin)
        {

            if (modelLogin.Email == "marisol@gmail.com" &&
                modelLogin.Password == "123"
                )
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, modelLogin.Email),
                    new Claim("OtherProperties", "Example Role")
                };
                ClaimsIdentity clamsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties() {
                
                    AllowRefresh = true,
                    IsPersistent = modelLogin.KeepLoggedIn
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme ,
                    new ClaimsPrincipal(clamsIdentity), properties);

                return RedirectToAction ("Index", "Home");
            }

            ViewData["ValidateMessage"] = "User not found";
            return View();
        }
    }
}
