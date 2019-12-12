using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace StartApp.Repository.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> FindAll();
        IList<T> FindWhere(Expression<Func<T, bool>> predicate);
        T FindWhereSingle(Expression<Func<T, bool>> predicate);
        T FindById(int id);
        T Add(T newEntity);
        void Remove(T entity);
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        public readonly IUnitOfWork UnitOfWork;

        public Repository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public T Add(T newEntity)
        {
            using (var tran = UnitOfWork.BeginTransaction())
            {
                try
                {
                    var id = UnitOfWork.Session.Save(newEntity);
                    tran.Commit();
                    var result = UnitOfWork.Session.Load<T>(id);
                    return result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }
        }

        public IQueryable<T> FindAll()
        {
            using (UnitOfWork.BeginTransaction())
            {
                return UnitOfWork.Session.Query<T>();
            }
        }

        public T FindById(int id)
        {
            using (UnitOfWork.BeginTransaction())
            {
                return UnitOfWork.Session.Get<T>(id);
            }
        }

        public IList<T> FindWhere(Expression<Func<T, bool>> predicate)
        {
            using (UnitOfWork.BeginTransaction())
            {
                var result = UnitOfWork.Session.QueryOver<T>().Where(predicate).List();
                return result;
            }
        }

        public T FindWhereSingle(Expression<Func<T, bool>> predicate)
        {
            using (UnitOfWork.BeginTransaction())
            {
                var result = UnitOfWork.Session.QueryOver<T>().Where(predicate).SingleOrDefault();
                return result;
            }
        }

        public void Remove(T entity)
        {
            using (UnitOfWork.BeginTransaction())
            {
                UnitOfWork.Session.Delete(entity);
            }
        }
    }
}