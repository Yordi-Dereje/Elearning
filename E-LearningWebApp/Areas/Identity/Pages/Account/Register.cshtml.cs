// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using E_LearningWebApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

namespace E_LearningWebApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<E_LearningWebAppUser> _signInManager;
        private readonly UserManager<E_LearningWebAppUser> _userManager;
        private readonly IUserStore<E_LearningWebAppUser> _userStore;
        private readonly IUserEmailStore<E_LearningWebAppUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private IWebHostEnvironment webHostEnvironment;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RegisterModel(
            UserManager<E_LearningWebAppUser> userManager,
            IUserStore<E_LearningWebAppUser> userStore,
            SignInManager<E_LearningWebAppUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IWebHostEnvironment webHostEnvironment,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            this.webHostEnvironment = webHostEnvironment;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }


        public IList<AuthenticationScheme> ExternalLogins { get; set; }


        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "FirstName")]
            public string FirstName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "LastName")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display (Name = "PhoneNumber")]
            public string PhoneNumber { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [DataType(DataType.Upload)]
            [Display(Name = "Profile Picture")]
            public IFormFile imagePath { get; set; }

            [Required]
            [Display(Name = "User Role")]
            public string UserRole { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ViewData["roles"] = _roleManager.Roles.ToList();
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }


        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            var role = _roleManager.FindByIdAsync(Input.UserRole).Result;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {

                string fileName = "";
                string uniqueCFileName = "";
                string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                if (Input.imagePath != null)
                {
                    fileName = Guid.NewGuid().ToString() + "-" + Input.imagePath.FileName;
                    uniqueCFileName = Path.Combine(uploadFolder, fileName);
                    Input.imagePath.CopyTo(new FileStream(uniqueCFileName, FileMode.Create));
                }

                var user = new E_LearningWebAppUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    PhoneNumber = Input.PhoneNumber,
                    imagePath = fileName == "" ? "/Images/default.jpg" : "/Images/" + fileName
                };
                /*  var user = CreateUser();*/

                /* await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                 await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);*/
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    await _userManager.AddToRoleAsync(user, "User");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect($"~/User/Index?id={userId}");
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            ViewData["roles"] = _roleManager.Roles.ToList();
            // If we got this far, something failed, redisplay form
            return Page();
        }

        private E_LearningWebAppUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<E_LearningWebAppUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(E_LearningWebAppUser)}'. " +
                    $"Ensure that '{nameof(E_LearningWebAppUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<E_LearningWebAppUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<E_LearningWebAppUser>)_userStore;
        }
    }
}
