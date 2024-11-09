using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManufacturingManager.Core.Models;

[Table("ColorCodeMatrix", Schema = "dbo")]
public class ColorCodeMatrix
{
    [Key]
    public int ColorCodeMatrixId { get; set; }

    [Required]
    [StringLength(25)]
    public string Color { get; set; }

    [Required]
    [StringLength(25)]
    public string HexColorCode { get; set; }

    [Required]
    [StringLength(25)]
    public string PantoneColor { get; set; }

    [Required]
    [StringLength(25)]
    public string RALColorCode { get; set; }
}