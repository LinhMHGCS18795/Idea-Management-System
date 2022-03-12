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
        [Required]
        [Display(Name = "Idea ID")]
        public int Idea_Id { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Title")]
        public string Idea_Title { get; set; }

        [Required]
        [Display(Name = "Description")]
        [Column(TypeName = "text")]
        public string Idea_Description { get; set; }

        [Required]
        [Display(Name = "Content")]
        [Column(TypeName = "text")]
        public string Idea_Content { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Idea_Create_date { get; set; }

        [StringLength(100)]
        public string Idea_File_path { get; set; }

        public int? User_Id { get; set; }

        public int? Category_Id { get; set; }

        public int? Submission_Id { get; set; }

        public bool? Idea_IsDisable { get; set; }

        public virtual Category Category { get; set; }

        public virtual Submission Submission { get; set; }

        public virtual User User { get; set; }
    }
}
