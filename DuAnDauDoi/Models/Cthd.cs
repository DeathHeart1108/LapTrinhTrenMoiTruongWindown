namespace DuAnDauDoi.Models
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
        [StringLength(10)]
        public string Macthd { get; set; }

        [Required]
        [StringLength(10)]
        public string Mahd { get; set; }

        public int Mamon { get; set; }

        public int? Sl { get; set; }

        public double? Khuyenmai { get; set; }

        public virtual Hoadon HMahdNavigation { get; set; }

        public virtual Mon MamonNavigation { get; set; }
    }
}
