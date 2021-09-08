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
    public class CreatedReviewTests
    {
        [Fact()]
        public void ValidateReviewTest1()
        {
            CreatedReview review = new CreatedReview();
            review.textReview = "kinda bad ngl";
            review.Stars = -5;
            var validationContext = new ValidationContext(review);

            var results = review.Validate(validationContext);
            int actual = results.Count() ;
            int expected = 1;
            Assert.Equal(actual, expected);
        }
        [Fact()]
        public void ValidateReviewTest2()
        {
            CreatedReview review = new CreatedReview();
            review.textReview = "kinda bad ngl";
            review.Stars = 1;
            var validationContext = new ValidationContext(review);

            var results = review.Validate(validationContext);
            int actual = results.Count();
            int expected = 0;
            Assert.Equal(actual, expected);
        }
        [Fact()]
        public void ValidateReviewTest3()
        {
            CreatedReview review = new CreatedReview();
            review.textReview = "";
            review.Stars = 1;
            var validationContext = new ValidationContext(review);

            var results = review.Validate(validationContext);
            int actual = results.Count();
            int expected = 1;
            Assert.Equal(actual, expected);
        }
    }
}