using System;
using System.Collections.Generic;
using Bombarder.Entities;
using Microsoft.Xna.Framework;

namespace Bombarder.MagicEffects;

public class ForceWave : MagicEffect
{
    public Color Colour = Color.Crimson;
    public override int ManaCost { get; protected set; } = 150;
    public const int DefaultDuration = 200;
    public float Radius { get; set; }
    public const float RadiusMax = 200;
    private const float DefaultRadius = 5;
    private const float RadiusSpread = 10;
    public const float RadiusDecreaseMultiplier = 0.9F;
    public const int DurationSpreadCutoff = 5;

    public static readonly int BorderWidth = 20;

    public ForceWave(Vector2 Position) : base(Position)
    {
        Radius = DefaultRadius;
        Duration = DefaultDuration;
        HasDuration = true;
    }


    public override void Update(Player Player, List<Entity> Entities, uint GameTick)
    {
        base.Update(Player, Entities, GameTick);
        EnactSpread();
        EnactForce(Entities);
        EnactDuration();
    }

    public override void DrawEffect()
    {
        var Game = BombarderGame.Instance;

        Game.SpriteBatch.Draw(
            Game.Textures.WhiteCircle,
            MathUtils.CreateRectangle(
                Position - new Vector2(Radius) + Game.ScreenCenter - Game.Player.Position,
                new Vector2(Radius) * 2
            ),
            Colour * 0.3F
        );

        for (int i = 0; i < BorderWidth; i++)
        {
            Game.SpriteBatch.Draw(
                Game.Textures.HollowCircle,
                MathUtils.CreateRectangle(
                    Position - new Vector2(Radius - i) + Game.ScreenCenter - Game.Player.Position,
                    new Vector2(Radius - i) * 2
                ),
                Colour * 0.7F
            );
        }
    }

    private void EnactForce(List<Entity> Entities)
    {
        foreach (Entity Entity in Entities)
        {
            Vector2 Diff = MathUtils.Abs(Position - Entity.Position);
            float Distance = MathUtils.HypotF(Diff);

            if (Distance > Radius)
            {
                continue;
            }

            Vector2 InverseDiff = Entity.Position - Position;
            float Angle = MathF.Atan2(InverseDiff.Y, InverseDiff.X);

            Vector2 PositionChange = new Vector2(
                (Radius - Distance) * MathF.Cos(Angle),
                (Radius - Distance) * MathF.Sin(Angle)
            );

            Entity.Position += PositionChange;
        }
    }

    private void EnactSpread()
    {
        if (Duration <= DurationSpreadCutoff)
        {
            return;
        }

        if (Radius < RadiusMax)
        {
            Radius += RadiusSpread;
        }
    }

    private void EnactDuration()
    {
        if (Duration >= DurationSpreadCutoff)
        {
            return;
        }

        if (Radius > 1)
        {
            HasDuration = false;

            Radius *= RadiusDecreaseMultiplier;
        }
        else
        {
            HasDuration = true;
        }
    }
}