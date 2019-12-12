using System;
using FluentNHibernate.Mapping;
using StartApp.Core.Domain.Identity;

namespace StartApp.DataAccess.Mappings.Identity
{
    public class AppUserMap : ClassMap<AppUser>
    {
        public AppUserMap()
        {
            Table("AspNetUsers");
            Id(x => x.Id).Column("Id").GeneratedBy.UuidHex("N").Length(36).CustomSqlType("NVARCHAR(36)").UnsavedValue(Guid.Empty);
           
            Map(x => x.AccessFailedCount).Column("AccessFailedCount").Not.Nullable().CustomSqlType("INT");

            Map(x => x.ConcurrencyStamp).Column("ConcurrencyStamp").CustomSqlType("NVARCHAR(36)");
      
            Map(x => x.Email).Column("Email").CustomSqlType("NVARCHAR(256)").Not.Nullable();
       
            Map(x => x.NormalizedEmail).Column("NormalizedEmail").CustomSqlType("NVARCHAR(256)").Not.Nullable();
      
            Map(x => x.EmailConfirmed).Column("EmailConfirmed").CustomSqlType("BIT").Not.Nullable();
          
            Map(x => x.LockoutEnabled).Column("LockoutEnabled").CustomSqlType("BIT").Not.Nullable();
       
            Map(x => x.LockoutEnd).Column("LockoutEnd").CustomSqlType("DateTimeOffset(7)");
           
            Map(x => x.PasswordHash).Column("PasswordHash").CustomSqlType("NVARCHAR(256)");
          
            Map(x => x.PhoneNumber).Column("PhoneNumber").CustomSqlType("NVARCHAR(128)");
         
            Map(x => x.PhoneNumberConfirmed).Column("PhoneNumberConfirmed").CustomSqlType("BIT");
           
            Map(x => x.TwoFactorEnabled).Column("TwoFactorEnabled").CustomSqlType("BIT");
            
            Map(x => x.UserName).Column("UserName").CustomSqlType("NVARCHAR(64)").Not.Nullable().Unique();
          
            Map(x => x.NormalizedUserName).Column("NormalizedUserName").CustomSqlType("NVARCHAR(64)").Not.Nullable().Unique();
           
            Map(x => x.SecurityStamp).Column("SecurityStamp").CustomSqlType("NVARCHAR(64)").Not.Nullable();
        }
    }
}