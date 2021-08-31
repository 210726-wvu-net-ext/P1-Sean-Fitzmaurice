using System;
using System.Collections.Generic;

namespace RestaurantReviews.Domain
{
    public interface IRepository
    {
        Restaurant AddRestaurant(Restaurant restaurant);

        Customer AddCustomer(Customer user);

        Customer GetCustomer(string name);

        List<Customer> SearchCustomers(string name);

        Review LeaveReview(Review review);

        List<Review> FindRatingsByRestaurantId(int Id);

        List<Review> FindReviewsByCustomer(Customer customer);

        Customer GetCustomerById(int Id);

        List<Restaurant> SearchRestaurantsName(string name);

        List<Restaurant> SearchRestaurantsAddress(string Address);

        List<Restaurant> SearchRestaurantsZip(int zip);

        void DeleteReview(Review review);

        void DeleteUser(Customer customer);

        void DeleteRestaurant(Restaurant restaurant);

        Review GetReviewById(int Id);

        Restaurant GetRestaurantById(int Id);
    }
}
