using System;
using System.Collections.Generic;
using GoCourtWebAPI.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GoCourtWebAPI.DAL.DBContext;

public partial class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblJenisLapangan> TblJenisLapangans { get; set; }

    public virtual DbSet<TblLapangan> TblLapangans { get; set; }

    public virtual DbSet<TblOrder> TblOrders { get; set; }

    public virtual DbSet<TblReview> TblReviews { get; set; }

    public virtual DbSet<TblTransaksi> TblTransaksis { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblLapangan>(entity =>
        {
            entity.HasOne(d => d.IdJenisLapanganNavigation).WithMany(p => p.TblLapangans).HasConstraintName("FK_tbl_lapangan_tbl_lapangan");
        });

        modelBuilder.Entity<TblOrder>(entity =>
        {
            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.TblOrders).HasConstraintName("FK_tbl_order_tbl_lapangan");
        });

        modelBuilder.Entity<TblReview>(entity =>
        {
            entity.HasOne(d => d.IdTransaksiNavigation).WithMany(p => p.TblReviews).HasConstraintName("FK_tbl_review_tbl_transaksi");
        });

        modelBuilder.Entity<TblTransaksi>(entity =>
        {
            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.TblTransaksis).HasConstraintName("FK_tbl_transaksi_tbl_transaksi");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.Property(e => e.IdUser).HasDefaultValueSql("(newid())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
