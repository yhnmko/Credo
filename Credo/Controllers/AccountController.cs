using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SLE_System.Models;

namespace Credo.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private IUserService _userService;

        public AccountController(UserManager<IdentityUser> userManager,
                              SignInManager<IdentityUser> signInManager, 
                              IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
        }

        #region UI
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "Home");
                }

                Log.Error("Invalid Login attempt");
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);

                if (result.Succeeded)
                {
                    var token = _userService.Authenticate(user);
                    HttpContext.Response.Cookies.Append("access_token", token, new CookieOptions { HttpOnly = true, Secure = true });
                    return View(user);
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(user);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
        #endregion

        #region REST
        //REST Services
        [HttpGet]
        [Route("[action]")]
        [Route("api/User/LoginUser")]
        public async Task<IActionResult> LoginUser([FromBody] LoginViewModel user)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);

                    if (result.Succeeded)
                    {
                        var token = _userService.Authenticate(user);
                        return Ok(token);
                    }
                }
                return Unauthorized();
            }
            catch
            {
                Log.Error("Custom Error msg");
                return BadRequest();

            }
        }

        [HttpGet]
        [Route("[action]")]
        [Route("api/User/Authenticate")]
        public IActionResult Authenticate([FromBody] LoginViewModel user)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var token = _userService.Authenticate(user);
                    return Ok(token);

                }
                return Unauthorized();
            }
            catch
            {
                Log.Error("Custom Error msg");
                return BadRequest();

            }
        }


        #endregion
    }
}
