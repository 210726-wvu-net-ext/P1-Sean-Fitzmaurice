using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantReviews.Domain;
using RestaurantReviews.DataAccess;

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
                Restaurants = _repo.SearchRestaurantsName(searchString);
            }else
            {
                Restaurants = _repo.SearchRestaurantsName("");
            }

            return View(Restaurants);

        }
        public IActionResult Reviews(int Id)
        {
            List<Review> reviews = _repo.FindRatingsByRestaurantId(Id);
            Restaurant restaurant = _repo.GetRestaurantById(Id);
            ViewData["Restaurant"] = restaurant.Name;
            ViewData["Address"] = $"{restaurant.Address} {restaurant.Zip}";
            return View(reviews);
        }

    }
}
