using System;
using Dapper;
using StartApp.Repository.Infrastructure;

namespace StartApp.Repository.Cqrs
{
    public interface IProductCommand
    {
        int Execute(string name, DateTime addedDate, DateTime modifiedDate);
        void Execute(int id, string name, DateTime addedDate, DateTime modifiedDate);
    }

    public class ProductCommand : IProductCommand
    {
        private const string CreateSql = @"
INSERT INTO [dbo].[Products]
           ([NAME],[AddedDate],[ModifiedDate],[Deleted])
     VALUES
           (@name, @AddedDate, @modifiedDate, 0)

SELECT SCOPE_IDENTITY()";

        private const string UpdateSql = @"
UPDATE dbo.Agreements
SET
     [Name] = @name,
     [ModifiedDate] = @modifiedDate,
     [AddedDate] = @AddedDate

WHERE Id = @id
";

        private readonly IUnitOfWork _unitOfWork;

        public ProductCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Execute(string name, DateTime addedDate, DateTime modifiedDate)
        {
            try
            {
                var parameters = new
                {
                    name,
                    modifiedDate,
                    AddedDate = addedDate
                };
                var result = _unitOfWork.Connection.QueryFirst<int>(CreateSql, parameters, _unitOfWork.Transaction);
                return result;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public void Execute(int id, string name, DateTime addedDate, DateTime modifiedDate)
        {
            try
            {
                var parameters = new
                {
                    id,
                    name,
                    modifiedDate,
                    AddedDate = addedDate
                };
                _unitOfWork.Connection.QueryFirst<int>(UpdateSql, parameters, _unitOfWork.Transaction);
            }
            catch (Exception e)
            {

            }
        }
    }
}