using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

namespace RestaurantReviews.Domain
{
    /// <summary>
    /// Review model containing ID, stars, textreview,textreview, customerID, restaurantID, date fields, constuctor with ID should only be used in repo and edit review
    /// </summary>
    public class Review
    {
        public Review() { }

        public Review(int reviewId, decimal stars, int customer, int restaurant, string textReview, DateTime leftAt)
        {
            this.Id = reviewId;
            this.Stars = stars;
            this.textReview = textReview;
            this.CustomerId = customer;
            this.RestaurantId = restaurant;
            this.Date = leftAt;
        }

        public Review(decimal stars, int customer, int restaurant, string textReview)
        {
            this.Stars = stars;
            this.textReview = textReview;
            this.CustomerId = customer;
            this.RestaurantId = restaurant;
            this.Date = DateTime.Now;
        }

        public DateTime Date{get; set;}
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int RestaurantId { get; set; }
        [DisplayName("Rating")]
        public decimal Stars { get; set; }
        [DisplayName("Comment")]
        public string textReview { get; set; }
    }
}
