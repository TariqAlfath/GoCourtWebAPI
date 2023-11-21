using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GoCourtWebAPI.DAL.Models;

[Table("tbl_transaksi")]
public partial class TblTransaksi
{
    [Key]
    [Column("id_transaksi")]
    public int IdTransaksi { get; set; }

    [Column("id_order")]
    public int? IdOrder { get; set; }

    [Column("harga_total", TypeName = "decimal(18, 0)")]
    public decimal? HargaTotal { get; set; }

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

    [ForeignKey("IdOrder")]
    [InverseProperty("TblTransaksis")]
    public virtual TblOrder? IdOrderNavigation { get; set; }

    [InverseProperty("IdTransaksiNavigation")]
    public virtual ICollection<TblReview> TblReviews { get; set; } = new List<TblReview>();
}
