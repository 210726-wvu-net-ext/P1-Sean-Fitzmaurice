using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantReviews.Domain;
using RestaurantReviews.DataAccess;
using RestaurantReviews.WebApp.Models;
using Serilog;

namespace RestaurantReviews.WebApp.Controllers
{
    /// <summary>
    /// restaurant controller handles most views having to do restaurants and reviews
    /// </summary>
    public class RestaurantController : Controller
    {
        private readonly IRepository _repo;

        /// <summary>
        /// initializes creation of crpo
        /// </summary>
        /// <param name="repo"></param>
        public RestaurantController(IRepository repo)
        {
            
            _repo = repo;

        }

        /// <summary>
        /// index action for restaurant controller, shows all restaurants in database in list
        /// </summary>
        /// <returns>view containing all restaurants in DB</returns>
        public IActionResult Index()
        {
            TempData.Remove("redirectController");
            TempData.Remove("redirectView");
            return View(_repo.SearchRestaurantsName(""));
        }

        /// <summary>
        /// search action get
        /// </summary>
        /// <returns>view containing search</returns>
        [HttpGet]
        public IActionResult Search()
        {
            return View(_repo.SearchRestaurantsName(""));
        }

        /// <summary>
        /// search action post, takes string entered by user in search bar, if entered string can be converted to int, assumes search for zip
        /// if it cannot, searches restaurants by both 'Address' and 'Name' columns in DB, combines returned lists and takes out duplicates
        /// </summary>
        /// <param name="searchString">entered search string by user</param>
        /// <returns>list of objects which match the entered search criteria</returns>
        [HttpPost]
        public IActionResult Search(string searchString)
        {
            TempData.Remove("redirectController");
            TempData.Remove("redirectView");
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

        /// <summary>
        /// shows all reviews for a given restaurant passed restaurant name and address through viewdata
        /// </summary>
        /// <param name="Id">ID primary key of restaurant to find reviews for</param>
        /// <returns>list of reviews for given restaurant</returns>
        public IActionResult Reviews(int restaurantId)
        {
            TempData.Remove("redirectController");
            TempData.Remove("redirectView");
            Restaurant restaurant = _repo.GetRestaurantById(restaurantId, true);
            ViewData["Restaurant"] = restaurant.Name;
            ViewData["Address"] = restaurant.GetFullAddress();
  
            if(restaurant.avgStars == -1)
            {
                ViewData["Rating"] = "-";
            }
            else
            {
                ViewData["Rating"] = restaurant.avgStars;
            }
             
            TempData["RestaurantId"] = restaurant.Id;
            TempData.Keep("RestaurantId");
            TempData.Keep("CurrentUserId");
            TempData.Keep("IsAdmin");
            return View(restaurant.reviews);
        }

        /// <summary>
        /// leave review action get, redirects the user to login page if they are not logged in, redirects user to edit if they already have a review
        /// </summary>
        /// <returns>view containing form for review</returns>
        [HttpGet]
        public IActionResult LeaveReview()
        {
            TempData["RestaurantName"] = _repo.GetRestaurantById((int)TempData["RestaurantId"], false).Name;
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
        /// <summary>
        /// leave review action post, if textReview field is left empty, automatically fills it with base comment, checks if review is valid, and if so, adds it to DB through repo
        /// </summary>
        /// <param name="viewModel">the validatible version of a review object</param>
        /// <returns>reviews page if left review is valid, returns the same view if not valid</returns>
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
                try
                {
                    _repo.LeaveReview(review);
                    Log.Information("Left Review");
                }
                catch (Exception e)
                {
                    Log.Fatal(e, "Unexpected Error When Wrtiting to Database");
                    return RedirectToAction("Error", "Error", new { message = "Unexpected Error When Writing to to Database" });
                }
                return RedirectToAction("Reviews", new { restaurantId = restaurantId });
            }
            else
            {
                return View(viewModel);
            }

        }

        /// <summary>
        /// checks if logged in customer has a review already for a restaurant
        /// </summary>
        /// <param name="customerId">PK ID of given in user</param>
        /// <param name="restaurantId">PK ID of given restaurant</param>
        /// <returns>returns a review object, if they do have an old review, returns that, otherwise returns new review object</returns>
        public Review CustomerHasReview(int customerId, int restaurantId)
        {
            List<Review> list = _repo.FindRatingsByRestaurantId(restaurantId);
            Review oldReview = list.FirstOrDefault(review => review.CustomerId == customerId);
            if(oldReview is null)
            {
                oldReview = new Review(); //lol
            }

            return oldReview;

        }
        /// <summary>
        /// get action for deleteing a review, checks if logged in user is an admin, or is the user who left the review
        /// </summary>
        /// <param name="deleteReview">the review being deleted</param>
        /// <returns>delete view displaying review objects details</returns>
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
        /// <summary>
        /// post action for deleting reviews, checks if password matches logged in user, if they do, removes it from DB using repo
        /// </summary>
        /// <param name="password">password entered by user</param>
        /// <returns>restaurant index if succesful, returns same view if password does not match</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteReview(string password)
        {
            int id = (int)TempData["ToDeleteId"];
            if (password == _repo.GetCustomerById((int)TempData["CurrentUserId"]).Pass)
            {
                try
                {
                    _repo.DeleteReview(_repo.GetReviewById(id));
                    Log.Information($"Deleted Review with Id of {id}");
                    TempData.Keep("IsAdmin");
                    TempData.Keep("CurrentUserId");
                    TempData.Remove("ToDeleteId");
                }
                catch(Exception e)
                {
                    Log.Fatal(e, "Unexpected Error When Deleting from Database");
                    return RedirectToAction("Error", "Error", new { message = "Unexpected Error When Deleting from Database" });
                }
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

        /// <summary>
        /// get action for editing a review, creates validatible review object which takes information from old review
        /// </summary>
        /// <param name="viewModel">the review being edited</param>
        /// <returns>view displaying validatible review object</returns>
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

        /// <summary>
        /// post action for editing a review, checks if new review is valid, transfers uneditible information from old review to new review, deletes
        /// old review and finally adds new review to DB using review
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>redirection to list of reviews for a given restaurant if valid, same view if new review is not valid</returns>
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
            try
            {
                _repo.DeleteReview(oldReview);
                _repo.LeaveReview(newReview);
                Log.Information($"Edited Review with Id of {newReview.Id}");
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Unexpected Error When Deleting/Writing from Database");
                return RedirectToAction("Error", "Error", new { message = "Unexpected Error When Deleting/Writing from Database" });
            }
            TempData.Keep("IsAdmin");
            TempData.Keep("CurrentUserId");
            return RedirectToAction("Reviews", new { restaurantId = newReview.RestaurantId });
        }

        /// <summary>
        /// get action for creating a restaurant, checks if user is logged in, redircts fto login page if not
        /// </summary>
        /// <returns>view containing form for restaurant information</returns>
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

        /// <summary>
        /// checks if restaurant is valid, creates domain object from validatible one if valid and passes it to repo for entry into DB
        /// </summary>
        /// <param name="viewModel">entered user infromation, in validatible restaurant</param>
        /// <returns>restaurant index if valid, same view if not valid</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateRestaurant(CreatedRestaurant viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            string address = $"{viewModel.StreetNumber} {viewModel.StreetName}, {viewModel.City} {viewModel.State}";
            Restaurant newRestaurant = new Restaurant(viewModel.Name, address, viewModel.Zip);
            try
            {
                _repo.AddRestaurant(newRestaurant);
                Log.Information($"Created new Restaurant");
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Unexpected Error When Writing to Database");
                return RedirectToAction("Error", "Error", new { message = "Unexpected Error When Wrtiting to Database" });
            }
            return RedirectToAction( "Index", "Restaurant");
        }
        
        /// <summary>
        /// get action for deleting a restaurant, checks if user is admin
        /// </summary>
        /// <param name="deleteRestaurant">restaurant object to be removed from DB</param>
        /// <returns>returns view with restaurant to be deleted info if admin, if not admin returns error page</returns>
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

        /// <summary>
        /// post action for deleting restaurant, checks if entered password and ID match
        /// </summary>
        /// <param name="toDeleteId">ID of restaurant to be deleted</param>
        /// <param name="password">password of logged in admin</param>
        /// <returns>error page if password or ID does not match, restaurant index if dletion is successful</returns>
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
                try
                {
                    _repo.DeleteRestaurant(_repo.GetRestaurantById(id, false));
                    Log.Information($"Deleted restaurant with Id of {id}");
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
                return RedirectToAction("Error", "Error", new { message = "Restaurant Id or Password was entered incorrectly, aborting delete of restaurant" });
            }
            return RedirectToAction("Index", "Restaurant");
        }

        /// <summary>
        /// details action page for a given review
        /// </summary>
        /// <param name="review">review to show details for</param>
        /// <returns>view with detials on given review</returns>
        public IActionResult ReviewDetails(Review review)
        {
            string name = _repo.GetCustomerById(review.CustomerId).Name;
            Restaurant restaurant = _repo.GetRestaurantById(review.RestaurantId,false);
            string restaurantString = $"{restaurant.Name}:   {restaurant.Address}";
            ViewData["name"] = name;
            ViewData["restaurant"] = restaurantString;
            return View(review);
        }
    }
}
