using System;
using System.Collections.Generic;

#nullable disable

namespace RestaurantReviews.DataAccess.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Restaurant
    {
        /// <summary>
        /// 
        /// </summary>
        public Restaurant()
        {
            Reviews = new HashSet<Review>();
        }

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Zip { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
