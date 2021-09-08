using Xunit;
using RestaurantReviews.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RestaurantReviews.WebApp.Models.Tests
{
    public class CreatedRestaurantTests
    {
        [Fact()]
        public void ValidateRestaurantTest1()
        {
            CreatedRestaurant restaurant = new CreatedRestaurant();
            restaurant.Name = "Antonellas";
            restaurant.State = "ny";
            restaurant.StreetName = "Albany Post Road";
            restaurant.City = "Poughkeepsie";
            restaurant.StreetNumber = 7;
            restaurant.Zip = 12601;
            var validationContext = new ValidationContext(restaurant);

            var results = restaurant.Validate(validationContext);
            int actual = results.Count();
            
            int expected = 0;
            Assert.Equal(actual, expected);
        }
        [Fact()]
        public void ValidateRestaurantTest2()
        {
            CreatedRestaurant restaurant = new CreatedRestaurant();
            restaurant.Name = "Antonellas";
            restaurant.State = "ny";
            restaurant.StreetName = "albany post road";
            restaurant.City = "Poughkeepsie";
            restaurant.StreetNumber = 7;
            restaurant.Zip = 12601;
            var validationContext = new ValidationContext(restaurant);

            var results = restaurant.Validate(validationContext);
            string actual = restaurant.StreetName;
          
            string expected = "Albany Post Road";
            Assert.Equal(actual, expected);
        }
        [Fact()]
        public void ValidateRestaurantTest3()
        {
            CreatedRestaurant restaurant = new CreatedRestaurant();
            restaurant.Name = "Antonellas";
            restaurant.State = "ny";
            restaurant.StreetName = "__________________________________________________________________________________________________________________________________________________________________________________________________________________________";
            restaurant.City = "Poughkeepsie";
            restaurant.StreetNumber = 7;
            restaurant.Zip = 12601;
            var validationContext = new ValidationContext(restaurant);

            var results = restaurant.Validate(validationContext);
            int actual = results.Count();
            
            int expected = 2;
            Assert.Equal(actual, expected);
        }
        [Fact()]
        public void ValidateRestaurantTest4()
        {
            CreatedRestaurant restaurant = new CreatedRestaurant();
            restaurant.Name = "a";
            restaurant.State = "ny";
            restaurant.StreetName = "albany post road";
            restaurant.City = "Poughkeepsie";
            restaurant.StreetNumber = 7;
            restaurant.Zip = 12601;
            var validationContext = new ValidationContext(restaurant);

            var results = restaurant.Validate(validationContext);
            int actual = results.Count();

            int expected = 1;
            Assert.Equal(actual, expected);
        }
    }
}