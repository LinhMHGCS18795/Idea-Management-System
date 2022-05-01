namespace IdeaManageApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("hr.Idea")]
    public partial class Idea
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Idea()
        {
            Comments = new HashSet<Comment>();
            Reactions = new HashSet<Reaction>();
            Views = new HashSet<View>();
        }

        [Key]
        [Required]
        [Display(Name = "ID")]
        public int Idea_Id { get; set; }

        [Display(Name = "Category ID")]
        public int? Category_Id { get; set; }

        [Display(Name = "Title")]
        [StringLength(50)]
        public string Idea_Title { get; set; }

        [Display(Name = "Cotent")]
        [Column(TypeName = "text")]
        public string Idea_Content { get; set; }

        [Display(Name = "Create Date")]
        [Column(TypeName = "date")]
        public DateTime? Idea_Create_date { get; set; }

        [DisplayName("Upload File")]
        public string Idea_File_path { get; set; }

        [NotMapped]
        public HttpPostedFileBase MyFile { get; set; }

        [Display(Name = "User ID")]
        public int? User_Id { get; set; }

        [Display(Name = "Terms and Conditions")]
        public bool TermsAndCondition { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reaction> Reactions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<View> Views { get; set; }
    }
}
