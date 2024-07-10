using System;
using System.Collections.Generic;
using Bombarder.Entities;
using Bombarder.Particles;
using Microsoft.Xna.Framework;

namespace Bombarder.MagicEffects;

    public class PlayerTeleport : MagicEffect
    {
        public const int ManaCost = 150;

        public const int DefaultDuration = 2;
        public const bool HasDurationWhenReached = true;

        public const float Speed = 80;
        public Vector2 Goal;
        public bool GoalReacted = false;
        public bool JustStarted = true;

        public const int ParticleCountMin = 65;
        public const int ParticleCountMed = 100;
        public const int ParticleCountMax = 150;
        public const int ParticleCountMaxDistanceThreshold = 400;
        
        public PlayerTeleport(Vector2 Position, Vector2 Goal) : base(Position)
        {
            this.Goal = Goal;
            Duration = DefaultDuration;
            HasDuration = false;
        }

        public override void EnactEffect(Player Player, List<Entity> Entities, uint GameTick)
        {
            if (JustStarted)
            {
                CreateParticles();
            }
            EnactMovement(Player);
            EnactDuration(Player);
        }

        public override void Draw(Game1 Game1)
        {
        }

        public void EnactMovement(Player Player)
        {
            float xDiff = Player.Position.X - Goal.X;
            float yDiff = Player.Position.Y - Goal.Y;
            float Distance = (float)Math.Sqrt(Math.Pow(xDiff, 2) + Math.Pow(yDiff, 2));
            float Angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);
            float AngleRadians = Angle * (float)(Math.PI / 180);

            float DistanceToMove = Speed;

            if (Distance <= Speed)
            {
                DistanceToMove = Distance;
                GoalReacted = true;
            }

            Player.Position -= new Vector2(
                DistanceToMove * (float)Math.Cos(AngleRadians), DistanceToMove * (float)Math.Sin(AngleRadians));
            Player.IsImmune = true;

            JustStarted = false;
        }
        public void EnactDuration(Player Player)
        {
            if (!GoalReacted)
            {
                return;
            }

            HasDuration = true;
            Player.IsImmune = false;
        }
        public void CreateParticles()
        {
            float XDiff = Position.X - Goal.X;
            float YDiff = Position.Y - Goal.Y;
            float Distance = (float)Math.Sqrt(Math.Pow(XDiff, 2) + Math.Pow(YDiff, 2));

            int Count = ParticleCountMed;
            if (Distance > ParticleCountMaxDistanceThreshold)
            {
                Count = ParticleCountMax;
            }

            for (int i = 0; i < Game1.random.Next(ParticleCountMin, Count); i++)
            {
                TeleportLine.SpawnBetween(Game1.Particles, new Vector2(Position.X, Position.Y), new Vector2(Goal.X, Goal.Y));
            }
        }
    }
