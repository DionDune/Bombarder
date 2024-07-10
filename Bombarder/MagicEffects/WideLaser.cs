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
        int Count = Game1.random.Next(1, 4);

        // Central Laser
        AngleRadians = (Angle + (float)Game1.random.Next(-Spread * 10, Spread * 10) / 10) * (float)(Math.PI / 180);
        Game1.Particles.Add(new LaserLine(new Vector2(Position.X, Position.Y), AngleRadians)
        {
            HasDuration = true,
            Duration = LaserLine.DurationMin,
            Length = LaserLine.LengthMax,
            Thickness = LaserLine.ThicknessMax,
            Speed = LaserLine.SpeedMax * 2,
            Colour = LaserLine.CentralLaserColor
        });

        //Others
        for (int i = 0; i < Count; i++)
        {
            AngleRadians = (Angle + Game1.random.Next(-LaserLine.AngleSpreadRange, LaserLine.AngleSpreadRange)) *
                           (float)(Math.PI / 180);

            Game1.Particles.Add(new LaserLine(new Vector2(Position.X, Position.Y), AngleRadians)
            {
                HasDuration = true,
                Duration = Game1.random.Next(LaserLine.DurationMin, LaserLine.DurationMax)
            });
        }
    }
}