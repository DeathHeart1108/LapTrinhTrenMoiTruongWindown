namespace DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HOADON")]
    public partial class Hoadon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Hoadon()
        {
            Cthds = new HashSet<Cthd>();
        }

        [Key]
        [StringLength(10)]
        public string Mahd { get; set; }

        [Column(TypeName = "date")]
        public DateTime Ngaylap { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Ngayxuat { get; set; }

        public int Status { get; set; }

        public int Maban { get; set; }

        [Column(TypeName = "money")]
        public decimal? Tongtien { get; set; }

        public double? KHUYENMAI_HD { get; set; }

        public virtual Ban Ban { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cthd> Cthds { get; set; }
    }
}
