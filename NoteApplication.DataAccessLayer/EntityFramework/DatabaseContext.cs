using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteApplication.Entities;

namespace NoteApplication.DataAccessLayer.EntityFramework
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Liked> Likes { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<NoteUser> NoteUsers { get; set; }
        public DatabaseContext()
        {
            Database.SetInitializer(new InitializerDatabase());
        }
    }
}
