using System;
using System.Collections.Generic;
using Bombarder.Entities;
using Microsoft.Xna.Framework;

namespace Bombarder.MagicEffects;

public class ForceWave : MagicEffect
{
    public Color Colour = Color.Crimson;

    public static readonly int ManaCost = 150;

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

    public override void Draw(Game1 Game1)
    {
        Game1.SpriteBatch.Draw(
            Game1.Textures.WhiteCircle,
            new Rectangle(
                (int)(Position.X - Radius + Game1.Graphics.PreferredBackBufferWidth / 2F - Game1.Player.Position.X),
                (int)(Position.Y - Radius + Game1.Graphics.PreferredBackBufferHeight / 2F - Game1.Player.Position.Y),
                (int)Radius * 2,
                (int)Radius * 2
            ),
            Colour * 0.3F
        );

        for (int i = 0; i < BorderWidth; i++)
        {
            Game1.SpriteBatch.Draw(
                Game1.Textures.HollowCircle,
                new Rectangle(
                    (int)(Position.X - Radius + i + Game1.Graphics.PreferredBackBufferWidth / 2F -
                          Game1.Player.Position.X),
                    (int)(Position.Y - Radius + i + Game1.Graphics.PreferredBackBufferHeight / 2F -
                          Game1.Player.Position.Y),
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
            float XDiff = Math.Abs(Position.X - Entity.Position.X);
            float YDiff = Math.Abs(Position.Y - Entity.Position.Y);
            float Distance = (float)Math.Sqrt(Math.Pow(XDiff, 2) + Math.Pow(YDiff, 2));

            if (Distance > Radius)
            {
                continue;
            }

            float xDiff = Entity.Position.X - Position.X;
            float yDiff = Entity.Position.Y - Position.Y;
            float Angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);
            float AngleRadians = Angle * (float)(Math.PI / 180);

            Vector2 PositionChange = new Vector2(
                (Radius - Distance) * (float)Math.Cos(AngleRadians),
                (Radius - Distance) * (float)Math.Sin(AngleRadians)
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