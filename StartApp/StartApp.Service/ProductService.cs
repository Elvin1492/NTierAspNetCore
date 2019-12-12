using System;
using StartApp.Core.Domain;
using StartApp.Repository;
using StartApp.Repository.Dtos;
using StartApp.Repository.Infrastructure;
using StartApp.Service.Infrastructure;

namespace StartApp.Service
{
    public interface IProductService : IServiceBase<Product>
    {
        ListResult<ProductDto> Get(int offset, int limit);
        ProductDto Get(int id);

        ProductDto Create(ProductDto productDto);
    }

    public class ProductService : ServiceBase<Product>, IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Repository = productRepository;
            _productRepository = productRepository;
        }

        public ProductDto Create(ProductDto productDto)
        {
            using (var tran = _unitOfWork.BeginTransaction())
            {
                try
                {
                    productDto.CreatedDate = productDto.ModifiedDate = DateTime.UtcNow;
                    var id = _productRepository.Create(productDto);
                    var result = _productRepository.Get(id);
                    tran.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    tran.Dispose();
                    throw;
                }

            }
        }

        public ListResult<ProductDto> Get(int offset, int limit)
        {
            using (_unitOfWork.BeginTransaction())
            {
                var result = _productRepository.Get(offset, limit);
                return result;
            }
        }

        public ProductDto Get(int id)
        {
            using (_unitOfWork.BeginTransaction())
            {
                var result = _productRepository.Get(id);
                return result;
            }
        }
    }
}
