using System;
using System.Collections.Generic;
using Bombarder.Entities;
using Microsoft.Xna.Framework;

namespace Bombarder.MagicEffects;

public abstract class MagicEffect
{
    public Vector2 Position { get; set; }

    public bool HostileToPlayer { get; set; }
    public bool HostileToNPC { get; set; }
    public int Damage { get; set; }
    public abstract int ManaCost { get; protected set; }

    public bool RadiusIsCircle { get; set; }
    public float DamageRadius { get; set; }
    public Point RadiusOffset { get; set; }
    public Point RadiusSize { get; set; }
    public int DamageDuration { get; set; }

    public bool HasDuration { get; set; }
    public int Duration { get; set; }
    public Rectangle HitBox => new(Position.ToPoint() + RadiusOffset, RadiusSize + RadiusSize);

    public List<MagicEffectPiece> Pieces { get; set; }

    public static readonly Dictionary<string, Func<Vector2, Vector2, MagicEffect>> MagicEffectsFactories = new()
    {
        { "DissipationWave", (Position, EntityPosition) => new DissipationWave(Position) },
        { "ForceContainer", (Position, EntityPosition) => new ForceContainer(EntityPosition, Position) },
        { "ForceWave", (Position, EntityPosition) => new ForceWave(Position) },
        { "NonStaticOrb", (Position, EntityPosition) => new NonStaticOrb(EntityPosition, Position) },
        { "PlayerTeleport", (Position, EntityPosition) => new PlayerTeleport(Position) },
        { "StaticOrb", (Position, EntityPosition_) => new StaticOrb(Position) },
        { "WideLaser", (Position, EntityPosition) => new WideLaser(EntityPosition, Position) },
    };

    protected MagicEffect(Vector2 Position)
    {
        this.Position = Position;
        HostileToPlayer = true;
        HostileToNPC = false;
        Damage = 4;
        DamageDuration = 150;
        RadiusIsCircle = false;
        DamageRadius = 0;
        RadiusOffset = new Point(-24, -24);
        RadiusSize = new Point(24, 24);

        Pieces = new List<MagicEffectPiece> { new() { LifeSpan = DamageDuration } };
    }

    public static void CreateMagic<T>(Vector2 SpawnPosition, Player? Player, Entity? Entity) where T : MagicEffect
    {
        var Factory = MagicEffectsFactories.GetValueOrDefault(typeof(T).Name);
        MagicEffect MagicEffect = null;

        if (Player != null)
        {
            MagicEffect = Factory?.Invoke(SpawnPosition, Player.Position);

            if (MagicEffect == null || !Player.CheckUseMana(MagicEffect.ManaCost))
            {
                return;
            }

            MagicEffect.SetHostitily(false, true);
        }
        else if (Entity != null)
        {
            MagicEffect = Factory.Invoke(SpawnPosition, Entity.Position);

            if (MagicEffect == null)
            {
                return;
            }

            MagicEffect.SetHostitily(true, false);
        }
        else return;
        

        BombarderGame.Instance.MagicEffects.Add(MagicEffect);
    }
    public void SetHostitily(bool hostileToPlayer, bool hostileToNPC)
    {
        HostileToPlayer = hostileToPlayer;
        HostileToNPC = hostileToNPC;
    }

    public virtual void Update(Player Player, List<Entity> Entities, uint GameTick)
    {
        if (HasDuration)
        {
            Duration--;
        }
    }
    public virtual void HandleEntityCollision(Player Player, List<Entity> Entities, uint GameTick)
    {

    }

    public abstract void DrawEffect();
    public void DrawDamageRadius()
    {
        var Game = BombarderGame.Instance;

        if (!Game.Settings.ShowDamageRadii)
        {
            return;
        }

        var Player = Game.Player;
        var Textures = Game.Textures;
        var Graphics = Game.Graphics;
        var SpriteBatch = Game.SpriteBatch;

        if (RadiusIsCircle)
        {
            SpriteBatch.Draw(
                Game.Textures.WhiteCircle,
                MathUtils.CreateRectangle(
                    Position - new Vector2(DamageRadius) + Game.ScreenCenter - Player.Position,
                    new Vector2(DamageRadius) * 2
                ),
                Color.DarkRed
            );
        }
        else
        {
            // Top Line
            SpriteBatch.Draw(Textures.White, new Rectangle(
                (int)(Position.X + RadiusOffset.X + Graphics.PreferredBackBufferWidth / 2F - Player.Position.X),
                (int)(Position.Y + RadiusOffset.Y + Graphics.PreferredBackBufferHeight / 2F - Player.Position.Y),
                RadiusSize.X * 2, 2), Color.White);
            // Bottom Line
            SpriteBatch.Draw(Textures.White, new Rectangle(
                    (int)(Position.X + RadiusOffset.X + Graphics.PreferredBackBufferWidth / 2F - Player.Position.X),
                    (int)(Position.Y + RadiusOffset.Y + RadiusSize.Y * 2 + Graphics.PreferredBackBufferHeight / 2F -
                          Player.Position.Y),
                    RadiusSize.X * 2,
                    2
                ),
                Color.White
            );
            // Left Line
            SpriteBatch.Draw(Textures.White, new Rectangle(
                    (int)(Position.X + RadiusOffset.X + Graphics.PreferredBackBufferWidth / 2F - Player.Position.X),
                    (int)(Position.Y + RadiusOffset.Y + Graphics.PreferredBackBufferHeight / 2F - Player.Position.Y),
                    2, RadiusSize.Y * 2
                ),
                Color.White
            );
            // Right Line
            SpriteBatch.Draw(Textures.White, new Rectangle(
                    (int)(Position.X + RadiusOffset.X + RadiusSize.X * 2 + Graphics.PreferredBackBufferWidth / 2F -
                          Player.Position.X),
                    (int)(Position.Y + RadiusOffset.Y + Graphics.PreferredBackBufferHeight / 2F - Player.Position.Y),
                    2,
                    RadiusSize.Y * 2
                ),
                Color.White
            );
        }
    }
    public void Draw()
    {
        DrawEffect();
        DrawDamageRadius();
    }

    public bool ShouldDelete()
    {
        return HasDuration && Duration == 0;
    }
}