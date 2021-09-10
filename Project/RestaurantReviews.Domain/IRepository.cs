using System;
using System.Collections.Generic;

namespace RestaurantReviews.Domain
{
    /// <summary>
    /// Interface for repository 
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="restaurant"></param>
        /// <returns></returns>
        Restaurant AddRestaurant(Restaurant restaurant);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>

        Customer AddCustomer(Customer user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>

        Customer GetCustomer(string name);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>

        List<Customer> SearchCustomers(string name);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="review"></param>
        /// <returns></returns>

        Review LeaveReview(Review review);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        List<Review> FindRatingsByRestaurantId(int Id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>

        List<Review> FindReviewsByCustomer(Customer customer);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        Customer GetCustomerById(int Id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>

        List<Restaurant> SearchRestaurantsName(string name);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>

        List<Restaurant> SearchRestaurantsAddress(string Address);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="zip"></param>
        /// <returns></returns>

        List<Restaurant> SearchRestaurantsZip(int zip);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="review"></param>

        void DeleteReview(Review review);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customer"></param>

        void DeleteUser(Customer customer);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="restaurant"></param>

        void DeleteRestaurant(Restaurant restaurant);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        Review GetReviewById(int Id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="needReviews"></param>
        /// <returns></returns>

        Restaurant GetRestaurantById(int Id, bool needReviews);


    }
}
