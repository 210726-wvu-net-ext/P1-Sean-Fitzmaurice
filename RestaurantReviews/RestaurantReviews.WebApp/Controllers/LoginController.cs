using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantReviews.Domain;
using Serilog;

namespace RestaurantReviews.WebApp.Controllers
{
    /// <summary>
    /// Login controller for handling login attempts by a user
    /// </summary>
    public class LoginController : Controller
    {
        private readonly IRepository _repo;
        
        /// <summary>
        /// initializes creation of repo
        /// </summary>
        /// <param name="repo"></param>
        public LoginController(IRepository repo)
        {
            _repo = repo;

        }
        /// <summary>
        /// Login Action get
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login(string message)
        {
            ViewData["message"] = message;
            return View();
        }

        /// <summary>
        /// Login action post, get user by entered customer Name and checks to see if entered password matches 
        /// if successful, uses TempData to track user ID and if user is an admin
        /// </summary>
        /// <param name="Name">entered name by user</param>
        /// <param name="pass">entered password by user</param>
        /// <returns>a view depending on what success of login</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string Name, string pass)
        {
            Customer customer = _repo.GetCustomer(Name);
            if (customer.Name != null)
            {
                if (customer.Pass == pass)
                {
                    Log.Information($"User ID {customer.Id} successfully logged in");
                    //login success
                    if (customer.Admin != null)
                    {
                        TempData["IsAdmin"] = 1;
                    }
                    
                    TempData["CurrentUserId"] = customer.Id;
                    TempData.Keep("CurrentUserId");
                    TempData.Keep("IsAdmin");
                    if(TempData["returnPath"] != null)
                    {
                        return Redirect((string)TempData["returnPath"]);
                    }

                    if (TempData["redirectController"] != null && TempData["redirectView"] != null)
                    {
                        TempData.Keep("RestaurantId");
                        return RedirectToAction((string)TempData["redirectView"], (string)TempData["redirectController"]);
                    }
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //password does not match username
                    return Login("Password does not match");
                }
            }
            else
            {
                // customer not found
                return Login("Username does not exist");
            }
        }
        /// <summary>
        /// redirects user to user controller, to create a new customer
        /// </summary>
        /// <returns>Action method for register in User controller</returns>
        public IActionResult Register()
        {
            return RedirectToAction("Register", "User");
        }

    }
}
