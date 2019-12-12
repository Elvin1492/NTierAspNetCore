using FluentNHibernate.Mapping;
using StartApp.Core.Domain.Identity;

namespace StartApp.DataAccess.Mappings.Identity
{
    public class AppUserRoleMap : ClassMap<AppUserRole>
    {
        public AppUserRoleMap()
        {
            Table("AspNetUserRoles");

            CompositeId().KeyProperty(x => x.UserId, "UserId")
                .KeyProperty(x => x.RoleId, "LoginProvider");
        }
    }
}