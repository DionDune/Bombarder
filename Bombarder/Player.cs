using System;
using System.Collections.Generic;
using System.Linq;
using Bombarder.MagicEffects;
using Bombarder.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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
    public Orientation HealthBarScreenOrientation { get; set; }
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
        HealthBarScreenOrientation = Orientation.BOTTOM_LEFT;
        HealthBarOffset = new Point(75, -20);
        HealthBarHorizontalFill = false;
        HealthBarVisible = true;
    }

    public void Update()
    {
        HandleKeypress();
        Position += Momentum;
        UpdateMana();
        UpdateHealth();
        CheckDeath();
    }

    public void Draw()
    {
        var Game = BombarderGame.Instance;

        Game.SpriteBatch.Draw(
            Game.Textures.White,
            new Rectangle(
                Game.Graphics.PreferredBackBufferWidth / 2 - Width / 2,
                Game.Graphics.PreferredBackBufferHeight / 2 - Height / 2,
                Width,
                Height
            ),
            Color.Red
        );
    }
    
    public void DrawBars()
    {
        DrawHealthBar();
        DrawManaBar();
    }

    public void DrawHealthBar()
    {
        if (HealthInfinite || Health >= HealthMax)
        {
            return;
        }

        var Game = BombarderGame.Instance;

        Point OrientPos = ManaBarScreenOrientation.ToPoint();
        float HealthPercent = (float)Health / HealthMax;

        Point HealthBarContainerPos = new Point(
            OrientPos.X + HealthBarOffset.X - 2,
            OrientPos.Y + HealthBarOffset.Y - HealthBarDimensions.Y - 2
        );
        Point HealthBarPos = new Point(
            OrientPos.X + HealthBarOffset.X,
            OrientPos.Y + HealthBarOffset.Y - (int)(HealthBarDimensions.Y * HealthPercent)
        );
        Point HealthBarDimensionsWithOffset = new Point(
            HealthBarDimensions.X + 4,
            HealthBarDimensions.Y + 4
        );
        
        Game.SpriteBatch.Draw(
            Game.Textures.White,
            new Rectangle(HealthBarContainerPos, HealthBarDimensionsWithOffset),
            Color.White * 0.3F
        );

        RenderUtils.RenderOutline(
            Game.Textures.White,
            Color.White,
            HealthBarContainerPos,
            HealthBarDimensionsWithOffset.X,
            HealthBarDimensionsWithOffset.Y,
            2,
            1F
        );

        Game.SpriteBatch.Draw(
            Game.Textures.White,
            new Rectangle(
                HealthBarPos.X,
                HealthBarPos.Y,
                HealthBarDimensions.X,
                (int)(HealthBarDimensions.Y * HealthPercent)
            ),
            Color.Red
        );
    }

    public void DrawManaBar()
    {
        if (ManaInfinite || Mana >= ManaMax)
        {
            return;
        }

        var Game = BombarderGame.Instance;

        Point OrientPos = ManaBarScreenOrientation.ToPoint();
        float ManaPercent = (float)Mana / ManaMax;

        Point ManaContainerPos = new Point(
            OrientPos.X + ManaBarOffset.X - 2,
            OrientPos.Y + ManaBarOffset.Y - ManaBarDimensions.Y - 2
        );
        Point ManaBarPos = new Point(
            OrientPos.X + ManaBarOffset.X,
            OrientPos.Y + ManaBarOffset.Y - (int)(ManaBarDimensions.Y * ManaPercent)
        );
        Point ManaBarDimensionsWithOffset = new Point(
            ManaBarDimensions.X + 4,
            ManaBarDimensions.Y + 4
        );

        Game.SpriteBatch.Draw(
            Game.Textures.White,
            new Rectangle(ManaContainerPos, ManaBarDimensionsWithOffset),
            Color.White * 0.3F
        );
        RenderUtils.RenderOutline(
            Game.Textures.White,
            Color.White,
            ManaContainerPos,
            ManaBarDimensionsWithOffset.X,
            ManaBarDimensionsWithOffset.Y,
            2,
            1F
        );

        Game.SpriteBatch.Draw(
            Game.Textures.White,
            new Rectangle(
                ManaBarPos.X,
                ManaBarPos.Y,
                ManaBarDimensions.X,
                (int)(ManaBarDimensions.Y * ManaPercent)
            ),
            Color.Blue
        );
    }

    public void HandleKeypress()
    {
        bool? HeadingUp = null;
        bool? HeadingLeft = null;

        var KeyboardInput = BombarderGame.Instance.KeyboardInput;

        const float unBoostDivider = 1.05f;

        float Speed = BaseSpeed;
        if (KeyboardInput.IsKeyDown(Keys.LeftShift))
        {
            Speed = BaseSpeed * BoostMultiplier;
        }
        else if (Momentum.LengthSquared() > BaseSpeed * BaseSpeed)
        {
            // Player is now slowing down from boost
            Speed = Momentum.Length() / unBoostDivider;
        }

        Vector2 AccelerationVector = new();

        // Upward
        if (KeyboardInput.IsKeyDown(Keys.W))
        {
            AccelerationVector -= Vector2.UnitY;

            if (KeyboardInput.IsKeyUp(Keys.S))
            {
                HeadingUp = true;
            }
        }

        // Downward
        if (KeyboardInput.IsKeyDown(Keys.S))
        {
            AccelerationVector += Vector2.UnitY;
            if (KeyboardInput.IsKeyUp(Keys.W))
            {
                HeadingUp = false;
            }
        }

        // Left
        if (KeyboardInput.IsKeyDown(Keys.A))
        {
            AccelerationVector -= Vector2.UnitX;
            if (KeyboardInput.IsKeyUp(Keys.D))
            {
                HeadingLeft = true;
            }
        }

        // Right
        if (KeyboardInput.IsKeyDown(Keys.D))
        {
            AccelerationVector += Vector2.UnitX;
            if (KeyboardInput.IsKeyUp(Keys.A))
            {
                HeadingLeft = false;
            }
        }

        if (AccelerationVector != Vector2.Zero)
        {
            AccelerationVector.Normalize();
            AccelerationVector *= Acceleration;
        }

        Vector2 DecelerationVector = new();

        // Slowdown
        if (KeyboardInput.IsKeyUp(Keys.W) && KeyboardInput.IsKeyUp(Keys.S) && Momentum.Y != 0)
        {
            DecelerationVector.Y = MathF.Sign(Momentum.Y);
        }

        if (KeyboardInput.IsKeyUp(Keys.A) && KeyboardInput.IsKeyUp(Keys.D) && Momentum.X != 0)
        {
            DecelerationVector.X = MathF.Sign(Momentum.X);
        }

        if (DecelerationVector != Vector2.Zero)
        {
            DecelerationVector.Normalize();
            DecelerationVector *= Slowdown;
            // Do not decelerate player past 0
            if (MathF.Abs(DecelerationVector.X) > MathF.Abs(Momentum.X))
            {
                DecelerationVector.X = Momentum.X;
            }

            if (MathF.Abs(DecelerationVector.Y) > MathF.Abs(Momentum.Y))
            {
                DecelerationVector.Y = Momentum.Y;
            }
        }

        Momentum += AccelerationVector - DecelerationVector;

        if (HeadingUp != null && HeadingLeft != null)
        {
            if ((HeadingUp == true && Momentum.Y > 0) || (HeadingUp == false && Momentum.Y < 0))
            {
                Momentum += new Vector2(0, AccelerationVector.Y - DecelerationVector.Y);
            }

            if ((HeadingLeft == true && Momentum.X > 0) || (HeadingLeft == false && Momentum.X < 0))
            {
                Momentum += new Vector2(AccelerationVector.X - DecelerationVector.X, 0);
            }
        }

        // Clamp momentum magnitude if it is greater than max speed
        float LengthSquared = Momentum.LengthSquared();
        if (LengthSquared <= Speed * Speed)
        {
            return;
        }

        Momentum = Vector2.Normalize(Momentum);
        Momentum *= Speed;
    }

    private void CheckDeath()
    {
        if (!IsDead)
        {
            return;
        }

        IsDead = false;
        Health = HealthMax;
        BombarderGame.Instance.ChangePage("DeathPage");
    }

    public void ToggleInvincibility()
    {
        IsInvincible = !IsInvincible;
    }

    public void ToggleInfiniteMana()
    {
        ManaInfinite = !ManaInfinite;
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
        int Angle = RngUtils.Random.Next(0, 360);
        float AngleRadians = MathUtils.ToRadians(Angle);
        int Distance = RngUtils.Random.Next(MinDistance, MaxDistance);

        Position += new Vector2(
            (int)(Distance * MathF.Cos(AngleRadians)),
            (int)(Distance * MathF.Sin(AngleRadians))
        );
    }

    private void UpdateHealth()
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

    public void UpdateMana()
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

    public void CreateMagic<T>(Vector2 SpawnPosition) where T : MagicEffect
    {
        var Factory = MagicEffects.MagicEffect.MagicEffectsFactories.GetValueOrDefault(typeof(T).Name);

        MagicEffect MagicEffect = Factory?.Invoke(SpawnPosition, this);

        if (MagicEffect == null || !CheckUseMana(MagicEffect.ManaCost))
        {
            return;
        }

        BombarderGame.Instance.MagicEffects.Add(MagicEffect);
    }
}