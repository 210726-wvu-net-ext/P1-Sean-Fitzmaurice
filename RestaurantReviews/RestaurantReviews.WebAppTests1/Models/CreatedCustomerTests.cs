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
    public class CreatedCustomerTests
    {
        [Fact()]
        public void ValidateCustomerTest1()
        {

            CreatedCustomer customer = new CreatedCustomer();
            customer.Name = "Bob";
            customer.Pass = "12345";
            customer.ConfirmPass = "123456";
            customer.Phone = "8455555555";
            customer.Email = "dumdum@gmail.com";
            var validationContext = new ValidationContext(customer);

            var results = customer.Validate(validationContext);
            int actual = results.Count();
            
            int expected = 1;
            Assert.Equal(actual, expected);
        }
        [Fact()]
        public void ValidateCustomerTest2()
        {

            CreatedCustomer customer = new CreatedCustomer();
            customer.Name = "jim";
            customer.Pass = "54321";
            customer.ConfirmPass = "54321";
            customer.Phone = "55555555555";
            customer.Email = "dumdum@yahoo.net";
            var validationContext = new ValidationContext(customer);

            var results = customer.Validate(validationContext);
            int actual = results.Count();
            
            int expected = 0;
            Assert.Equal(actual, expected);
        }
        [Fact()]
        public void ValidateCustomerTest3()
        {

            CreatedCustomer customer = new CreatedCustomer();
            customer.Name = "jim";
            customer.Pass = "54321";
            customer.ConfirmPass = "54321";
            customer.Phone = "5555555555522222222222222222222222222222222222222222";
            customer.Email = "dumdum@yahoo.net";
            var validationContext = new ValidationContext(customer);

            var results = customer.Validate(validationContext);
            int actual = results.Count();

            int expected = 1;
            Assert.Equal(actual, expected);
        }
    }
}