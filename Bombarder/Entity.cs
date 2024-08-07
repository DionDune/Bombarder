﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bombarder
{
    public class Entity
    {
        public Vector2 Position { get; set; }

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
            Position = Vector2.Zero;
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
            else if (EntityType == "Bombarder.Entity+Spider")
            {
                Health = Spider.HealthDefault;
                HealthMax = Spider.HealthMax;
                HealthBarVisible = Spider.HealthBarVisible;

                Parts = Spider.Parts;
                HitboxOffset = Spider.HitboxOffset;
                HitboxSize = Spider.HitboxSize;
                HealthBarOffset = Spider.HealthBarOffset;
                HealthBarDimentions = Spider.HealthBarDimentions;
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

            public const int Damage = 30;
            public const int SelfDamage = (int)HealthMax / 2;

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


            public const int killHealthReward = 0;
            public const int killManaReward = 15;

            public static void EnactAI(Entity Entity, Player Player)
            {
                MoveTowards(Player.Position, Entity, BaseSpeed);
                EnactAttack(Entity, Player);
            }
            public static void EnactAttack(Entity Entity, Player Player)
            {
                Point TopLeft = new Point((int)Entity.Position.X + HitboxOffset.X, (int)Entity.Position.Y + HitboxOffset.Y);
                Point TopRight = new Point((int)Entity.Position.X + HitboxOffset.X + HitboxSize.X, (int)Entity.Position.Y + HitboxOffset.Y);
                Point BottomLeft = new Point((int)Entity.Position.X + HitboxOffset.X, (int)Entity.Position.Y + HitboxOffset.Y + HitboxSize.Y);
                Point BottomRight = new Point((int)Entity.Position.X + HitboxOffset.X + HitboxSize.X, (int)Entity.Position.Y + HitboxOffset.Y + HitboxSize.Y);

                Point PlayerTopLeft = new Point((int)Player.Position.X - (Player.Width / 2), (int)Player.Position.Y - (Player.Height / 2));
                Point PlayerTopRight = new Point((int)Player.Position.X + (Player.Width / 2), (int)Player.Position.Y - (Player.Height / 2));
                Point PlayerBottomLeft = new Point((int)Player.Position.X - (Player.Width / 2), (int)Player.Position.Y + (Player.Height / 2));
                Point PlayerBottomRight = new Point((int)Player.Position.X + (Player.Width / 2), (int)Player.Position.Y + (Player.Height / 2));

                if (TopLeft.X >= PlayerTopLeft.X && TopLeft.X <= PlayerTopRight.X &&
                    TopLeft.Y >= PlayerTopLeft.Y && TopLeft.Y <= PlayerBottomLeft.Y)
                {
                    Player.GiveDamage(Damage);
                    Entity.GiveDamage(SelfDamage);
                }
                else if (TopRight.X >= PlayerTopLeft.X && TopRight.X <= PlayerTopRight.X &&
                    TopRight.Y >= PlayerTopLeft.Y && TopRight.Y <= PlayerBottomLeft.Y)
                {
                    Player.GiveDamage(Damage);
                    Entity.GiveDamage(SelfDamage);
                }
                else if (BottomLeft.X >= PlayerTopLeft.X && BottomLeft.X <= PlayerTopRight.X &&
                    BottomLeft.Y >= PlayerTopLeft.Y && BottomLeft.Y <= PlayerBottomLeft.Y)
                {
                    Player.GiveDamage(Damage);
                    Entity.GiveDamage(SelfDamage);
                }
                else if (BottomRight.X >= PlayerTopLeft.X && BottomRight.X <= PlayerTopRight.X &&
                    BottomRight.Y >= PlayerTopLeft.Y && BottomRight.Y <= PlayerBottomLeft.Y)
                {
                    Player.GiveDamage(Damage);
                    Entity.GiveDamage(SelfDamage);
                }
            }

            public static void CreateDeathParticles(Entity Entity)
            {
                EntityBlock Block = Entity.Parts.First();
                Vector2 StartPoint = new Vector2(Entity.Position.X + Block.Offset.X, 
                                                 Entity.Position.Y + Block.Offset.Y);

                float XDifference;
                float YDifference;
                float Angle;

                for (int y = 0; y < Block.Height / Particle.RedCubeSegment.Height; y++)
                {
                    for (int x = 0; x < Block.Width / Particle.RedCubeSegment.Width; x++)
                    {
                        float ParticleX = StartPoint.X + (x * Particle.RedCubeSegment.Width);
                        float ParticleY = StartPoint.Y + (y * Particle.RedCubeSegment.Height);

                        XDifference = ParticleX - Entity.Position.X;
                        YDifference = ParticleY - Entity.Position.Y;
                        Angle = ( ((float)Math.Atan2(YDifference, XDifference) * (float)(180 / Math.PI)) +
                                 ((float)Game1.random.Next((int)(-Particle.RedCubeSegment.AngleOffsetAllowance * 10), (int)(Particle.RedCubeSegment.AngleOffsetAllowance * 10)) / 10) ) *
                                 (float)(Math.PI / 180);

                        Game1.Particles.Add(new Particle( (int)ParticleX,
                                                          (int)ParticleY )
                        { 
                            HasDuration = true,
                            Duration = Game1.random.Next(Particle.RedCubeSegment.DurationMin, Particle.RedCubeSegment.DurationMax),

                            ParticleObj = new Particle.RedCubeSegment()
                            {
                                Angle = Angle
                            }
                        });
                    }
                }
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

            public const float BaseSpeed = 3;
            public const bool ChaseModeDefault = false;

            public const int SpawnIntervalMin = 15;
            public const int SpawnIntervalMax = 100;
            public const int SpawnDistanceMin = 150;
            public const int SpawnDistanceMax = 350;
            public uint NextSpawnFrame = 0;

            public const float PreferredDistance = 3500;

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



            public const int killHealthReward = 2000;
            public const int killManaReward = 2000;


            public static void EnactAI(Entity Entity, Player Player)
            {
                EnactMovement(Entity, Player);
                EnactSpawn(Entity);
            } 
            public static void EnactMovement(Entity Entity, Player Player)
            {
                float Distance = Vector2.Distance(Entity.Position,
                                                   Player.Position);

                if (Distance > PreferredDistance)
                {
                    MoveTowards(Player.Position, Entity, BaseSpeed);
                }
                else if (Distance < PreferredDistance)
                {
                    MoveAwayFrom(Player.Position, Entity, BaseSpeed);
                }
            }
            public static void EnactSpawn(Entity Entity)
            {
                CubeMother Mother = (CubeMother)Entity.EntityObj;
                if (Mother.NextSpawnFrame == Game1.GameTick || Game1.GameTick > Mother.NextSpawnFrame)
                {
                    float SpawnAngle = Game1.random.Next(0, 360) * (float)(Math.PI / 180);
                    int SpawnDistance = Game1.random.Next(SpawnDistanceMin, SpawnDistanceMax);
                    Vector2 SpawnPoint = new Vector2(Entity.Position.X + (SpawnDistance * (float)Math.Cos(SpawnAngle)),
                                                        Entity.Position.Y + (SpawnDistance * (float)Math.Sin(SpawnAngle)));

                    //Red Cube
                    Game1.EntitiesToAdd.Add(new Entity(new Entity.RedCube())
                    {
                        Position = new Vector2(SpawnPoint.X, SpawnPoint.Y)
                    });

                    Mother.NextSpawnFrame = (uint)(Game1.GameTick + Game1.random.Next(SpawnIntervalMin, SpawnIntervalMax));
                }
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

            public const int Damage = 15;
            public const int DamageInterval = 10;
            public uint LastDamageFrame = 0;

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



            public const int killHealthReward = 0;
            public const int killManaReward = 100;

            public static void EnactAI(Entity Entity, Player Player)
            {
                MoveTowards(Player.Position, Entity, BaseSpeed);
                EnactAttack(Entity, Player);
            }
            public static void EnactAttack(Entity Entity, Player Player)
            {
                DemonEye Eye = (DemonEye)Entity.EntityObj;
                if (Game1.GameTick - Eye.LastDamageFrame >= DamageInterval)
                {
                    Point TopLeft = new Point((int)Entity.Position.X + HitboxOffset.X, (int)Entity.Position.Y + HitboxOffset.Y);
                    Point TopRight = new Point((int)Entity.Position.X + HitboxOffset.X + HitboxSize.X, (int)Entity.Position.Y + HitboxOffset.Y);
                    Point BottomLeft = new Point((int)Entity.Position.X + HitboxOffset.X, (int)Entity.Position.Y + HitboxOffset.Y + HitboxSize.Y);
                    Point BottomRight = new Point((int)Entity.Position.X + HitboxOffset.X + HitboxSize.X, (int)Entity.Position.Y + HitboxOffset.Y + HitboxSize.Y);

                    Point PlayerTopLeft = new Point((int)Player.Position.X - (Player.Width / 2), (int)Player.Position.Y - (Player.Height / 2));
                    Point PlayerTopRight = new Point((int)Player.Position.X + (Player.Width / 2), (int)Player.Position.Y - (Player.Height / 2));
                    Point PlayerBottomLeft = new Point((int)Player.Position.X - (Player.Width / 2), (int)Player.Position.Y + (Player.Height / 2));
                    Point PlayerBottomRight = new Point((int)Player.Position.X + (Player.Width / 2), (int)Player.Position.Y + (Player.Height / 2));


                    bool Contact = false;

                    if (PlayerTopLeft.X >= TopLeft.X && PlayerTopLeft.X <= TopRight.X &&
                        PlayerTopLeft.Y >= TopLeft.Y && PlayerTopLeft.Y <= BottomRight.Y)
                    {
                        Contact = true;
                    }
                    else if (PlayerTopRight.X >= TopLeft.X && PlayerTopRight.X <= TopRight.X &&
                             PlayerTopRight.Y >= TopLeft.Y && PlayerTopRight.Y <= BottomLeft.Y)
                    {
                        Contact = true;
                    }
                    else if (PlayerBottomLeft.X >= TopLeft.X && PlayerBottomLeft.X <= TopRight.X &&
                             PlayerBottomLeft.Y >= TopLeft.Y && PlayerBottomLeft.Y <= BottomLeft.Y)
                    {
                        Contact = true;
                    }
                    else if (PlayerBottomRight.X >= TopLeft.X && PlayerBottomRight.X <= TopRight.X &&
                             PlayerBottomRight.Y >= TopLeft.Y && PlayerBottomRight.Y <= BottomLeft.Y)
                    {
                        Contact = true;
                    }


                    if (Contact)
                    {
                        Player.GiveDamage(Damage);
                        Eye.LastDamageFrame = Game1.GameTick;
                    }
                }
            }
        }
        public class Spider
        {
            public const float HealthMax = 650;
            public const float HealthDefault = HealthMax;
            public const bool HealthBarVisible = true;

            public static readonly Point HitboxOffset = new Point(-100, -100);
            public static readonly Point HitboxSize = new Point(200, 200);
            public static readonly Point HealthBarDimentions = new Point(80, 16);
            public static readonly Point HealthBarOffset = new Point(-40, -HitboxOffset.Y + 5);

            public static readonly List<EntityBlock> Parts = new List<EntityBlock>() {
                new EntityBlock()
                {
                    Width = 200,
                    Height = 200,
                    Offset = new Vector2(-100, -100),
                    Color = Color.MediumPurple,
                }
            };



            public const int killHealthReward = 50;
            public const int killManaReward = 400;


            public const int Damage = 125;
            public uint LastDamageFrame = 0;
            public const int DamageInterval = 40;

            public const int JumpIntervalMin = 60;
            public const int JumpIntervalMax = 250;
            public const int JumpIntervalErraticMin = 60;
            public const int JumpIntervalErraticMax = 140;
            public const int ErraticDistanceThreshold = 800;
            public uint NextJumpFrame = 0;

            public const float JumpVelocityMin = 20;
            public const float JumpVelocityMed = 40;
            public const float JumpVelocityMax = 55;
            public const float JumpVelocityFullThreshhold = 650;
            public const float VelocityMultiplier = 0.95F;
            public float Velocity = 0;
            public float Angle = 0;

            public static void EnactAI(Entity Entity, Player Player)
            {
                EnactJump(Entity, Player);
                EnactVelocity(Entity, Player);
                EnactDamage(Entity, Player);
            }
            public static void EnactJump(Entity Entity, Player Player)
            {
                Spider spider = (Spider)Entity.EntityObj;


                if (spider.NextJumpFrame <= Game1.GameTick)
                {
                    float XDiff = Entity.Position.X - Player.Position.X;
                    float YDiff = Entity.Position.Y - Player.Position.Y;
                    float Angle = (float)Math.Atan2(YDiff, XDiff);
                    float Velocity;

                    float PlayerDistance = (float)Math.Sqrt(Math.Pow(XDiff, 2) + Math.Pow(YDiff, 2));
                    if (PlayerDistance > JumpVelocityFullThreshhold)
                    {
                        Velocity = JumpVelocityMax;
                    }
                    else
                    {
                        Velocity = (float)Game1.random.Next((int)JumpVelocityMin * 100, (int)JumpVelocityMed * 100) / 100;
                    }
                    

                    spider.Angle = Angle;
                    spider.Velocity = Velocity;

                    if (PlayerDistance > ErraticDistanceThreshold)
                    {
                        spider.NextJumpFrame = Game1.GameTick + (uint)Game1.random.Next(JumpIntervalErraticMin, JumpIntervalErraticMax);
                    }
                    else
                    {
                        spider.NextJumpFrame = Game1.GameTick + (uint)Game1.random.Next(JumpIntervalMin, JumpIntervalMax);
                    }
                }
            }
            public static void EnactVelocity(Entity Entity, Player Player)
            {
                Spider spider = (Spider)Entity.EntityObj;


                if (spider.Velocity > 0)
                {
                    Vector2 PositionChange = new Vector2(
                        spider.Velocity * (float)Math.Cos(spider.Angle),
                        spider.Velocity * (float)Math.Sin(spider.Angle)
                        );
                    Entity.Position -= PositionChange;

                    spider.Velocity *= VelocityMultiplier;
                    if (spider.Velocity <= 1)
                    {
                        spider.Velocity = 0;
                    }
                }
            }
            public static void EnactDamage(Entity Entity, Player Player)
            {
                Spider spider = (Spider)Entity.EntityObj;

                if (Game1.GameTick - spider.LastDamageFrame >= DamageInterval)
                {
                    Point TopLeft = new Point((int)Entity.Position.X + HitboxOffset.X, (int)Entity.Position.Y + HitboxOffset.Y);
                    Point TopRight = new Point((int)Entity.Position.X + HitboxOffset.X + HitboxSize.X, (int)Entity.Position.Y + HitboxOffset.Y);
                    Point BottomLeft = new Point((int)Entity.Position.X + HitboxOffset.X, (int)Entity.Position.Y + HitboxOffset.Y + HitboxSize.Y);
                    Point BottomRight = new Point((int)Entity.Position.X + HitboxOffset.X + HitboxSize.X, (int)Entity.Position.Y + HitboxOffset.Y + HitboxSize.Y);

                    Point PlayerTopLeft = new Point((int)Player.Position.X - (Player.Width / 2), (int)Player.Position.Y - (Player.Height / 2));
                    Point PlayerTopRight = new Point((int)Player.Position.X + (Player.Width / 2), (int)Player.Position.Y - (Player.Height / 2));
                    Point PlayerBottomLeft = new Point((int)Player.Position.X - (Player.Width / 2), (int)Player.Position.Y + (Player.Height / 2));
                    Point PlayerBottomRight = new Point((int)Player.Position.X + (Player.Width / 2), (int)Player.Position.Y + (Player.Height / 2));


                    bool Contact = false;

                    if (PlayerTopLeft.X >= TopLeft.X && PlayerTopLeft.X <= TopRight.X &&
                        PlayerTopLeft.Y >= TopLeft.Y && PlayerTopLeft.Y <= BottomRight.Y)
                    {
                        Contact = true;
                    }
                    else if (PlayerTopRight.X >= TopLeft.X && PlayerTopRight.X <= TopRight.X &&
                             PlayerTopRight.Y >= TopLeft.Y && PlayerTopRight.Y <= BottomLeft.Y)
                    {
                        Contact = true;
                    }
                    else if (PlayerBottomLeft.X >= TopLeft.X && PlayerBottomLeft.X <= TopRight.X &&
                             PlayerBottomLeft.Y >= TopLeft.Y && PlayerBottomLeft.Y <= BottomLeft.Y)
                    {
                        Contact = true;
                    }
                    else if (PlayerBottomRight.X >= TopLeft.X && PlayerBottomRight.X <= TopRight.X &&
                             PlayerBottomRight.Y >= TopLeft.Y && PlayerBottomRight.Y <= BottomLeft.Y)
                    {
                        Contact = true;
                    }


                    if (Contact)
                    {
                        Player.GiveDamage(Damage);
                        spider.LastDamageFrame = Game1.GameTick ;
                    }
                }
            }
        }
        


        public static void MoveTowards( Vector2 Goal, Entity Entity, float Speed )
        {
            float XDifference = Entity.Position.X - Goal.X;
            float YDifference = Entity.Position.Y - Goal.Y;
            float Angle = (float)(Math.Atan2(YDifference, XDifference));

            Entity.Direction = Angle;
            Vector2 PositionChange = new Vector2(
                Speed * (float)Math.Cos(Angle),
                Speed * (float)Math.Sin(Angle)
                );

            Entity.Position -= PositionChange;
        }
        public static void MoveAwayFrom( Vector2 Position, Entity Entity, float Speed)
        {
            float XDifference = Entity.Position.X - Position.X;
            float YDifference = Entity.Position.Y - Position.Y;
            float Angle = (float)(Math.Atan2(YDifference, XDifference));

            Entity.Direction = Angle;
            Vector2 PositionChange = new Vector2(
                Speed * (float)Math.Cos(Angle),
                Speed * (float)Math.Sin(Angle)
                );

            Entity.Position += PositionChange;
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
                int x = Game1.random.Next((int)Position.X + HitboxOffset.X, (int)Position.X + HitboxOffset.X + HitboxSize.X);
                int y = Game1.random.Next((int)Position.Y + HitboxOffset.Y, (int)Position.Y + HitboxOffset.Y + HitboxSize.Y);

                Game1.Particles.Add(new Particle(x, y)
                {
                    HasDuration = true,
                    Duration = Particle.HitMarker.DefaultDuration,
                    ParticleObj = new Particle.HitMarker()
                });

                LastHitMarkerFrame = Game1.GameTick;
            }
        }
        public static void PurgeDead(List<Entity> Entities, Player Player)
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
                if (Entity.EntityObj is RedCube)
                {
                    RedCube.CreateDeathParticles(Entity);
                }

                Entities.Remove(Entity);
                Player.GiveKillReward(Entity);
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
