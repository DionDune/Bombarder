using System;
using System.Collections.Generic;
using System.Linq;
using Bombarder.Entities;
using Microsoft.Xna.Framework;

namespace Bombarder.MagicEffects;

public class ForceContainer : MagicEffect
{
    public readonly Color Colour = Color.OrangeRed;

    public override int ManaCost { get; protected set; } = 150;
    public const int DurationDefault = 350;
    public const float Radius = 250;
    public const float RadiusMoving = 50;
    public const float MovementSpeed = 25;
    public const float EdgeEffectWith = 10;
    public const float RadiusIncrease = 16;
    public const float OpacityDefault = 0.7F;
    public const float DeathOpacityMultiplier = 0.9F;

    public const bool HasDuration_Moving = false;
    public const bool HasDuration_DestinationReached = true;
    public const bool IsActiveDefault = true;

    public Vector2 Destination;
    public bool DestinationReached = false;
    public float CurrentRadius = RadiusMoving;
    public bool IsActive = IsActiveDefault;

    public static readonly int BorderWidth = 15;
    public float Opacity = OpacityDefault;

    public List<Entity> ContainedEntities = new();

    public ForceContainer(Vector2 Position, Vector2 Destination) : base(Position)
    {
        this.Position = Position;
        this.Destination = Destination;
        Duration = DurationDefault;
        HasDuration = HasDuration_Moving;
    }

    public override void EnactEffect(Player Player, List<Entity> Entities, uint GameTick)
    {
        EnactMovement();
        EnactForce(Entities);
        EnactDuration();
    }

    public override void Draw()
    {
        BombarderGame.Instance.SpriteBatch.Draw(
            BombarderGame.Instance.Textures.WhiteCircle,
            new Rectangle(
                (int)(
                    Position.X -
                    CurrentRadius +
                    BombarderGame.Instance.Graphics.PreferredBackBufferWidth / 2F -
                    BombarderGame.Instance.Player.Position.X
                ),
                (int)(
                    Position.Y -
                    CurrentRadius +
                    BombarderGame.Instance.Graphics.PreferredBackBufferHeight / 2F -
                    BombarderGame.Instance.Player.Position.Y
                ),
                (int)CurrentRadius * 2,
                (int)CurrentRadius * 2
            ),
            Colour * Opacity
        );
    }

    public void EnactMovement()
    {
        if (!DestinationReached)
        {
            Vector2 Diff = Destination - Position;
            float Distance = (float)Utils.Hypot(Diff);
            float Angle = (float)(Math.Atan2(Diff.Y, Diff.X) * 180.0 / Math.PI);
            float AngleRadians = Angle * (float)(Math.PI / 180);

            if (Distance <= MovementSpeed)
            {
                Position = Destination.Copy();
                DestinationReached = true;
                HasDuration = HasDuration_DestinationReached;
            }
            else
            {
                Position += new Vector2(
                    MovementSpeed * (float)Math.Cos(AngleRadians),
                    MovementSpeed * (float)Math.Sin(AngleRadians)
                );
            }
        }
        else if (CurrentRadius < Radius)
        {
            if (Radius - CurrentRadius < RadiusIncrease)
            {
                CurrentRadius = Radius;
            }
            else
            {
                CurrentRadius += RadiusIncrease;
            }
        }
    }

    public void EnactForce(List<Entity> Entities)
    {
        if (!IsActive)
        {
            return;
        }

        // Store new entities in Container list
        foreach (var Entity in Entities.Where(Entity => !ContainedEntities.Contains(Entity)))
        {
            Vector2 Diff = Utils.Abs(Position - Entity.Position);
            float Distance = (float)Utils.Hypot(Diff);

            if (Distance <= CurrentRadius)
            {
                ContainedEntities.Add(Entity);
            }
        }

        // Keep all contained entities within confines
        foreach (Entity Entity in ContainedEntities)
        {
            Vector2 Diff = Utils.Abs(Position - Entity.Position);
            float Distance = (float)Utils.Hypot(Diff);

            if (Distance < CurrentRadius - EdgeEffectWith)
            {
                continue;
            }

            Vector2 InverseDiff = Entity.Position - Position;
            float Angle = (float)(Math.Atan2(InverseDiff.Y, InverseDiff.X) * 180.0 / Math.PI);
            float AngleRadians = Angle * (float)(Math.PI / 180);

            Vector2 PositionChange = new Vector2(
                Math.Abs(CurrentRadius - EdgeEffectWith - Distance) * (float)Math.Cos(AngleRadians),
                Math.Abs(CurrentRadius - EdgeEffectWith - Distance) * (float)Math.Sin(AngleRadians)
            );

            Entity.Position -= PositionChange;
        }
    }

    public void EnactDuration()
    {
        if (Duration >= 3)
        {
            return;
        }

        if (IsActive)
        {
            HasDuration = false;
            IsActive = false;
        }


        if (Opacity > 0.05F)
        {
            Opacity *= DeathOpacityMultiplier;
        }
        else
        {
            HasDuration = true;
        }
    }
}