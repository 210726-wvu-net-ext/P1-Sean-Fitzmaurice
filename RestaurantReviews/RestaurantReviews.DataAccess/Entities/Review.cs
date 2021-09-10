using System;
using System.Collections.Generic;

#nullable disable

namespace RestaurantReviews.DataAccess.Entities
{
    public partial class Review
    {
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
        public string Comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Stars { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime LeftAt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual Restaurant Restaurant { get; set; }
    }
}
