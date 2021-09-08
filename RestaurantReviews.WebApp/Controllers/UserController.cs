using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantReviews.Domain;
using RestaurantReviews.WebApp.Models;
using Serilog;

namespace RestaurantReviews.WebApp.Controllers
{
    /// <summary>
    /// Controller for user management
    /// </summary>
    public class UserController : Controller
    {
        private readonly IRepository _repo;

        /// <summary>
        /// initilizes the repo
        /// </summary>
        /// <param name="repo"></param>
        public UserController(IRepository repo)
        {
            _repo = repo;
            
        }

        /// <summary>
        /// action for viewing profile of logged in user
        /// </summary>
        /// <returns>view containing detila of logged in user</returns>
        public IActionResult Profile()
        {
            int userId = (int)TempData["CurrentUserId"];
            TempData["customerId"] = userId;
            TempData["returnView"] = "Profile";
            TempData.Keep("customerId");
            TempData.Keep("IsAdmin");
            TempData.Keep("CurrentUserId");
            Customer customer = _repo.GetCustomerById(userId);
            return View(customer);
        }

        /// <summary>
        /// action for viewing details for admin view
        /// </summary>
        /// <param name="customer">customer to be viewed</param>
        /// <returns>view containing details of user</returns>
        public IActionResult Details(Customer customer)
        {
            if(customer.Name is null)
            {
                if(TempData["customerId"] != null)
                {
                    customer = _repo.GetCustomerById((int)TempData["customerId"]);
                }
                else
                {
                    return RedirectToAction("Error", "Error", new { message = "Something went wrong, user Id does not exist" });
                }
            }
            TempData["customerId"] = customer.Id;
            TempData["returnView"] = "Details";
            TempData.Keep("customerId");
            return View(customer);
        }

        /// <summary>
        /// get action for registering a new user
        /// </summary>
        /// <returns>view of from for registering new user</returns>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// post action for creation of new user, checks to see if entered information is valid and that name is not a duplicate
        /// if successul automatically logs user in with use of TempData
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
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
            Customer customer = new Customer(viewModel.Name, viewModel.Pass,  viewModel.Phone, viewModel.Email, null);
            try
            {
                _repo.AddCustomer(customer);
                Log.Information($"Created User {viewModel.Name}");
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Unexpected Error When Writing to Database");
                return RedirectToAction("Error", "Error", new { message = "Unexpected Error When Writing to Database" });
            }
            Customer newCustomer = _repo.GetCustomer(customer.Name);
            TempData["CurrentUserId"] = newCustomer.Id;
            TempData.Keep("IsAdmin");
            TempData.Keep("CurrentUserId");
            return RedirectToAction("Profile");
        }
        
        /// <summary>
        /// action method for logging out, removes IsAdmin and userID from dictionary
        /// </summary>
        /// <returns>home index</returns>
        public IActionResult Logout()
        {
            if(TempData["IsAdmin"] != null)
            {
                TempData.Remove("IsAdmin");
            }
            TempData.Remove("CurrentUserId");
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// search user action checks if user is an admin
        /// </summary>
        /// <param name="searchString">string to search</param>
        /// <returns>view of users matching search criteria</returns>
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

        /// <summary>
        /// get action for deleting a user, checks if user is an admin
        /// </summary>
        /// <param name="deleteCustomer">customer object to delete</param>
        /// <returns>view displaying detials of customer being deleted</returns>
        [HttpGet]
        public IActionResult deleteUser(Customer deleteCustomer)
        {
            Customer admin = _repo.GetCustomerById((int)TempData["CurrentUserId"]);
            if ((int)TempData["CurrentUserId"] == deleteCustomer.Id)
            {
                return RedirectToAction("Error", "Error", new { message = "Cannot delete an account from itself." });
            }
            if(admin.Admin is null)
            {
                TempData.Keep("IsAdmin");
                TempData.Keep("CurrentUserId");
                return RedirectToAction("Error", "Error",new { message = "Must be an Admin to take this action!" });
            }
            TempData["ToDeleteId"] = deleteCustomer.Id;
            TempData.Keep("IsAdmin");
            TempData.Keep("CurrentUserId");
            TempData.Keep("ToDeleteId");
            return View(deleteCustomer);
        }

        /// <summary>
        /// post action for deletion of customer, checks if password and ID entered matches
        /// </summary>
        /// <param name="toDeleteId">PK Id of user to be delted</param>
        /// <param name="password">password of logged in admin</param>
        /// <returns>if successful returns to user search, if not returns error page and aborts deltion</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult deleteUser(string toDeleteId, string password)
        {
            int id;
            if(!int.TryParse(toDeleteId, out id))
            {
                return RedirectToAction("Error", "Error", new { message = "Id was entered incorrectly, aborting user delete." });
            }
            if(password == _repo.GetCustomerById((int)TempData["CurrentUserId"]).Pass && id == (int)TempData["ToDeleteId"])
            {
                try
                {
                    _repo.DeleteUser(_repo.GetCustomerById(id));
                    Log.Information($"Deleted User with Id of {id}");
                }
                catch (Exception e)
                {
                    Log.Fatal(e, "Unexpected Error When Deleting from Database");
                    return RedirectToAction("Error", "Error", new { message = "Unexpected Error When Deleting from Database" });
                }
                TempData.Keep("IsAdmin");
                TempData.Keep("CurrentUserId");
                TempData.Remove("ToDeleteId");
            }
            else
            {
                return RedirectToAction("Error", "Error", new { message = "User ID or Password was entered incorrectly, aborting user delete." });
            }
            return RedirectToAction("Search", "User");
        }
        /// <summary>
        /// show all reviews for a given user
        /// </summary>
        /// <param name="id">PK ID of user to display review of</param>
        /// <returns>view containing a lis tof reviews by the given user</returns>
        public IActionResult ShowReviews(int id)
        {
            Customer customer = _repo.GetCustomerById(id);
            List<Review> list = _repo.FindReviewsByCustomer(customer);
            TempData.Keep("customerId");
            return View(list);
        }
    }
}
