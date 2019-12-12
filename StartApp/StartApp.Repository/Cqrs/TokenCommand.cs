using System;
using Dapper;
using StartApp.Repository.Infrastructure;

namespace StartApp.Repository.Cqrs
{
    public interface ITokenCommand
    {
        void Execute(string userId, string loginProvider, string name, string value, DateTime addedDate, int type);
        void Remove(string userId, string loginProvider, string name);
        void RemoveByRefreshToken(string refreshToken);
    }

    public class TokenCommand : ITokenCommand
    {
        private const string CreateSql = @"
INSERT INTO dbo.AspNetUserTokens
VALUES
(   @userId,
    @loginProvider,
    @name,
    @value,
    @addedDate, 
    @type 
)";

        private const string RemoveSql = @"DELETE FROM dbo.AspNetUserTokens WHERE UserId=@userId AND loginProvider = @loginProvider AND [Name]=@name";

        private const string RemoveByValue = @"DELETE FROM dbo.AspNetUserTokens WHERE Value = @refreshToken";

        private readonly IUnitOfWork _unitOfWork;

        public TokenCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Execute(string userId, string loginProvider, string name, string value, DateTime addedDate,
            int type)
        {
            SqlMapper.AddTypeMap(typeof(DateTime), System.Data.DbType.DateTime2);
            var parameters = new
            {
                userId,
                loginProvider,
                name,
                value,
                addedDate,
                type
            };

            _unitOfWork.Connection.Execute(CreateSql, parameters, _unitOfWork.Transaction);
        }

        public void Remove(string userId, string loginProvider, string name)
        {
            var parameters = new
            {
                userId,
                loginProvider,
                name
            };
            _unitOfWork.Connection.Execute(RemoveSql, parameters, _unitOfWork.Transaction);
        }

        public void RemoveByRefreshToken(string refreshToken)
        {
            var parameters = new
            {
                refreshToken
            };
            _unitOfWork.Connection.Execute(RemoveByValue, parameters, _unitOfWork.Transaction);
        }
    }
}