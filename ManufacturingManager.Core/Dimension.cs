using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ManufacturingManager.Core;

public class Dimension : IValidatableObject
{

    [Key]
   public int DimensionId { get; set; }
    [DisplayName("Inspector")]
    [Required(ErrorMessage = "Inspector Name is required")]
    [RegularExpression(RegExValidation.RegExInvalidCharacters, ErrorMessage = RegExValidation.RegExInvalidCharactersMessage)]
    public string InspectorName {get;set;}

    [DisplayName("Tube - Thickness #26")]
    [Range(4.00, 4.24, ErrorMessage = "Please enter a value between 4.00 and 4.24")]
    public double TubeThickness26 { get; set; } = 4.2;
    public string TubeThickness26String
    {
        get => TubeThickness26.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                TubeThickness26 = val;
        }
    }

    [DisplayName("Clamp Thickness")]
    [Range(2.40, 2.60, ErrorMessage = "Please enter a value between 2.40 and 2.60")]
    public double ClampThickness { get; set; } = 2.5;
    public string ClampThicknessString
    {
        get => ClampThickness.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                ClampThickness = val;
        }
    }

    [DisplayName("Rail Tube Height #23")]
    [Range(84.50, 85.50, ErrorMessage = "Please enter a value between 84.50 and 85.50")]
    public double RailTubeHeight23 { get; set; } = 85;
    public string RailTubeHeight23String
    {
        get => RailTubeHeight23.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                RailTubeHeight23 = val;
        }
    }

    [DisplayName("Rail Tube Width #22")]
    [Range(29.50, 30.50, ErrorMessage = "Please enter a value between 29.50 and 30.50")]
    public double RailTubeWidth22 { get; set; } = 29.75;
    public string RailTubeWidth22String
    {
        get => RailTubeWidth22.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                RailTubeWidth22 = val;
        }
    }

    [DisplayName("Tube Weld seam Location #24")]
    [Range(37.50, 47.50, ErrorMessage = "Please enter a value between 37.50 and 47.50")]
    public double TubeWeldSeamLocation24 { get; set; } = 38;
    public string TubeWeldSeamLocation24String
    {
        get => TubeWeldSeamLocation24.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                TubeWeldSeamLocation24 = val;
        }
    }

    [DisplayName("Tube Length #2")]
    [Range(2696.00, 2704.00, ErrorMessage = "Please enter a value between 2696.00 and 2704.00")]
    public double TubeLength2 { get; set; } = 2701.01;
    public string TubeLength2String
    {
        get => TubeLength2.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                TubeLength2 = val;
        }
    }

    [DisplayName("Paint Identification")]    
    public int PaintIdentification {get;set;}
    public string PaintIdentificationString
    {
        get => PaintIdentification.ToString();
        set
        {
            if (int.TryParse(value, out int val))
                PaintIdentification = val;
        }
    }

    [DisplayName("Tube Corner Radius @ 2'o clock #25")]
    public int TubeCornerRadius2Clock25 {get;set;}
    public string TubeCornerRadius2Clock25String
    {
        get => TubeCornerRadius2Clock25.ToString();
        set
        {
            if (int.TryParse(value, out int val))
                TubeCornerRadius2Clock25 = val;
        }
    }

    [DisplayName("Tube corner Radius @ 4'o clock #25")]
    public int TubeCornerRadius4Clock25 {get;set;}
    public string TubeCornerRadius4Clock25String
    {
        get => TubeCornerRadius4Clock25.ToString();
        set
        {
            if (int.TryParse(value, out int val))
                TubeCornerRadius4Clock25 = val;
        }
    }

    [DisplayName("Tube corner Radius @ 8'o clock #25")]
    public int TubeCornerRadius8Clock25 {get;set;}
    public string TubeCornerRadius8Clock25String
    {
        get => TubeCornerRadius8Clock25.ToString();
        set
        {
            if (int.TryParse(value, out int val))
                TubeCornerRadius8Clock25 = val;
        }
    }

    [DisplayName("Tube corner Radius @ 10'o clock #25")]
    public int TubeCornerRadius10Clock25 {get;set;}
    public string TubeCornerRadius10Clock25String
    {
        get => TubeCornerRadius10Clock25.ToString();
        set
        {
            if (int.TryParse(value, out int val))
                TubeCornerRadius10Clock25 = val;
        }
    }

    [DisplayName("End of tube to CL Distance")]
    [Range(14.50, 15.50, ErrorMessage = "Please enter a value between 14.50 and 15.50")]
    public double EndOfTubeToCLDistance { get; set; } = 15.00;
    public string EndOfTubeToCLDistanceString
    {
        get => EndOfTubeToCLDistance.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                EndOfTubeToCLDistance = val;
        }
    }

    [DisplayName("Torque per Note 2")]
    [Range(35.00, 38.50, ErrorMessage = "Please enter a value between 35.00 and 38.50")]
    public double TorquePerNote2 { get; set; } = 36.75;
    public string TorquePerNote2String
    {
        get => TorquePerNote2.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                TorquePerNote2 = val;
        }
    }

    [DisplayName("Torque per Note 3")]
    [Range(25.00, 27.50, ErrorMessage = "Please enter a value between 25.00 and 27.50")]
    public double TorquePerNote3 { get; set; } = 26.2;
    public string TorquePerNote3String
    {
        get => TorquePerNote3.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                TorquePerNote3 = val;
        }
    }

    [DisplayName("Torque Marking")]
    public int TorqueMarking {get;set;}
    
    public string TorqueMarkingString
    {
        get => TorqueMarking.ToString();
        set
        {
            if (int.TryParse(value, out int val))
                TorqueMarking = val;
        }
    }
    
    [DisplayName("Tube Pre Galv Coating Thickness")]
    [Range(19.30, double.MaxValue, ErrorMessage = "Please enter a value between 19.30 and NA")]
    public double TubePreGalvCoatingThickness { get; set; } = 22;
    public string TubePreGalvCoatingThicknessString
    {
        get => TubePreGalvCoatingThickness.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                TubePreGalvCoatingThickness = val;
        }
    }

    [DisplayName("Clamp Pre Coating Thickness")]
    [Range(14.50, double.MaxValue, ErrorMessage = "Please enter a value between 19.30 and NA")]
    public double ClampPreCoatingThickness { get; set; } = 2200.01;
    public string ClampPreCoatingThicknessString
    {
        get => ClampPreCoatingThickness.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                ClampPreCoatingThickness = val;
        }
    }

    [DisplayName("Total No of Holes (Bottom), #3")]
    public int TotalNoOfHolesBottom3 {get;set;}
    public string TotalNoOfHolesBottom3String
    {
        get => TotalNoOfHolesBottom3.ToString();
        set
        {
            if (int.TryParse(value, out int val))
                TotalNoOfHolesBottom3 = val;
        }
    }

    [DisplayName("Part Marking")]
    public int PartMarking {get;set;}
    public string PartMarkingString
    {
        get => PartMarking.ToString();
        set
        {
            if (int.TryParse(value, out int val))
                PartMarking = val;
        }
    }

    [DisplayName("Rivet Presence")]
    public int RivetPresence {get;set;}
    public string RivetPresenceString
    {
        get => RivetPresence.ToString();
        set
        {
            if (int.TryParse(value, out int val))
                RivetPresence = val;
        }
    }

    [DisplayName("Appearance")]
    public int Appearance {get;set;}
    public string AppearanceString
    {
        get => Appearance.ToString();
        set
        {
            if (int.TryParse(value, out int val))
                Appearance = val;
        }
    }

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
