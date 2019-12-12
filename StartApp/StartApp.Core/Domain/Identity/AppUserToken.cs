using System;
using NHibernate.AspNetCore.Identity;

namespace StartApp.Core.Domain.Identity
{
    public class AppUserToken : IdentityUserToken
    {
        public AppUserToken()
        {

        }

        public virtual int Id { get; set; }
        public virtual DateTime AddedDate { get; set; }
        public virtual int Type { get; set; }

        public virtual AppUser User { get; set; }
    }
}