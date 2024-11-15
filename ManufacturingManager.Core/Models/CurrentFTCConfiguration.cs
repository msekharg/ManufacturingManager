using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManufacturingManager.Core.Models;

[Table("CurrentFTCConfiguration", Schema = "dbo")]
public class CurrentFTCConfiguration
{
    [Key]
    public int CurrentFTCConfigurationId { get; set; }
    
    [StringLength(50)]
    public string PartNumber { get; set; }
        
    [StringLength(20)]
    public string AssemblyConfiguration { get; set; }
    
    public double Height { get; set; }
    
    public double Length { get; set; }
    
    public double Thickness { get; set; }
    
    public bool IsActive { get; set; }
    
    public DateTime StartDateTime { get; set; }
    
    public DateTime? EndDateTime { get; set; }
    
    public string CreatedBy { get; set; }
    
    public DateTime CreatedDate { get; set; }
}