using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace IdeaManageApp.Models
{
    public partial class IdeaModel : DbContext
    {
        public IdeaModel()
            : base("name=IdeasModel")
        {
        }

        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>()
                .Property(e => e.Department_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.Role_Name)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.User_Name)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Phone_Number)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);
        }
    }
}
