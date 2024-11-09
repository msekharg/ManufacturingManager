using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ManufacturingManager.Core.Helpers;

namespace ManufacturingManager.Core;

public class SolarConfiguration
{
    [ValidateFalse(ErrorMessage = "The value must be true.")]
    public bool IsButtonClicked { get; set; } = false;

}
