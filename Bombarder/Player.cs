using System;
using System.Collections.Generic;
using Bombarder.Entities;
using Bombarder.MagicEffects;
using Microsoft.Xna.Framework;

namespace Bombarder;

public class Player
{
    public Vector2 Position { get; set; }

    public int Width { get; set; }
    public int Height { get; set; }

    public Vector2 Momentum { get; set; }
    public float BaseSpeed { get; set; }
    public float BoostMultiplier { get; set; }
    public float Acceleration { get; set; }
    public float Slowdown { get; set; }
    public Rectangle HitBox => new((int)Position.X - Width / 2, (int)Position.Y - Height / 2, Width, Height);

    public bool IsInvincible;
    public bool IsDead = false;
    public int Health { get; set; }
    public int HealthMax { get; set; }
    public bool HealthInfinite = false;
    public const uint HealthRegainInterval = 4;
    public const int HealthRegainDefault = 4;
    public int HealthRegain = HealthRegainDefault;


    public int Mana { get; set; }
    public int ManaMax { get; set; }
    public bool ManaInfinite = false;
    public const uint ManaRegainInterval = 6;
    public const int ManaRegainDefault = 3;
    public int ManaRegain = ManaRegainDefault;

    public Point ManaBarDimensions { get; set; }
    public Orientation ManaBarScreenOrientation { get; set; }
    public Point ManaBarOffset { get; set; }
    public bool ManaBarHorizontalFill { get; set; }
    public bool ManaBarVisible { get; set; }

    public Point HealthBarDimensions { get; set; }
    public string HealthBarScreenOrientation { get; set; }
    public Point HealthBarOffset { get; set; }
    public bool HealthBarHorizontalFill { get; set; }
    public bool HealthBarVisible { get; set; }


    public Player()
    {
        Position = new Vector2();

        Width = 50;
        Height = 100;

        Health = 130;
        HealthMax = 150;

        Mana = 0;
        ManaMax = 750;


        Momentum = new Vector2();
        BaseSpeed = 5;
        BoostMultiplier = 1.85F;

        Acceleration = 0.45F;
        Slowdown = 0.75F;


        ManaBarDimensions = new Point(25, 450);
        ManaBarScreenOrientation = Orientation.BOTTOM_LEFT;
        ManaBarOffset = new Point(20, -20);
        ManaBarHorizontalFill = false;
        ManaBarVisible = true;
        HealthBarDimensions = new Point(25, 450);
        ManaBarScreenOrientation = Orientation.BOTTOM_LEFT;
        HealthBarOffset = new Point(75, -20);
        HealthBarVisible = true;
    }

    public void ToggleInvincibility()
    {
        IsInvincible = !IsInvincible;
    }

    public void SetDefaultStats()
    {
        Health = HealthMax;
        Mana = ManaMax;

        Momentum = new Vector2();
    }

    public void ResetPosition()
    {
        Position = new Vector2();
    }

    public void SetRandomLocalPosition(int MinDistance, int MaxDistance)
    {
        int Angle = BombarderGame.random.Next(0, 360);
        float AngleRadians = Angle * (float)(Math.PI / 180);
        int Distance = BombarderGame.random.Next(MinDistance, MaxDistance);

        Position += new Vector2(
            (int)(Distance * (float)Math.Cos(AngleRadians)),
            (int)(Distance * (float)Math.Sin(AngleRadians))
        );
    }

    public void Handler()
    {
        ManaHandler();
        HealthHandler();
    }

    private void HealthHandler()
    {
        if (Health < HealthMax && BombarderGame.Instance.GameTick % HealthRegainInterval == 0)
        {
            if (HealthMax - Health < HealthRegain)
            {
                Health = HealthMax;
            }
            else
            {
                Health += HealthRegain;
            }
        }

        if (Health <= 0)
        {
            IsDead = true;
        }
    }

    public void GiveDamage(int Damage)
    {
        if (IsInvincible)
        {
            return;
        }

        Health -= Damage;

        if (Health <= 0)
        {
            IsDead = true;
        }
    }

    public void GiveHealth(int Amount)
    {
        Health += Amount;

        if (Health > HealthMax)
        {
            Health = HealthMax;
        }
    }

    public void ManaHandler()
    {
        if (Mana >= ManaMax || BombarderGame.Instance.GameTick % ManaRegainInterval != 0)
        {
            return;
        }

        if (ManaMax - Mana < ManaRegain)
        {
            Mana = ManaMax;
        }
        else
        {
            Mana += ManaRegain;
        }
    }

    public bool CheckUseMana(int Cost)
    {
        if (ManaInfinite)
        {
            return true;
        }


        if (Mana <= Cost)
        {
            return false;
        }

        Mana -= Cost;

        return true;
    }

    public void GiveMana(int Amount)
    {
        Mana += Amount;

        if (Mana > ManaMax)
        {
            Mana = ManaMax;
        }
    }

    public void CreateMagic(Vector2 SpawnPosition, Type MagicType)
    {
        var Factory = MagicEffects.MagicEffect.MagicEffectsFactory.GetValueOrDefault(MagicType.Name);

        MagicEffect MagicEffect = Factory?.Invoke(SpawnPosition, this);

        if (MagicEffect == null || !CheckUseMana(MagicEffect.ManaCost))
        {
            return;
        }

        BombarderGame.Instance.MagicEffects.Add(MagicEffect);
    }
}