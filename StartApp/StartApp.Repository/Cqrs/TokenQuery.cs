using Dapper;
using StartApp.Core.Domain.Identity;
using StartApp.Repository.Infrastructure;

namespace StartApp.Repository.Cqrs
{
    public interface ITokenQuery
    {
        AppUserToken FindByKeys(string loginProvider, string refreshToken);
    }

    public class TokenQuery : ITokenQuery
    {
        private const string ByKeysSql = @"SELECT * FROM AspnetUserTokens WHERE LoginProvider = @loginProvider AND [Value] = @refreshToken";
        private readonly IUnitOfWork _unitOfWork;

        public TokenQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public AppUserToken FindByKeys(string loginProvider, string refreshToken)
        {
            var parameters = new
            {
                loginProvider,
                refreshToken
            };

            var result = _unitOfWork.Connection.QuerySingle<AppUserToken>(ByKeysSql, parameters, _unitOfWork.Transaction);
            return result;
        }
    }
}