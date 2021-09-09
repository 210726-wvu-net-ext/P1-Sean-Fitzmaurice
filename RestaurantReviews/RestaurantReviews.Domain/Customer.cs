﻿using System;
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
        public Customer() { }

        public Customer(string name, string pass, string phone, string email, bool? admin)
        {
            this.Pass = pass;
            this.Name = name;
            this.Phone = phone;
            this.Email = email;
            this.Admin = admin;
        }
        public Customer(int id, string name, string pass, string phone, string email, bool? admin)
        {
            this.Id = id;
            this.Pass = pass;
            this.Name = name;
            this.Phone = phone;
            this.Email = email;
            this.Admin = admin;
        }
        public int Id { get; set; }
        [DisplayName("Username")]
        public string Name { get; set; }
        [DisplayName("Password")]
        public string Pass { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool? Admin { get; set; }

    }
}