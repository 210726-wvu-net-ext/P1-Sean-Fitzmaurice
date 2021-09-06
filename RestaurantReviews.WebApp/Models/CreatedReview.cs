using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using RestaurantReviews.Domain;
using System.ComponentModel;

namespace RestaurantReviews.WebApp.Models
{
    public class CreatedReview : IValidatableObject
    {
        [Required]
        [DisplayName("Rating")]
        public decimal Stars { get; set; }
        [Required]
        [DisplayName("Comment")]
        public string textReview { get; set; }
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

            return results;
            //still need to add validation check for stars between 1 and 5
        }
    }
}
