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
        [HttpGet]
        public IActionResult Search()
        {
            return View(_repo.SearchRestaurantsName(""));
        }
        [HttpPost]
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
                    List<Restaurant> templist;
                    templist = _repo.SearchRestaurantsName(searchString);
                    templist.AddRange(_repo.SearchRestaurantsAddress(searchString));
                    Restaurants = templist.GroupBy(x => x.Id).Select(x => x.First()).ToList();
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
            decimal rating = AverageRating(reviews);
            if(rating == -1)
            {
                ViewData["Rating"] = "-";
            }
            else
            {
                ViewData["Rating"] = rating;
            }
             
            TempData["RestaurantId"] = restaurant.Id;
            TempData.Keep("RestaurantId");
            TempData.Keep("CurrentUserId");
            TempData.Keep("IsAdmin");
            return View(reviews);
        }
        public decimal AverageRating(List<Review> reviews)
        {
            decimal avg = 0;
            int count = reviews.Count;
            if(count == 0)
            {
                return -1;
            }
            foreach(Review review in reviews)
            {
                avg += review.Stars;
            }
            avg = avg / count;
            return avg;
        }
        [HttpGet]
        public IActionResult LeaveReview()
        {
            TempData["RestaurantName"] = _repo.GetRestaurantById((int)TempData["RestaurantId"]).Name;
            TempData.Keep("RestaurantName");
            TempData.Keep("RestaurantId");
            if(TempData["CurrentUserId"] is null)
            {
                TempData["redirectController"] = "Restaurant";
                TempData["redirectView"] = "LeaveReview";
                return RedirectToAction("Login", "Login");
            }
            int userId = (int)TempData["CurrentUserId"];
            int restaurantId = (int)TempData["RestaurantId"];
            Review oldReview = CustomerHasReview(userId, restaurantId);
            if (oldReview.textReview != null)
            {
                TempData.Keep("IsAdmin");
                TempData.Keep("CurrentUserId");
                return RedirectToAction("EditReview", oldReview ); //edit 
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
                return RedirectToAction("Reviews", new { id = restaurantId });
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
        [HttpGet]
        public IActionResult DeleteReview(Review deleteReview)
        {
            int currentId = (int)TempData["CurrentUserId"];
            Customer customer = _repo.GetCustomerById(currentId);
            if (customer.Admin is null)
            {
                if(deleteReview.Id == currentId)
                {
                    TempData.Keep("IsAdmin");
                    TempData.Keep("CurrentUserId");
                    return RedirectToAction("Error", "Error", new { message = "Must be an Admin to take this action!" });
                }

            }
            TempData["ToDeleteId"] = deleteReview.Id;
            TempData.Keep("IsAdmin");
            TempData.Keep("CurrentUserId");
            TempData.Keep("ToDeleteId");
            return View(deleteReview);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteReview(string password)
        {
            int id = (int)TempData["ToDeleteId"];
            if (password == _repo.GetCustomerById((int)TempData["CurrentUserId"]).Pass)
            {

                _repo.DeleteReview(_repo.GetReviewById(id));
                TempData.Keep("IsAdmin");
                TempData.Keep("CurrentUserId");
                TempData.Remove("ToDeleteId");
            }
            else
            {
                TempData.Keep("IsAdmin");
                TempData.Keep("CurrentUserId");
                TempData.Keep("ToDeleteId");
                ViewData["message"] = "Incorrect Password";
                return View(_repo.GetReviewById(id));
            }
            return RedirectToAction("Index", "Restaurant");
        }

        [HttpGet]
        public IActionResult EditReview(Review viewModel)
        {
            CreatedReview editReview = new CreatedReview();
            editReview.Stars = viewModel.Stars;
            editReview.textReview = viewModel.textReview;
            TempData["ToEditId"] = viewModel.Id;
            TempData.Keep("IsAdmin");
            TempData.Keep("CurrentUserId");
            TempData.Keep("ToEditId");
            return View(editReview);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditReview(CreatedReview viewModel)
        {
            if (String.IsNullOrWhiteSpace(viewModel.textReview))
            {
                viewModel.textReview = "User left not comment";
            }
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            Review newReview = new Review();
            Review oldReview = _repo.GetReviewById((int)TempData["ToEditId"]);
            newReview.Id = oldReview.Id;
            newReview.RestaurantId = oldReview.RestaurantId;
            newReview.CustomerId = oldReview.CustomerId;
            newReview.Stars = viewModel.Stars;
            newReview.textReview = viewModel.textReview;
            newReview.Date = DateTime.Now;
            _repo.DeleteReview(oldReview);
            _repo.LeaveReview(newReview);
            TempData.Keep("IsAdmin");
            TempData.Keep("CurrentUserId");
            return RedirectToAction("Reviews", new { id = newReview.RestaurantId });
        }

        [HttpGet]
        public IActionResult CreateRestaurant()
        {
            if (TempData["CurrentUserId"] is null)
            {
                TempData["redirectController"] = "CreateRestaurant";
                TempData["redirectView"] = "Restaurant";
                return RedirectToAction("Login", "Login");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateRestaurant(CreatedRestaurant viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            string address = $"{viewModel.StreetNumber} {viewModel.StreetName}, {viewModel.City}, {viewModel.State}";
            Restaurant newRestaurant = new Restaurant(viewModel.Name, address, viewModel.Zip);
            _repo.AddRestaurant(newRestaurant);
            return RedirectToAction( "Index", "Restaurant");
        }
        [HttpGet]
        public IActionResult DeleteRestaurant(Restaurant deleteRestaurant)
        {
            int currentId = (int)TempData["CurrentUserId"];
            Customer customer = _repo.GetCustomerById(currentId);
            if (customer.Admin is null)
            {
                    return RedirectToAction("Error", "Error", new { message = "Must be an Admin to take this action!" });  
            }
            TempData["ToDeleteId"] = deleteRestaurant.Id;
            TempData.Keep("IsAdmin");
            TempData.Keep("CurrentUserId");
            TempData.Keep("ToDeleteId");
            return View(deleteRestaurant);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteRestaurant(string toDeleteId, string password)
        {
            int id;
            if (!int.TryParse(toDeleteId, out id))
            {
                return RedirectToAction("Error", "Error", new { message = "Restaurant Id was entered incorrectly, aborting delete of restaurant" });
            }
            if (password == _repo.GetCustomerById((int)TempData["CurrentUserId"]).Pass && id == (int)TempData["ToDeleteId"])
            {

                _repo.DeleteRestaurant(_repo.GetRestaurantById(id));
                TempData.Keep("IsAdmin");
                TempData.Keep("CurrentUserId");
                TempData.Remove("ToDeleteId");
            }
            else
            {
                return RedirectToAction("Error", "Error", new { message = "Restaurant Id or Password was entered incorrectly, aborting delete of restaurant" });
            }
            return RedirectToAction("Index", "Restaurant");
        }
        public IActionResult ReviewDetails(Review review)
        {
            string name = _repo.GetCustomerById(review.CustomerId).Name;
            Restaurant restaurant = _repo.GetRestaurantById(review.RestaurantId);
            string restaurantString = $"{restaurant.Name}:   {restaurant.Address}";
            ViewData["name"] = name;
            ViewData["restaurant"] = restaurantString;
            return View(review);
        }
    }
}
