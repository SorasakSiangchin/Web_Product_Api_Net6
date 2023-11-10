using Web_Product.Models;

namespace Web_Product.interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> FindAll();
        Task<Product> FindById(int id);
        Task Create (Product product);
        Task Update (Product product);
        Task Delete (Product product);
        Task<IEnumerable<Product>> Search(string name);
        Task<(string errorMessage, string imageName)> UploadImage(IFormFileCollection formFiles);
        Task DeleteImage (string fileName);   
    }
}
