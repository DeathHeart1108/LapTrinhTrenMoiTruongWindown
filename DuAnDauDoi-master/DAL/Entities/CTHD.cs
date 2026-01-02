namespace DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CTHD")]
    public partial class Cthd
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string Macthd { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string Mahd { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Mamon { get; set; }

        public int? Sl { get; set; }

        public double? Khuyenmai { get; set; }

        public virtual Hoadon Hoadon { get; set; }

        public virtual Mon Mon { get; set; }
    }
}
