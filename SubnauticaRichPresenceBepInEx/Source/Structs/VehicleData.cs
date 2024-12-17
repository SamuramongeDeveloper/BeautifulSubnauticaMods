using System;
using System.Collections.Generic;
using System.Text;

namespace SubnauticaRichPresenceBepInEx.Source.Structs;

public readonly struct VehicleData
{
    public long Client { get; init; }

    public string Pronoun { get; init; }

    public string Name { get; init; }

    public string Image { get; init; }
}
