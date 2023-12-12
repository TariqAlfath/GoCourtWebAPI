using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GoCourtWebAPI.DAL.Models;

[Table("tbl_user")]
public partial class TblUser
{
    [Key]
    [Column("id_user")]
    public Guid IdUser { get; set; }

    [Column("username")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Username { get; set; }

    [Column("password")]
    [Unicode(false)]
    public string? Password { get; set; }

    [Column("nama")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Nama { get; set; }

    [Column("alamat")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Alamat { get; set; }

    [Column("nomor_telefon")]
    [StringLength(25)]
    [Unicode(false)]
    public string? NomorTelefon { get; set; }

    [Column("email")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Email { get; set; }

    [Column("role")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Role { get; set; }

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

    [InverseProperty("IdUserNavigation")]
    public virtual ICollection<TblOrder> TblOrders { get; set; } = new List<TblOrder>();
}
