﻿using System;
using System.Collections.Generic;

#nullable disable

namespace RestaurantReviews.DataAccess.Entities
{
    public partial class Review
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int RestaurantId { get; set; }
        public string Comment { get; set; }
        public decimal Stars { get; set; }
        public DateTime LeftAt { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
}
