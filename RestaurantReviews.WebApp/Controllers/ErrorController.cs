using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantReviews.WebApp.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error(string message)
        {
            ViewData["message"] = message;
            return View();
        }
    }
}
