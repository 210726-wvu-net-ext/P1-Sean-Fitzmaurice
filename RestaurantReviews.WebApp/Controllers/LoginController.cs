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
        public IActionResult Login()
        {
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
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //password does not match username
                    return View();
                }
            }
            else
            {
                // customer not found
                return View();
            }
        }

    }
}
