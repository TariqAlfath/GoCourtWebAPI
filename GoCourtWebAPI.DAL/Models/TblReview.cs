using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GoCourtWebAPI.DAL.Models;

[Table("tbl_review")]
public partial class TblReview
{
    [Key]
    [Column("id_review")]
    public int IdReview { get; set; }

    [Column("id_transaksi")]
    public int? IdTransaksi { get; set; }

    [Column("deskripsi")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Deskripsi { get; set; }

    [Column("score")]
    public int? Score { get; set; }

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

    [ForeignKey("IdTransaksi")]
    [InverseProperty("TblReviews")]
    public virtual TblTransaksi? IdTransaksiNavigation { get; set; }
}
