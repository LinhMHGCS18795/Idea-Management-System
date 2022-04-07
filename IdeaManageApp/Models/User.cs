namespace IdeaManageApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("hr.User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Comments = new HashSet<Comment>();
            Ideas = new HashSet<Idea>();
            Reactions = new HashSet<Reaction>();
            Views = new HashSet<View>();
        }

        [Key]
        public int User_Id { get; set; }

        [StringLength(50)]
        public string User_Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_of_birth { get; set; }

        [StringLength(50)]
        public string Phone_Number { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(5)]
        public string Password { get; set; }

        [StringLength(50)]
        public string Staff_Id { get; set; }

        public int? Department_Id { get; set; }

        public int? Role_Id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        public virtual Department Department { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Idea> Ideas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reaction> Reactions { get; set; }

        public virtual Role Role { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<View> Views { get; set; }
    }
}
