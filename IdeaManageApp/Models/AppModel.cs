using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace IdeaManageApp.Models
{
    public partial class AppModel : DbContext
    {
        public AppModel()
            : base("name=AppModel")
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Idea> Ideas { get; set; }
        public virtual DbSet<Reaction> Reactions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Submission> Submissions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<View> Views { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .Property(e => e.Category_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .Property(e => e.Category_Description)
                .IsUnicode(false);

            modelBuilder.Entity<Comment>()
                .Property(e => e.Content)
                .IsUnicode(false);

            modelBuilder.Entity<Department>()
                .Property(e => e.Department_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Idea>()
                .Property(e => e.Idea_Title)
                .IsUnicode(false);

            modelBuilder.Entity<Idea>()
                .Property(e => e.Idea_Content)
                .IsUnicode(false);

            modelBuilder.Entity<Idea>()
                .Property(e => e.Idea_File_path)
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
