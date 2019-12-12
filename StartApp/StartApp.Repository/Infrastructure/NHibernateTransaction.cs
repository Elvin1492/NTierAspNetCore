using System;
using System.Data;
using NHibernate;

namespace StartApp.Repository.Infrastructure
{
    public class NHibernateTransaction : ITransaction
    {
        private readonly NHibernate.ITransaction _transaction;
        private readonly ISession _session;

        public NHibernateTransaction(ISession session, IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted)
        {
            this._session = session;

            _transaction = session.BeginTransaction(isolationLevel);
        }

        protected virtual void Dispose(bool disposeManaged)
        {
            if (disposeManaged)
            {
                _transaction.Dispose();
                _session.Dispose();
            }
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
