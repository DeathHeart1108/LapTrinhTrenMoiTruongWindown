using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DAL.Entities
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Ban> Bans { get; set; }
        public virtual DbSet<Cthd> Cthds { get; set; }
        public virtual DbSet<Hoadon> Hoadons { get; set; }
        public virtual DbSet<Loaimon> Loaimons { get; set; }
        public virtual DbSet<Mon> Mons { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ban>()
                .HasMany(e => e.Hoadons)
                .WithRequired(e => e.Ban)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Cthd>()
                .Property(e => e.Macthd)
                .IsUnicode(false);

            modelBuilder.Entity<Cthd>()
                .Property(e => e.Mahd)
                .IsUnicode(false);

            modelBuilder.Entity<Hoadon>()
                .Property(e => e.Mahd)
                .IsUnicode(false);

            modelBuilder.Entity<Hoadon>()
                .Property(e => e.Tongtien)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Hoadon>()
                .HasMany(e => e.Cthds)
                .WithRequired(e => e.Hoadon)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Loaimon>()
                .HasMany(e => e.Mons)
                .WithRequired(e => e.Loaimon)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Mon>()
                .Property(e => e.Giamon)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Mon>()
                .HasMany(e => e.Cthds)
                .WithRequired(e => e.Mon)
                .WillCascadeOnDelete(false);
        }
    }
}
