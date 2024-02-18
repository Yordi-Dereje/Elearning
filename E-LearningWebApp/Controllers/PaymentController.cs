using ChapaNET;
using E_LearningWebApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using E_LearningWebApp.Models;

using System;

namespace E_LearningWebApp.Controllers
{
    public class PaymentController : Controller
    {
        private readonly UserManager<E_LearningWebAppUser> _userManager;
        public PaymentController(UserManager<E_LearningWebAppUser> _userManager)
        {
            this._userManager = _userManager;
        }
        public class UserResponse
        {
            public string CheckoutUrl { get; set; }
            public E_LearningWebAppUser user { get; set; }
        }
        [HttpGet]
        public async Task<IActionResult> UserPayment(string userid)
        {


            //Initialize your Chapa Instance
            Chapa chapa = new("CHAPUBK_TEST-nZgZI5lxfdJJu1hriKT3UdnKaRz4LqMA");

            //Get a unique transaction ID
            var ID = Chapa.GetUniqueRef();


            //Get Banks
            Console.WriteLine("-----Fetching Banks------");

            var banks = await chapa.GetBanksAsync();
            Console.WriteLine(string.Join(',', banks.AsEnumerable()));

            //Make a request

            Console.WriteLine("-----Making A Request------");
            var user = await _userManager.FindByIdAsync(userid);
            var Request = new ChapaRequest(
                amount: 1,
                email: user.Email,
                firstName: user.FirstName,
                lastName: user.LastName,
                tx_ref: ID,

                callback_url: "User/Index"
                );


            //Process the request and get a response asynchronously
            var Result = await chapa.RequestAsync(Request);

            //Print out the checkout link
            Console.WriteLine("Checkout Url:" + Result.CheckoutUrl);

            /*var landlord = await _landLordRepo.GetLandLordByIdAsync(id);*/
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                var response = new UserResponse
                {
                    CheckoutUrl = Result.CheckoutUrl,
                    user = user
                };
                return Ok(response);


            }
        }


    }
}

