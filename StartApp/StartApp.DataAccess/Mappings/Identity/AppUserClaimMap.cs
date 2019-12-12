using FluentNHibernate.Mapping;
using StartApp.Core.Domain.Identity;

namespace StartApp.DataAccess.Mappings.Identity
{
    public class AppUserClaimMap : ClassMap<AppUserClaim>
    {
        public AppUserClaimMap()
        {
            Table("AspNetUserClaims");
            Id(x => x.Id).Column("Id").GeneratedBy.Identity();
            Map(x => x.ClaimType).Column("ClaimType").CustomSqlType("NVARCHAR(1024)").Not.Nullable();
            Map(x => x.ClaimValue).Column("ClaimValue").CustomSqlType("NVARCHAR(1024)").Not.Nullable();
            Map(x => x.UserId).Column("UserId").CustomSqlType("NVARCHAR(36)").Not.Nullable();
        }
    }
}