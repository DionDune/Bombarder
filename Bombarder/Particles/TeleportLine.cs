using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework;

namespace Bombarder.Particles;

public class TeleportLine : Particle
{
    private static readonly (int Min, int Max) LengthRange = (5, 25);
    private static readonly (int Min, int Max) ThicknessRange = (3, 6);
    private const int AngleSpreadAllowance = 8;
    private static readonly (int Min, int Max) DurationRange = (30, 30);
    private static readonly (int Min, int Max) MovementSpeedRange = (5, 13);

    public static readonly IList<Color> Colours = new ReadOnlyCollection<Color>
        (new List<Color> { Color.Purple, Color.MediumPurple, Color.Turquoise });

    public const float OpacityDefault = 0;
    public const float OpacityIncreasingChange = 0.15F;
    public const int OpacityIncreaseInterval = 1;
    public const float OpacityDecreasingChange = 0.075F;
    public const int OpacityDecreasingInterval = 3;
    public bool OpacityIncreasing = true;

    public float Length { get; set; }
    public float Thickness { get; set; }
    public float MovementAngle { get; set; }
    private float MovementSpeed;
    public Color Colour { get; set; }
    public float Opacity { get; set; }


    public TeleportLine(Vector2 Position) : base(Position)
    {
        DrawLater = true;
    }

    public override void Update(uint Tick)
    {
        base.Update(Tick);
        EnactMovement();
        EnactOpacityChange();
    }

    public override void Draw()
    {
        Vector2 DrawPosition = Position + BombarderGame.Instance.ScreenCenter - BombarderGame.Instance.Player.Position;
        RenderUtils.DrawLine(DrawPosition, Length, MovementAngle, Colour * Opacity, Thickness);
    }

    private void EnactOpacityChange()
    {
        if (OpacityIncreasing)
        {
            if (BombarderGame.Instance.GameTick % OpacityIncreaseInterval != 0)
            {
                return;
            }

            Opacity += OpacityIncreasingChange;

            if (Opacity >= 1)
            {
                OpacityIncreasing = false;
            }
        }
        else
        {
            if (BombarderGame.Instance.GameTick % OpacityDecreasingInterval != 0)
            {
                return;
            }

            Opacity -= OpacityDecreasingChange;

            if (Opacity > 0)
            {
                return;
            }

            Duration = 1;
            HasDuration = true;
        }
    }
    private void EnactMovement()
    {
        Position -= new Vector2(
                    MovementSpeed * MathF.Cos(MovementAngle),
                    MovementSpeed * MathF.Sin(MovementAngle)
                );
    }
    public static void Create(Vector2 Position, float Angle)
    {
        BombarderGame.Instance.World.Particles.Add(new TeleportLine(Position)
        {
            HasDuration = false,
            Duration = RngUtils.Random.Next(DurationRange.Min, DurationRange.Max),
            Length = RngUtils.Random.Next(LengthRange.Min, LengthRange.Max),
            Thickness = RngUtils.Random.Next(ThicknessRange.Min, ThicknessRange.Max),
            MovementAngle = Angle + MathUtils.ToRadians(RngUtils.Random.Next(-AngleSpreadAllowance, AngleSpreadAllowance)),
            MovementSpeed = (float)RngUtils.Random.Next(MovementSpeedRange.Min, MovementSpeedRange.Max),
            Colour = Colours[RngUtils.Random.Next(0, Colours.Count)],
            Opacity = OpacityDefault,
        });
    }
}