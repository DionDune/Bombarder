using System;
using System.Collections.Generic;
using Bombarder.Entities;
using Bombarder.Particles;
using Microsoft.Xna.Framework;

namespace Bombarder.MagicEffects;

public class WideLaser : MagicEffect
{
    public const int ManaCost = 2;
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
        // float AngleRadians = Angle * (float)(Math.PI / 180);
        // float RightAngleRadians = (Angle + 90) * (float)(Math.PI / 180);
        //
        // Vector2 LeftLine = new Vector2(
        //     BombarderGame.Instance.Graphics.PreferredBackBufferWidth / 2F - Width / 2F * (float)Math.Cos(RightAngleRadians),
        //     BombarderGame.Instance.Graphics.PreferredBackBufferHeight / 2F - Width / 2F * (float)Math.Sin(RightAngleRadians)
        // );
        // LeftLine.X += InitialDistance * (float)Math.Cos(AngleRadians);
        // LeftLine.Y += InitialDistance * (float)Math.Sin(AngleRadians);
        // Vector2 RightLine = new Vector2(
        //     BombarderGame.Instance.Graphics.PreferredBackBufferWidth / 2F + (Width / 2F - 5) * (float)Math.Cos(RightAngleRadians),
        //     BombarderGame.Instance.Graphics.PreferredBackBufferHeight / 2F + (Width / 2F - 5) * (float)Math.Sin(RightAngleRadians)
        // );
        // RightLine.X += InitialDistance * (float)Math.Cos(AngleRadians);
        // RightLine.Y += InitialDistance * (float)Math.Sin(AngleRadians);
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
        float AngleRadians = Angle * (float)(Math.PI / 180);
        float AngleRadiansLeft = (Angle - TrueSpread) * (float)(Math.PI / 180);
        float AngleRadiansRight = (Angle + TrueSpread) * (float)(Math.PI / 180);

        Vector2 Start = new(
            Position.X + BombarderGame.Instance.Graphics.PreferredBackBufferWidth / 2F - BombarderGame.Instance.Player.Position.X,
            Position.Y + BombarderGame.Instance.Graphics.PreferredBackBufferHeight / 2F - BombarderGame.Instance.Player.Position.Y
        );

        BombarderGame.Instance.DrawLine(Start, Range, AngleRadiansLeft, SecondaryColor, 10);
        BombarderGame.Instance.DrawLine(Start, Range, AngleRadians, MarkerColor, 10);
        BombarderGame.Instance.DrawLine(Start, Range, AngleRadiansRight, SecondaryColor, 10);
    }

    private void EnactDamage(Player Player, List<Entity> Entities, uint Tick)
    {
        float AngleRadians = Angle * (float)(Math.PI / 180);

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
        float SpreadValue = (float)Math.Sin(Spread * (float)(Math.PI / 180));

        foreach (Entity Entity in Entities)
        {
            XDiff = Math.Abs(Position.X - Entity.Position.X);
            YDiff = Math.Abs(Position.Y - Entity.Position.Y);
            Distance = (float)Math.Sqrt(Math.Pow(XDiff, 2) + Math.Pow(YDiff, 2));

            if (!(Distance < Range))
            {
                continue;
            }

            // Entity is close enough to the laser
            // Point along laser with equal distance as Entity
            RotatedX = Position.X + Distance * (float)Math.Cos(AngleRadians);
            RotatedY = Position.Y + Distance * (float)Math.Sin(AngleRadians);

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
        AngleRadians = (Angle + (float)BombarderGame.random.Next(-Spread * 10, Spread * 10) / 10) * (float)(Math.PI / 180);
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
            AngleRadians = (Angle + BombarderGame.random.Next(-LaserLine.AngleSpreadRange, LaserLine.AngleSpreadRange)) *
                           (float)(Math.PI / 180);

            BombarderGame.Instance.Particles.Add(new LaserLine(Position.Copy(), AngleRadians)
            {
                HasDuration = true,
                Duration = BombarderGame.random.Next(LaserLine.DurationMin, LaserLine.DurationMax)
            });
        }
    }
}