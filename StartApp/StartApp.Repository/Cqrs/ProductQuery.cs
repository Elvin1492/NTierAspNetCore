using System;
using Dapper;
using StartApp.Repository.Dtos;
using StartApp.Repository.Infrastructure;

namespace StartApp.Repository.Cqrs
{
    public interface IProductQuery
    {
        ListResult<ProductDto> Get(int offset, int limit);
        ProductDto Get(int id);
    }

    public class ProductQuery : IProductQuery
    {
        private const string All = @"
SELECT * FROM dbo.Products
ORDER BY Id 
OFFSET @offset ROWS 
FETCH NEXT @limit ROWS ONLY

SELECT COUNT(Id) FROM Products";

        private const string ByIdSql = @"SELECT * FROM dbo.Products WHERE Id=@id";

        private readonly IUnitOfWork _unitOfWork;

        public ProductQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ProductDto Get(int id)
        {
            try
            {
                var result = _unitOfWork.Connection.QuerySingle<ProductDto>(ByIdSql, new { id }, _unitOfWork.Transaction);
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public ListResult<ProductDto> Get(int offset, int limit)
        {
            try
            {
                var grid = _unitOfWork.Connection.QueryMultiple(All, new { offset, limit }, _unitOfWork.Transaction);

                var result = new ListResult<ProductDto>
                {
                    List = grid.Read<ProductDto>(),
                    TotalCount = grid.ReadFirst<int>()
                };

                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}