using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

namespace RestaurantReviews.Domain
{
    /// <summary>
    /// Customer model containing fields ID, Name, Pass, Phone, Email, Admin, constuctor with ID should only be used in repo and edit review
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// 
        /// </summary>
        public Customer() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="admin"></param>
        public Customer(string name, string pass, string phone, string email, bool? admin)
        {
            this.Pass = pass;
            this.Name = name;
            this.Phone = phone;
            this.Email = email;
            this.Admin = admin;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="admin"></param>
        public Customer(int id, string name, string pass, string phone, string email, bool? admin)
        {
            this.Id = id;
            this.Pass = pass;
            this.Name = name;
            this.Phone = phone;
            this.Email = email;
            this.Admin = admin;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Username")]
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Password")]
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
        public bool? Admin { get; set; }

    }
}