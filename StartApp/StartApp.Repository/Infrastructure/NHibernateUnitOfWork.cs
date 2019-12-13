using System;
using System.Data;
using NHibernate;

namespace StartApp.Repository.Infrastructure
{
    public class NHibernateUnitOfWork : IUnitOfWork, IDisposable
    {
        private ISession _currentSession;
        private readonly ISessionFactory _sessionFactory;

        public NHibernateUnitOfWork(ISessionFactory sessionFactoryProvider)
        {
            this._sessionFactory = sessionFactoryProvider;
        }

        public ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted)
        {
            if (_currentSession == null || !_currentSession.IsOpen)
            {
                _currentSession = _sessionFactory
                    .OpenSession();
            }

            return new NHibernateTransaction(_currentSession, isolationLevel);
        }

        public IDbConnection Connection => _currentSession.Connection;

        public ISession Session => _currentSession;

        public IDbTransaction Transaction
        {
            get
            {
                using (var command = _currentSession.Connection.CreateCommand())
                {
                    _currentSession.Transaction.Enlist(command);
                    return command.Transaction;
                }
            }
        }

        public void Dispose()
        {
            _currentSession?.Dispose();
            _sessionFactory?.Dispose();
            GC.SuppressFinalize(this);
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if(_disposed) return;

            if (disposing)
            {
                GC.SuppressFinalize(this);
            }

            _disposed = true;
        }

        ~NHibernateUnitOfWork()
        {
            Dispose(false);
        }
    }
}
