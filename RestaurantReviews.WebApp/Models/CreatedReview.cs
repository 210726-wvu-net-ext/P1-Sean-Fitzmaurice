using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using RestaurantReviews.Domain;
using System.ComponentModel;

namespace RestaurantReviews.WebApp.Models
{

    /// <summary>
    /// validateible review class
    /// </summary>
    public class CreatedReview : IValidatableObject
    {
        [Required]
        [DisplayName("Rating")]
        public decimal Stars { get; set; }
        [Required]
        [DisplayName("Comment")]
        public string textReview { get; set; }

        /// <summary>
        /// validation method, checks if stars rating is in bounds, checks if varchar will not be too long
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns>validation results list</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            var results = new List<ValidationResult>();
                Validator.TryValidateProperty(this.Stars,
                new ValidationContext(this, null, null) { MemberName = "Stars", },
                results);
                Validator.TryValidateProperty(this.textReview,
                new ValidationContext(this, null, null) { MemberName = "textReview", },
                results);
            if (this.Stars > 5 || this.Stars < 0)
            {
                results.Add(new ValidationResult("Rating out of bounds!"));
            }
            if (this.textReview.Length > 300)
            {
                results.Add(new ValidationResult("Review is too long, maximum 300 characters"));
            }

            return results;
            
        }
    }
}
