using System;
using RestaurantReviews.Domain;
using RestaurantReviews.DataAccess.Entities;

namespace RestaurantReviews.DataAccess
{
    public class Repository : IRepository
    {
        private readonly RestaurantReviewContext _context;
        public Repository(RestaurantReviewContext context)
        {
            _context = context;
        }
    }
}
