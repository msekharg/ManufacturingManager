using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ManufacturingManager.Core;

public class FinalInspection : IValidatableObject
{
    [Key]
   public int FinalInspectionId { get; set; }
    [DisplayName("Inspector")]
    [Required(ErrorMessage = "Inspector Name is required")]
    [RegularExpression(RegExValidation.RegExInvalidCharacters, ErrorMessage = RegExValidation.RegExInvalidCharactersMessage)]
    public string InspectorName {get;set;}

    [DisplayName("Tube - Thickness")]
    // [Range(3.00, 3.18, ErrorMessage = "Value should be between 3.00 and 3.18")]
    
    public string PartName { get; set; }
    public double TubeThickness { get; set; } = 3.1;
    public string TubeThicknessString
    {
        get => TubeThickness.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                TubeThickness = val;
        }
    }

    [DisplayName("Mid Hanger Clamp Thickness")]
    [Range(2.40, 2.60, ErrorMessage = "Value should be between 2.40 and 2.60")]
    public double MidHangerClampThickness { get; set; } = 2.5;
    public string MidHangerClampThicknessString
    {
        get => MidHangerClampThickness.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                MidHangerClampThickness = val;
        }
    }

    [DisplayName("Rail Tube Height")]
    // [Range(84.50, 85.50, ErrorMessage = "Value should be between 84.50 and 85.50")]
    public double RailTubeHeight { get; set; } = 85;
    public string RailTubeHeightString
    {
        get => RailTubeHeight.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                RailTubeHeight = val;
        }
    }

    [DisplayName("Rail Tube Width")]
    [Range(29.50, 30.50, ErrorMessage = "Value should be between 29.50 and 30.50")]
    public double RailTubeWidth { get; set; } = 29.75;
    public string RailTubeWidthString
    {
        get => RailTubeWidth.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                RailTubeWidth = val;
        }
    }

    [DisplayName("Tube Weld seam Location")]
    [Range(37.50, 47.50, ErrorMessage = "Value should be between 37.50 and 47.50")]
    public double TubeWeldSeamLocation { get; set; } = 42;
    public string TubeWeldSeamLocationString
    {
        get => TubeWeldSeamLocation.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                TubeWeldSeamLocation = val;
        }
    }

    [DisplayName("Tube Length")]
    // [Range(2896.00, 2904.00, ErrorMessage = "Value should be between 2896.00 and 2904.00")]
    public double TubeLength { get; set; } = 2900.01;
    public string TubeLengthString
    {
        get => TubeLength.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                TubeLength = val;
        }
    }

    [DisplayName("Tube corner Radius @ 4 Places")]
    [Range(5.75, 6.25, ErrorMessage = "Value should be between 5.75 and 6.25")]
    public int TubeCornerRadius4Places { get; set; } = 6;
    public string TubeCornerRadius4PlacesString
    {
        get => TubeCornerRadius4Places.ToString();
        set
        {
            if (int.TryParse(value, out int val))
                TubeCornerRadius4Places = val;
        }
    }

    [DisplayName("End of tube to CL Distance")]
    [Range(1449, 1451, ErrorMessage = "Value should be between 1449 and 1451")]
    public double EndOfTubeToCLDistance { get; set; } = 1450;
    public string EndOfTubeToCLDistanceString
    {
        get => EndOfTubeToCLDistance.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                EndOfTubeToCLDistance = val;
        }
    }
    
    [DisplayName("End of Tube to Clamp 1 Bolt @ 2 Places")]
    [Range(64, 66, ErrorMessage = "Value should be between 64 and 66")]
    public double EndOfTubeToClamp1Bolt_2Places  { get; set; } = 65;
    public string EndOfTubeToClamp1Bolt_2PlacesString
    {
        get => EndOfTubeToClamp1Bolt_2Places.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                EndOfTubeToClamp1Bolt_2Places = val;
        }
    }
    
    [DisplayName("Clamp 1 Bolt to Clamp 2 Bolt Distance @ 2 Places")]
    [Range(74.5, 75.5, ErrorMessage = "Value should be between 74.5 and 75.5")]
    public double Clamp1BoltToClamp2BoltDistance_2Places  { get; set; } = 75;
    public string Clamp1BoltToClamp2BoltDistance_2PlacesString
    {
        get => Clamp1BoltToClamp2BoltDistance_2Places.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                Clamp1BoltToClamp2BoltDistance_2Places = val;
        }
    }
    
    [DisplayName("End of Tube to Clamp 3 Bolt @ 2 Places")]
    [Range(839, 841, ErrorMessage = "Value should be between 839 and 841")]
    public double EndOfTubeToClamp3Bolt_2Places  { get; set; } = 840;
    public string EndOfTubeToClamp3Bolt_2PlacesString
    {
        get => EndOfTubeToClamp3Bolt_2Places.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                EndOfTubeToClamp3Bolt_2Places = val;
        }
    }
    
    [DisplayName("Clamp 3 Bolt to Clamp 4 Bolt Distance @ 2 Places")]
    [Range(114, 116, ErrorMessage = "Value should be between 114 and 116")]
    public double Clamp3BoltToClamp4BoltDistance_2Places  { get; set; } = 115;
    public string Clamp3BoltToClamp4BoltDistance_2PlacesString
    {
        get => Clamp3BoltToClamp4BoltDistance_2Places.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                Clamp3BoltToClamp4BoltDistance_2Places = val;
        }
    }
    
    [DisplayName("Torque(Clamp 1) @ 2 Places")]
    [Range(47.5, 52.3, ErrorMessage = "Value should be between 47.5 and 52.3")]
    public double TorqueClamp1_2Places  { get; set; } = 49;
    public string TorqueClamp1_2PlacesString
    {
        get => TorqueClamp1_2Places.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                TorqueClamp1_2Places = val;
        }
    }
    
    [DisplayName("Torque(Clamp 2) @ 2 Places")]
    [Range(33.9, 37.3, ErrorMessage = "Value should be between 33.9 and 37.3")]
    public double TorqueClamp2_2Places  { get; set; } = 35;
    public string TorqueClamp2_2PlacesString
    {
        get => TorqueClamp2_2Places.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                TorqueClamp2_2Places = val;
        }
    }
    
    [DisplayName("Torque(Clamp 3) @ 2 Places")]
    [Range(33.9, 37.3, ErrorMessage = "Value should be between 33.9 and 37.3")]
    public double TorqueClamp3_2Places  { get; set; } = 36;
    public string TorqueClamp3_2PlacesString
    {
        get => TorqueClamp3_2Places.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                TorqueClamp3_2Places = val;
        }
    }
    
    [DisplayName("Torque(Clamp 4) @ 2 Places")]
    [Range(33.9, 37.3, ErrorMessage = "Value should be between 33.9 and 37.3")]
    public double TorqueClamp4_2Places  { get; set; } = 37.1;
    public string TorqueClamp4_2PlacesString
    {
        get => TorqueClamp4_2Places.ToString();
        set
        {
            if (double.TryParse(value, out double val))
                TorqueClamp4_2Places = val;
        }
    }
    
    [DisplayName("Torque Marking")]
    public int TorqueMarking {get;set;}
    
    public string TorqueMarkingString
    {
        get;
        set;
        // get => TorqueMarking.ToString();
        // set
        // {
        //     if (int.TryParse(value, out int val))
        //         TorqueMarking = val;
        // }
    }

    [DisplayName("Part Marking")]
    public int PartMarking {get;set;}
    public string PartMarkingString
    {
        get;
        set;
        // get => PartMarking.ToString();
        // set
        // {
        //     if (int.TryParse(value, out int val))
        //         PartMarking = val;
        // }
    }

    [DisplayName("Rivet Presence Qty=2")]
    public int RivetPresence {get;set;}
    public string RivetPresenceString
    {
        get;
        set;
        // get => RivetPresence.ToString();
        // set
        // {
        //     if (int.TryParse(value, out int val))
        //         RivetPresence = val;
        // }
    }

    [DisplayName("Mid Hanger Holes Align")]
    public int MidHangerHolesAlign {get;set;}
    public string MidHangerHolesAlignString
    {
        get;
        set;
        // get => MidHangerHolesAlign.ToString();
        // set
        // {
        //     if (int.TryParse(value, out int val))
        //         MidHangerHolesAlign = val;
        // }
    }
    
    [DisplayName("Washer plate presence @6 locations")]
    public int WasherPlatePresence6Locations
    {get;set;}
    public string WasherPlatePresence6LocationsString
    {
        get;
        set;
        // get => WasherPlatePresence6Locations.ToString();
        // set
        // {
        //     if (int.TryParse(value, out int val))
        //         WasherPlatePresence6Locations = val;
        // }
    }
    
    [DisplayName("Color Code")]
    public int ColorCode {get;set;}
    public string ColorCodeString
    {
        get;
        set;
        // get => ColorCode.ToString();
        // set
        // {
        //     if (int.TryParse(value, out int val))
        //         ColorCode = val;
        // }
    }
    
    [DisplayName("Appearance")]
    public int Appearance {get;set;}
    public string AppearanceString
    {
        get;
        set;
        // get => Appearance.ToString();
        // set
        // {
        //     if (int.TryParse(value, out int val))
        //         Appearance = val;
        // }
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
