namespace DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MON")]
    public partial class Mon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Mon()
        {
            Cthds = new HashSet<Cthd>();
        }

        [Key]
        public int Mamon { get; set; }

        [StringLength(100)]
        public string Tenmon { get; set; }

        [Column(TypeName = "money")]
        public decimal? Giamon { get; set; }

        public int? Maloai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cthd> Cthds { get; set; }

        public virtual Loaimon Loaimon { get; set; }
    }
}
