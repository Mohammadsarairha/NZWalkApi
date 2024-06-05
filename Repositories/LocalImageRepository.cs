using System;
using NZWalks.Data;
using NZWalks.Models.Domain;

namespace NZWalks.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHost;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly NZWalksDbContext dbContext;

        public LocalImageRepository(IWebHostEnvironment webHost, IHttpContextAccessor httpContextAccessor, NZWalksDbContext dbContext)
        {
            this.webHost = webHost;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }
        // Windows OS 
        //public async Task<Image> Upload(Image image)
        //{
        //    var localFilePath = Path.Combine(webHost.ContentRootPath, "Images",
        //        $"{image.FileName}{image.FileExtension}");

        //    using var stream = new FileStream(localFilePath, FileMode.Create);
        //    await image.File.CopyToAsync(stream);

        //    var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.Path}/Images/{image.FileName}{image.FileExtension}";

        //    image.FilePath = urlFilePath;

        //    await dbContext.Images.AddAsync(image);
        //    await dbContext.SaveChangesAsync();

        //    return image;
        //}

        //Mac OS
        public async Task<Image> Upload(Image image)
        {
            // Define the directory and file paths
            var imagesDirectory = Path.Combine(webHost.ContentRootPath, "Images");
            var localFilePath = Path.Combine(imagesDirectory, $"{image.FileName}{image.FileExtension}");

            // Ensure the Images directory exists
            if (!Directory.Exists(imagesDirectory))
            {
                Directory.CreateDirectory(imagesDirectory);
            }

            // Create the file in the Images directory
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            // Generate the URL for the saved file
            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";

            // Update the Image object with the file path URL
            image.FilePath = urlFilePath;

            // Save the Image object to the database
            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();

            return image;
        }
    }
}

