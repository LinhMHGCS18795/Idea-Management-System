namespace IdeaManageApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("hr.Category")]
    public partial class Category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category()
        {
            Ideas = new HashSet<Idea>();
        }

        [Key]
        [Required]
        [Display(Name = "ID")]
        public int Category_Id { get; set; }

        [Display(Name = "Category Name")]
        [StringLength(50)]
        public string Category_Name { get; set; }

        [Display(Name = "Description")]
        [Column(TypeName = "text")]
        public string Category_Description { get; set; }

        [Display(Name = "Submission ID")]
        public int? Submission_Id { get; set; }

        public virtual Submission Submission { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Idea> Ideas { get; set; }
    }
}
