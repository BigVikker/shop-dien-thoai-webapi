namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ADMIN")]
    public partial class ADMIN
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(255)]
        public string UserUsername { get; set; }

        [Required]
        [StringLength(255)]
        public string UserPassword { get; set; }

        [StringLength(255)]
        public string UserName { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
