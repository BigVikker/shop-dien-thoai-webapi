namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PRODUCT")]
    public partial class PRODUCT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PRODUCT()
        {
            CONFIGURATIONs = new HashSet<CONFIGURATION>();
            ORDERDETAILs = new HashSet<ORDERDETAIL>();
        }

        public int ProductID { get; set; }

        [Required]
        [StringLength(255)]
        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public decimal ProductPrice { get; set; }

        public decimal? PromotionPrice { get; set; }

        public int? Rating { get; set; }

        [StringLength(4000)]
        public string ProductImage { get; set; }

        public int? ProductStock { get; set; }

        [StringLength(255)]
        public string ProductURL { get; set; }

        public int? Viewcount { get; set; }

        public bool? ProductStatus { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int BrandID { get; set; }

        public virtual BRAND BRAND { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CONFIGURATION> CONFIGURATIONs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ORDERDETAIL> ORDERDETAILs { get; set; }
    }
}
