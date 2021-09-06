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
            //this.Name = char.ToUpper(this.Name[0]) + this.Name.Substring(1);
            //this.StreetName = char.ToUpper(this.StreetName[0]) + this.StreetName.Substring(1);
            //this.City = char.ToUpper(this.City[0]) + this.City.Substring(1);
            this.State = this.State.ToUpper();
            Name = Capitalize(Name);
            StreetName = Capitalize(StreetName);
            City = Capitalize(City);
            return results;
        }
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
