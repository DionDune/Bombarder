using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bombarder;

public static class RenderUtils
{
    public static void DrawGrid()
    {
        var Game = BombarderGame.Instance;
        var Settings = Game.Settings;

        if (!Game.Settings.ShowGrid)
        {
            return;
        }
        
        var Player = Game.Player;
        var Graphics = Game.Graphics;
        var SpriteBatch = Game.SpriteBatch;
        var Textures = Game.Textures;

        int BigLineWidth = 2 * Settings.GridLineSizeMult;
        int ThinLineWidth = 1 * Settings.GridLineSizeMult;
        Color GridColor = Settings.GridColor;

        if (Settings.TranceMode)
        {
            BigLineWidth = 2 * Settings.TranceModeGridLineMult;
            ThinLineWidth = 1 * Settings.TranceModeGridLineMult;
            GridColor = Settings.TranceModeGridColor;
        }

        Point ScreenStart = new Point(
            (int)(Player.Position.X - Graphics.PreferredBackBufferWidth / 2F),
            (int)(Player.Position.Y - Graphics.PreferredBackBufferHeight / 2F)
        );

        for (int y = 0; y < Graphics.PreferredBackBufferHeight; y++)
        {
            if ((y + ScreenStart.Y) % (300 * Settings.GridSizeMultiplier) == 0)
            {
                SpriteBatch.Draw(Textures.White,
                    new Rectangle(0, y - 1, Graphics.PreferredBackBufferWidth, BigLineWidth),
                    GridColor * 0.7F * Settings.GridOpacityMultiplier);
            }

            if ((y + ScreenStart.Y) % (100 * Settings.GridSizeMultiplier) == 0)
            {
                SpriteBatch.Draw(Textures.White,
                    new Rectangle(0, y, Graphics.PreferredBackBufferWidth, ThinLineWidth),
                    GridColor * 0.45F * Settings.GridOpacityMultiplier);
            }
        }

        for (int x = 0; x < Graphics.PreferredBackBufferWidth; x++)
        {
            if ((x + ScreenStart.X) % (300 * Settings.GridSizeMultiplier) == 0)
            {
                SpriteBatch.Draw(Textures.White,
                    new Rectangle(x - 1, 0, BigLineWidth, Graphics.PreferredBackBufferWidth),
                    GridColor * 0.7F * Settings.GridOpacityMultiplier);
            }

            if ((x + ScreenStart.X) % (100 * Settings.GridSizeMultiplier) == 0)
            {
                SpriteBatch.Draw(Textures.White,
                    new Rectangle(x, 0, ThinLineWidth, Graphics.PreferredBackBufferWidth),
                    GridColor * 0.45F * Settings.GridOpacityMultiplier);
            }
        }
    }

    public static void DrawLine(Vector2 point, float Length, float Angle, Color Color, float Thickness)
    {
        var Game = BombarderGame.Instance;
        var Origin = new Vector2(0f, 0.5f);
        var Scale = new Vector2(Length, Thickness);

        Game.SpriteBatch.Draw(Game.Textures.White, point, null, Color, Angle, Origin, Scale, SpriteEffects.None, 0);
    }

    public static void DrawRotatedTexture(
        Vector2 Point,
        Texture2D Texture,
        float Width,
        float Height,
        float Angle,
        bool Centered,
        Color Color)
    {
        float AngleRadians = MathUtils.ToRadians(Angle);

        var Origin = new Vector2(0f, 0.5f);
        Vector2 Scale = new Vector2(Width, Height);

        if (!Centered)
        {
            Point.Y -= Texture.Width * Scale.Y / 2 * MathF.Cos(AngleRadians);
            Point.X += Texture.Height * Scale.Y / 2 * MathF.Sin(AngleRadians);
        }

        var Game = BombarderGame.Instance;

        Game.SpriteBatch.Draw(Texture, Point, null, Color, AngleRadians, Origin, Scale, SpriteEffects.None, 0);
    }

    public static void RenderOutline(
        Texture2D Texture,
        Color color,
        Point Position,
        int Width,
        int Height,
        int BorderWidth,
        float BorderTransparency)
    {
        var SpriteBatch = BombarderGame.Instance.SpriteBatch;
        SpriteBatch.Draw(
            Texture,
            new Rectangle(Position.X, Position.Y, Width, BorderWidth),
            color * BorderTransparency
        );
        SpriteBatch.Draw(
            Texture,
            new Rectangle(
                Position.X + Width - BorderWidth,
                Position.Y + BorderWidth,
                BorderWidth,
                Height - BorderWidth
            ),
            color * BorderTransparency
        );
        SpriteBatch.Draw(
            Texture,
            new Rectangle(Position.X, Position.Y + Height - BorderWidth, Width - BorderWidth, BorderWidth),
            color * BorderTransparency
        );
        SpriteBatch.Draw(
            Texture,
            new Rectangle(Position.X, Position.Y + BorderWidth, BorderWidth, Height - BorderWidth * 2),
            color * BorderTransparency
        );
    }
}