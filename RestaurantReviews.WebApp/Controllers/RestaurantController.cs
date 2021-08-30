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
    }
}
