using System;
using System.Collections.Generic;
using System.Text;

namespace WinterRose.Formatting.Units;

/// <summary>
/// Represents a temperature scale.
/// </summary>
public enum TemperatureScale
{
    /// <summary>
    /// Celsius scale. Water freezes at 0 °C and boils at 100 °C at standard atmospheric pressure.
    /// </summary>
    Celsius,

    /// <summary>
    /// Fahrenheit scale. Water freezes at 32 °F and boils at 212 °F at standard atmospheric pressure.
    /// </summary>
    Fahrenheit,

    /// <summary>
    /// Kelvin scale. Water freezes at 273.15 K and boils at 373.15 K at standard atmospheric pressure.
    /// </summary>
    Kelvin,

    /// <summary>
    /// Rankine scale. Water freezes at 491.67 °R and boils at 671.67 °R at standard atmospheric pressure.
    /// </summary>
    Rankine,

    /// <summary>
    /// Newton scale. Water freezes at 0 °N and boils at approximately 33 °N at standard atmospheric pressure.
    /// </summary>
    Newton,
}
