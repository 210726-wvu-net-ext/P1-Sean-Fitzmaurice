using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantReviews.WebApp.Controllers
{
    /// <summary>
    /// controller for showing error messages
    /// </summary>
    public class ErrorController : Controller
    {  
        /// <summary>
        /// Action method for showing errors to user
        /// </summary>
        /// <param name="message">error message for showing user</param>
        /// <returns>view displaying error message</returns>
        public IActionResult Error(string message)
        {
            TempData.Keep("CurrentUserId");
            TempData.Keep("IsAdmin");
            ViewData["message"] = message;
            return View();
        }
    }
}
