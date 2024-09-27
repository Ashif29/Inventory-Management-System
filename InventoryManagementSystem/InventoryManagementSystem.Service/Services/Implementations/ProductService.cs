using ImageMagick;
using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Data.Entities.NotMapped;
using InventoryManagementSystem.Data.Repositories.Core;
using InventoryManagementSystem.Service.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Linq.Expressions;

namespace InventoryManagementSystem.Service.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<bool> AddAsync(Product product, IFormFile? imageFile)
        {
            if (imageFile is not null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\ProductImage");

                CreateDirectoryIfNotExists(imagePath);

                string imageUrl = ResizeAndSaveImage(imageFile, imagePath, fileName);

                product.ImageUrl = imageUrl;
            }
            else
            {
                product.ImageUrl = "https://placehold.co/600x400";
            }

            await _unitOfWork.ProductRepository.AddAsync(product);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(e => e.Id == id);

            if (product == null)
            {
                return false;
            }

            DeleteFile(product.ImageUrl);

            await _unitOfWork.ProductRepository.DeleteAsync(product);

            return await _unitOfWork.CompleteAsync();
        }

        public async Task<ProductsData> GetAllAsync(ProductQueryParameters? queryParameters, int pageNumber, int pageSize, string? includeProperties = null)
        {
            var filter = ApplySearching(queryParameters);

            // Retrieve filtered products
            var products = await _unitOfWork.ProductRepository.GetAllAsync(filter, includeProperties);

            // Apply sorting
            products = ApplySorting(products, queryParameters);

            // Handle pagination
            var pagedProducts = await PaginatedList<Product>.CreateAsync(products, pageNumber, pageSize);

            return new ProductsData
            {
                products = products,
                pagedProducts = pagedProducts
            };
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _unitOfWork.ProductRepository.GetAllAsync();
        }

        public async Task<Product> GetByIdAsync(Expression<Func<Product, bool>> filter, string? includeProperties = null)
        {
            return await _unitOfWork.ProductRepository.GetByIdAsync(filter, includeProperties);
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            if (product.Image != null)
            {
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    DeleteFile(product.ImageUrl);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(product.Image.FileName);
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\Product");

                CreateDirectoryIfNotExists(imagePath);

                string imageUrl = ResizeAndSaveImage(product.Image, imagePath, fileName);

                product.ImageUrl = imageUrl;
            }
            else
            {
                product.ImageUrl = product.ImageUrl;
            }

            await _unitOfWork.ProductRepository.UpdateAsync(product);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> IsExistsAsync(Expression<Func<Product, bool>> filter)
        {
            return await _unitOfWork.ProductRepository.IsExistsAsync(filter);
        }

        private void DeleteFile(string? filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, filePath.TrimStart('\\'));

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
        }

        private string ResizeAndSaveImage(IFormFile imageFile, string imagePath, string fileName)
        {
            using var memoryStream = new MemoryStream();
            imageFile.CopyTo(memoryStream);
            memoryStream.Position = 0;

            using (var image = new MagickImage(memoryStream.ToArray()))
            {
                image.Resize(300, 300);
                image.Quality = 75;

                string fullPath = Path.Combine(imagePath, fileName);
                image.Write(fullPath);

                return @"\images\Product\" + fileName;
            }
        }

        private void CreateDirectoryIfNotExists(string imagePath)
        {
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }
        }

        private Expression<Func<Product, bool>> ApplySearching(ProductQueryParameters? queryParameters)
        {
            return e =>
                (string.IsNullOrEmpty(queryParameters.Name) ||
                 (e.Name).Contains(queryParameters.Name));
        }

        private IQueryable<Product> ApplySorting(IQueryable<Product> products, ProductQueryParameters queryParameters)
        {
            switch (queryParameters.SortColumn)
            {
                case "name":
                    products = queryParameters.SortOrder == "dsc"
                        ? products.OrderByDescending(e => e.Name)
                        : products.OrderBy(e => e.Name);
                    break;
                default:
                    break;
            }

            return products;
        }
    }

}
