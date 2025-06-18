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

    public static readonly Dictionary<string, Func<Vector2, Entity>> EntityFactories = new()
    {
        { "CubeMother", Position => new CubeMother(Position) },
        { "DemonEye", Position => new DemonEye(Position) },
        { "RedCube", Position => new RedCube(Position) },
        { "Spider", Position => new Spider(Position) },
    };

    protected Entity(Vector2 Position)
    {
        this.Position = Position;
    }

    public void MoveTowards(Vector2 Goal, float Distance)
    {
        float XDifference = Position.X - Goal.X;
        float YDifference = Position.Y - Goal.Y;
        float Angle = MathF.Atan2(YDifference, XDifference);

        Direction = Angle;
        Vector2 PositionChange = new Vector2(
            Distance * MathF.Cos(Angle),
            Distance * MathF.Sin(Angle)
        );

        Position -= PositionChange;
    }

    public void MoveAwayFrom(Vector2 TargetPosition, float Distance)
    {
        float XDifference = Position.X - TargetPosition.X;
        float YDifference = Position.Y - TargetPosition.Y;
        float Angle = MathF.Atan2(YDifference, XDifference);

        Direction = Angle;
        Vector2 PositionChange = new Vector2(
            Distance * MathF.Cos(Angle),
            Distance * MathF.Sin(Angle)
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

        int x = RngUtils.Random.Next(
            (int)Position.X + HitBoxOffset.X,
            (int)Position.X + HitBoxOffset.X + HitBoxSize.X
        );
        int y = RngUtils.Random.Next(
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
                Position.ToPoint() +
                HitBoxOffset +
                BombarderGame.Instance.ScreenCenter.ToPoint() -
                BombarderGame.Instance.Player.Position.ToPoint(),
                new Point(HitBoxSize.X, 2)
            ),
            Color.White
        );

        // Bottom Line
        BombarderGame.Instance.SpriteBatch.Draw(
            BombarderGame.Instance.Textures.White,
            new Rectangle(
                Position.ToPoint() +
                HitBoxOffset +
                BombarderGame.Instance.ScreenCenter.ToPoint() -
                BombarderGame.Instance.Player.Position.ToPoint() +
                new Point(0, HitBoxSize.Y),
                new Point(HitBoxSize.X, 2)
            ),
            Color.White
        );

        // Left Line
        BombarderGame.Instance.SpriteBatch.Draw(
            BombarderGame.Instance.Textures.White,
            new Rectangle(
                Position.ToPoint() +
                HitBoxOffset +
                BombarderGame.Instance.ScreenCenter.ToPoint() -
                BombarderGame.Instance.Player.Position.ToPoint(),
                new Point(2, HitBoxSize.Y)
            ),
            Color.White
        );

        // Right Line
        BombarderGame.Instance.SpriteBatch.Draw(
            BombarderGame.Instance.Textures.White,
            new Rectangle(
                Position.ToPoint() +
                HitBoxOffset +
                BombarderGame.Instance.ScreenCenter.ToPoint() -
                BombarderGame.Instance.Player.Position.ToPoint() +
                new Point(HitBoxSize.X, 0),
                new Point(2, HitBoxSize.Y)
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
        
        var Game = BombarderGame.Instance;

        // Empty Health Bar
        Game.SpriteBatch.Draw(
            Game.Textures.White,
            MathUtils.CreateRectangle(
                Position +
                HealthBarOffset.ToVector2() +
                Game.ScreenCenter -
                Game.Player.Position,
                HealthBarDimensions.ToVector2()
            ),
            Color.LightGray
        );

        // Filled Health Bar
        Game.SpriteBatch.Draw(
            Game.Textures.White,
            MathUtils.CreateRectangle(
                Position + HealthBarOffset.ToVector2() + Game.ScreenCenter - Game.Player.Position + new Vector2(2, 2),
                new Vector2((HealthBarDimensions.X - 4) * (Health / HealthMax), HealthBarDimensions.Y - 4)
                ),
            Color.Green);
    }
}