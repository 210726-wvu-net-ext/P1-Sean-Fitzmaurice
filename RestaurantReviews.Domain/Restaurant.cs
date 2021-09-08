using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

namespace RestaurantReviews.Domain
{
    /// <summary>
    /// Restaurant Model containing fields ID name address and zip, constuctor with ID should only be used in repo and edit review. fields reviews and avgstars are only used when needed, and therefore not in constructor
    /// </summary>
    public class Restaurant
    {

        public Restaurant() { }

        public Restaurant(string name, string address, int zip)
        {
            this.Zip = zip;
            this.Address = address;
            this.Name = name;
        }
        public Restaurant(int id, string name, string address, int zip)
        {
            this.Id = id;
            this.Zip = zip;
            this.Address = address;
            this.Name = name;
        }
        /// <summary>
        /// comcatinates zip and address into single string for display
        /// </summary>
        /// <returns>concatinated string</returns>
        public string GetFullAddress()
        {
            string fullAddress = $"{Address}, {Zip}";
            return fullAddress;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Zip { get; set; }
        public List<Review> reviews { get; set; }
        [DisplayName("Average Rating")]
        public decimal avgStars { get; set; }
        /// <summary>
        /// calculate average rating of all reviews for this restaurant
        /// </summary>
        public void calcAvg()
        {
            decimal avg = 0;
            if (reviews != null)
            {
                int count = reviews.Count;
                if (count == 0)
                {
                    this.avgStars = -1;
                }
                else
                {
                    foreach (Review review in reviews)
                    {
                        avg += review.Stars;
                    }
                    avg = avg / count;
                    avg = Decimal.Round(avg, 2);
                    this.avgStars = avg;
                }
            }
            else
            {
                this.avgStars = -1;
            }
            

        }
    }
}
