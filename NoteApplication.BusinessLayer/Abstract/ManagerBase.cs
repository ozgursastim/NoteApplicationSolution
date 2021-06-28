using NoteApplication.Core.DataAccess;
using NoteApplication.DataAccessLayer.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NoteApplication.BusinessLayer.Abstract
{
    public abstract class ManagerBase<T> : IDataAccess<T> where T : class
    {
        private Repository<T> repository = new Repository<T>();

        public virtual int Delete(T obj)
        {
            return repository.Delete(obj);
        }

        public virtual T Find(Expression<Func<T, bool>> where)
        {
            return repository.Find(where);
        }

        public virtual int Insert(T obj)
        {
            return repository.Insert(obj);
        }

        public virtual List<T> List()
        {
            return repository.List();
        }

        public virtual List<T> List(Expression<Func<T, bool>> where)
        {
            return repository.List(where);
        }

        public virtual IQueryable<T> ListIQueryable()
        {
            return repository.ListIQueryable();
        }

        public virtual int Save()
        {
            return repository.Save();
        }

        public virtual int Update(T obj)
        {
            return repository.Update(obj);
        }
    }
}
