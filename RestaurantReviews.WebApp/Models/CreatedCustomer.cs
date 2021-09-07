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
        [Required]
        public string Phone { get; set; }
        [EmailAddress]
        [Required]
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
            if(this.Pass.Length > 100)
            {
                results.Add(new ValidationResult("Password is too long, maximum 100 characters"));
            }
            if (this.Name.Length > 100)
            {
                results.Add(new ValidationResult("Name is too long, maximum 100 characters"));
            }
            if (this.Phone.Length > 14)
            {
                results.Add(new ValidationResult("Phone number is too long, maximum 14 characters"));
            }
            if (this.Email.Length > 100)
            {
                results.Add(new ValidationResult("Email is too long, maximum 100 characters"));
            }

            return results;
        }
    }
}
