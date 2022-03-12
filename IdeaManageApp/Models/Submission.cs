namespace IdeaManageApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("hr.Submission")]
    public partial class Submission
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Submission()
        {
            Ideas = new HashSet<Idea>();
        }

        [Key]
        public int Submission_Id { get; set; }

        [StringLength(50)]
        public string Submission_Name { get; set; }

        [Column(TypeName = "text")]
        public string Submission_Description { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Submission_Closure_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Submission_Final_closure_date { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Idea> Ideas { get; set; }
    }
}
