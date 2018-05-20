using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SAMoviesAPI.Models
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public double GetAverageRating()
        {
            double result = -1.0;
            if (Ratings == null) return result;
            double sum = 0;
            foreach(Rating rate in Ratings)
            {
                sum += rate.Rate;
            }
            result = (sum / Ratings.Count)*1.0;
            return Math.Truncate(result*10)/10;
        }
    }
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }
        public string UserFullname { get; set; }
        [Range(1,10)]
        public int Rate { get; set; }
    }
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }
        public string UserFullname { get; set; }
        public string Content { get; set; }
    }
}
