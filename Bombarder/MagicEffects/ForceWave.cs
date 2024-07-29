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


    public override void EnactEffect(Player Player, List<Entity> Entities, uint GameTick)
    {
        EnactSpread();
        EnactForce(Entities);
        EnactDuration();
    }

    public override void Draw()
    {
        BombarderGame.Instance.SpriteBatch.Draw(
            BombarderGame.Instance.Textures.WhiteCircle,
            new Rectangle(
                (int)(
                    Position.X -
                    Radius +
                    BombarderGame.Instance.Graphics.PreferredBackBufferWidth / 2F -
                    BombarderGame.Instance.Player.Position.X
                ),
                (int)(
                    Position.Y -
                    Radius +
                    BombarderGame.Instance.Graphics.PreferredBackBufferHeight / 2F -
                    BombarderGame.Instance.Player.Position.Y
                ),
                (int)Radius * 2,
                (int)Radius * 2
            ),
            Colour * 0.3F
        );

        for (int i = 0; i < BorderWidth; i++)
        {
            BombarderGame.Instance.SpriteBatch.Draw(
                BombarderGame.Instance.Textures.HollowCircle,
                new Rectangle(
                    (int)(
                        Position.X -
                        Radius +
                        i +
                        BombarderGame.Instance.Graphics.PreferredBackBufferWidth / 2F -
                        BombarderGame.Instance.Player.Position.X
                    ),
                    (int)(
                        Position.Y -
                        Radius +
                        i +
                        BombarderGame.Instance.Graphics.PreferredBackBufferHeight / 2F -
                        BombarderGame.Instance.Player.Position.Y
                    ),
                    (int)Radius * 2 - i * 2,
                    (int)Radius * 2 - i * 2
                ),
                Colour * 0.7F
            );
        }
    }

    private void EnactForce(List<Entity> Entities)
    {
        foreach (Entity Entity in Entities)
        {
            Vector2 Diff = Utils.Abs(Position - Entity.Position);
            float Distance = Utils.HypotF(Diff);

            if (Distance > Radius)
            {
                continue;
            }

            Vector2 InverseDiff = Entity.Position - Position;
            float Angle = (MathF.Atan2(InverseDiff.Y, InverseDiff.X) * 180F / MathF.PI);
            float AngleRadians = Angle * MathF.PI / 180F;

            Vector2 PositionChange = new Vector2(
                (Radius - Distance) * MathF.Cos(AngleRadians),
                (Radius - Distance) * MathF.Sin(AngleRadians)
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