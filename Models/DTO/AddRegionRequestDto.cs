using System;
using System.ComponentModel.DataAnnotations;

namespace NZWalks.Models.DTO
{
    public class AddRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code hase minimum of 3 characters")]
        [MaxLength(3, ErrorMessage = "Code hase maximum of 3 characters")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Name hase maximum of 100 characters")]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}

