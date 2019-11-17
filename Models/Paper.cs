using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MySql.Data.MySqlClient;

namespace SAM2020.Models
{
    public class Paper
    {
        public int id { get; set; }
        [Required]
        public string title { get; set; }
        public int author { get; set; }
        [Required]
        public string topic { get; set; }
        public string referenceName { get; set; }
        public string fileReference { get; set; }
        public int version { get; set; }
        [Required]
        public string coAuthors { get; set; }
        public DateTime submissionDate { get; set; }
        public int status { get; set; }
        public string authorName { get; set; }
        public List<string> fileList { get; set; }
        public string finalReview { get; set; }

    }
}
