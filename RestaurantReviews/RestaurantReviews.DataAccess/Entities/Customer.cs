using System;
using System.Collections.Generic;

#nullable disable

namespace RestaurantReviews.DataAccess.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Customer
    {
        /// <summary>
        /// 
        /// </summary>
        public Customer()
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
        public string Pass { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? IsAdmin { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
