using Microsoft.EntityFrameworkCore;
using Web_Product.interfaces;
using Web_Product.Models;

namespace Web_Product.Services
{
    public class ProductService : IProductService
    {
        public readonly DatabaseContext db;
        private readonly IUploadFileService us;

        public ProductService(DatabaseContext databaseContext , IUploadFileService uploadFileService)
        {
            db = databaseContext;
            us = uploadFileService;
        }

        public async Task Create(Product product)
        {
            db.Products.AddAsync(product);
            await db.SaveChangesAsync();
        }

        public async Task Delete(Product product)
        {
            db.Remove(product);
            await db.SaveChangesAsync();
        }

        public async Task DeleteImage(string fileName)
        {
           await us.DeleteImage(fileName);
        }

        public async Task<IEnumerable<Product>> FindAll()
        {
            var data = await db.Products.Include(e => e.Category).ToListAsync(); 
            return data;
        }

        public async Task<Product> FindById(int id)

        {                                                        // SingleOrDefaultAsync(x => x.Id.Equals(id))
            var result = await db.Products.Include(e => e.Category).FirstAsync(x => x.Id.Equals(id));
            return result;
        }

        public async Task<IEnumerable<Product>> Search(string name)
        {
           var data = await db.Products.Where(p => p.Name.Contains(name)).Include(e => e.Category).ToListAsync();
            return data;
        }

        public async Task Update(Product product)
        {
             db.Products.Update(product);
            await db.SaveChangesAsync();
        }
                                                //List<string> imageName
        public async Task<(string errorMessage, string imageName)> UploadImage(IFormFileCollection formFiles)
        {
            var errorMessage= string.Empty;
            //var imageName = new List<string>();
            var imageName = string.Empty;
            if (us.IsUpload(formFiles))
            {
                errorMessage = us.Validation(formFiles);
                if (string.IsNullOrEmpty(errorMessage))
                {
                  imageName = (await us.UploadImages(formFiles))[0];
                }
            }
            return (errorMessage, imageName);
        }
    }
}
