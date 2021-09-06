using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using RestaurantReviews.Domain;
using System.ComponentModel;

namespace RestaurantReviews.WebApp.Models
{
    public class CreatedRestaurant : IValidatableObject
    {

        public string Name { get; set; }
        public string Address { get; set; }
        public int Zip { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            
            
            return results;
        }
    }
}
