using System;
using System.Collections.Generic;
using Bombarder.Entities;
using Bombarder.Particles;
using Microsoft.Xna.Framework;

namespace Bombarder.MagicEffects;

public class WideLaser : MagicEffect
{
    public override int ManaCost { get; protected set; } = 2;
    public const uint ManaCostInterval = 2;
    const int Damage = 4;
    const int DamageInterval = 3;

    public const int Range = 1500;
    public const int InitialDistance = 40;
    public const int Width = 60;
    public const int MarkerDistance = 75;

    public Color PrimaryColor = Color.Turquoise;
    public Color SecondaryColor = Color.White;
    public Color MarkerColor = Color.Red;
    public const float Opacity = 0.8F;

    public const int Spread = 1;
    public const float TrueSpreadMultiplier = 4.5F;

    public const int DefaultDuration = -1;

    public float Angle { get; set; }

    public WideLaser(Vector2 Position, Vector2 Destination) : base(Position)
    {
        Vector2 DestinationDiff = Destination - Position;
        Angle = Utils.ToDegrees(MathF.Atan2(DestinationDiff.Y, DestinationDiff.X));
        Duration = DefaultDuration;
    }

    public WideLaser(Vector2 Position, float Angle) : base(Position)
    {
        this.Angle = Angle;
        Duration = DefaultDuration;
    }

    public override void EnactEffect(Player Player, List<Entity> Entities, uint GameTick)
    {
        if (!Player.CheckUseMana(ManaCost))
        {
            return;
        }

        EnactDamage(Player, Entities, GameTick);
        CreateParticles();
    }

    public override void Draw()
    {
        if (!BombarderGame.Instance.Settings.ShowDamageRadii)
        {
            return;
        }

        // Old
        // float AngleRadians = Utils.ToRadians(Angle);
        // float RightAngleRadians = Utils.ToRadians(Angle + 90);
        //
        // Vector2 LeftLine = new Vector2(
        //     BombarderGame.Instance.Graphics.PreferredBackBufferWidth / 2F - Width / 2F * MathF.Cos(RightAngleRadians),
        //     BombarderGame.Instance.Graphics.PreferredBackBufferHeight / 2F - Width / 2F * MathF.Sin(RightAngleRadians)
        // );
        // LeftLine.X += InitialDistance * MathF.Cos(AngleRadians);
        // LeftLine.Y += InitialDistance * MathF.Sin(AngleRadians);
        // Vector2 RightLine = new Vector2(
        //     BombarderGame.Instance.Graphics.PreferredBackBufferWidth / 2F + (Width / 2F - 5) * MathF.Cos(RightAngleRadians),
        //     BombarderGame.Instance.Graphics.PreferredBackBufferHeight / 2F + (Width / 2F - 5) * MathF.Sin(RightAngleRadians)
        // );
        // RightLine.X += InitialDistance * MathF.Cos(AngleRadians);
        // RightLine.Y += InitialDistance * MathF.Sin(AngleRadians);
        //
        //
        // BombarderGame.Instance.DrawRotatedTexture(LeftLine, BombarderGame.Instance.Textures.White, Width, Range, Angle + 90, false, PrimaryColor * Opacity);
        // BombarderGame.Instance.DrawRotatedTexture(LeftLine, BombarderGame.Instance.Textures.White, 5, Range, Angle + 90, false, SecondaryColor);
        // BombarderGame.Instance.DrawRotatedTexture(RightLine, BombarderGame.Instance.Textures.White, 5, Range, Angle + 90, false, SecondaryColor);
        //
        // float Scale = (float)Width / BombarderGame.Instance.Textures.HalfWhiteCirlce.Width;
        // BombarderGame.Instance.DrawRotatedTexture(LeftLine, BombarderGame.Instance.Textures.HalfWhiteCirlce, Scale, Scale, Angle + 90, true, PrimaryColor * Opacity);

        // New accounting for Laser Spread
        float TrueSpread = Spread * TrueSpreadMultiplier;
        float AngleRadians = Utils.ToRadians(Angle);
        float AngleRadiansLeft = Utils.ToRadians(Angle - TrueSpread);
        float AngleRadiansRight = Utils.ToRadians(Angle + TrueSpread);

        Vector2 Start = new(
            Position.X + BombarderGame.Instance.Graphics.PreferredBackBufferWidth / 2F -
            BombarderGame.Instance.Player.Position.X,
            Position.Y + BombarderGame.Instance.Graphics.PreferredBackBufferHeight / 2F -
            BombarderGame.Instance.Player.Position.Y
        );

        BombarderGame.Instance.DrawLine(Start, Range, AngleRadiansLeft, SecondaryColor, 10);
        BombarderGame.Instance.DrawLine(Start, Range, AngleRadians, MarkerColor, 10);
        BombarderGame.Instance.DrawLine(Start, Range, AngleRadiansRight, SecondaryColor, 10);
    }

    private void EnactDamage(Player Player, List<Entity> Entities, uint Tick)
    {
        float AngleRadians = Utils.ToRadians(Angle);

        float XDiff;
        float YDiff;
        float Distance;

        float RotatedX;
        float RotatedY;
        float CurrentLaserWidth;
        float EntityStartX;
        float EntityStartY;


        if (Tick % DamageInterval != 0)
        {
            return;
        }

        //Calculate radius of the Lasers Spread every 1 Distance
        float SpreadValue = MathF.Sin(Utils.ToRadians(Spread));

        foreach (Entity Entity in Entities)
        {
            XDiff = Math.Abs(Position.X - Entity.Position.X);
            YDiff = Math.Abs(Position.Y - Entity.Position.Y);
            Distance = Utils.HypotF(XDiff, YDiff);

            if (Distance >= Range)
            {
                continue;
            }

            // Entity is close enough to the laser
            // Point along laser with equal distance as Entity
            RotatedX = Position.X + Distance * MathF.Cos(AngleRadians);
            RotatedY = Position.Y + Distance * MathF.Sin(AngleRadians);

            EntityStartX = Entity.Position.X + Entity.HitBoxOffset.X;
            EntityStartY = Entity.Position.Y + Entity.HitBoxOffset.Y;

            //Calculate radius of the Lasers Current spread
            CurrentLaserWidth = SpreadValue * Distance * TrueSpreadMultiplier;

            if (RotatedX >= EntityStartX - CurrentLaserWidth &&
                RotatedX <= EntityStartX + Entity.HitBoxSize.X + CurrentLaserWidth &&
                RotatedY >= EntityStartY - CurrentLaserWidth &&
                RotatedY <= EntityStartY + Entity.HitBoxSize.Y + CurrentLaserWidth)
            {
                Entity.GiveDamage(Damage);
            }
        }
    }

    public void CreateParticles()
    {
        float AngleRadians;
        int Count = BombarderGame.random.Next(1, 4);

        // Central Laser
        AngleRadians = Utils.ToRadians(Angle + (float)BombarderGame.random.Next(-Spread * 10, Spread * 10) / 10);
        BombarderGame.Instance.Particles.Add(new LaserLine(Position.Copy(), AngleRadians)
        {
            HasDuration = true,
            Duration = LaserLine.DurationMin,
            Length = LaserLine.LengthMax,
            Thickness = LaserLine.ThicknessMax,
            Speed = LaserLine.SpeedMax * 2,
            Colour = LaserLine.CentralLaserColor
        });

        // Others
        for (int i = 0; i < Count; i++)
        {
            AngleRadians = Utils.ToRadians(
                Angle + BombarderGame.random.Next(-LaserLine.AngleSpreadRange, LaserLine.AngleSpreadRange)
            );

            BombarderGame.Instance.Particles.Add(new LaserLine(Position.Copy(), AngleRadians)
            {
                HasDuration = true,
                Duration = BombarderGame.random.Next(LaserLine.DurationMin, LaserLine.DurationMax)
            });
        }
    }
}