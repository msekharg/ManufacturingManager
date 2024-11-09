using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManufacturingManager.Core.Models;

[Table("ClampsPositioning", Schema = "dbo")]
public class ClampsPositioning
{
    [Key]
    public int ClampsPositioningId { get; set; }
    
    [StringLength(6)]
    public string CXPX { get; set; }
        
    [StringLength(4)]
    public string CX { get; set; }
        
    [StringLength(4)]
    public string Clamp1 { get; set; }
        
    [StringLength(4)]
    public string Clamp2 { get; set; }
        
    [StringLength(4)]
    public string PX { get; set; }
        
    [StringLength(4)]
    public string Clamp3 { get; set; }
        
    [StringLength(4)]
    public string Clamp4 { get; set; }
        
    [StringLength(1)]
    public string HoleSetDrilled { get; set; }
}