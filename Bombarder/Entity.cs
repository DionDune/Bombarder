using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bombarder
{
    internal class Entity
    {
        public float X { get; set; }
        public float Y { get; set; }

        public float Health { get; set; }
        public float HealthMax { get; set; }
        public bool HealthBarVisible { get; set; }
        public uint LastHitMarkerFrame;
        public bool ChaseMode { get; set; }
        public float Direction { get; set; }

        public List<EntityBlock> Parts { get; set; }

        public Point HitboxOffset { get; set; }
        public Point HitboxSize { get; set; }
        public Point HealthBarOffset { get; set; }
        public Point HealthBarDimentions { get; set; }

        public object EntityObj { get; set; }



        public Entity(object EntityClass)
        {
            X = 0;
            Y = 0;
            Direction = 0;
            LastHitMarkerFrame = 0;

            EntityObj = EntityClass;

            string EntityType = EntityClass.ToString();
            if (EntityType == "Bombarder.Entity+RedCube")
            {
                Health = RedCube.HealthDefault;
                HealthMax = RedCube.HealthMax;
                HealthBarVisible = RedCube.HealthBarVisible;

                ChaseMode = RedCube.ChaseModeDefault;

                Parts = RedCube.Parts;
                HitboxOffset = RedCube.HitboxOffset;
                HitboxSize = RedCube.HitboxSize;
                HealthBarOffset = RedCube.HealthBarOffset;
                HealthBarDimentions = RedCube.HealthBarDimentions;

                //EntityObj = EntityClass;
            }
            else if (EntityType == "Bombarder.Entity+DemonEye")
            {
                Health = DemonEye.HealthDefault;
                HealthMax = DemonEye.HealthMax;
                HealthBarVisible = DemonEye.HealthBarVisible;

                ChaseMode = DemonEye.ChaseModeDefault;

                Parts = DemonEye.Parts;
                HitboxOffset = DemonEye.HitboxOffset;
                HitboxSize = DemonEye.HitboxSize;
                HealthBarOffset = DemonEye.HealthBarOffset;
                HealthBarDimentions = DemonEye.HealthBarDimentions;

                //EntityObj = EntityClass;
            }
            else if (EntityType == "Bombarder.Entity+CubeMother")
            {
                Health = CubeMother.HealthDefault;
                HealthMax = CubeMother.HealthMax;
                HealthBarVisible = CubeMother.HealthBarVisible;

                ChaseMode = CubeMother.ChaseModeDefault;

                Parts = CubeMother.Parts;
                HitboxOffset = CubeMother.HitboxOffset;
                HitboxSize = CubeMother.HitBoxSize;
                HealthBarOffset = CubeMother.HealthBarOffset;
                HealthBarDimentions = CubeMother.HealthBarDimentions;
            }
        }

        public class RedCube
        {
            public const float HealthMax = 100;
            public const float HealthDefault = HealthMax;
            public const bool HealthBarVisible = true;

            public static readonly Point HitboxOffset = new Point(-33, -33);
            public static readonly Point HitboxSize = new Point(66, 66);
            public static readonly Point HealthBarDimentions = new Point(40, 10);
            public static readonly Point HealthBarOffset = new Point(-20, -HitboxOffset.Y + 5);

            public const float BaseSpeed = 5;
            public const bool ChaseModeDefault = true;

            public static readonly List<EntityBlock>Parts = new List<EntityBlock>() { 
                new EntityBlock()
                {
                    Width = 66,
                    Height = 66,
                    Offset = new Vector2(-33, -33),
                    Color = Color.DarkRed,
                }, 
                new EntityBlock()
                {
                    Width = 56, 
                    Height = 56, 
                    Offset = new Vector2(-28, -28), 
                    Color = Color.Red
                }
            };

            public static void EnactAI(Entity Entity, Player Player)
            {
                MoveTowards(new Vector2(Player.X, Player.Y), Entity, BaseSpeed);
            }
        }
        public class CubeMother
        {
            public const float HealthMax = 1500;
            public const float HealthDefault = 1500;
            public const int HealthRegainInterval = 3;
            public const float HealthRegainAmount = 1;
            public const bool HealthBarVisible = true;

            public static readonly Point HitboxOffset = new Point(-231, -231);
            public static readonly Point HitBoxSize = new Point(462, 462);
            public static readonly Point HealthBarDimentions = new Point(350, 74);
            public static readonly Point HealthBarOffset = new Point(-175, -37);

            public const float BaseSpeed = 2;
            public const bool ChaseModeDefault = false;

            public static readonly List<EntityBlock> Parts = new List<EntityBlock>() {
                new EntityBlock()
                {
                    Width = 462,
                    Height = 462,
                    Offset = new Vector2(-231, -231),
                    Color = Color.DarkRed,
                },
                new EntityBlock()
                {
                    Width = 442,
                    Height = 442,
                    Offset = new Vector2(-221, -221),
                    Color = Color.Red
                }
            };


            public static void EnactAI(Entity Entity, Player Player)
            {
                MoveTowards(new Vector2(Player.X, Player.Y), Entity, BaseSpeed);
            } 
        }
        public class DemonEye
        {
            public const float HealthMax = 150;
            public const float HealthDefault = HealthMax;
            public const bool HealthBarVisible = true;

            public static readonly Point HitboxOffset = new Point(-119, -100);
            public static readonly Point HitboxSize = new Point(238, 200);
            public static readonly Point HealthBarDimentions = new Point(80, 16);
            public static readonly Point HealthBarOffset = new Point(-40, -HitboxOffset.Y + 5);

            public const float BaseSpeed = 4;
            public const bool ChaseModeDefault = true;

            public static readonly List<EntityBlock> Parts = new List<EntityBlock>() {
                new EntityBlock()
                {
                    Width = 0,
                    Height = 0,
                    Offset = new Vector2(0, 0),
                    Color = Color.White,
                }
            };

            public static void EnactAI(Entity Entity, Player Player)
            {
                MoveTowards(new Vector2(Player.X, Player.Y), Entity, BaseSpeed);
            }
        }
        


        public static void MoveTowards( Vector2 Goal, Entity Entity, float Speed)
        {
            float XDifference = Entity.X - Goal.X;
            float YDifference = Entity.Y - Goal.Y;
            float Angle = (float)(Math.Atan2(YDifference, XDifference));

            Entity.Direction = Angle;

            Entity.X -= Speed * (float)Math.Cos(Angle);
            Entity.Y -= Speed * (float)Math.Sin(Angle);
        }
        public void GiveDamage(int Damage)
        {
            Health -= Damage;

            ApplyHitMarker();
        }
        public void ApplyHitMarker()
        {
            if (Math.Abs(Game1.GameTick - LastHitMarkerFrame) > 20)
            {
                int x = Game1.random.Next((int)X + HitboxOffset.X, (int)X + HitboxOffset.X + HitboxSize.X);
                int y = Game1.random.Next((int)Y + HitboxOffset.Y, (int)Y + HitboxOffset.Y + HitboxSize.Y);

                Game1.Particles.Add(new Particle(x, y)
                {
                    HasDuration = true,
                    Duration = Particle.HitMarker.DefaultDuration,
                    ParticleObj = new Particle.HitMarker()
                });

                LastHitMarkerFrame = Game1.GameTick;
            }
        }
        public static void PurgeDead(List<Entity> Entities)
        {
            List<Entity> DeadEntities = new List<Entity>();

            foreach (Entity Entity in Entities)
            {
                if (Entity.Health <= 0)
                {
                    DeadEntities.Add(Entity);
                }
            }

            foreach (Entity Entity in DeadEntities)
            {
                Entities.Remove(Entity);
            }
        }
    }


    public class EntityBlock
    {
        public List<Texture2D> Textures { get; set; }
        public Vector2 Offset { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Color Color { get; set; }

        public EntityBlock()
        {
            Width = 66;
            Height = 66;
            Offset = new Vector2(-33, -33);

            Color = Color.DarkRed;

            Textures = null;
        }
    }
}
