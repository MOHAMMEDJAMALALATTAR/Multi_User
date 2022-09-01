using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Multi_User.Models
{
    public class ApiUser:IdentityUser
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        [NotMapped]
        public IFormFile Logo { get; set; }
        public string LogoPath { get; set; }
        public DateTime CreationDate { get; set; }

    }
}
