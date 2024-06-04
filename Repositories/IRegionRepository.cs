using System;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;

namespace NZWalks.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();
        Task<Region?> GetById(Guid id);
        Task Create(Region region);
        Task<Region?> Update(Guid id, Region region);
        Task<Region?> Delete(Guid id);
    }
}

