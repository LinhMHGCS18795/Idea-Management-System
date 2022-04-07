namespace IdeaManageApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("hr.View")]
    public partial class View
    {
        [Key]
        public int View_Id { get; set; }

        public int? User_Id { get; set; }

        public int? Idea_Id { get; set; }

        public virtual Idea Idea { get; set; }

        public virtual User User { get; set; }
    }
}
