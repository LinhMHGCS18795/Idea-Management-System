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
            Categories = new HashSet<Category>();
        }

        [Key]
        [Required]
        [Display(Name = "ID")]
        public int Submission_Id { get; set; }

        [Display(Name = "Name")]
        [StringLength(50)]
        public string Submission_Name { get; set; }

        [Display(Name = "Description")]
        [Column(TypeName = "text")]
        public string Submission_Description { get; set; }

        [Display(Name = "Closure Date")]
        [Column(TypeName = "date")]
        public DateTime? Submission_Closure_date { get; set; }

        [Display(Name = "Final Closure Date")]
        [Column(TypeName = "date")]
        public DateTime? Submission_Final_closure_date { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Category> Categories { get; set; }
    }
}
