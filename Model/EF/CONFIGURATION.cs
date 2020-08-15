namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONFIGURATION")]
    public partial class CONFIGURATION
    {
        [Key]
        public int ConfigID { get; set; }

        [StringLength(255)]
        public string OSName { get; set; }

        [StringLength(255)]
        public string OSVersion { get; set; }

        [StringLength(255)]
        public string SizeDisplay { get; set; }

        [StringLength(255)]
        public string FrontCamera { get; set; }

        [StringLength(255)]
        public string RearCamera { get; set; }

        [StringLength(255)]
        public string Cpu { get; set; }

        [StringLength(255)]
        public string Ram { get; set; }

        [StringLength(255)]
        public string Rom { get; set; }

        [StringLength(255)]
        public string SimStatus { get; set; }

        [StringLength(255)]
        public string Battery { get; set; }

        public int? ProductID { get; set; }

        public virtual PRODUCT PRODUCT { get; set; }
    }
}
