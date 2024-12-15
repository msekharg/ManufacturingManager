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

    [Required(ErrorMessage = "Thickness is required.")]
    [Range(0.01, 999.99, ErrorMessage = "Thickness must be between 0.01 and 999.99.")]
    [Column(TypeName = "decimal(5, 2)")]
    public decimal Thickness { get; set; }

    [Required]
    public int Length { get; set; }

    [Required(ErrorMessage = "RailWeight is required.")]
    [Range(0.001, 99.999, ErrorMessage = "RailWeight must be between 0.001 and 99.999.")]
    [Column(TypeName = "decimal(5, 3)")]
    public decimal RailWeight { get; set; }
}