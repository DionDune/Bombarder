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

    public override void Update(Player Player, List<Entity> Entities, uint GameTick)
    {
        base.Update(Player, Entities, GameTick);
        EnactMovement();
        EnactForce(Entities);
        EnactDuration();
    }

    public override void DrawEffect()
    {
        var Game = BombarderGame.Instance;

        Game.SpriteBatch.Draw(
            Game.Textures.WhiteCircle,
            MathUtils.CreateRectangle(
                Position - new Vector2(CurrentRadius) + Game.ScreenCenter - Game.Player.Position,
                new Vector2(CurrentRadius) * 2
            ),
            Colour * Opacity
        );
    }

    public void EnactMovement()
    {
        if (!DestinationReached)
        {
            Vector2 Diff = Destination - Position;
            float Distance = MathUtils.HypotF(Diff);
            float Angle = MathF.Atan2(Diff.Y, Diff.X);

            if (Distance <= MovementSpeed)
            {
                Position = Destination.Copy();
                DestinationReached = true;
                HasDuration = HasDuration_DestinationReached;
            }
            else
            {
                Position += new Vector2(
                    MovementSpeed * MathF.Cos(Angle),
                    MovementSpeed * MathF.Sin(Angle)
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
            Vector2 Diff = MathUtils.Abs(Position - Entity.Position);
            float Distance = MathUtils.HypotF(Diff);

            if (Distance <= CurrentRadius)
            {
                ContainedEntities.Add(Entity);
            }
        }

        // Keep all contained entities within confines
        foreach (Entity Entity in ContainedEntities)
        {
            Vector2 Diff = MathUtils.Abs(Position - Entity.Position);
            float Distance = MathUtils.HypotF(Diff);

            if (Distance < CurrentRadius - EdgeEffectWith)
            {
                continue;
            }

            Vector2 InverseDiff = Entity.Position - Position;
            float Angle = MathF.Atan2(InverseDiff.Y, InverseDiff.X);

            Vector2 PositionChange = new Vector2(
                Math.Abs(CurrentRadius - EdgeEffectWith - Distance) * MathF.Cos(Angle),
                Math.Abs(CurrentRadius - EdgeEffectWith - Distance) * MathF.Sin(Angle)
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