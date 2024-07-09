using System;
using System.Collections.Generic;
using System.Linq;
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

    public void MoveTowards(Vector2 Goal, float Speed)
    {
        float XDifference = Position.X - Goal.X;
        float YDifference = Position.Y - Goal.Y;
        float Angle = (float)(Math.Atan2(YDifference, XDifference));

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
        if (Math.Abs(Game1.GameTick - LastHitMarkerFrame) <= 20)
        {
            return;
        }

        int x = Game1.random.Next(
            (int)Position.X + HitBoxOffset.X,
            (int)Position.X + HitBoxOffset.X + HitBoxSize.X
        );
        int y = Game1.random.Next(
            (int)Position.Y + HitBoxOffset.Y,
            (int)Position.Y + HitBoxOffset.Y + HitBoxSize.Y
        );

        Game1.Particles.Add(new Particle(x, y)
        {
            HasDuration = true,
            Duration = Particle.HitMarker.DefaultDuration,
            ParticleObj = new Particle.HitMarker()
        });

        LastHitMarkerFrame = Game1.GameTick;
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

            Entities.Remove(Entity);
            Player.GiveKillReward(Entity);
        }
    }

    public void GiveKillReward(Player Player)
    {
        Player.GiveHealth(KillHealthReward);
        Player.GiveMana(KillManaReward);
    }

    public abstract void EnactAI(Player Player);

    public void Draw(Game1 Game1)
    {
        DrawEntity(Game1);
        DrawHitBoxes(Game1);
        DrawHealthBar(Game1);
    }

    public abstract void DrawEntity(Game1 Game1);

    public void DrawHitBoxes(Game1 Game1)
    {
        if (!Game1.Settings.ShowHitBoxes)
        {
            return;
        }

        // Top Line
        Game1.SpriteBatch.Draw(
            Game1.Textures.White,
            new Rectangle(
                (int)(
                    Position.X +
                    HitBoxOffset.X +
                    Game1.Graphics.PreferredBackBufferWidth / 2F -
                    Game1.Player.Position.X
                ),
                (int)(
                    Position.Y +
                    HitBoxOffset.Y +
                    Game1.Graphics.PreferredBackBufferHeight / 2F -
                    Game1.Player.Position.Y
                ),
                HitBoxSize.X,
                2
            ),
            Color.White
        );

        // Bottom Line
        Game1.SpriteBatch.Draw(
            Game1.Textures.White,
            new Rectangle(
                (int)(
                    Position.X +
                    HitBoxOffset.X +
                    Game1.Graphics.PreferredBackBufferWidth / 2F -
                    Game1.Player.Position.X
                ),
                (int)(
                    Position.Y +
                    HitBoxOffset.Y +
                    HitBoxSize.Y +
                    Game1.Graphics.PreferredBackBufferHeight / 2F -
                    Game1.Player.Position.Y
                ),
                HitBoxSize.X,
                2
            ),
            Color.White
        );

        // Left Line
        Game1.SpriteBatch.Draw(
            Game1.Textures.White,
            new Rectangle(
                (int)(
                    Position.X +
                    HitBoxOffset.X +
                    Game1.Graphics.PreferredBackBufferWidth / 2F -
                    Game1.Player.Position.X
                ),
                (int)(Position.Y +
                      HitBoxOffset.Y +
                      Game1.Graphics.PreferredBackBufferHeight / 2F -
                      Game1.Player.Position.Y
                ),
                2,
                HitBoxSize.Y
            ),
            Color.White
        );

        // Right Line
        Game1.SpriteBatch.Draw(
            Game1.Textures.White,
            new Rectangle(
                (int)(
                    Position.X +
                    HitBoxOffset.X +
                    HitBoxSize.X +
                    Game1.Graphics.PreferredBackBufferWidth / 2F -
                    Game1.Player.Position.X),
                (int)(
                    Position.Y +
                    HitBoxOffset.Y +
                    Game1.Graphics.PreferredBackBufferHeight / 2F -
                    Game1.Player.Position.Y
                ),
                2,
                HitBoxSize.Y
            ),
            Color.White
        );
    }

    public void DrawHealthBar(Game1 Game1)
    {
        if (!HealthBarVisible)
        {
            return;
        }

        // Empty Health Bar
        Game1.SpriteBatch.Draw(
            Game1.Textures.White,
            new Rectangle(
                (int)(
                    Position.X +
                    HealthBarOffset.X +
                    Game1.Graphics.PreferredBackBufferWidth / 2F -
                    Game1.Player.Position.X
                ),
                (int)(
                    Position.Y +
                    HealthBarOffset.Y +
                    Game1.Graphics.PreferredBackBufferHeight / 2F -
                    Game1.Player.Position.Y
                ),
                HealthBarDimensions.X,
                HealthBarDimensions.Y
            ),
            Color.LightGray
        );

        // Filled Health Bar
        Game1.SpriteBatch.Draw(
            Game1.Textures.White,
            new Rectangle(
                (int)(
                    Position.X +
                    HealthBarOffset.X +
                    Game1.Graphics.PreferredBackBufferWidth / 2F -
                    Game1.Player.Position.X + 2
                ),
                (int)(
                    Position.Y +
                    HealthBarOffset.Y +
                    Game1.Graphics.PreferredBackBufferHeight / 2F -
                    Game1.Player.Position.Y + 2
                ),
                (int)(
                    (HealthBarDimensions.X - 4) * (Health / HealthMax)
                ),
                HealthBarDimensions.Y - 4
            ),
            Color.Green);
    }
}