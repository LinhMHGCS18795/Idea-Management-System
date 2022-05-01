namespace IdeaManageApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("hr.Comment")]
    public partial class Comment
    {
        [Key]
        [Required]
        [Display(Name = "ID")]
        public int Comment_Id { get; set; }

        [Display(Name = "Content")]
        [Column(TypeName = "text")]
        public string Content { get; set; }

        [Display(Name = "Create date")]
        [Column(TypeName = "date")]
        public DateTime? Created_date { get; set; }

        [Display(Name = "User ID")]
        public int? User_Id { get; set; }

        [Display(Name = "Idea ID")]
        public int? Idea_Id { get; set; }

        public virtual Idea Idea { get; set; }

        public virtual User User { get; set; }
    }
}
