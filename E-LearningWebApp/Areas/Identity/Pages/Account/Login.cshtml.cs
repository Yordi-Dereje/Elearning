﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using E_LearningWebApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RestSharp;
using System.Web;
using Microsoft.Net.Http.Headers;
using E_LearningWebApp.Models;

namespace E_LearningWebApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<E_LearningWebAppUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<E_LearningWebAppUser> _userManager;
        public LoginModel(SignInManager<E_LearningWebAppUser> signInManager, ILogger<LoginModel> logger, UserManager<E_LearningWebAppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        
        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

      
        public string ReturnUrl { get; set; }

      
        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

           
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/User/Index");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                
                if (result.Succeeded)
                {
                   
                    _logger.LogInformation("User logged in.");
                    var resp = new HttpResponseMessage();
                   
                    var user = await _userManager.FindByNameAsync(Input.Email);
                    var userrole = await _userManager.GetRolesAsync(user);
                  
                    string userId = user.Id.ToString();
                    if (userrole.Contains("User"))
                    {
                        return LocalRedirect($"~/User/Index?id={userId}");
                    }
                    else
                    {
                        
                        return LocalRedirect($"~/Admin/Index?id={userId}");
                    }
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}