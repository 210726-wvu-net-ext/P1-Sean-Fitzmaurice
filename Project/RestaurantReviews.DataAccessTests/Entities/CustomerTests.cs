using Xunit;
using RestaurantReviews.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReviews.DataAccess.Entities.Tests
{
    public class CustomerTests
    {
        [Fact()]
        public void CustomerTest()
        {
            var customer =
                new Customer
                {
                    Name = "Sean",
                    Pass = "12345",
                    Phone = "5555555",
                    Email = "sean@yahoo.com",
                    IsAdmin = true
                };
            string actual = customer.Name;
            string expected = "Sean";
            Assert.Equal(actual, expected);
            
        }
    }
}