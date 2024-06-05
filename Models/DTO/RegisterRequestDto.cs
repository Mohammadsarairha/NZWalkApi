using System;
using System.ComponentModel.DataAnnotations;

namespace NZWalks.Models.DTO
{
    public class RegisterRequestDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string[] Roles { get; set; }
    }
}

