using System;
using System.Collections.Generic;
using System.Linq;
using Bombarder.Entities;
using Microsoft.Xna.Framework;

namespace Bombarder.MagicEffects;

public class ForceContainer : MagicEffect
{
    public readonly Color Colour = Color.OrangeRed;

    public static readonly int ManaCost = 150;

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

    public override void Draw(BombarderGame Game)
    {
        Game.SpriteBatch.Draw(
            Game.Textures.WhiteCircle,
            new Rectangle(
                (int)(
                    Position.X -
                    CurrentRadius +
                    Game.Graphics.PreferredBackBufferWidth / 2F -
                    Game.Player.Position.X
                ),
                (int)(
                    Position.Y -
                    CurrentRadius +
                    Game.Graphics.PreferredBackBufferHeight / 2F -
                    Game.Player.Position.Y
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
            float XDiff = Destination.X - Position.X;
            float YDiff = Destination.Y - Position.Y;
            float Distance = (float)Math.Sqrt(Math.Pow(XDiff, 2) + Math.Pow(YDiff, 2));
            float Angle = (float)(Math.Atan2(YDiff, XDiff) * 180.0 / Math.PI);
            float AngleRadians = Angle * (float)(Math.PI / 180);

            if (Distance <= MovementSpeed)
            {
                Position = new Vector2(Destination.X, Destination.Y);
                DestinationReached = true;
                HasDuration = HasDuration_DestinationReached;
            }
            else
            {
                Position += new Vector2(
                    (int)(MovementSpeed * (float)Math.Cos(AngleRadians)),
                    (int)(MovementSpeed * (float)Math.Sin(AngleRadians))
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

        float XDiff;
        float YDiff;
        float Distance;


        //Store new entities in Container list
        foreach (var Entity in Entities.Where(Entity => !ContainedEntities.Contains(Entity)))
        {
            XDiff = Math.Abs(Position.X - Entity.Position.X);
            YDiff = Math.Abs(Position.Y - Entity.Position.Y);
            Distance = (float)Math.Sqrt(Math.Pow(XDiff, 2) + Math.Pow(YDiff, 2));

            if (Distance <= CurrentRadius)
            {
                ContainedEntities.Add(Entity);
            }
        }

        //Keep all contained entities within confines
        foreach (Entity Entity in ContainedEntities)
        {
            XDiff = Math.Abs(Position.X - Entity.Position.X);
            YDiff = Math.Abs(Position.Y - Entity.Position.Y);
            Distance = (float)Math.Sqrt(Math.Pow(XDiff, 2) + Math.Pow(YDiff, 2));

            if (Distance < CurrentRadius - EdgeEffectWith)
            {
                continue;
            }

            float xDiff = Entity.Position.X - Position.X;
            float yDiff = Entity.Position.Y - Position.Y;
            float Angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);
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