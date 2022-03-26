namespace IdeaManageApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("hr.Idea")]
    public partial class Idea
    {
        [Key]
        public int Idea_Id { get; set; }

        public int Category_Id { get; set; }

        [StringLength(50)]
        public string Idea_Title { get; set; }

        [Column(TypeName = "text")]
        public string Idea_Content { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Idea_Create_date { get; set; }

        [StringLength(1000)]
        public string Idea_File_path { get; set; }

        public int User_Id { get; set; }

        [Display(Name = "Terms and Conditions")]
        [MustBeTrue(ErrorMessage = "You have to agree with terms and condition")]
        public bool TermsAndCondition { get; set; }

        public virtual Category Category { get; set; }

        public virtual User User { get; set; }
    }

    public class MustBeTrueAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value is bool && (bool)value;
        }
    }
}
