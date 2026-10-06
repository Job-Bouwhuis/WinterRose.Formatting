using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WinterRose.Formatting.TimeFormats;

internal sealed record DurationFormatToken(
    DurationUnit Unit,
    int FractionDigits,
    bool TrimFractionZeroes,
    int Padding,
    bool Abbreviated,
    bool LongName,
    bool Optional,
    string? Literal = null,
    string? TrailingLiteral = null);
