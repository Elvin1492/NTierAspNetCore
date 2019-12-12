using StartApp.Core.Domain;
using StartApp.Repository.Cqrs;
using StartApp.Repository.Dtos;
using StartApp.Repository.Infrastructure;

namespace StartApp.Repository
{
    public interface IProductRepository:IRepository<Product>
    {
        ListResult<ProductDto> Get(int offset, int limit);
        ProductDto Get(int id);
        int Create(ProductDto thingDto);
    }

    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly IProductCommand _productCommand;
        private readonly IProductQuery _productQuery;

        public ProductRepository(IUnitOfWork unitOfWork, IProductCommand productCommand, IProductQuery productQuery) : base(unitOfWork)
        {
            _productCommand = productCommand;
            _productQuery = productQuery;
        }

        public ListResult<ProductDto> Get(int offset, int limit)
        {
            var result = _productQuery.Get(offset, limit);
            return result;
        }

        public ProductDto Get(int id)
        {
            var result = _productQuery.Get(id);
            return result;
        }

        public int Create(ProductDto directionDto)
        {
            var result = _productCommand.Execute(directionDto.Name, directionDto.CreatedDate, directionDto.ModifiedDate);
            return result;
        }
    }
}
