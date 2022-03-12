namespace IdeaManageApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("hr.Notification")]
    public partial class Notification
    {
        [Key]
        public int Notification_Id { get; set; }

        public int? User_Id { get; set; }

        [Column(TypeName = "text")]
        public string Notification_Content { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Notification_Date_Recieve { get; set; }

        public virtual User User { get; set; }
    }
}
