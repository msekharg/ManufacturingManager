using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManufacturingManager.Core.Models;

[Table("MidRailConfiguration", Schema = "dbo")]
public class MidRailConfiguration
{
    [Key]
    public int MidRailConfigurationId { get; set; }

    [Required]
    [StringLength(25)]
    public string PartNumber { get; set; }

    [Required]
    public int Height { get; set; }

    [Required]
    [Column(TypeName = "decimal(5, 2)")]
    public decimal Thickness { get; set; }

    [Required]
    public int Length { get; set; }

    [Required]
    [Column(TypeName = "decimal(3, 3)")]
    public decimal RailWeight { get; set; }
}