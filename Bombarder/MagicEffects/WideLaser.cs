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
        Angle = MathUtils.ToDegrees(MathF.Atan2(DestinationDiff.Y, DestinationDiff.X));
        Duration = DefaultDuration;
    }

    public WideLaser(Vector2 Position, float Angle) : base(Position)
    {
        this.Angle = Angle;
        Duration = DefaultDuration;
    }

    public override void Update(Player Player, List<Entity> Entities, uint GameTick)
    {
        base.Update(Player, Entities, GameTick);
        if (!Player.CheckUseMana(ManaCost))
        {
            return;
        }

        UpdatePosition();
        EnactDamage(Player, Entities, GameTick);
        CreateParticles();
    }

    private void UpdatePosition()
    {
        var Game = BombarderGame.Instance;
        var MousePosition = Game.MouseInput.Position;
        
        float XDiff = MousePosition.X - Game.Graphics.PreferredBackBufferWidth / 2F;
        float YDiff = MousePosition.Y - Game.Graphics.PreferredBackBufferHeight / 2F;
        Angle = MathUtils.ToDegrees(MathF.Atan2(YDiff, XDiff));
        Position = Game.Player.Position.Copy();
    }

    public override void DrawEffect()
    {
        if (!BombarderGame.Instance.Settings.ShowDamageRadii)
        {
            return;
        }

        // Old
        // float AngleRadians = MathUtils.ToRadians(Angle);
        // float RightAngleRadians = MathUtils.ToRadians(Angle + 90);
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
        float AngleRadians = MathUtils.ToRadians(Angle);
        float AngleRadiansLeft = MathUtils.ToRadians(Angle - TrueSpread);
        float AngleRadiansRight = MathUtils.ToRadians(Angle + TrueSpread);

        Vector2 Start = new(
            Position.X + BombarderGame.Instance.Graphics.PreferredBackBufferWidth / 2F -
            BombarderGame.Instance.Player.Position.X,
            Position.Y + BombarderGame.Instance.Graphics.PreferredBackBufferHeight / 2F -
            BombarderGame.Instance.Player.Position.Y
        );

        RenderUtils.DrawLine(Start, Range, AngleRadiansLeft, SecondaryColor, 10);
        RenderUtils.DrawLine(Start, Range, AngleRadians, MarkerColor, 10);
        RenderUtils.DrawLine(Start, Range, AngleRadiansRight, SecondaryColor, 10);
    }

    private void EnactDamage(Player Player, List<Entity> Entities, uint Tick)
    {
        float AngleRadians = MathUtils.ToRadians(Angle);

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
        float SpreadValue = MathF.Sin(MathUtils.ToRadians(Spread));

        foreach (Entity Entity in Entities)
        {
            XDiff = Math.Abs(Position.X - Entity.Position.X);
            YDiff = Math.Abs(Position.Y - Entity.Position.Y);
            Distance = MathUtils.HypotF(XDiff, YDiff);

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
        int Count = RngUtils.Random.Next(1, 4);

        // Central Laser
        AngleRadians = MathUtils.ToRadians(Angle + (float)RngUtils.Random.Next(-Spread * 10, Spread * 10) / 10);
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
            AngleRadians = MathUtils.ToRadians(
                Angle + RngUtils.Random.Next(-LaserLine.AngleSpreadRange, LaserLine.AngleSpreadRange)
            );

            BombarderGame.Instance.Particles.Add(new LaserLine(Position.Copy(), AngleRadians)
            {
                HasDuration = true,
                Duration = RngUtils.Random.Next(LaserLine.DurationMin, LaserLine.DurationMax)
            });
        }
    }
}