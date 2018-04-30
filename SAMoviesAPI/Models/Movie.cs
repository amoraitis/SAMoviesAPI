﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SAMoviesAPI.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public double GetAverageRating()
        {
            double sum = 0;
            foreach(Rating rate in Ratings)
            {
                sum += rate.Rate;
            }
            return sum / Ratings.Count;
        }
    }
    public class Rating
    {
        [Key]
        public int UserId { get; set; }
        public int Rate { get; set; }
    }
    public class Comment
    {
        [Key]
        public int UserId { get; set; }
        public string Content { get; set; }
    }
}
