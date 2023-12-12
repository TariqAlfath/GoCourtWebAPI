using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GoCourtWebAPI.DAL.Models;

[Table("tbl_jenis_lapangan")]
public partial class TblJenisLapangan
{
    [Key]
    [Column("id_jenis_lapangan")]
    public int IdJenisLapangan { get; set; }

    [Column("nama_jenis_lapangan")]
    [StringLength(88)]
    [Unicode(false)]
    public string? NamaJenisLapangan { get; set; }

    [Column("status")]
    public bool? Status { get; set; }

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

    [InverseProperty("IdJenisLapanganNavigation")]
    public virtual ICollection<TblLapangan> TblLapangans { get; set; } = new List<TblLapangan>();
}
