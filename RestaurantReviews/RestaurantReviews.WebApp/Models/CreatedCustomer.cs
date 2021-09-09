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
    /// validitible customer model
    /// </summary>
    public class CreatedCustomer : IValidatableObject
    {
        [Required]
        [MinLength(3)]
        [MaxLength (30)]
        [DisplayName("Username")]
        public string Name { get; set; }
        [Required]
        [MinLength(5)]
        [DisplayName("Password")]
        public string Pass { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(40)]
        [DisplayName("Confirm Password")]
        public string ConfirmPass { get; set; }
        [Phone]
        [Required]
        [MaxLength(14)]
        public string Phone { get; set; }
        [EmailAddress]
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
        
        /// <summary>
        /// validation method, checks if passwords entered match, checks if varchars are not too long
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns>validation checks list</returns>
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
