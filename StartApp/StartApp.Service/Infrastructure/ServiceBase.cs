using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using StartApp.Repository.Infrastructure;

namespace StartApp.Service.Infrastructure
{
    public interface IServiceBase<T> where T : class
    {
        IQueryable<T> FindAll();
        IList<T> FindWhere(Expression<Func<T, bool>> predicate);
        T FindWhereSingle(Expression<Func<T, bool>> predicate);
        T FindById(int id);
        T Add(T newEntity);
        void Remove(T entity);
    }

    public class ServiceBase<T>: IServiceBase<T> where T : class
    {
        public IRepository<T> Repository;

        public  IQueryable<T> FindAll()
        {
            var result = Repository.FindAll();
            return result;
        }

        public IList<T> FindWhere(Expression<Func<T, bool>> predicate)
        {
            var result = Repository.FindWhere(predicate);

            return result;
        }
        
        public T FindWhereSingle(Expression<Func<T, bool>> predicate)
        {
            var result = Repository.FindWhereSingle(predicate);

            return result;
        }

        public T FindById(int id)
        {
            var result = Repository.FindById(id);
            return result;
        }

        public T Add(T newEntity)
        {
            var result = Repository.Add(newEntity);
            return result;
        }

        public void Remove(T entity)
        {
            Repository.Remove(entity);
        }
    }
}
