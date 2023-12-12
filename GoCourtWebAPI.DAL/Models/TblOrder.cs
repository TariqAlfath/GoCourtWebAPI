using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GoCourtWebAPI.DAL.Models;

[Table("tbl_order")]
public partial class TblOrder
{
    [Key]
    [Column("id_order")]
    public int IdOrder { get; set; }

    [Column("id_lapangan")]
    public int? IdLapangan { get; set; }

    [Column("id_user")]
    public Guid? IdUser { get; set; }

    [Column("rent_start", TypeName = "datetime")]
    public DateTime? RentStart { get; set; }

    [Column("rent_end", TypeName = "datetime")]
    public DateTime? RentEnd { get; set; }

    [Column("estimatedPrice", TypeName = "decimal(18, 0)")]
    public decimal? EstimatedPrice { get; set; }

    [Column("payment_proof")]
    public byte[]? PaymentProof { get; set; }

    [Column("payemnt_proof_file_name")]
    [StringLength(255)]
    [Unicode(false)]
    public string? PayemntProofFileName { get; set; }

    [Column("payemnt_proof_file_type")]
    [StringLength(50)]
    [Unicode(false)]
    public string? PayemntProofFileType { get; set; }

    [Column("status")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Status { get; set; }

    [Column("catatan")]
    public string? Catatan { get; set; }

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

    [ForeignKey("IdLapangan")]
    [InverseProperty("TblOrders")]
    public virtual TblLapangan? IdLapanganNavigation { get; set; }

    [ForeignKey("IdUser")]
    [InverseProperty("TblOrders")]
    public virtual TblUser? IdUserNavigation { get; set; }

    [InverseProperty("IdOrderNavigation")]
    public virtual ICollection<TblTransaksi> TblTransaksis { get; set; } = new List<TblTransaksi>();
}
