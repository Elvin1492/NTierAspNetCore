using System.Data;
using NHibernate;

namespace StartApp.Repository.Infrastructure
{
    public interface IDbContext
    {
        ISession Session { get; }
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
    }

    public class DbContext : IDbContext
    {
        public ISession Session { get; set; }
        public IDbConnection Connection { get; set; }
        public IDbTransaction Transaction { get; set; }
    }
}
