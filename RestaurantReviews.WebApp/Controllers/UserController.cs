﻿using Microsoft.AspNetCore.Mvc;
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
            TempData.Keep("IsAdmin");
            TempData.Keep("CurrentUserId");
            Customer customer = _repo.GetCustomerById(userId);
            return View(customer);
        }
        public IActionResult Details(Customer customer)
        {
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
            Customer customer = new Customer(viewModel.Name, viewModel.Pass, viewModel.Phone, viewModel.Email, null);
            _repo.AddCustomer(customer);
            Customer newCustomer = _repo.GetCustomer(customer.Name);
            TempData["CurrentUserId"] = newCustomer.Id;
            TempData.Keep("IsAdmin");
            TempData.Keep("CurrentUserId");
            return RedirectToAction("Profile");
        }
        public IActionResult Logout()
        {
            if(TempData["IsAdmin"] != null)
            {
                TempData.Remove("IsAdmin");
            }
            TempData.Remove("CurrentUserId");
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Search(string searchString)
        {
            
            if(TempData["IsAdmin"] is null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Customer> list;
            if (String.IsNullOrEmpty(searchString))
            {
                list = _repo.SearchCustomers("");
            }
            else
            {
                list = _repo.SearchCustomers(searchString);
            }
            TempData.Keep("IsAdmin");
            return View(list);
        }
        [HttpGet]
        public IActionResult deleteUser(Customer deleteCustomer)
        {
            Customer admin = _repo.GetCustomerById((int)TempData["CurrentUserId"]);
            if ((int)TempData["CurrentUserId"] == deleteCustomer.Id)
            {
                return RedirectToAction("Error", "Error", "Cannot delete an account from itself.");
            }
            if(admin.Admin is null)
            {
                TempData.Keep("IsAdmin");
                TempData.Keep("CurrentUserId");
                return RedirectToAction("Error", "Error", "Must be an Admin to take this action!");
            }
            TempData["ToDeleteId"] = deleteCustomer.Id;
            TempData.Keep("IsAdmin");
            TempData.Keep("CurrentUserId");
            TempData.Keep("ToDeleteId");
            return View(deleteCustomer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult deleteUser(string toDeleteId, string password)
        {
            int id;
            if(!int.TryParse(toDeleteId, out id))
            {
                return RedirectToAction("Error", "Error", "Must be an Admin to take this action!");
            }
            if(password == _repo.GetCustomerById((int)TempData["CurrentUserId"]).Pass && id == (int)TempData["ToDeleteId"])
            {
                
                _repo.DeleteUser(_repo.GetCustomerById(id));
                TempData.Keep("IsAdmin");
                TempData.Keep("CurrentUserId");
                TempData.Remove("ToDeleteId");
            }
            else
            {
                return RedirectToAction("Error", "Error", "Must be an Admin to take this action!");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
