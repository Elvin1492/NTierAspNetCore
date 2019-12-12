using StartApp.Core;
using FluentNHibernate.Mapping;

namespace StartApp.DataAccess
{
    public class EntityBaseMap<T> : ClassMap<T> where T : EntityBase
    {
        public EntityBaseMap()
        {
            Id(x => x.Id).Column("Id").GeneratedBy.Identity();
            Map(x => x.Deleted).Column("Deleted").CustomSqlType("BIT");

            Map(x => x.AddedDate).Column("AddedDate").CustomSqlType("DATETIME2(7)").Not.Nullable();
            Map(x => x.ModifiedDate).Column("ModifiedDate").CustomSqlType("DATETIME2(7)").Not.Nullable();
        }
    }
}
