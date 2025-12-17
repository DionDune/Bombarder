using Microsoft.Xna.Framework;

namespace Bombarder;

public class Settings
{
    public float CursorSizeMultiplier { get; set; } = 1.66F;
    public bool ShowGrid { get; set; } = true;
    public float GridOpacityMultiplier { get; set; } = 0.15F;
    public float GridSizeMultiplier { get; set; } = 1;
    public Color GridColor { get; set; } = Color.White;
    public Color BackgroundColor { get; set; } = Color.Black;
    public int GridLineSizeMult { get; set; } = 1;

    public bool ShowDamageRadii { get; set; } = false;
    public bool ShowHitBoxes { get; set; } = false;

    public bool TranceMode { get; set; } = false;
    public Color TranceModeGridColor { get; set; } = Color.Black;
    public int TranceModeGridLineMult { get; set; } = 100;
    public bool TranceModeClearScreen { get; set; } = false;

    public bool RunEntityAI { get; set; } = true;
}