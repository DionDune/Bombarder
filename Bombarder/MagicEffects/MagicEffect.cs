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

    public static readonly Dictionary<string, Func<Vector2, Player, MagicEffect>> MagicEffectsFactories = new()
    {
        { "DissipationWave", (Position, _) => new DissipationWave(Position) },
        { "ForceContainer", (Position, Player) => new ForceContainer(Player.Position, Position) },
        { "ForceWave", (Position, _) => new ForceWave(Position) },
        { "NonStaticOrb", (Position, Player) => new NonStaticOrb(Player.Position, Position) },
        { "PlayerTeleport", (Position, _) => new PlayerTeleport(Position) },
        { "StaticOrb", (Position, _) => new StaticOrb(Position) },
        { "WideLaser", (Position, Player) => new WideLaser(Player.Position, Position) },
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