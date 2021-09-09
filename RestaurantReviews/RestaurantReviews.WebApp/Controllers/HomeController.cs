using Microsoft.AspNetCore.Mvc;
using RestaurantReviews.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using RestaurantReviews.Domain;
using Serilog;

namespace RestaurantReviews.WebApp.Controllers
{
    /// <summary>
    /// Controller for home page
    /// </summary>
    public class HomeController : Controller
    {
        
        private readonly IRepository _repo;

        /// <summary>
        /// constuctor which initializes repo and logger
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="repo"></param>
        public HomeController( IRepository repo)
        {
            
            _repo = repo;

        }

        /// <summary>
        /// Home index view
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            TempData.Remove("redirectController");
            TempData.Remove("redirectView");
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
