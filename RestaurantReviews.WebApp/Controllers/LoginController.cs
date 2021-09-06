using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantReviews.Domain;

namespace RestaurantReviews.WebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IRepository _repo;
        public LoginController(IRepository repo)
        {
            _repo = repo;

        }
        [HttpGet]
        public IActionResult Login(string message)
        {
            ViewData["message"] = message;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string Name, string pass)
        {
            Customer customer = _repo.GetCustomer(Name);
            if (customer.Name != null)
            {
                if (customer.Pass == pass)
                {
                    //login success
                    if(customer.Admin != null)
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
        public IActionResult Register()
        {
            return RedirectToAction("Register", "User");
        }

    }
}
