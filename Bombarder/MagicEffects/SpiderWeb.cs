using System;
using System.Collections.Generic;
using System.Linq;
using Bombarder.Entities;
using Microsoft.Xna.Framework;

namespace Bombarder.MagicEffects
{
    public class SpiderWeb : MagicEffect
    {
        public readonly Color Colour = Color.White;

        public override int ManaCost { get; protected set; } = 150;

        public const int MovingDuration = 300;
        public const int StaticDuration = 350;
        public const float MovingSpeed = 25;
        public const int WidthMoving = 50;
        public const int WidthStatic = 300;
        public const int HitboxHalfSize = 20;
        public const float OpacityMoving = 1;
        public const float OpaictyStatic = 0.75f;
        public const float DeathOpacityMultiplier = 0.9f;
        public const int WidthGrowAmount = 2;

        public Vector2 Destination;
        public Vector2 TargetHoldPosition;
        public bool DestinationReached = false;
        private bool TargetDestinationReached = false;
        public bool IsActive = true;
        private float WidthCurrent = WidthMoving;
        private float OpacityCurrent = OpacityMoving;

        public Entity TargetEntity = null;
        public Player TargetPlayer = null;
        

        public SpiderWeb(Vector2 Position, Vector2 Destination) : base(Position)
        {
            this.Position = Position;
            this.Destination = Destination;
            Duration = MovingDuration;
            HasDuration = true;

            RadiusOffset = new Point(-HitboxHalfSize, -HitboxHalfSize);
            RadiusSize = new Point(HitboxHalfSize, HitboxHalfSize);
        }

        public override void Update(Player Player, List<Entity> Entities, uint GameTick)
        {
            base.Update(Player, Entities, GameTick);

            if (IsActive)
                HandleEntityCollision(Player, Entities, GameTick);

            EnactMovement();
            EnactDuration();
        }

        private void MoveTowardsDestination()
        {
            if (TargetDestinationReached || 
                    (DestinationReached && TargetPlayer == null && TargetEntity == null))
                return;

            Vector2 Diff = Destination - Position;
            float Distance = MathUtils.HypotF(Diff);
            float Angle = MathF.Atan2(Diff.Y, Diff.X);

            if (Distance <= MovingSpeed)
            {
                Position = Destination.Copy();

                if (TargetEntity != null || TargetPlayer != null)
                {
                    // Reached the interacted entity's center position
                    TargetDestinationReached = true;
                    return;
                }
                
                DestinationReached = true;
            }
            else
            {
                Position += new Vector2(
                    MovingSpeed * MathF.Cos(Angle),
                    MovingSpeed * MathF.Sin(Angle)
                );
            }
        }
        public void EnactMovement()
        {
            if (!DestinationReached || 
                (DestinationReached && !TargetDestinationReached))
            {
                MoveTowardsDestination();
            }
            else if (WidthCurrent < WidthStatic)
            {
                if (WidthStatic - WidthCurrent < WidthGrowAmount)
                {
                    WidthCurrent = WidthStatic;
                }
                else
                {
                    WidthCurrent += WidthGrowAmount;
                }
            }

            // Handling Opacity
            if (!IsActive)
            {
                OpacityCurrent *= DeathOpacityMultiplier;
            }
        }


        public override void HandleEntityCollision(Player Player, List<Entity> Entities, uint GameTick)
        {
            base.HandleEntityCollision(Player, Entities, GameTick);

            // If no entity has been targeted yet
            if (TargetEntity == null && TargetPlayer == null)
            {
                if (HostileToPlayer)
                {
                    if (HitBox.Intersects(Player.HitBox))
                    {
                        TargetPlayer = Player;
                        DestinationReached = true;
                        Destination = Player.Position;
                        TargetHoldPosition = Player.Position;
                        OpacityCurrent = OpaictyStatic;
                        Duration = StaticDuration;
                    }
                }
                else if (HostileToNPC)
                {
                    Entity CollidedEntity = Entities.Where(Entity => HitBox.Intersects(Entity.HitBox)).ToList().First();

                    if (CollidedEntity != null)
                    {
                        TargetEntity = CollidedEntity;
                        DestinationReached = true;
                        Destination = TargetEntity.Position;
                        TargetHoldPosition = TargetEntity.Position;
                        OpacityCurrent = OpaictyStatic;
                        Duration = StaticDuration;
                    }
                }

                return;
            }
            
            EnactTargetHold();
        }
        public void EnactTargetHold()
        {
            if (HostileToPlayer && TargetPlayer != null)
            {
                TargetPlayer.Position = TargetHoldPosition;
            }
            else if (HostileToNPC && TargetEntity != null)
            {
                TargetEntity.Position = TargetHoldPosition;
            }
        }

        public override void DrawEffect()
        {
            var Game = BombarderGame.Instance;

            Game.SpriteBatch.Draw(
            Game.Textures.WhiteCircle,
            MathUtils.CreateRectangle(
                Position - new Vector2(WidthCurrent) + Game.ScreenCenter - Game.Player.Position,
                new Vector2(WidthCurrent) * 2
            ),
            Colour * OpacityCurrent
        );
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


            if (OpacityCurrent > 0.05F)
            {
                OpacityCurrent *= DeathOpacityMultiplier;
            }
            else
            {
                HasDuration = true;
            }
        }
    }
}
