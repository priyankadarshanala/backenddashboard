﻿using System.ComponentModel.DataAnnotations;

namespace AngularAuthYtAPI.Models
{
    public class Applied
    {
        [Key]
       
        public int JobId { get; set; }
        public string? CompanyName { get; set; }
        public string? JobTitle { get; set; }
        public string? Experience { get; set; }
        public string? Skills { get; set; }
        public int Vacancy { get; set; }
        public string? JobType { get; set; }
        public string? Qualification { get; set; }
        public DateTime PostedDate { get; set; }

        public DateTime EndDate { get; set; }

        public float? Salary { get; set; }
        public string? Location { get; set; }
        public string? JobDescription { get; set; }
    }
}
