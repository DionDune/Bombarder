using System;
using System.Collections.Generic;
using System.Linq;
using Bombarder.Entities;
using Bombarder.MagicEffects;
using Bombarder.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Bombarder;

public class Game1 : Game
{
    #region Variable Defenition

    public GraphicsDeviceManager Graphics { get; }
    public SpriteBatch SpriteBatch { get; private set; }

    public static Random random = new();
    public static uint GameTick;

    List<UIPage> UIPages = new();
    UIPage UIPage_Current;
    string GameState;
    string UIState;

    Song BackgroundSong;
    public Textures Textures { get; private set; }
    public Settings Settings { get; private set; }
    InputStates Input;
    public Player Player { get; private set; }

    public static readonly List<Entity> Entities = new();
    public static readonly List<Entity> EntitiesToAdd = new();
    public static readonly List<Particle> Particles = new();
    List<MagicEffect> MagicEffects = new();
    List<MagicEffect> SelectedEffects = new();

    #endregion

    #region Initialization

    public Game1()
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
        GameState = "Start";
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
        UI_ChangePage("Play");
    }

    public void ResumeGame()
    {
        UI_ChangePage("Play");
    }

    public void StartNewGame()
    {
        ResetGame();
        UI_ChangePage("Play");
    }

    private void TogglePause()
    {
        switch (GameState)
        {
            case "Play":
                UI_ChangePage("Pause");
                break;
            case "Pause":
            case "Settings":
                UI_ChangePage("Play");
                break;
        }
    }

    public void OpenSettings()
    {
        UI_ChangePage("Settings");
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

        foreach (var Page in UIPages.Where(page => page.Type == GameState))
        {
            UIPage_Current = Page;
        }
    }

    private void UserControl_ButtonPress(List<string> Data)
    {
        if (Data.Contains("Start New"))
        {
            StartNewGame();
        }
        else if (Data.Contains("Resume"))
        {
            ResumeGame();
        }
        else if (Data.Contains("Settings"))
        {
            OpenSettings();
        }
        else if (Data.Contains("Respawn"))
        {
            StartNewGame();
        }
        else if (Data.Contains("Resurrect"))
        {
            ResurrectPlayer();
        }
        else if (Data.Contains("Quit"))
        {
            Environment.Exit(0);
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
        UI_ChangePage("Death");
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

    /////////////////////////////////////////

    #region Magic Interaction

    private void CreateMagic(int X, int Y, Type MagicType)
    {
        if (MagicType == typeof(StaticOrb))
        {
            if (Player.CheckUseMana(StaticOrb.ManaCost))
            {
                MagicEffects.Add(new StaticOrb(new Vector2(X, Y)));
            }
        }
        else if (MagicType == typeof(NonStaticOrb))
        {
            //Calculating Angle
            float XDiff = X - Player.Position.X;
            float YDiff = Y - Player.Position.Y;
            float Angle = (float)(Math.Atan2(YDiff, XDiff) * 180.0 / Math.PI);

            MagicEffects.Add(new NonStaticOrb(new Vector2(Player.Position.X, Player.Position.Y), Angle));
        }
        else if (MagicType == typeof(DissipationWave))
        {
            if (Player.CheckUseMana(DissipationWave.ManaCost))
            {
                MagicEffects.Add(new DissipationWave(new Vector2(X, Y)));
            }
        }
        else if (MagicType == typeof(ForceWave))
        {
            if (Player.CheckUseMana(ForceWave.ManaCost))
            {
                MagicEffects.Add(new ForceWave(new Vector2(X, Y)));
            }
        }
        else if (MagicType == typeof(ForceContainer))
        {
            if (Player.CheckUseMana(ForceContainer.ManaCost))
            {
                MagicEffects.Add(new ForceContainer(new Vector2(Player.Position.X, Player.Position.Y),
                    new Vector2(X, Y)));
            }
        }
        else if (MagicType == typeof(WideLaser))
        {
            //Calculating Angle
            float XDiff = X - Player.Position.X;
            float YDiff = Y - Player.Position.Y;
            float Angle = (float)(Math.Atan2(YDiff, XDiff) * 180.0 / Math.PI);

            MagicEffects.Add(new WideLaser(new Vector2((int)Player.Position.X, (int)Player.Position.Y), Angle));
        }
        else if (MagicType == typeof(PlayerTeleport))
        {
            if (Player.CheckUseMana(PlayerTeleport.ManaCost))
            {
                MagicEffects.Add(new PlayerTeleport(new Vector2(Player.Position.X, Player.Position.Y),
                    new Vector2(X, Y)));
            }
        }
    }

    private void EnactMagic()
    {
        foreach (MagicEffect Effect in MagicEffects)
        {
            Effect.EnactEffect(Player, Entities, GameTick);
        }
    }

    #endregion

    /////////////////////////////////////////

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
            (Point, Point) ElementBounds = Item.getElementBounds(Graphics);
            Point TopLeft = ElementBounds.Item1;
            Point BottomRight = ElementBounds.Item2;


            if (Item.Type != "Button")
            {
                continue;
            }

            if (Mouse.GetState().X > TopLeft.X && Mouse.GetState().X < BottomRight.X &&
                Mouse.GetState().Y > TopLeft.Y && Mouse.GetState().Y < BottomRight.Y)
            {
                Item.SetHighlight(true);
            }
            else
            {
                Item.SetHighlight(false);
            }
        }
    }

    private void CheckMouseClick()
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
                        (Point, Point) ElementBounds = Item.getElementBounds(Graphics);
                        Point TopLeft = ElementBounds.Item1;
                        Point BottomRight = ElementBounds.Item2;

                        if (
                            Mouse.GetState().X <= TopLeft.X || Mouse.GetState().X >= BottomRight.X ||
                            Mouse.GetState().Y <= TopLeft.Y || Mouse.GetState().Y >= BottomRight.Y
                        )
                        {
                            continue;
                        }

                        UIClicked = true;

                        if (Item.Type == "Button")
                        {
                            UserControl_ButtonPress(Item.Data);
                        }
                    }
                }

                if (!UIClicked)
                {
                    CreateMagic(
                        (int)(Mouse.GetState().X - Graphics.PreferredBackBufferWidth / 2F + Player.Position.X),
                        (int)(Mouse.GetState().Y - Graphics.PreferredBackBufferHeight / 2F + Player.Position.Y),
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
                    CreateMagic(
                        (int)(Mouse.GetState().X - Graphics.PreferredBackBufferWidth / 2F + Player.Position.X),
                        (int)(Mouse.GetState().Y - Graphics.PreferredBackBufferHeight / 2F + Player.Position.Y),
                        typeof(WideLaser)
                    );
                    SelectedEffects.Add(MagicEffects.Last());
                }
                else
                {
                    CreateMagic(
                        (int)(Mouse.GetState().X - Graphics.PreferredBackBufferWidth / 2F + Player.Position.X),
                        (int)(Mouse.GetState().Y - Graphics.PreferredBackBufferHeight / 2F + Player.Position.Y),
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
                        float xDiff = Mouse.GetState().X - Graphics.PreferredBackBufferWidth / 2F;
                        float yDiff = Mouse.GetState().Y - Graphics.PreferredBackBufferHeight / 2F;
                        float Angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);
                        Effect.Angle = Angle;
                        Effect.Position = new Vector2((int)Player.Position.X, (int)Player.Position.Y);
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

        //Middle Click
        if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
        {
            if (!Input.IsClickingMiddle)
            {
                CreateMagic((int)Player.Position.X, (int)Player.Position.Y, typeof(DissipationWave));
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

        if (GameState == "Play")
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
                CreateMagic((int)Player.Position.X, (int)Player.Position.Y, typeof(ForceWave));
            }

            if (IsNewlyPressed(Keys_NewlyPressed, Keys.Tab))
            {
                CreateMagic((int)(Mouse.GetState().X - Graphics.PreferredBackBufferWidth / 2F + Player.Position.X),
                    (int)(Mouse.GetState().Y - Graphics.PreferredBackBufferHeight / 2F + Player.Position.Y),
                    typeof(ForceContainer));
            }

            if (IsNewlyPressed(Keys_NewlyPressed, Keys.T))
            {
                CreateMagic((int)(Mouse.GetState().X - Graphics.PreferredBackBufferWidth / 2F + Player.Position.X),
                    (int)(Mouse.GetState().Y - Graphics.PreferredBackBufferHeight / 2F + Player.Position.Y),
                    typeof(PlayerTeleport));
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

    void DrawRotatedTexture(Vector2 Point, Texture2D Texture, float Width, float Height, float Angle, bool Centered,
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

        if (GameState == "Play")
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
            EnactMagic();
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
            Particle.Draw(this);
        }


        //Grid
        if (GameState == "Play" || Settings.TranceMode)
        {
            DrawGrid();
        }

        //Ingame
        if (GameState == "Play")
        {
            //Player
            SpriteBatch.Draw(Textures.White, new Rectangle(
                Graphics.PreferredBackBufferWidth / 2 - Player.Width / 2,
                Graphics.PreferredBackBufferHeight / 2 - Player.Height / 2,
                Player.Width, Player.Height), Color.Red);


            //Entities
            foreach (Entity Entity in Entities)
            {
                Entity.Draw(this);
            }

            //Magic
            foreach (MagicEffect Effect in MagicEffects)
            {
                switch (Effect)
                {
                    case DissipationWave DissipationWave:
                        SpriteBatch.Draw(Textures.WhiteCircle, new Rectangle(
                                (int)(DissipationWave.Position.X - DissipationWave.Radius +
                                    Graphics.PreferredBackBufferWidth / 2F - Player.Position.X),
                                (int)(DissipationWave.Position.Y - DissipationWave.Radius +
                                    Graphics.PreferredBackBufferHeight / 2F - (int)Player.Position.Y),
                                (int)DissipationWave.Radius * 2, (int)DissipationWave.Radius * 2),
                            DissipationWave.Colour * DissipationWave.Opacity);
                        break;
                    case ForceWave ForceWave:
                    {
                        SpriteBatch.Draw(Textures.WhiteCircle, new Rectangle(
                            (int)(ForceWave.Position.X - ForceWave.Radius + Graphics.PreferredBackBufferWidth / 2F -
                                  Player.Position.X),
                            (int)(ForceWave.Position.Y - ForceWave.Radius + Graphics.PreferredBackBufferHeight / 2F -
                                  Player.Position.Y),
                            (int)(ForceWave.Radius * 2), (int)(ForceWave.Radius * 2)), ForceWave.Colour * 0.3F);
                        for (int i = 0; i < ForceWave.BorderWidth; i++)
                        {
                            SpriteBatch.Draw(Textures.HollowCircle, new Rectangle(
                                    (int)(ForceWave.Position.X - ForceWave.Radius + i +
                                        Graphics.PreferredBackBufferWidth / 2F - Player.Position.X),
                                    (int)(ForceWave.Position.Y - ForceWave.Radius + i +
                                        Graphics.PreferredBackBufferHeight / 2F - (int)Player.Position.Y),
                                    (int)(ForceWave.Radius * 2) - i * 2, (int)(ForceWave.Radius * 2) - i * 2),
                                ForceWave.Colour * 0.7F);
                        }

                        break;
                    }
                    case ForceContainer Container:
                        SpriteBatch.Draw(Textures.WhiteCircle, new Rectangle(
                                (int)(Container.Position.X - Container.CurrentRadius +
                                    (Graphics.PreferredBackBufferWidth / 2F) - Player.Position.X),
                                (int)(Container.Position.Y - Container.CurrentRadius +
                                    Graphics.PreferredBackBufferHeight / 2F - Player.Position.Y),
                                (int)Container.CurrentRadius * 2, (int)Container.CurrentRadius * 2),
                            Container.Colour * Container.Opacity);
                        break;
                    case WideLaser Laser:
                    {
                        //Old
                        // if (Settings.ShowDamageRadii)
                        // {
                        //     float AngleRadians = Laser.Angle * (float)(Math.PI / 180);
                        //     float RightAngleRadians = (Laser.Angle + 90) * (float)(Math.PI / 180);
                        //
                        //     Vector2 LeftLine = new Vector2(
                        //         Graphics.PreferredBackBufferWidth / 2F -
                        //         WideLaser.Width / 2F * (float)Math.Cos(RightAngleRadians),
                        //         Graphics.PreferredBackBufferHeight / 2F -
                        //         WideLaser.Width / 2F * (float)Math.Sin(RightAngleRadians)
                        //     );
                        //     LeftLine.X += WideLaser.InitialDistance * (float)Math.Cos(AngleRadians);
                        //     LeftLine.Y += WideLaser.InitialDistance * (float)Math.Sin(AngleRadians);
                        //     Vector2 RightLine = new Vector2(
                        //         Graphics.PreferredBackBufferWidth / 2F +
                        //         (WideLaser.Width / 2F - 5) * (float)Math.Cos(RightAngleRadians),
                        //         Graphics.PreferredBackBufferHeight / 2F +
                        //         (WideLaser.Width / 2F - 5) * (float)Math.Sin(RightAngleRadians)
                        //     );
                        //     RightLine.X += WideLaser.InitialDistance * (float)Math.Cos(AngleRadians);
                        //     RightLine.Y += WideLaser.InitialDistance * (float)Math.Sin(AngleRadians);
                        //
                        //
                        //     DrawRotatedTexture(LeftLine, Textures.White, WideLaser.Width,
                        //         WideLaser.Range, Laser.Angle + 90, false,
                        //         Laser.PrimaryColor * WideLaser.Opacity);
                        //     DrawRotatedTexture(LeftLine, Textures.White, 5, WideLaser.Range,
                        //         Laser.Angle + 90, false, Laser.SecondaryColor);
                        //     DrawRotatedTexture(RightLine, Textures.White, 5, WideLaser.Range,
                        //         Laser.Angle + 90, false, Laser.SecondaryColor);
                        //
                        //     float Scale = (float)WideLaser.Width / Textures.HalfWhiteCirlce.Width;
                        //     DrawRotatedTexture(LeftLine, Textures.HalfWhiteCirlce, Scale, Scale, Laser.Angle + 90, true,
                        //         Laser.PrimaryColor * WideLaser.Opacity);
                        // }

                        //New accounting for Laser Spread
                        if (Settings.ShowDamageRadii)
                        {
                            float TrueSpread = WideLaser.Spread * WideLaser.TrueSpreadMultiplier;
                            float AngleRadians = Laser.Angle * (float)(Math.PI / 180);
                            float AngleRadiansLeft = (Laser.Angle - TrueSpread) * (float)(Math.PI / 180);
                            float AngleRadiansRight = (Laser.Angle + TrueSpread) * (float)(Math.PI / 180);

                            Vector2 Start = new Vector2(
                                Laser.Position.X + Graphics.PreferredBackBufferWidth / 2F - (int)Player.Position.X,
                                Laser.Position.Y + Graphics.PreferredBackBufferHeight / 2F - (int)Player.Position.Y);

                            DrawLine(Start, WideLaser.Range, AngleRadiansLeft, Laser.SecondaryColor, 10);
                            DrawLine(Start, WideLaser.Range, AngleRadians, Laser.MarkerColor, 10);
                            DrawLine(Start, WideLaser.Range, AngleRadiansRight, Laser.SecondaryColor, 10);
                        }

                        break;
                    }
                }

                if (Settings.ShowDamageRadii)
                {
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
            }


            //Health Bar
            if (!Player.HealthInfinite && Player.Health < Player.HealthMax)
            {
                Point OrientPos = UIItem.GetOritentationPosition(Graphics, Player.ManaBarScreenOrientation);
                float HealthPercent = (float)Player.Health / Player.HealthMax;

                Point HealthBarContainerPos = new Point(OrientPos.X + Player.HealthBarOffset.X,
                    OrientPos.Y + Player.HealthBarOffset.Y - Player.HealthBarDimentions.Y);
                Point HealthBarPos = new Point(OrientPos.X + Player.HealthBarOffset.X,
                    OrientPos.Y + Player.HealthBarOffset.Y - (int)(Player.HealthBarDimentions.Y * HealthPercent));


                SpriteBatch.Draw(Textures.White, new Rectangle(HealthBarContainerPos.X - 2,
                    HealthBarContainerPos.Y - 2,
                    Player.ManaBarDimentions.X + 4, Player.ManaBarDimentions.Y + 4), Color.White * 0.3F);
                UIPage.RenderOutline(SpriteBatch, Textures.White, Color.White, HealthBarContainerPos.X - 2,
                    HealthBarContainerPos.Y - 2, Player.ManaBarDimentions.X + 4, Player.ManaBarDimentions.Y + 4, 2,
                    1F);

                SpriteBatch.Draw(Textures.White, new Rectangle(HealthBarPos.X, HealthBarPos.Y,
                    Player.ManaBarDimentions.X, (int)(Player.ManaBarDimentions.Y * HealthPercent)), Color.Red);
            }

            //Mana Bar
            if (!Player.ManaInfinite && Player.Mana < Player.ManaMax)
            {
                Point OrientPos = UIItem.GetOritentationPosition(Graphics, Player.ManaBarScreenOrientation);
                float ManaPercent = (float)Player.Mana / Player.ManaMax;

                Point ManaContainerPos = new Point(OrientPos.X + Player.ManaBarOffset.X,
                    OrientPos.Y + Player.ManaBarOffset.Y - Player.ManaBarDimentions.Y);
                Point ManaBarPos = new Point(OrientPos.X + Player.ManaBarOffset.X,
                    OrientPos.Y + Player.ManaBarOffset.Y - (int)(Player.ManaBarDimentions.Y * ManaPercent));


                SpriteBatch.Draw(Textures.White, new Rectangle(ManaContainerPos.X - 2, ManaContainerPos.Y - 2,
                    Player.ManaBarDimentions.X + 4, Player.ManaBarDimentions.Y + 4), Color.White * 0.3F);
                UIPage.RenderOutline(SpriteBatch, Textures.White, Color.White, ManaContainerPos.X - 2,
                    ManaContainerPos.Y - 2, Player.ManaBarDimentions.X + 4, Player.ManaBarDimentions.Y + 4, 2, 1F);

                SpriteBatch.Draw(Textures.White, new Rectangle(ManaBarPos.X, ManaBarPos.Y,
                    Player.ManaBarDimentions.X, (int)(Player.ManaBarDimentions.Y * ManaPercent)), Color.Blue);
            }
        }

        //Later Particles
        foreach (var Particle in Particles.Where(Particle => Particle.DrawLater))
        {
            Particle.Draw(this);
        }


        //UI
        foreach (var Page in UIPages.Where(Page => Page.Type == GameState))
        {
            UIPage.RenderElements(SpriteBatch, Graphics, Textures, Page.UIItems);
        }

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