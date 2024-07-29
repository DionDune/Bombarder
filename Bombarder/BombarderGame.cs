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
    }

    protected override void Initialize()
    {
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
        base.Initialize();
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

    #region Game/Player state interaction

    public void ResetGame()
    {
        Entities.Clear();
        EntitiesToAdd.Clear();

        Particles.Clear();

        MagicEffects.Clear();
        SelectedEffects.Clear();

        Player.SetDefaultStats(Player);
        Player.ResetPosition(Player);
    }

    public void ResurrectPlayer()
    {
        Player.SetDefaultStats(Player);
        Player.SetRandomLocalPosition(Player, 500, 1000);
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

    private void Window_ToggleFullscreen()
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

    private void UI_ChangePage(string PageType)
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

    #region Player

    private void PlayerMovement_InputHandler(List<Keys> NewPresses)
    {
        bool? HeadingUp = null;
        bool? HeadingLeft = null;


        const float unBoostDivider = 1.05f;

        float Speed = Player.BaseSpeed;
        if (NewPresses.Contains(Keys.LeftShift))
        {
            Speed = Player.BaseSpeed * Player.BoostMultiplier;
        }
        else if (Player.Momentum.LengthSquared() > Player.BaseSpeed * Player.BaseSpeed)
        {
            // Player is now slowing down from boost
            Speed = Player.Momentum.Length() / unBoostDivider;
        }

        Vector2 AccelerationVector = new();

        //Upward
        if (NewPresses.Contains(Keys.W))
        {
            AccelerationVector -= Vector2.UnitY;

            if (!NewPresses.Contains(Keys.S))
            {
                HeadingUp = true;
            }
        }

        //Downward
        if (NewPresses.Contains(Keys.S))
        {
            AccelerationVector += Vector2.UnitY;
            if (!NewPresses.Contains(Keys.W))
            {
                HeadingUp = false;
            }
        }

        //Left
        if (NewPresses.Contains(Keys.A))
        {
            AccelerationVector -= Vector2.UnitX;
            if (!NewPresses.Contains(Keys.D))
            {
                HeadingLeft = true;
            }
        }

        //Right
        if (NewPresses.Contains(Keys.D))
        {
            AccelerationVector += Vector2.UnitX;
            if (!NewPresses.Contains(Keys.A))
            {
                HeadingLeft = false;
            }
        }

        if (AccelerationVector != Vector2.Zero)
        {
            AccelerationVector.Normalize();
            AccelerationVector *= Player.Acceleration;
        }

        Vector2 DecelerationVector = new();

        //Slowdown
        if (!NewPresses.Contains(Keys.W) && !NewPresses.Contains(Keys.S) && Player.Momentum.Y != 0)
        {
            DecelerationVector.Y = MathF.Sign(Player.Momentum.Y);
        }

        if (!NewPresses.Contains(Keys.A) && !NewPresses.Contains(Keys.D) && Player.Momentum.X != 0)
        {
            DecelerationVector.X = MathF.Sign(Player.Momentum.X);
        }

        if (DecelerationVector != Vector2.Zero)
        {
            DecelerationVector.Normalize();
            DecelerationVector *= Player.Slowdown;
            // Do not decelerate player past 0
            if (MathF.Abs(DecelerationVector.X) > MathF.Abs(Player.Momentum.X))
            {
                DecelerationVector.X = Player.Momentum.X;
            }

            if (MathF.Abs(DecelerationVector.Y) > MathF.Abs(Player.Momentum.Y))
            {
                DecelerationVector.Y = Player.Momentum.Y;
            }
        }

        Player.Momentum += AccelerationVector - DecelerationVector;

        if (HeadingUp != null && HeadingLeft != null)
        {
            if ((HeadingUp == true && Player.Momentum.Y > 0) || (HeadingUp == false && Player.Momentum.Y < 0))
            {
                Player.Momentum += new Vector2(0, AccelerationVector.Y - DecelerationVector.Y);
            }

            if ((HeadingLeft == true && Player.Momentum.X > 0) || (HeadingLeft == false && Player.Momentum.X < 0))
            {
                Player.Momentum += new Vector2(AccelerationVector.X - DecelerationVector.X, 0);
            }
        }

        // Clamp momentum magnitude if it is greater than max speed
        float LengthSquared = Player.Momentum.LengthSquared();
        if (LengthSquared <= Speed * Speed)
        {
            return;
        }

        Player.Momentum = Vector2.Normalize(Player.Momentum);
        Player.Momentum *= Speed;
    }

    private void PlayerMovement_EnactMomentum()
    {
        Player.Position += Player.Momentum;
    }

    private void CheckEnactPlayerDeath()
    {
        if (!Player.IsDead)
        {
            return;
        }

        Player.IsDead = false;
        Player.Health = Player.HealthMax;
        UI_ChangePage("DeathPage");
    }

    #endregion

    #region Entity Interaction

    private void SpawnRandomEnemy(bool OnFringe)
    {
        int SpawnCount = random.Next(Settings.EnemySpawnCountRange.Item1, Settings.EnemySpawnCountRange.Item2 + 1);

        for (int i = 0; i < SpawnCount; i++)
        {
            if (random.Next(0, 4) == 0)
            {
                //Demon Eye
                SpawnEnemy(typeof(DemonEye), OnFringe, Vector2.Zero);
            }
            else
            {
                //Red Cube
                SpawnEnemy(typeof(RedCube), OnFringe, Vector2.Zero);
            }
        }
    }

    private void SpawnEnemy(Type EnemyClass, bool RandomLocation, Vector2 Location)
    {
        Vector2 SpawnPoint = Location;

        if (RandomLocation)
            //Spawns randomly from edges of screen
        {
            float SpawnAngle = random.Next(0, 360) * (float)(Math.PI / 180);
            int SpawnDistance = random.Next(
                (int)(Graphics.PreferredBackBufferWidth * 0.6F),
                (int)(Graphics.PreferredBackBufferWidth * 1.2)
            );
            SpawnPoint = new Vector2(
                Player.Position.X + SpawnDistance * (float)Math.Cos(SpawnAngle),
                Player.Position.Y + SpawnDistance * (float)Math.Sin(SpawnAngle)
            );
        }

        if (EnemyClass == typeof(RedCube))
            //Red Cube
            Entities.Add(new RedCube
            {
                Position = SpawnPoint
            });
        else if (EnemyClass == typeof(DemonEye))
            //Demon Eye
            Entities.Add(new DemonEye
            {
                Position = SpawnPoint
            });
        else if (EnemyClass == typeof(CubeMother))
            //Cube Mother
            Entities.Add(new CubeMother
            {
                Position = SpawnPoint
            });
        else if (EnemyClass == typeof(Spider))
            //Spider
            Entities.Add(new Spider
            {
                Position = SpawnPoint
            });
    }

    private void EnactEntities()
    {
        if (!Settings.RunEntityAI)
        {
            return;
        }

        foreach (Entity Entity in Entities)
        {
            Entity.EnactAI(Player);
        }

        foreach (var Entity in EntitiesToAdd)
        {
            Entities.Add(Entity);
        }

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
                    Player.CreateMagic(
                        new Vector2(
                            Mouse.GetState().X - Graphics.PreferredBackBufferWidth / 2F + Player.Position.X,
                            Mouse.GetState().Y - Graphics.PreferredBackBufferHeight / 2F + Player.Position.Y
                        ),
                        typeof(StaticOrb)
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
                    Player.CreateMagic(
                        new Vector2(
                            Mouse.GetState().X - Graphics.PreferredBackBufferWidth / 2F + Player.Position.X,
                            Mouse.GetState().Y - Graphics.PreferredBackBufferHeight / 2F + Player.Position.Y
                        ),
                        typeof(WideLaser)
                    );
                    SelectedEffects.Add(MagicEffects.Last());
                }
                else
                {
                    Player.CreateMagic(
                        new Vector2(
                            Mouse.GetState().X - Graphics.PreferredBackBufferWidth / 2F + Player.Position.X,
                            Mouse.GetState().Y - Graphics.PreferredBackBufferHeight / 2F + Player.Position.Y
                        ),
                        typeof(NonStaticOrb)
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
                        float Angle = (float)(Math.Atan2(YDiff, XDiff) * 180.0 / Math.PI);
                        Effect.Angle = Angle;
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
                Player.CreateMagic(Player.Position.Copy(), typeof(DissipationWave));
            }

            Input.IsClickingMiddle = true;
        }
        else
        {
            Input.IsClickingMiddle = false;
        }
    }

    #endregion

    #region Keyboard

    private bool IsNewlyPressed(List<Keys> NewPresses, Keys Key) =>
        NewPresses.Contains(Key) && !Input.PreviousKeys.Contains(Key);

    private void KeyboardHandler()
    {
        List<Keys> Keys_NewlyPressed = Keyboard.GetState().GetPressedKeys().ToList();


        //Toggle Fullscreen
        if (IsNewlyPressed(Keys_NewlyPressed, Keys.F))
        {
            Window_ToggleFullscreen();
        }

        //Toggle Pause
        if (IsNewlyPressed(Keys_NewlyPressed, Keys.Escape))
        {
            TogglePause();
        }

        if (GameState == "PlayPage")
        {
            //Movement
            PlayerMovement_InputHandler(Keys_NewlyPressed);


            //Enemy Spawning
            if (IsNewlyPressed(Keys_NewlyPressed, Keys.V))
            {
                SpawnRandomEnemy(true);
            }

            if (IsNewlyPressed(Keys_NewlyPressed, Keys.D1))
            {
                SpawnEnemy(typeof(CubeMother), false, new Vector2(0, 0));
            }

            if (IsNewlyPressed(Keys_NewlyPressed, Keys.D2))
            {
                SpawnEnemy(typeof(Spider), false, new Vector2(0, 0));
            }


            //Magic Creation
            if (IsNewlyPressed(Keys_NewlyPressed, Keys.Q))
            {
                Player.CreateMagic(Player.Position.Copy(), typeof(ForceWave));
            }

            if (IsNewlyPressed(Keys_NewlyPressed, Keys.Tab))
            {
                Player.CreateMagic(
                    new Vector2(
                        Mouse.GetState().X - Graphics.PreferredBackBufferWidth / 2F + Player.Position.X,
                        Mouse.GetState().Y - Graphics.PreferredBackBufferHeight / 2F + Player.Position.Y
                    ),
                    typeof(ForceContainer)
                );
            }

            if (IsNewlyPressed(Keys_NewlyPressed, Keys.T))
            {
                Player.CreateMagic(
                    new Vector2(
                        Mouse.GetState().X - Graphics.PreferredBackBufferWidth / 2F + Player.Position.X,
                        Mouse.GetState().Y - Graphics.PreferredBackBufferHeight / 2F + Player.Position.Y
                    ),
                    typeof(PlayerTeleport)
                );
            }


            //Settings Changes
            //  Entity AI
            if (IsNewlyPressed(Keys_NewlyPressed, Keys.I))
            {
                Settings.RunEntityAI = !Settings.RunEntityAI;
            }

            //  Toggle Hitboxes
            if (IsNewlyPressed(Keys_NewlyPressed, Keys.O))
            {
                Settings.ShowHitBoxes = !Settings.ShowHitBoxes;
                Settings.ShowDamageRadii = !Settings.ShowDamageRadii;
            }

            //  Toggle Trance Mode
            if (IsNewlyPressed(Keys_NewlyPressed, Keys.Back))
            {
                Settings.TranceMode = !Settings.TranceMode;
            }
        }


        Input.PreviousKeys = Keys_NewlyPressed;
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
        float AngleRadians = Angle * (float)(Math.PI / 180);

        var Origin = new Vector2(0f, 0.5f);
        Vector2 Scale = new Vector2(Width, Height);

        if (!Centered)
        {
            Point.Y -= Texture.Width * Scale.Y / 2 * (float)Math.Cos(AngleRadians);
            Point.X += Texture.Height * Scale.Y / 2 * (float)Math.Sin(AngleRadians);
        }

        SpriteBatch.Draw(Texture, Point, null, Color, AngleRadians, Origin, Scale, SpriteEffects.None, 0);
    }

    #endregion

    #region Fundamentals

    protected override void Update(GameTime gameTime)
    {
        GameTick++;

        KeyboardHandler();
        MouseHandler();

        if (GameState == "PlayPage")
        {
            //Player Interaction
            PlayerMovement_EnactMomentum();
            Player.Handler();
            CheckEnactPlayerDeath();

            //Entity Functions
            EnactEntities();
            Entity.PurgeDead(Entities, Player);
            //Particles
            Particle.EnactDuration(Particles);
            Particle.EnactParticles(Particles, GameTick);
            Particle.SpawnParticles(Particles, Player.Position, Graphics, GameTick);

            //Magic Functions
            MagicEffect.Update();
            MagicEffect.EnactDuration(MagicEffects);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime GameTime)
    {
        if (!Settings.TranceMode || (Settings.TranceMode && Settings.TranceModeClearScreen))
            GraphicsDevice.Clear(Settings.BackgroundColor);

        // BEGIN Draw ----
        SpriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp,
            DepthStencilState.None,
            RasterizerState.CullNone
        );


        //Particles
        foreach (var Particle in Particles.Where(Particle => !Particle.DrawLater))
        {
            Particle.Draw();
        }


        //Grid
        if (GameState == "PlayPage" || Settings.TranceMode)
        {
            DrawGrid();
        }

        //Ingame
        if (GameState == "PlayPage")
        {
            //Player
            SpriteBatch.Draw(Textures.White, new Rectangle(
                Graphics.PreferredBackBufferWidth / 2 - Player.Width / 2,
                Graphics.PreferredBackBufferHeight / 2 - Player.Height / 2,
                Player.Width, Player.Height), Color.Red);


            //Entities
            foreach (Entity Entity in Entities)
            {
                Entity.Draw();
            }

            //Magic
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
                    //Top Line
                    SpriteBatch.Draw(Textures.White, new Rectangle(
                        (int)(Effect.Position.X + Effect.RadiusOffset.X + Graphics.PreferredBackBufferWidth / 2F -
                              Player.Position.X),
                        (int)(Effect.Position.Y + Effect.RadiusOffset.Y + Graphics.PreferredBackBufferHeight / 2F -
                              Player.Position.Y),
                        Effect.RadiusSize.X * 2, 2), Color.White);
                    //Bottom Line
                    SpriteBatch.Draw(Textures.White, new Rectangle(
                        (int)(Effect.Position.X + Effect.RadiusOffset.X + Graphics.PreferredBackBufferWidth / 2F -
                              Player.Position.X),
                        (int)(Effect.Position.Y + Effect.RadiusOffset.Y + Effect.RadiusSize.Y * 2 +
                            Graphics.PreferredBackBufferHeight / 2F - Player.Position.Y),
                        Effect.RadiusSize.X * 2, 2), Color.White);
                    //Left Line
                    SpriteBatch.Draw(Textures.White, new Rectangle(
                        (int)(Effect.Position.X + Effect.RadiusOffset.X + Graphics.PreferredBackBufferWidth / 2F -
                              Player.Position.X),
                        (int)(Effect.Position.Y + Effect.RadiusOffset.Y + Graphics.PreferredBackBufferHeight / 2F -
                              Player.Position.Y),
                        2, Effect.RadiusSize.Y * 2), Color.White);
                    //Right Line
                    SpriteBatch.Draw(Textures.White, new Rectangle(
                        (int)(Effect.Position.X + Effect.RadiusOffset.X + Effect.RadiusSize.X * 2 +
                            Graphics.PreferredBackBufferWidth / 2F - Player.Position.X),
                        (int)(Effect.Position.Y + Effect.RadiusOffset.Y + Graphics.PreferredBackBufferHeight / 2F -
                              Player.Position.Y),
                        2, Effect.RadiusSize.Y * 2), Color.White);
                }
            }


            //Health Bar
            if (!Player.HealthInfinite && Player.Health < Player.HealthMax)
            {
                Point OrientPos = Player.ManaBarScreenOrientation.ToPoint(Graphics);
                float HealthPercent = (float)Player.Health / Player.HealthMax;

                Point HealthBarContainerPos = new Point(
                    OrientPos.X + Player.HealthBarOffset.X - 2,
                    OrientPos.Y + Player.HealthBarOffset.Y - Player.HealthBarDimensions.Y - 2
                );
                Point HealthBarPos = new Point(
                    OrientPos.X + Player.HealthBarOffset.X,
                    OrientPos.Y + Player.HealthBarOffset.Y - (int)(Player.HealthBarDimensions.Y * HealthPercent)
                );
                Point HealthBarDimensionsWithOffset = new Point(
                    Player.HealthBarDimensions.X + 4,
                    Player.HealthBarDimensions.Y + 4
                );

                SpriteBatch.Draw(
                    Textures.White,
                    new Rectangle(HealthBarContainerPos, HealthBarDimensionsWithOffset),
                    Color.White * 0.3F
                );

                UIPage.RenderOutline(
                    SpriteBatch,
                    Textures.White,
                    Color.White,
                    HealthBarContainerPos,
                    HealthBarDimensionsWithOffset.X,
                    HealthBarDimensionsWithOffset.Y,
                    2,
                    1F
                );

                SpriteBatch.Draw(
                    Textures.White,
                    new Rectangle(
                        HealthBarPos.X,
                        HealthBarPos.Y,
                        Player.HealthBarDimensions.X,
                        (int)(Player.HealthBarDimensions.Y * HealthPercent)
                    ),
                    Color.Red
                );
            }

            //Mana Bar
            if (!Player.ManaInfinite && Player.Mana < Player.ManaMax)
            {
                Point OrientPos = Player.ManaBarScreenOrientation.ToPoint(Graphics);
                float ManaPercent = (float)Player.Mana / Player.ManaMax;

                Point ManaContainerPos = new Point(
                    OrientPos.X + Player.ManaBarOffset.X - 2,
                    OrientPos.Y + Player.ManaBarOffset.Y - Player.ManaBarDimensions.Y - 2
                );
                Point ManaBarPos = new Point(
                    OrientPos.X + Player.ManaBarOffset.X,
                    OrientPos.Y + Player.ManaBarOffset.Y - (int)(Player.ManaBarDimensions.Y * ManaPercent)
                );
                Point ManaBarDimensionsWithOffset = new Point(
                    Player.ManaBarDimensions.X + 4,
                    Player.ManaBarDimensions.Y + 4
                );

                SpriteBatch.Draw(
                    Textures.White,
                    new Rectangle(ManaContainerPos, ManaBarDimensionsWithOffset),
                    Color.White * 0.3F
                );
                UIPage.RenderOutline(
                    SpriteBatch,
                    Textures.White,
                    Color.White,
                    ManaContainerPos,
                    ManaBarDimensionsWithOffset.X,
                    ManaBarDimensionsWithOffset.Y,
                    2,
                    1F
                );

                SpriteBatch.Draw(
                    Textures.White,
                    new Rectangle(
                        ManaBarPos.X,
                        ManaBarPos.Y,
                        Player.ManaBarDimensions.X,
                        (int)(Player.ManaBarDimensions.Y * ManaPercent)
                    ),
                    Color.Blue
                );
            }
        }

        //Later Particles
        foreach (var Particle in Particles.Where(Particle => Particle.DrawLater))
        {
            Particle.Draw();
        }


        UIPage_Current.RenderElements(SpriteBatch, Graphics, Textures);

        //Cursor
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