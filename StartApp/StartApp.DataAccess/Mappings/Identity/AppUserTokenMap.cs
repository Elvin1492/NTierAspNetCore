using FluentNHibernate.Mapping;
using StartApp.Core.Domain.Identity;

namespace StartApp.DataAccess.Mappings.Identity
{
    public class AppUserTokenMap : ClassMap<AppUserToken>
    {
        public AppUserTokenMap()
        {
            Table("AspNetUserTokens");

            CompositeId().KeyProperty(x => x.UserId, "UserId")
                .KeyProperty(x => x.LoginProvider, "LoginProvider")
                .KeyProperty(x => x.Name, "Name");
       
            Map(x => x.Value).Column("Value").CustomSqlType("NVARCHAR(256)");
        
            Map(x => x.AddedDate).Column("AddedDate").CustomSqlType("DATETIME2(7)").Not.Nullable();
     
            Map(x => x.Type).Column("Type").CustomSqlType("INT").Not.Nullable();
        }
    }
}