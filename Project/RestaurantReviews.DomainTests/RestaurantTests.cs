using Xunit;
using RestaurantReviews.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReviews.Domain.Tests
{
    public class RestaurantTests
    {
        [Fact()]
        public void RestaurantTest()
        {
            Restaurant restaurant = new Restaurant("Bobs", "7 arnold street", 12538);
            restaurant.calcAvg();
            decimal actual = restaurant.avgStars;
            decimal expected = -1;
            Assert.Equal(actual, expected);
        }
    }
}