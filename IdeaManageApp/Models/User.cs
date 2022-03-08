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
        [Key]
        [Display(Name = "User ID")]
        public int User_Id { get; set; }

        [Display(Name = "User Name")]
        [StringLength(50)]
        public string User_Name { get; set; }

        [Display(Name = "Date Of Birth")]
        [Column(TypeName = "date")]
        public DateTime? Date_of_birth { get; set; }

        [Display(Name = "Phone number")]
        [StringLength(50)]
        public string Phone_Number { get; set; }

        [Display(Name = "Email")]
        [StringLength(50)]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [StringLength(5)]
        public string Password { get; set; }

        [Display(Name = "Staff ID")]
        public int? Staff_Id { get; set; }

        [Display(Name = "Department ID")]
        public int? Department_Id { get; set; }

        [Display(Name = "Role ID")]
        public int? Role_Id { get; set; }

        public virtual Department Department { get; set; }

        public virtual Role Role { get; set; }
    }
}
