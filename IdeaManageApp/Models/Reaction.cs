namespace IdeaManageApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("hr.Reaction")]
    public partial class Reaction
    {
        [Key]
        public int Reaction_Id { get; set; }

        public bool? is_like { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Create_Date { get; set; }

        public int? User_Id { get; set; }

        public int? Idea_Id { get; set; }

        public virtual Idea Idea { get; set; }

        public virtual User User { get; set; }
    }
}
