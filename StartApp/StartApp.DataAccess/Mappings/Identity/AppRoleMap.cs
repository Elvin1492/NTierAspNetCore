using System;
using FluentNHibernate.Mapping;
using StartApp.Core.Domain.Identity;

namespace StartApp.DataAccess.Mappings.Identity
{
    public class AppRoleMap : ClassMap<AppRole>
    {
        public AppRoleMap()
        {
            Table("AspNetRoles");
            Id(x => x.Id).Column("Id").GeneratedBy.UuidHex("N").Length(36).CustomSqlType("NVARCHAR(36)").UnsavedValue(Guid.Empty);
            Map(x => x.Name).Column("Name").CustomSqlType("NVARCHAR(64)").Not.Nullable();
            Map(x => x.NormalizedName).Column("NormalizedName").CustomSqlType("NVARCHAR(64)").Not.Nullable();
            Map(x => x.ConcurrencyStamp).Column("ConcurrencyStamp").CustomSqlType("NVARCHAR(36)").Not.Nullable();
        }
    }
}