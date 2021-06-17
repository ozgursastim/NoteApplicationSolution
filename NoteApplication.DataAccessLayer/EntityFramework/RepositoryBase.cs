using NoteApplication.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteApplication.DataAccessLayer.EntityFramework
{
    public class RepositoryBase
    {
        protected static DatabaseContext _databaseContext;
        private static object _lockSync = new object();

        protected RepositoryBase()
        {
            CreateContext();
        }

        private static void CreateContext()
        {
            if (_databaseContext == null)
            {
                lock (_lockSync)
                {
                    if (_databaseContext == null)
                        _databaseContext = new DatabaseContext();

                }
            }
        }
    }
}
