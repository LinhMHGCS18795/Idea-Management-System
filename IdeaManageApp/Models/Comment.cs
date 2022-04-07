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
        public int Comment_Id { get; set; }

        [Column(TypeName = "text")]
        public string Content { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Created_date { get; set; }

        public int? User_Id { get; set; }

        public int? Idea_Id { get; set; }

        public virtual Idea Idea { get; set; }

        public virtual User User { get; set; }
    }
}
