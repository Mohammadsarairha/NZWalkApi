using System;
using NZWalks.Models.Domain;

namespace NZWalks.Models.DTO
{
    public class AddWalksRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double LenthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }
    }
}

