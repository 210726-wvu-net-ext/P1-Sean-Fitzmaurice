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
    /// validatible restuarant object, breaks down address string into individual fields so they can each be
    /// validated seperately, and combined into address later
    /// </summary>
    public class CreatedRestaurant : IValidatableObject
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [DisplayName("Street Number")]
        public int StreetNumber { get; set; }
        [Required]
        [DisplayName("Street")]
        public string StreetName { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [DisplayName("State/Province")]
        public string State { get; set;  }

        [Required]
        public int Zip { get; set; }

        /// <summary>
        /// validation method, checks if address will be valid varchar, checks if name is valid varchar, capitalizes individual compnents of address
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns>validation list</returns>

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(this.Name,
                new ValidationContext(this, null, null) { MemberName = "Name", },
                results);
            Validator.TryValidateProperty(this.StreetNumber,
                new ValidationContext(this, null, null) { MemberName = "StreetNumber", },
                results);
            Validator.TryValidateProperty(this.City,
                new ValidationContext(this, null, null) { MemberName = "City", },
                results);
            Validator.TryValidateProperty(this.State,
                new ValidationContext(this, null, null) { MemberName = "State", },
                results);
            Validator.TryValidateProperty(this.StreetName,
                new ValidationContext(this, null, null) { MemberName = "StreetName", },
                results);
            Validator.TryValidateProperty(this.Zip,
                new ValidationContext(this, null, null) { MemberName = "Zip", },
                results);
            this.State = this.State.ToUpper();
            Name = Capitalize(Name);
            StreetName = Capitalize(StreetName);
            City = Capitalize(City);
            if (this.Name.Length > 100)
            {
                results.Add(new ValidationResult("Name is too long, maximum 100 characters"));
            }
            if ((this.StreetNumber.ToString().Length + this.StreetName.Length +this.City.Length+this.State.Length) > 100)
            {
                results.Add(new ValidationResult("Address is too long, maximum 100 characters"));
            }
            return results;
        }

        /// <summary>
        /// method used to capitalize every seperate word in a string
        /// </summary>
        /// <param name="str">string to be capitalized</param>
        /// <returns>capitalized string</returns>
        private string Capitalize(string str)
        {
            List<int> foundSpaces = new List<int>();
            string newStr;
            int len = str.Length;
            if(len > 100)
            {
                return str;
            }
            for (int i = 0; i < len; i++ )
            {
                if(str[i] == ' ')
                {
                    foundSpaces.Add(i);
                }
                    
               
            }
            newStr = char.ToUpper(str[0]) + str.Substring(1);
            foreach (int i in foundSpaces)
            {
                if(i + 2 < len)
                {
                    newStr = newStr.Substring(0,i+1) + char.ToUpper(newStr[i+1]) + newStr.Substring(i + 2);
                }
                
            }
            return newStr;
            
        }
    }
}
