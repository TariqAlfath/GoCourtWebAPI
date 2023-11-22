using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GoCourtWebAPI.DAL.Models;

[Table("tbl_lapangan")]
public partial class TblLapangan
{
    [Key]
    [Column("id_lapangan")]
    public int IdLapangan { get; set; }

    [Column("id_jenis_lapangan")]
    public int? IdJenisLapangan { get; set; }

    [Column("nama_lapangan")]
    [StringLength(88)]
    [Unicode(false)]
    public string? NamaLapangan { get; set; }

    [Column("harga_lapangan", TypeName = "decimal(18, 0)")]
    public decimal? HargaLapangan { get; set; }

    [Column("createdAt", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("createdBy")]
    [StringLength(50)]
    [Unicode(false)]
    public string? CreatedBy { get; set; }

    [Column("modifiedAt", TypeName = "datetime")]
    public DateTime? ModifiedAt { get; set; }

    [Column("modifiedBy")]
    [StringLength(50)]
    [Unicode(false)]
    public string? ModifiedBy { get; set; }

    [ForeignKey("IdJenisLapangan")]
    [InverseProperty("TblLapangans")]
    public virtual TblJenisLapangan? IdJenisLapanganNavigation { get; set; }

    [InverseProperty("IdLapanganNavigation")]
    public virtual ICollection<TblOrder> TblOrders { get; set; } = new List<TblOrder>();
}
