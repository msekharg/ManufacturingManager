using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ManufacturingManager.Core;

public class InProcessCheck : IValidatableObject
{

    [Key]
   public int InProcessCheckId { get; set; }
   
    [DisplayName("Inspector")]
    [Required(ErrorMessage = "Inspector Name is required")]
    [RegularExpression(RegExValidation.RegExInvalidCharacters, ErrorMessage = RegExValidation.RegExInvalidCharactersMessage)]
    public string InspectorName {get;set;}

    /// <summary>Primary Saw</summary>
    [DisplayName("Nominal")]
    public double PrimarySawLengthNominal { get; set; } = 1974;
    public string PrimarySawLengthNominalString
    {
        get => PrimarySawLengthNominal.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                PrimarySawLengthNominal = val;
        }
    }
    
    [DisplayName("Tol+")]
    public double PrimarySawLengthTolPlus { get; set; } = 3;
    public string PrimarySawLengthTolPlusString
    {
        get => PrimarySawLengthTolPlus.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                PrimarySawLengthTolPlus = val;
        }
    }
    
    [DisplayName("Tol-")]
    public double PrimarySawLengthTolMinus { get; set; } = 3;
    public string PrimarySawLengthTolMinusString
    {
        get => PrimarySawLengthTolMinus.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                PrimarySawLengthTolMinus = val;
        }
    }
    
    [DisplayName("Value")]
    public double PrimarySawLengthValue { get; set; }
    public string PrimarySawLengthValueString
    {
        get => PrimarySawLengthValue.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                PrimarySawLengthValue = val;
        }
    }
    /// <summary>Primary Saw</summary>

    /// <summary>Bending</summary>
    [DisplayName("Nominal")]
    public double BendingLengthNominal { get; set; } = 1966;
    public string BendingLengthNominalString
    {
        get => BendingLengthNominal.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                BendingLengthNominal = val;
        }
    }
    
    [DisplayName("Tol+")]
    public double BendingLengthTolPlus { get; set; } = 1969;
    public string BendingLengthTolPlusString
    {
        get => BendingLengthTolPlus.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                BendingLengthTolPlus = val;
        }
    }
    
    [DisplayName("Tol-")]
    public double BendingLengthTolMinus { get; set; } = 1963;
    public string BendingLengthTolMinusString
    {
        get => BendingLengthTolMinus.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                BendingLengthTolMinus = val;
        }
    }
    
    [DisplayName("Value")]
    public double BendingLengthValue { get; set; }
    public string BendingLengthValueString
    {
        get => BendingLengthValue.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                BendingLengthValue = val;
        }
    }
    
    [DisplayName("Nominal")]
    public double BendingODNominal { get; set; } = 130.0;
    public string BendingODNominalString
    {
        get => BendingODNominal.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                BendingODNominal = val;
        }
    }
    
    [DisplayName("Tol+")]
    public double BendingODTolPlus { get; set; } = 131.0;
    public string BendingODTolPlusString
    {
        get => BendingODTolPlus.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                BendingODTolPlus = val;
        }
    }
    
    [DisplayName("Tol-")]
    public double BendingODTolMinus { get; set; } = 129.5;
    public string BendingODTolMinusString
    {
        get => BendingODTolMinus.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                BendingODTolMinus = val;
        }
    }
    
    [DisplayName("Value")]
    public double BendingODValue { get; set; }
    public string BendingODValueString
    {
        get => BendingODValue.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                BendingODValue = val;
        }
    }
    
    [DisplayName("Nominal")]
    public double BendingWallThicknessNominal { get; set; } = 4.50;
    public string BendingWallThicknessNominalString
    {
        get => BendingWallThicknessNominal.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                BendingWallThicknessNominal = val;
        }
    }
    
    [DisplayName("Tol+")]
    public double BendingWallThicknessTolPlus { get; set; } = 5.10;
    public string BendingWallThicknessTolPlusString
    {
        get => BendingWallThicknessTolPlus.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                BendingWallThicknessTolPlus = val;
        }
    }
    
    [DisplayName("Tol-")]
    public double BendingWallThicknessTolMinus { get; set; } = 4.40;
    public string BendingWallThicknessTolMinusString
    {
        get => BendingWallThicknessTolMinus.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                BendingWallThicknessTolMinus = val;
        }
    }
    
    [DisplayName("Value")]
    public double BendingWallThicknessValue { get; set; }
    public string BendingWallThicknessValueString
    {
        get => BendingWallThicknessValue.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                BendingWallThicknessValue = val;
        }
    }
    /// <summary>Bending</summary>

    /// <summary>Secondary Saw</summary>
    [DisplayName("Nominal")]
    public double SecondarySawLengthNominal { get; set; } = 983;
    public string SecondarySawLengthNominalString
    {
        get => SecondarySawLengthNominal.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                SecondarySawLengthNominal = val;
        }
    }
    
    [DisplayName("Tol+")]
    public double SecondarySawLengthTolPlus { get; set; } = 985;
    public string SecondarySawLengthTolPlusString
    {
        get => SecondarySawLengthTolPlus.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                SecondarySawLengthTolPlus = val;
        }
    }
    
    [DisplayName("Tol-")]
    public double SecondarySawLengthTolMinus { get; set; } = 981;
    public string SecondarySawLengthTolMinusString
    {
        get => SecondarySawLengthTolMinus.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                SecondarySawLengthTolMinus = val;
        }
    }
    
    [DisplayName("Value")]
    public double SecondarySawLengthValue { get; set; }
    public string SecondarySawLengthValueString
    {
        get => SecondarySawLengthValue.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                SecondarySawLengthValue = val;
        }
    }
    /// <summary>Primary Saw</summary>
    
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime UpdatedDate { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        DateTime currentDateTime = DateTime.Now;
        if (CreatedDate > currentDateTime)
        {
            yield return new ValidationResult(
                $"Date requested must be less or equal to {currentDateTime}",
                new[] { nameof(CreatedDate) });
        }
    }
}
