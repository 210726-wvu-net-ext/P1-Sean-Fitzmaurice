using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RestaurantReviews.Domain;
using System.ComponentModel;

namespace RestaurantReviews.WebApp.Models
{
    public class CreatedCustomer : IValidatableObject
    {
        [Required]
        [MinLength(3)]
        [DisplayName("Username")]
        public string Name { get; set; }
        [Required]
        [MinLength(5)]
        [DisplayName("Password")]
        public string Pass { get; set; }
        [Required]
        [MinLength(5)]
        [DisplayName("Confirm Password")]
        public string ConfirmPass { get; set; }
        [Phone]
        public string Phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(this.Name,
                new ValidationContext(this, null, null) { MemberName = "Name", },
                results);
            Validator.TryValidateProperty(this.Pass,
                new ValidationContext(this, null, null) { MemberName = "Pass", },
                results);
            Validator.TryValidateProperty(this.ConfirmPass,
                new ValidationContext(this, null, null) { MemberName = "ConfirmPass", },
                results);
            Validator.TryValidateProperty(this.Phone,
                new ValidationContext(this, null, null) { MemberName = "Phone", },
                results);
            Validator.TryValidateProperty(this.Email,
                new ValidationContext(this, null, null) { MemberName = "Email", },
                results);
            if (this.Pass != this.ConfirmPass)
            {
                results.Add(new ValidationResult("Passwords do not match"));
            }

            return results;
        }
    }
}
