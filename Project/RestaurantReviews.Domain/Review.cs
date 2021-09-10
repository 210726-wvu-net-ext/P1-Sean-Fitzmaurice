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
        /// <summary>
        /// 
        /// </summary>
        public Review() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="stars"></param>
        /// <param name="customer"></param>
        /// <param name="restaurant"></param>
        /// <param name="textReview"></param>
        /// <param name="leftAt"></param>
        public Review(int reviewId, decimal stars, int customer, int restaurant, string textReview, DateTime leftAt)
        {
            this.Id = reviewId;
            this.Stars = stars;
            this.textReview = textReview;
            this.CustomerId = customer;
            this.RestaurantId = restaurant;
            this.Date = leftAt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stars"></param>
        /// <param name="customer"></param>
        /// <param name="restaurant"></param>
        /// <param name="textReview"></param>
        public Review(decimal stars, int customer, int restaurant, string textReview)
        {
            this.Stars = stars;
            this.textReview = textReview;
            this.CustomerId = customer;
            this.RestaurantId = restaurant;
            this.Date = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Date{get; set;}
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int RestaurantId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Rating")]
        public decimal Stars { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Comment")]
        public string textReview { get; set; }
    }
}
