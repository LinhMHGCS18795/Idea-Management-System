using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace IdeaManageApp.Models
{
    public partial class IdeaModel : DbContext
    {
        public IdeaModel()
            : base("name=IdeaModel")
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Idea> Ideas { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Submission> Submissions { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .Property(e => e.Category_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .Property(e => e.Category_Description)
                .IsUnicode(false);

            modelBuilder.Entity<Department>()
                .Property(e => e.Department_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Idea>()
                .Property(e => e.Idea_Title)
                .IsUnicode(false);

            modelBuilder.Entity<Idea>()
                .Property(e => e.Idea_Description)
                .IsUnicode(false);

            modelBuilder.Entity<Idea>()
                .Property(e => e.Idea_Content)
                .IsUnicode(false);

            modelBuilder.Entity<Idea>()
                .Property(e => e.Idea_File_path)
                .IsUnicode(false);

            modelBuilder.Entity<Notification>()
                .Property(e => e.Notification_Content)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.Role_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Submission>()
                .Property(e => e.Submission_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Submission>()
                .Property(e => e.Submission_Description)
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

            modelBuilder.Entity<User>()
                .Property(e => e.Staff_Id)
                .IsUnicode(false);
        }
    }
}
