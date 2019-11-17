using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MySql.Data.MySqlClient;

namespace SAM2020.Models
{
    public class Review
    {
        public int id { get; set; }
        public string paperReferenceName { get; set; }
        public string reviewerName { get; set; }
        public string reviewerEmail { get; set; }
        public string text { get; set; }
        public DateTime editedDate { get; set; }

    }
}
