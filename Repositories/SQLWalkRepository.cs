using System;
using Microsoft.EntityFrameworkCore;
using NZWalks.Data;
using NZWalks.Models.Domain;

namespace NZWalks.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();

            return walk;
        }


        public async Task<List<Walk>> GetAllAsync()
        {
            //type-safe. The compiler checks the property names, so if you make a typo, it will be caught at compile time.
            //return await dbContext.Walks.Include(w => w.Difficulty).Include(w => w.Region).ToListAsync();

            //the error will only be caught at runtime, and it may lead to unexpected behavior.
            return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walkUpdate)
        {
            var walk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (walk == null)
                return null;

            walk.Name = walkUpdate.Name;
            walk.Description = walkUpdate.Description;
            walk.LenthInKm = walkUpdate.LenthInKm;
            walk.WalkImageUrl = walkUpdate.WalkImageUrl;
            walk.DifficultyId = walkUpdate.DifficultyId;
            walk.RegionId = walkUpdate.RegionId;

            await dbContext.SaveChangesAsync();

            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var walk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (walk == null)
                return null;

            dbContext.Walks.Remove(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }
    }
}

