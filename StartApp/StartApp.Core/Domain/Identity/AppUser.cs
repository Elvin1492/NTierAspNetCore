using System;
using NHIdentityUser = NHibernate.AspNetCore.Identity.IdentityUser;

namespace StartApp.Core.Domain.Identity
{
    public class AppUser: NHIdentityUser
    {
        public virtual DateTime CreatedDate { get; set; }

        public virtual DateTime LastModifiedDate { get; set; }

        public virtual string DisplayName { get; set; }

        public virtual DateTime? LastLogin { get; set; }

        public virtual int LoginCount { get; set; }

        public virtual int Token { get; set; }
    }
}
