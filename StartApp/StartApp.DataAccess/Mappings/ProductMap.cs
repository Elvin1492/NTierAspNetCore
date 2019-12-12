using StartApp.Core.Domain;

namespace StartApp.DataAccess.Mappings
{
    public class ProductMap : EntityBaseMap<Product>
    {
        public ProductMap()
        {
            Table("Products");
            Map(x => x.Name).Column("Name").CustomSqlType("NVARCHAR(50)").Not.Nullable();
        }
    }
}