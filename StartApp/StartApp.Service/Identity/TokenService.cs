using StartApp.Core.Domain.Identity;
using StartApp.Repository.Identity;
using StartApp.Repository.Infrastructure;

namespace StartApp.Service.Identity
{
    public interface ITokenService
    {
        void Add(AppUserToken appUserToken);
        AppUserToken FindByKeys(string loginProvider, string refreshToken);
        void Remove(AppUserToken appUserToken);
        void RemoveByRefreshToken(string refreshToken);
    }

    public class TokenService : ITokenService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TokenService(IUnitOfWork unitOfWork, ITokenRepository tokenRepository)
        {
            _unitOfWork = unitOfWork;
            _tokenRepository = tokenRepository;
        }

        public void Add(AppUserToken appUserToken)
        {
            using var tran = _unitOfWork.BeginTransaction();
            _tokenRepository.Add(appUserToken);
            tran.Commit();
        }

        public AppUserToken FindByKeys(string loginProvider, string refreshToken)
        {
            using (_unitOfWork.BeginTransaction())
            {
                var result = _tokenRepository.FindByKeys(loginProvider, refreshToken);
                return result;
            }
        }

        public void Remove(AppUserToken appUserToken)
        {
            using (var tran = _unitOfWork.BeginTransaction())
            {
                _tokenRepository.Remove(appUserToken);
                tran.Commit();
            }
        }

        public void RemoveByRefreshToken(string refreshToken)
        {
            using (var tran = _unitOfWork.BeginTransaction())
            {
                _tokenRepository.RemoveByRefreshToken(refreshToken);
                tran.Commit();
            }
        }
    }
}