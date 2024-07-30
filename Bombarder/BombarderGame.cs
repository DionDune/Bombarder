using System;
using System.Collections.Generic;
using System.Linq;
using Bombarder.Entities;
using Bombarder.MagicEffects;
using Bombarder.Particles;
using Bombarder.UI;
using Bombarder.UI.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Bombarder;

public sealed class BombarderGame : Game
{
    public GraphicsDeviceManager Graphics { get; }
    public SpriteBatch SpriteBatch { get; private set; }

    public static readonly Random random = new();
    public uint GameTick;

    List<UIPage> UIPages = new();
    UIPage UIPage_Current;
    string GameState;
    string UIState;

    Song BackgroundSong;
    public Textures Textures { get; private set; }
    public Settings Settings { get; private set; }
    InputStates Input;
    public readonly KeyboardInput KeyboardInput;
    public Player Player { get; private set; }

    public readonly List<Entity> Entities = new();
    public readonly List<Entity> EntitiesToAdd = new();
    public readonly List<Particle> Particles = new();
    public readonly List<MagicEffect> MagicEffects = new();
    List<MagicEffect> SelectedEffects = new();

    public static readonly BombarderGame Instance = new();

    #region Initialization

    private BombarderGame()
    {
        Graphics = new GraphicsDeviceManager(this);
        Graphics.PreferredBackBufferWidth = 1800;
        Graphics.PreferredBackBufferHeight = 1000;
        Graphics.ApplyChanges();

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
        KeyboardInput = new KeyboardInput();
    }

    protected override void Initialize()
    {
        base.Initialize();

        GameTick = 0;

        UIPages = UIPage.GeneratePages();
        UIPage_Current = UIPages[0];
        GameState = "StartPage";
        UIState = "Default";

        Textures = new Textures();
        Settings = new Settings();
        Input = new InputStates();
        Player = new Player();


        IsMouseVisible = false;

        KeyboardInput.AddKeyPressAction(Keys.F, ToggleFullscreen, "ToggleFullscreen");
        KeyboardInput.AddKeyPressAction(Keys.Escape, TogglePause, "TogglePause");

        // TODO: Add check for GameState
        KeyboardInput.AddKeyPressAction(Keys.V, SpawnRandomEnemy, "SpawnRandomEnemy");
        KeyboardInput.AddKeyPressAction(Keys.D1, () => SpawnEnemy<CubeMother>(Vector2.Zero), "SpawnCubeMother");
        KeyboardInput.AddKeyPressAction(Keys.D2, () => SpawnEnemy<Spider>(Vector2.Zero), "SpawnSpider");
        KeyboardInput.AddKeyPressAction(Keys.Q, () => Player.CreateMagic<ForceWave>(Player.Position.Copy()), "ForceWave");
        KeyboardInput.AddKeyPressAction(Keys.Tab, () => Player.CreateMagic<ForceContainer>(
            new Vector2(
                Mouse.GetState().X - Graphics.PreferredBackBufferWidth / 2F + Player.Position.X,
                Mouse.GetState().Y - Graphics.PreferredBackBufferHeight / 2F + Player.Position.Y
            )), "ForceContainer");
        KeyboardInput.AddKeyPressAction(Keys.T, () => Player.CreateMagic<PlayerTeleport>(
            new Vector2(
                Mouse.GetState().X - Graphics.PreferredBackBufferWidth / 2F + Player.Position.X,
                Mouse.GetState().Y - Graphics.PreferredBackBufferHeight / 2F + Player.Position.Y
            )), "PlayerTeleport");
        KeyboardInput.AddKeyPressAction(Keys.I, () => Settings.RunEntityAI = !Settings.RunEntityAI, "ToggleEntityAI");
        KeyboardInput.AddKeyPressAction(Keys.O, () =>
        {
            Settings.ShowHitBoxes = !Settings.ShowHitBoxes;
            Settings.ShowDamageRadii = !Settings.ShowDamageRadii;
        }, "ToggleHitboxes");
        KeyboardInput.AddKeyPressAction(Keys.Back, () => Settings.TranceMode = !Settings.TranceMode, "ToggleTranceMode");
    }

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);


        BackgroundSong = Content.Load<Song>("Jon Shuemaker - Neurosis");
        //MediaPlayer.Play(BackgroundSong);
        MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
        MediaPlayer.IsRepeating = true;


        //Procedurally Creating and Assigning a 1x1 white texture to Color_White
        Textures.White = new Texture2D(GraphicsDevice, 1, 1);
        Textures.White.SetData(new[] { Color.White });

        Textures.WhiteCircle = Content.Load<Texture2D>("Circle");
        Textures.HalfWhiteCirlce = Content.Load<Texture2D>("HalfCircle");
        Textures.HollowCircle = Content.Load<Texture2D>("HollowCircle");

        Textures.Cursor = Content.Load<Texture2D>("Cursor");
        Textures.HitMarker = Content.Load<Texture2D>("HitMarker");

        //Demon Eye Textures
        Textures.DemonEye = (Content.Load<Texture2D>("DemonEye"), Content.Load<Texture2D>("DemonIris"));
    }

    private void MediaPlayer_MediaStateChanged(object Sender, EventArgs E)
    {
        // 0.0f is silent, 1.0f is full volume
        MediaPlayer.Volume -= 0.1f;
        MediaPlayer.Play(BackgroundSong);
    }

    #endregion

    /////////////////////////////////////////

    #region GameState interaction

    public void ResetGame()
    {
        Entities.Clear();
        EntitiesToAdd.Clear();

        Particles.Clear();

        MagicEffects.Clear();
        SelectedEffects.Clear();

        Player.SetDefaultStats();
        Player.ResetPosition();
    }

    public void ResurrectPlayer()
    {
        Player.SetDefaultStats();
        Player.SetRandomLocalPosition(500, 1000);
        UI_ChangePage("PlayPage");
    }

    public void ResumeGame()
    {
        UI_ChangePage("PlayPage");
    }

    public void StartNewGame()
    {
        ResetGame();
        UI_ChangePage("PlayPage");
    }

    private void TogglePause()
    {
        switch (GameState)
        {
            case "PlayPage":
                UI_ChangePage("PausePage");
                break;
            case "PausePage":
            case "SettingsPage":
                UI_ChangePage("PlayPage");
                break;
        }
    }

    public void OpenSettings()
    {
        UI_ChangePage("SettingsPage");
    }

    #endregion

    #region UI

    private void ToggleFullscreen()
    {
        if (!Graphics.IsFullScreen)
        {
            Graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            Graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            Graphics.ApplyChanges();
        }
        else
        {
            Graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width / 3 * 2;
            Graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height / 3 * 2;
            Graphics.ApplyChanges();
        }

        Graphics.ToggleFullScreen();
    }

    public void UI_ChangePage(string PageType)
    {
        GameState = PageType;

        if (UIPage_Current == null)
        {
            return;
        }

        foreach (var Page in UIPages.Where(Page => Page.GetType().Name == GameState))
        {
            UIPage_Current = Page;
        }
    }

    #endregion

    #region Entity Interaction

    private void SpawnRandomEnemy()
    {
        int SpawnCount = random.Next(Settings.EnemySpawnCountRange.Item1, Settings.EnemySpawnCountRange.Item2 + 1);

        for (int i = 0; i < SpawnCount; i++)
        {
            if (random.Next(0, 4) == 0)
            {
                // Demon Eye
                SpawnEnemy<DemonEye>();
            }
            else
            {
                // Red Cube
                SpawnEnemy<RedCube>();
            }
        }
    }

    private Vector2 GetRandomSpawnPoint()
    {
        //Spawns randomly from edges of screen
        float SpawnAngle = Utils.ToRadians(random.Next(0, 360));
        int SpawnDistance = random.Next(
            (int)(Graphics.PreferredBackBufferWidth * 0.6F),
            (int)(Graphics.PreferredBackBufferWidth * 1.2)
        );
        return new Vector2(
            Player.Position.X + SpawnDistance * MathF.Cos(SpawnAngle),
            Player.Position.Y + SpawnDistance * MathF.Sin(SpawnAngle)
        );
    }

    private void SpawnEnemy<T>(Vector2? Location = null) where T : Entity
    {
        Vector2 SpawnPoint = Location ?? GetRandomSpawnPoint();

        var Factory = Entity.EntityFactories[typeof(T).Name];
        var Enemy = Factory?.Invoke(SpawnPoint);

        if (Enemy == null)
        {
            return;
        }

        EntitiesToAdd.Add(Enemy);
    }

    private void UpdateEntities()
    {
        if (!Settings.RunEntityAI)
        {
            return;
        }

        foreach (Entity Entity in Entities)
        {
            Entity.EnactAI(Player);
        }

        EntitiesToAdd.ForEach(Entities.Add);
        EntitiesToAdd.Clear();
    }

    #endregion

    #region UserInput

    #region Mouse

    private void MouseHandler()
    {
        CheckMouseMove();
        CheckMouseClick();
    }

    private void CheckMouseMove()
    {
        if (UIPage_Current == null)
        {
            return;
        }

        foreach (UIItem Item in UIPage_Current.UIItems)
        {
            Rectangle ElementBounds = Item.GetElementBounds(Graphics);

            if (Item is not ButtonUIElement)
            {
                continue;
            }

            Item.SetHighlight(ElementBounds.Contains(Mouse.GetState().Position));
        }
    }

    public void CheckMouseClick()
    {
        //Left Click
        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            if (!Input.IsClickingLeft)
            {
                bool UIClicked = false;

                if (UIPage_Current != null)
                {
                    foreach (UIItem Item in UIPage_Current.UIItems)
                    {
                        Rectangle ElementBounds = Item.GetElementBounds(Graphics);

                        if (!ElementBounds.Contains(Mouse.GetState().Position))
                        {
                            continue;
                        }

                        UIClicked = true;

                        Item.Click();
                    }
                }

                if (!UIClicked)
                {
                    Player.CreateMagic<StaticOrb>(
                        new Vector2(
                            Mouse.GetState().X - Graphics.PreferredBackBufferWidth / 2F + Player.Position.X,
                            Mouse.GetState().Y - Graphics.PreferredBackBufferHeight / 2F + Player.Position.Y
                        )
                    );
                }
            }

            Input.IsClickingLeft = true;
        }
        else
        {
            Input.IsClickingLeft = false;
        }

        //Right Click
        if (Mouse.GetState().RightButton == ButtonState.Pressed)
        {
            if (!Input.IsClickingRight)
            {
                if (true)
                {
                    Player.CreateMagic<WideLaser>(
                        new Vector2(
                            Mouse.GetState().X - Graphics.PreferredBackBufferWidth / 2F + Player.Position.X,
                            Mouse.GetState().Y - Graphics.PreferredBackBufferHeight / 2F + Player.Position.Y
                        )
                    );
                    SelectedEffects.Add(MagicEffects.Last());
                }
                else
                {
                    Player.CreateMagic<NonStaticOrb>(
                        new Vector2(
                            Mouse.GetState().X - Graphics.PreferredBackBufferWidth / 2F + Player.Position.X,
                            Mouse.GetState().Y - Graphics.PreferredBackBufferHeight / 2F + Player.Position.Y
                        )
                    );
                }
            }
            else
            {
                if (MagicEffects.Count > 0)
                {
                    foreach (WideLaser Effect in SelectedEffects.OfType<WideLaser>())
                    {
                        float XDiff = Mouse.GetState().X - Graphics.PreferredBackBufferWidth / 2F;
                        float YDiff = Mouse.GetState().Y - Graphics.PreferredBackBufferHeight / 2F;
                        Effect.Angle = Utils.ToDegrees(MathF.Atan2(YDiff, XDiff));
                        Effect.Position = Player.Position.Copy();
                    }
                }
            }

            Input.IsClickingRight = true;
        }
        else
        {
            if (SelectedEffects.Count > 0)
            {
                List<MagicEffect> ToRemove = SelectedEffects.OfType<WideLaser>().Cast<MagicEffect>().ToList();

                foreach (MagicEffect Effect in ToRemove)
                {
                    MagicEffects.Remove(Effect);
                }
            }

            Input.IsClickingRight = false;
        }

        // Middle Click
        if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
        {
            if (!Input.IsClickingMiddle)
            {
                Player.CreateMagic<DissipationWave>(Player.Position.Copy());
            }

            Input.IsClickingMiddle = true;
        }
        else
        {
            Input.IsClickingMiddle = false;
        }
    }

    #endregion

    #endregion

    #region General Rendering Functions

    private void DrawGrid()
    {
        if (!Settings.ShowGrid)
        {
            return;
        }

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

    public void DrawLine(Vector2 point, float Length, float Angle, Color Color, float Thickness)
    {
        var Origin = new Vector2(0f, 0.5f);
        var Scale = new Vector2(Length, Thickness);

        SpriteBatch.Draw(Textures.White, point, null, Color, Angle, Origin, Scale, SpriteEffects.None, 0);
    }

    public void DrawRotatedTexture(
        Vector2 Point,
        Texture2D Texture,
        float Width,
        float Height,
        float Angle,
        bool Centered,
        Color Color)
    {
        float AngleRadians = Utils.ToRadians(Angle);

        var Origin = new Vector2(0f, 0.5f);
        Vector2 Scale = new Vector2(Width, Height);

        if (!Centered)
        {
            Point.Y -= Texture.Width * Scale.Y / 2 * MathF.Cos(AngleRadians);
            Point.X += Texture.Height * Scale.Y / 2 * MathF.Sin(AngleRadians);
        }

        SpriteBatch.Draw(Texture, Point, null, Color, AngleRadians, Origin, Scale, SpriteEffects.None, 0);
    }

    #endregion

    #region Fundamentals

    protected override void Update(GameTime gameTime)
    {
        GameTick++;

        KeyboardInput.Update();
        MouseHandler();

        if (GameState == "PlayPage")
        {
            //Player Interaction
            Player.Update();

            //Entity Functions
            UpdateEntities();
            Entity.PurgeDead(Entities, Player);
            //Particles
            Particle.EnactDuration(Particles);
            Particle.Update(Particles, GameTick);
            Particle.SpawnParticles(Particles, Player.Position, Graphics, GameTick);

            //Magic Functions
            MagicEffect.Update();
            MagicEffect.EnactDuration(MagicEffects);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime GameTime)
    {
        if (!Settings.TranceMode || Settings.TranceModeClearScreen)
            GraphicsDevice.Clear(Settings.BackgroundColor);

        // BEGIN Draw ----
        SpriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp,
            DepthStencilState.None,
            RasterizerState.CullNone
        );


        // Particles
        foreach (var Particle in Particles.Where(Particle => !Particle.DrawLater))
        {
            Particle.Draw();
        }


        // Grid
        if (GameState == "PlayPage" || Settings.TranceMode)
        {
            DrawGrid();
        }

        // Ingame
        if (GameState == "PlayPage")
        {
            // Player
            Player.Draw();

            // Entities
            foreach (Entity Entity in Entities)
            {
                Entity.Draw();
            }

            // Magic
            foreach (MagicEffect Effect in MagicEffects)
            {
                Effect.Draw();

                if (!Settings.ShowDamageRadii)
                {
                    continue;
                }

                if (Effect.RadiusIsCircle)
                {
                    SpriteBatch.Draw(Textures.WhiteCircle, new Rectangle(
                        (int)(Effect.Position.X - Effect.DamageRadius + Graphics.PreferredBackBufferWidth / 2F -
                              Player.Position.X),
                        (int)(Effect.Position.Y - Effect.DamageRadius + Graphics.PreferredBackBufferHeight / 2F -
                              Player.Position.Y),
                        (int)Effect.DamageRadius * 2, (int)Effect.DamageRadius * 2), Color.DarkRed);
                }
                else
                {
                    // Top Line
                    SpriteBatch.Draw(Textures.White, new Rectangle(
                        (int)(Effect.Position.X + Effect.RadiusOffset.X + Graphics.PreferredBackBufferWidth / 2F -
                              Player.Position.X),
                        (int)(Effect.Position.Y + Effect.RadiusOffset.Y + Graphics.PreferredBackBufferHeight / 2F -
                              Player.Position.Y),
                        Effect.RadiusSize.X * 2, 2), Color.White);
                    // Bottom Line
                    SpriteBatch.Draw(Textures.White, new Rectangle(
                        (int)(Effect.Position.X + Effect.RadiusOffset.X + Graphics.PreferredBackBufferWidth / 2F -
                              Player.Position.X),
                        (int)(Effect.Position.Y + Effect.RadiusOffset.Y + Effect.RadiusSize.Y * 2 +
                            Graphics.PreferredBackBufferHeight / 2F - Player.Position.Y),
                        Effect.RadiusSize.X * 2, 2), Color.White);
                    // Left Line
                    SpriteBatch.Draw(Textures.White, new Rectangle(
                        (int)(Effect.Position.X + Effect.RadiusOffset.X + Graphics.PreferredBackBufferWidth / 2F -
                              Player.Position.X),
                        (int)(Effect.Position.Y + Effect.RadiusOffset.Y + Graphics.PreferredBackBufferHeight / 2F -
                              Player.Position.Y),
                        2, Effect.RadiusSize.Y * 2), Color.White);
                    // Right Line
                    SpriteBatch.Draw(Textures.White, new Rectangle(
                        (int)(Effect.Position.X + Effect.RadiusOffset.X + Effect.RadiusSize.X * 2 +
                            Graphics.PreferredBackBufferWidth / 2F - Player.Position.X),
                        (int)(Effect.Position.Y + Effect.RadiusOffset.Y + Graphics.PreferredBackBufferHeight / 2F -
                              Player.Position.Y),
                        2, Effect.RadiusSize.Y * 2), Color.White);
                }
            }

            Player.DrawBars();
        }

        // Later Particles
        foreach (var Particle in Particles.Where(Particle => Particle.DrawLater))
        {
            Particle.Draw();
        }


        UIPage_Current.RenderElements(SpriteBatch, Graphics, Textures);

        // Cursor
        SpriteBatch.Draw(Textures.Cursor, new Rectangle(
            Mouse.GetState().X - (int)(Textures.Cursor.Width / 2F * Settings.CursorSizeMultiplier),
            Mouse.GetState().Y - (int)(Textures.Cursor.Height / 2F * Settings.CursorSizeMultiplier),
            (int)(Textures.Cursor.Width * Settings.CursorSizeMultiplier),
            (int)(Textures.Cursor.Height * Settings.CursorSizeMultiplier)), Color.White);


        SpriteBatch.End();
        // END Draw ------

        base.Draw(GameTime);
    }

    #endregion
}