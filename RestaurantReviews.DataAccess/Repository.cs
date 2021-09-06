using System;
using RestaurantReviews.Domain;
using RestaurantReviews.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantReviews.DataAccess
{
    public class Repository : IRepository
    {
        
        private readonly RestaurantReviewContext _context;
        public Repository(RestaurantReviewContext context)
        {
            _context = context;
        }

        public List<Domain.Restaurant> SearchRestaurantsName(string name)
        {
            List<Domain.Restaurant> list = _context.Restaurants.Select(
                Restaurant => new Domain.Restaurant(Restaurant.Id, Restaurant.Name, Restaurant.Address, Restaurant.Zip)
            ).ToList();
            List<Domain.Restaurant> query = list.Where(Restaurant => Restaurant.Name.ToLower().Contains(name.ToLower())).ToList();


            return query;
        }

        public List<Domain.Restaurant> SearchRestaurantsAddress(string address)
        {
            List<Domain.Restaurant> list = _context.Restaurants.Select(
                Restaurant => new Domain.Restaurant(Restaurant.Id, Restaurant.Name, Restaurant.Address, Restaurant.Zip)
            ).ToList();
            List<Domain.Restaurant> query = list.Where(Restaurant => Restaurant.Address.ToLower().Contains(address.ToLower())).ToList();


            return query;
        }

        public List<Domain.Restaurant> SearchRestaurantsZip(int zip)
        {
            List<Domain.Restaurant> list = _context.Restaurants.Select(
                Restaurant => new Domain.Restaurant(Restaurant.Id, Restaurant.Name, Restaurant.Address, Restaurant.Zip)
            ).ToList();
            List<Domain.Restaurant> query = list.Where(Restaurant => Restaurant.Zip == zip).ToList();


            return query;
        }

        public Domain.Customer AddCustomer(Domain.Customer customer)
        {
            _context.Customers.Add(
                new Entities.Customer
                {
                    Name = customer.Name,
                    Pass = customer.Pass,
                    Phone = customer.Phone,
                    Email = customer.Email,
                    IsAdmin = customer.Admin
                }
            );
            _context.SaveChanges();

            return customer;
        }
        public Domain.Restaurant AddRestaurant(Domain.Restaurant restaurant)
        {
            _context.Restaurants.Add(
                new Entities.Restaurant
                {
                    Name = restaurant.Name,
                    Address = restaurant.Address,
                    Zip = restaurant.Zip
                }
            );
            _context.SaveChanges();

            return restaurant;
        }

        public List<Domain.Review> FindRatingsByRestaurantId(int Id)
        {
            
            List<Domain.Review> list = _context.Reviews.Select(
                Review => new Domain.Review(Review.Id, Review.Stars, Review.CustomerId, Review.RestaurantId, Review.Comment)
            ).ToList();
            List<Domain.Review> query = list.Where(Review => Review.RestaurantId == Id).ToList();
            query.Reverse();
            return query;

        }

        public List<Domain.Review> FindReviewsByCustomer(Domain.Customer customer)
        {

            List<Domain.Review> list = _context.Reviews.Select(
                Review => new Domain.Review(Review.Id, Review.Stars, Review.CustomerId, Review.RestaurantId, Review.Comment)
            ).ToList();
            List<Domain.Review> query = list.Where(Review => Review.CustomerId == customer.Id).ToList();
            query.Reverse();
            return query;
        }



        public List<Domain.Customer> SearchCustomers(string name)
        {
            List<Domain.Customer> list = _context.Customers.Select(
                Customer => new Domain.Customer(Customer.Id, Customer.Name, Customer.Pass, Customer.Phone, Customer.Email, Customer.IsAdmin)
            ).ToList();
            List<Domain.Customer> query = list.Where(Customer => Customer.Name.Contains(name)).ToList();


            return query;
        }


        public Domain.Customer GetCustomerById(int Id)
        {
            Entities.Customer foundCustomer = _context.Customers
                .FirstOrDefault(customer => customer.Id == Id);
            if (foundCustomer != null)
            {
                return new Domain.Customer(foundCustomer.Id, foundCustomer.Name, foundCustomer.Pass, foundCustomer.Phone, foundCustomer.Email, foundCustomer.IsAdmin);
            }
            return new Domain.Customer();
        }

        public Domain.Review LeaveReview(Domain.Review review)
        {
            _context.Reviews.Add(
                new Entities.Review
                {
                    CustomerId = review.CustomerId,
                    RestaurantId = review.RestaurantId,
                    Comment = review.textReview,
                    Stars = review.Stars
                }
            );
            _context.SaveChanges();

            return review;
        }

        public void DeleteUser(Domain.Customer customer)
        {
            List<Domain.Review> usersReviews = FindReviewsByCustomer(customer);
            foreach (Domain.Review review in usersReviews)
            {
                DeleteReview(review);
            }
            Entities.Customer userToDelete = new Entities.Customer
            {
                Id = customer.Id,
                Name = customer.Name,
                Pass = customer.Pass,
                Phone = customer.Phone,
                Email = customer.Email,
                IsAdmin = customer.Admin
            };
            _context.Entry(userToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Customers.Remove(userToDelete);
            _context.SaveChanges();
        }

        public void DeleteReview(Domain.Review review)
        {
            Entities.Review reviewToDelete = new Entities.Review
            {
                Id = review.Id,
                CustomerId = review.CustomerId,
                RestaurantId = review.RestaurantId,
                Comment = review.textReview,
                Stars = review.Stars
            };

            _context.Reviews.Remove(reviewToDelete);
            _context.SaveChanges();
        }

        public void DeleteRestaurant(Domain.Restaurant restaurant)
        {
            List<Domain.Review> restaurantReviews = FindRatingsByRestaurantId(restaurant.Id);
            foreach (Domain.Review review in restaurantReviews)
            {
                DeleteReview(review);
            }
            Entities.Restaurant restaurantToDelete = new Entities.Restaurant
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Address = restaurant.Address,
                Zip = restaurant.Zip
            };
            _context.Restaurants.Remove(restaurantToDelete);
            _context.SaveChanges();
        }

        public Domain.Customer GetCustomer(string name)
        {
            Entities.Customer foundCustomer = _context.Customers
                .FirstOrDefault(customer => customer.Name == name);
            if (foundCustomer != null)
            {
                return new Domain.Customer(foundCustomer.Id, foundCustomer.Name, foundCustomer.Pass, foundCustomer.Phone, foundCustomer.Email, foundCustomer.IsAdmin);
            }
            return new Domain.Customer();
        }
        public Domain.Review GetReviewById(int Id)
        {
            Entities.Review foundReview = _context.Reviews
                .FirstOrDefault(review => review.Id == Id);
            if (foundReview != null)
            {
                return new Domain.Review(foundReview.Id, foundReview.Stars, foundReview.CustomerId, foundReview.RestaurantId, foundReview.Comment);
            }
            return new Domain.Review();
        }
        public Domain.Restaurant GetRestaurantById(int Id)
        {
            Entities.Restaurant foundRestaurant = _context.Restaurants
                .FirstOrDefault(restaurant => restaurant.Id == Id);
            if (foundRestaurant != null)
            {
                return new Domain.Restaurant(foundRestaurant.Id, foundRestaurant.Name, foundRestaurant.Address, foundRestaurant.Zip);
            }
            return new Domain.Restaurant();
        }

    }
}
