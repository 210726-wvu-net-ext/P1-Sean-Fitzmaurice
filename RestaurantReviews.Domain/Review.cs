﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

namespace RestaurantReviews.Domain
{
    /// <summary>
    /// Review model
    /// </summary>
    public class Review
    {
        public Review() { }

        public Review(int reviewId, decimal stars, int customer, int restaurant, string textReview)
        {
            this.Id = reviewId;
            this.Stars = stars;
            this.textReview = textReview;
            this.CustomerId = customer;
            this.RestaurantId = restaurant;
        }

        public Review(decimal stars, int customer, int restaurant, string textReview)
        {
            this.Stars = stars;
            this.textReview = textReview;
            this.CustomerId = customer;
            this.RestaurantId = restaurant;
        }


        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int RestaurantId { get; set; }
        [DisplayName("Rating")]
        public decimal Stars { get; set; }
        [DisplayName("Comment")]
        public string textReview { get; set; }
    }
}
