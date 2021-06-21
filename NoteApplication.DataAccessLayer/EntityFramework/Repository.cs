using NoteApplication.Common;
using NoteApplication.DataAccessLayer;
using NoteApplication.DataAccessLayer.Abstract;
using NoteApplication.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NoteApplication.DataAccessLayer.EntityFramework
{
    public class Repository<T> : RepositoryBase, IRepository<T> where T:class
    {
        private DbSet<T> _objectSet;

        public Repository()
        {
            _objectSet = _databaseContext.Set<T>();
        
        }

        public List<T> List()
        {
            return _objectSet.ToList();
        }

        public List<T> List(Expression<Func<T, bool>> where)
        {
            return _objectSet.Where(where).ToList();
        }

        public T Find(Expression<Func<T, bool>> where)
        {
            return _objectSet.FirstOrDefault(where);
        }

        public int Insert(T obj)
        {
            _objectSet.Add(obj);

            // if the obj is EntityBase, assigned default value the fields.
            if (obj is EntityBase)
            {
                EntityBase entityBase = obj as EntityBase;
                DateTime now = DateTime.Now;
                entityBase.CreatedDate = now;
                entityBase.ModifiedDate = now;
                entityBase.ModifiedUsername = App.Common.GetCurrentUsername();

            }
            return Save();
        }

        public int Update(T obj)
        {
            // if the obj is EntityBase, assigned default value the fields.
            if (obj is EntityBase)
            {
                EntityBase entityBase = obj as EntityBase;
                entityBase.ModifiedDate = DateTime.Now;
                entityBase.ModifiedUsername = App.Common.GetCurrentUsername();

            }
            return Save();
        }

        public int Delete(T obj)
        {
            _objectSet.Remove(obj);
            return Save();
        }

        public int Save()
        {
            return _databaseContext.SaveChanges();
        }
    }
}
