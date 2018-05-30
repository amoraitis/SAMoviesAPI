using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SAMoviesAPI.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Avatar_hash { get; set; }
        public string Favorites { get; set; }
        public string Seens { get; set; }
    }
}
