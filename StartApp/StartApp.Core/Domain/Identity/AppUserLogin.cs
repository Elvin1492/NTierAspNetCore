using NHibernate.AspNetCore.Identity;

namespace StartApp.Core.Domain.Identity
{
    public class AppUserLogin : IdentityUserLogin
    {
        public AppUserLogin()
        {

        }

        public virtual AppUser AppUser { get; set; }
    }
}