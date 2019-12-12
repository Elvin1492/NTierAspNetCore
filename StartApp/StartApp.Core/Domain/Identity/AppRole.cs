using NHIdentityRole = NHibernate.AspNetCore.Identity.IdentityRole;

namespace StartApp.Core.Domain.Identity
{

    public class AppRole : NHIdentityRole
    {
        public AppRole()
        {
            
        }
        public AppRole(string roleName)
        {
            Name = roleName;
        }
        public virtual string Description { get; set; }

    }
}
