using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantReviews.Domain;
using RestaurantReviews.DataAccess;
using RestaurantReviews.WebApp.Models;

namespace RestaurantReviews.WebApp.Controllers
{
    
    public class RestaurantController : Controller
    {
        private readonly IRepository _repo;
        public RestaurantController(IRepository repo)
        {
            _repo = repo;

        }
        public IActionResult Index()
        {
            return View(_repo.SearchRestaurantsName(""));
        }
        public IActionResult Search(string searchString)
        {
            List<Restaurant> Restaurants;
            if (!String.IsNullOrEmpty(searchString))
            {
                int zip;
                if(int.TryParse(searchString, out zip))
                {
                    Restaurants = _repo.SearchRestaurantsZip(zip);
                }
                else
                {
                    Restaurants = _repo.SearchRestaurantsName(searchString);
                }
            }else
            {
                Restaurants = _repo.SearchRestaurantsName("");
            }
            TempData.Keep("IsAdmin");
            TempData.Keep("CurrentUserId");
            return View(Restaurants);

        }
        public IActionResult Reviews(int Id)
        {
            List<Review> reviews = _repo.FindRatingsByRestaurantId(Id);
            Restaurant restaurant = _repo.GetRestaurantById(Id);
            ViewData["Restaurant"] = restaurant.Name;
            ViewData["Address"] = $"{restaurant.Address} {restaurant.Zip}";
            TempData["RestaurantId"] = restaurant.Id;
            TempData.Keep("RestaurantId");
            TempData.Keep("CurrentUserId");
            TempData.Keep("IsAdmin");
            return View(reviews);
        }
        [HttpGet]
        public IActionResult LeaveReview()
        {
            TempData.Keep("RestaurantId");
            if(TempData["CurrentUserId"] is null)
            {
                return RedirectToAction("Login", "Login");
            }
            int userId = (int)TempData["CurrentUserId"];
            int restaurantId = (int)TempData["RestaurantId"];
            if (CustomerHasReview(userId, restaurantId).textReview is null)
            {
                return View(); //edit 
            }
            TempData.Keep("IsAdmin");
            TempData.Keep("CurrentUserId");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LeaveReview(CreatedReview viewModel)
        {
            if (String.IsNullOrWhiteSpace(viewModel.textReview))
            {
                viewModel.textReview = "User left not comment";
            }
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            if((int)TempData["CurrentUserId"] != -1 && TempData["RestaurantId"] != null){
                int userId = (int)TempData["CurrentUserId"];
                int restaurantId = (int)TempData["RestaurantId"];
                Review review = new Review(viewModel.Stars, userId, restaurantId, viewModel.textReview);
                _repo.LeaveReview(review);
                return RedirectToAction("Index");
            }
            else
            {
                return View(viewModel);
            }

        }
        public Review CustomerHasReview(int customerId, int restaurantId)
        {
            List<Review> list = _repo.FindRatingsByRestaurantId(restaurantId);
            Review oldReview = list.FirstOrDefault(review => review.CustomerId == customerId);
            if(oldReview is null)
            {
                oldReview = new Review();
            }

            return oldReview;

        }
    }
}
