using FluentNHibernate.Mapping;
using StartApp.Core.Domain.Identity;

namespace StartApp.DataAccess.Mappings.Identity
{
    public class AppUserLoginMap : ClassMap<AppUserLogin>
    {
        public AppUserLoginMap()
        {
            Table("AspNetUserLogins");

            CompositeId()
                .KeyProperty(x => x.LoginProvider, "LoginProvider")
                .KeyProperty(x => x.ProviderKey, "Name");

            Map(x => x.ProviderDisplayName).Column("ProviderDisplayName").CustomSqlType("NVARCHAR(32)");

            References(x => x.AppUser).Column("UserId").Not.Nullable();
        }
    }
}