using NHibernate;
using System.Data;

namespace StartApp.Repository.Infrastructure
{
    public interface IUnitOfWork
    {
        ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted);
        ISession Session { get; }
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
    }
}
