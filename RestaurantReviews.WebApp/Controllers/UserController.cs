using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantReviews.Domain;
using RestaurantReviews.WebApp.Models;

namespace RestaurantReviews.WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IRepository _repo;
        public UserController(IRepository repo)
        {
            _repo = repo;

        }
        public IActionResult Profile()
        {
            int userId = (int)TempData["CurrentUserId"];
            TempData.Keep("CurrentUserId");
            Customer customer = _repo.GetCustomerById(userId);
            return View(customer);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(CreatedCustomer viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            if(_repo.GetCustomer(viewModel.Name).Name != null)
            {
                ModelState.AddModelError(key:"Name", errorMessage: "Username is already taken");
                return View(viewModel);
            }
            Customer customer = new Customer(viewModel.Name, viewModel.Pass, viewModel.Email, viewModel.Pass, null);
            _repo.AddCustomer(customer);
            Customer newCustomer = _repo.GetCustomer(customer.Name);
            TempData["CurrentUserId"] = newCustomer.Id;
            TempData.Keep("CurrentUserId");
            return RedirectToAction("Profile");
        }
        public IActionResult Logout()
        {
            TempData.Remove("CurrentUserId");
            return RedirectToAction("Index", "Home");
        }
    }
}
