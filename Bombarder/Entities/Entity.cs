using System;
using System.Collections.Generic;
using System.Linq;
using Bombarder.Particles;
using Microsoft.Xna.Framework;

namespace Bombarder.Entities;

public abstract class Entity
{
    public Vector2 Position { get; set; } = Vector2.Zero;

    public float Health { get; set; }
    public float HealthMax { get; set; }
    public bool HealthBarVisible { get; set; }
    public uint LastHitMarkerFrame;
    public bool ChaseMode { get; set; }
    public float Direction { get; set; }

    public List<EntityBlock> Parts { get; set; }

    public Point HitBoxOffset { get; set; }
    public Point HitBoxSize { get; set; }
    public Point HealthBarOffset { get; set; }
    public Point HealthBarDimensions { get; set; }
    public int KillHealthReward { get; protected set; }
    public int KillManaReward { get; protected set; }
    public Rectangle HitBox => new(Position.ToPoint() + HitBoxOffset, HitBoxSize);

    public void MoveTowards(Vector2 Goal, float Speed)
    {
        float XDifference = Position.X - Goal.X;
        float YDifference = Position.Y - Goal.Y;
        float Angle = (float)Math.Atan2(YDifference, XDifference);

        Direction = Angle;
        Vector2 PositionChange = new Vector2(
            Speed * (float)Math.Cos(Angle),
            Speed * (float)Math.Sin(Angle)
        );

        Position -= PositionChange;
    }

    public void MoveAwayFrom(Vector2 TargetPosition, float Speed)
    {
        float XDifference = Position.X - TargetPosition.X;
        float YDifference = Position.Y - TargetPosition.Y;
        float Angle = (float)Math.Atan2(YDifference, XDifference);

        Direction = Angle;
        Vector2 PositionChange = new Vector2(
            Speed * (float)Math.Cos(Angle),
            Speed * (float)Math.Sin(Angle)
        );

        Position += PositionChange;
    }

    public void GiveDamage(int Damage)
    {
        Health -= Damage;

        ApplyHitMarker();
    }

    public void ApplyHitMarker()
    {
        if (Math.Abs(BombarderGame.Instance.GameTick - LastHitMarkerFrame) <= 20)
        {
            return;
        }

        int x = BombarderGame.random.Next(
            (int)Position.X + HitBoxOffset.X,
            (int)Position.X + HitBoxOffset.X + HitBoxSize.X
        );
        int y = BombarderGame.random.Next(
            (int)Position.Y + HitBoxOffset.Y,
            (int)Position.Y + HitBoxOffset.Y + HitBoxSize.Y
        );

        BombarderGame.Instance.Particles.Add(new HitMarker(new Vector2(x, y))
        {
            HasDuration = true,
            Duration = HitMarker.DefaultDuration
        });

        LastHitMarkerFrame = BombarderGame.Instance.GameTick;
    }

    public static void PurgeDead(List<Entity> Entities, Player Player)
    {
        List<Entity> DeadEntities = Entities.Where(Entity => Entity.Health <= 0).ToList();

        foreach (var Entity in DeadEntities)
        {
            if (Entity is RedCube RedCube)
            {
                RedCube.CreateDeathParticles();
            }

            Entity.GiveKillReward(Player);
            Entities.Remove(Entity);
        }
    }

    public void GiveKillReward(Player Player)
    {
        Player.GiveHealth(KillHealthReward);
        Player.GiveMana(KillManaReward);
    }

    public abstract void EnactAI(Player Player);

    public void Draw()
    {
        DrawEntity();
        DrawHitBoxes();
        DrawHealthBar();
    }

    public abstract void DrawEntity();

    public void DrawHitBoxes()
    {
        if (!BombarderGame.Instance.Settings.ShowHitBoxes)
        {
            return;
        }

        // Top Line
        BombarderGame.Instance.SpriteBatch.Draw(
            BombarderGame.Instance.Textures.White,
            new Rectangle(
                (int)(
                    Position.X +
                    HitBoxOffset.X +
                    BombarderGame.Instance.Graphics.PreferredBackBufferWidth / 2F -
                    BombarderGame.Instance.Player.Position.X
                ),
                (int)(
                    Position.Y +
                    HitBoxOffset.Y +
                    BombarderGame.Instance.Graphics.PreferredBackBufferHeight / 2F -
                    BombarderGame.Instance.Player.Position.Y
                ),
                HitBoxSize.X,
                2
            ),
            Color.White
        );

        // Bottom Line
        BombarderGame.Instance.SpriteBatch.Draw(
            BombarderGame.Instance.Textures.White,
            new Rectangle(
                (int)(
                    Position.X +
                    HitBoxOffset.X +
                    BombarderGame.Instance.Graphics.PreferredBackBufferWidth / 2F -
                    BombarderGame.Instance.Player.Position.X
                ),
                (int)(
                    Position.Y +
                    HitBoxOffset.Y +
                    HitBoxSize.Y +
                    BombarderGame.Instance.Graphics.PreferredBackBufferHeight / 2F -
                    BombarderGame.Instance.Player.Position.Y
                ),
                HitBoxSize.X,
                2
            ),
            Color.White
        );

        // Left Line
        BombarderGame.Instance.SpriteBatch.Draw(
            BombarderGame.Instance.Textures.White,
            new Rectangle(
                (int)(
                    Position.X +
                    HitBoxOffset.X +
                    BombarderGame.Instance.Graphics.PreferredBackBufferWidth / 2F -
                    BombarderGame.Instance.Player.Position.X
                ),
                (int)(Position.Y +
                      HitBoxOffset.Y +
                      BombarderGame.Instance.Graphics.PreferredBackBufferHeight / 2F -
                      BombarderGame.Instance.Player.Position.Y
                ),
                2,
                HitBoxSize.Y
            ),
            Color.White
        );

        // Right Line
        BombarderGame.Instance.SpriteBatch.Draw(
            BombarderGame.Instance.Textures.White,
            new Rectangle(
                (int)(
                    Position.X +
                    HitBoxOffset.X +
                    HitBoxSize.X +
                    BombarderGame.Instance.Graphics.PreferredBackBufferWidth / 2F -
                    BombarderGame.Instance.Player.Position.X),
                (int)(
                    Position.Y +
                    HitBoxOffset.Y +
                    BombarderGame.Instance.Graphics.PreferredBackBufferHeight / 2F -
                    BombarderGame.Instance.Player.Position.Y
                ),
                2,
                HitBoxSize.Y
            ),
            Color.White
        );
    }

    public void DrawHealthBar()
    {
        if (!HealthBarVisible)
        {
            return;
        }

        // Empty Health Bar
        BombarderGame.Instance.SpriteBatch.Draw(
            BombarderGame.Instance.Textures.White,
            new Rectangle(
                (int)(
                    Position.X +
                    HealthBarOffset.X +
                    BombarderGame.Instance.Graphics.PreferredBackBufferWidth / 2F -
                    BombarderGame.Instance.Player.Position.X
                ),
                (int)(
                    Position.Y +
                    HealthBarOffset.Y +
                    BombarderGame.Instance.Graphics.PreferredBackBufferHeight / 2F -
                    BombarderGame.Instance.Player.Position.Y
                ),
                HealthBarDimensions.X,
                HealthBarDimensions.Y
            ),
            Color.LightGray
        );

        // Filled Health Bar
        BombarderGame.Instance.SpriteBatch.Draw(
            BombarderGame.Instance.Textures.White,
            new Rectangle(
                (int)(
                    Position.X +
                    HealthBarOffset.X +
                    BombarderGame.Instance.Graphics.PreferredBackBufferWidth / 2F -
                    BombarderGame.Instance.Player.Position.X + 2
                ),
                (int)(
                    Position.Y +
                    HealthBarOffset.Y +
                    BombarderGame.Instance.Graphics.PreferredBackBufferHeight / 2F -
                    BombarderGame.Instance.Player.Position.Y + 2
                ),
                (int)(
                    (HealthBarDimensions.X - 4) * (Health / HealthMax)
                ),
                HealthBarDimensions.Y - 4
            ),
            Color.Green);
    }
}