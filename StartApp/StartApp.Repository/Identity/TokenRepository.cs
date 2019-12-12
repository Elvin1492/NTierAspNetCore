using System;
using StartApp.Core.Domain.Identity;
using StartApp.Repository.Cqrs;

namespace StartApp.Repository.Identity
{
    public interface ITokenRepository
    {
        void Add(AppUserToken appUserToken);
        AppUserToken FindByKeys(string loginProvider, string refreshToken);
        void Remove(AppUserToken appUserToken);
        void RemoveByRefreshToken(string refreshToken);
    }

    public class TokenRepository : ITokenRepository
    {
        private readonly ITokenCommand _tokenCommand;
        private readonly ITokenQuery _tokenQuery;

        public TokenRepository(ITokenCommand tokenCommand, ITokenQuery tokenQuery)
        {
            _tokenCommand = tokenCommand;
            _tokenQuery = tokenQuery;
        }

        public void Add(AppUserToken appUserToken)
        {
            _tokenCommand.Execute(appUserToken.UserId, appUserToken.LoginProvider, appUserToken.Name, appUserToken.Value, DateTime.Now, appUserToken.Type);
        }

        public AppUserToken FindByKeys(string loginProvider, string refreshToken)
        {
            var result = _tokenQuery.FindByKeys(loginProvider, refreshToken);
            return result;
        }

        public void Remove(AppUserToken appUserToken)
        {
            _tokenCommand.Remove(appUserToken.UserId, appUserToken.LoginProvider, appUserToken.Name);
        }

        public void RemoveByRefreshToken(string refreshToken)
        {
            _tokenCommand.RemoveByRefreshToken(refreshToken);
        }
    }
}