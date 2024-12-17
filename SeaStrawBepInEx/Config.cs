using Nautilus.Options;
using Nautilus.Json;
using Nautilus.Options.Attributes;
using SeaStraw.Items;

namespace SeaStraw;

[Menu("SeaStraw")]
public class Config : ConfigFile
{
    [Slider("SeaStraw regeneration rate", 1f, 2f, DefaultValue = 1.5f, Step = 0.1f, Format = "{0:F1}")]
    public float SeaStrawRegenerationRate = 1.5f;

    [Slider("RefillerChip regeneration rate", 0.05f, 1f, DefaultValue = 0.05f, Step = 0.01f, Format = "{0:F2}")]
    public float RefillerChipRegenerationRate = 0.05f;

    [Slider("Consume", 1f, 100f, DefaultValue = 12f)]
    public float Consume = 12f;
}
