using System;
using System.Collections.Generic;
using System.Linq;
using Bombarder.Entities;
using Bombarder.MagicEffects;
using Bombarder.Particles;
using Bombarder.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Bombarder;

public sealed class BombarderGame : Game
{
    public GraphicsDeviceManager Graphics { get; }
    public SpriteBatch SpriteBatch { get; private set; }

    public uint GameTick;

    List<UIPage> UIPages = new();
    UIPage CurrentPage;
    string GameState;
    string UIState;

    Song BackgroundSong;
    public Textures Textures { get; private set; }
    public Settings Settings { get; private set; }
    public readonly MouseInput MouseInput;
    public readonly KeyboardInput KeyboardInput;
    public Player Player { get; private set; }
    public World World { get; private set; }

    public Vector2 ScreenSize => new(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
    public Vector2 ScreenCenter => ScreenSize / 2F;

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
        MouseInput = new MouseInput();
    }

    protected override void Initialize()
    {
        GameTick = 0;

        UIPages = UIPage.GeneratePages();
        CurrentPage = UIPages[0];
        GameState = "StartPage";
        UIState = "Default";

        Textures = new Textures();
        Settings = new Settings();
        Player = new Player();
        World = new World();


        IsMouseVisible = false;

        KeyboardInput.AddKeyPressAction(Keys.F, ToggleFullscreen, "ToggleFullscreen");
        KeyboardInput.AddKeyPressAction(Keys.Escape, TogglePause, "TogglePause");

        // TODO: Add check for GameState
        KeyboardInput.AddKeyPressAction(Keys.V, World.EnemySpawner.SpawnRandomEnemy, "SpawnRandomEnemy");
        KeyboardInput.AddKeyPressAction(
            Keys.D1,
            () => World.SpawnEnemy<CubeMother>(Vector2.Zero),
            "SpawnCubeMother"
        );
        KeyboardInput.AddKeyPressAction(
            Keys.D2,
            () => World.SpawnEnemy<Spider>(Vector2.Zero),
            "SpawnSpider"
        );
        KeyboardInput.AddKeyPressAction(
            Keys.Q,
            () => Player.CreateMagic<ForceWave>(Player.Position.Copy()),
            "ForceWave"
        );
        KeyboardInput.AddKeyPressAction(Keys.Tab, () => Player.CreateMagic<ForceContainer>(
                MouseInput.Position - ScreenCenter + Player.Position
            ),
            "ForceContainer"
        );
        KeyboardInput.AddKeyPressAction(Keys.E, () => Player.CreateMagic<SpiderWeb>(
                MouseInput.Position - ScreenCenter + Player.Position
            ),
            "SpiderWeb"
        );
        KeyboardInput.AddKeyPressAction(Keys.T, () => Player.CreateMagic<PlayerTeleport>(
            MouseInput.Position - ScreenCenter + Player.Position
        ), "PlayerTeleport");
        KeyboardInput.AddKeyPressAction(Keys.I, () => Settings.RunEntityAI = !Settings.RunEntityAI, "ToggleEntityAI");
        KeyboardInput.AddKeyPressAction(Keys.O, () =>
            {
                Settings.ShowHitBoxes = !Settings.ShowHitBoxes;
                Settings.ShowDamageRadii = !Settings.ShowDamageRadii;
            },
            "ToggleHitboxes"
        );
        KeyboardInput.AddKeyPressAction(
            Keys.Back,
            () => Settings.TranceMode = !Settings.TranceMode,
            "ToggleTranceMode"
        );

        MouseInput.AddClickAction(MouseButtons.Left, () =>
        {
            if (GameState != "PlayPage")
            {
                return;
            }

            Player.CreateMagic<StaticOrb>(MouseInput.Position - ScreenCenter + Player.Position);
        }, "PrimaryAttack");

        MouseInput.AddClickAction(MouseButtons.Right, () =>
        {
            Player.CreateMagic<WideLaser>(MouseInput.Position - ScreenCenter + Player.Position);
            World.SelectedEffects.Add(World.MagicEffects.Last());
            
        }, "FireLaser");

        MouseInput.AddClickAction(MouseButtons.Middle,
            () => Player.CreateMagic<DissipationWave>(Player.Position.Copy()), "SpawnDissipationWave");

        MouseInput.AddReleaseAction(MouseButtons.Right, () =>
            {
                if (World.SelectedEffects.Count <= 0)
                {
                    return;
                }

                List<MagicEffect> ToRemove = World.SelectedEffects.OfType<WideLaser>().Cast<MagicEffect>().ToList();

                foreach (MagicEffect Effect in ToRemove)
                {
                    World.MagicEffects.Remove(Effect);
                }
            },
            "StopFiringLasers"
        );

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

    #region GameState interaction

    public void ResetGame()
    {
        World.Reset();

        Player.SetDefaultStats();
        Player.ResetPosition();
    }

    public void ResurrectPlayer()
    {
        Player.SetDefaultStats();
        Player.SetRandomLocalPosition(500, 1000);
        ChangePage("PlayPage");
    }

    public void ResumeGame()
    {
        ChangePage("PlayPage");
    }

    public void StartNewGame()
    {
        ResetGame();
        ChangePage("PlayPage");
    }

    private void TogglePause()
    {
        switch (GameState)
        {
            case "PlayPage":
                ChangePage("PausePage");
                break;
            case "PausePage":
            case "SettingsPage":
                ChangePage("PlayPage");
                break;
        }
    }

    public void OpenSettings()
    {
        ChangePage("SettingsPage");
    }

    #endregion

    #region UI

    private void ToggleFullscreen()
    {
        if (!Graphics.IsFullScreen)
        {
            Graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            Graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
        }
        else
        {
            Graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width / 3 * 2;
            Graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height / 3 * 2;
        }

        Graphics.ApplyChanges();
        Graphics.ToggleFullScreen();
    }

    public void ChangePage(string PageType)
    {
        GameState = PageType;

        if (CurrentPage == null)
        {
            return;
        }

        CurrentPage = UIPages.Find(Page => Page.GetType().Name == GameState);
    }

    #endregion

    #region Fundamentals

    protected override void Update(GameTime gameTime)
    {
        GameTick++;

        KeyboardInput.Update();
        MouseInput.Update();

        if (CurrentPage != null)
        {
            foreach (UIItem Item in CurrentPage.UIItems)
            {
                Item.Update();
            }
        }

        if (GameState == "PlayPage")
        {
            //Player Interaction
            Player.Update();

            //Entity Functions
            World.Update();
            if (Settings.RunEntityAI)
            {
                World.Entities.ForEach(Entity => Entity.EnactAI(Player));
                World.EntitiesToAdd.ForEach(World.Entities.Add);
                World.EntitiesToAdd.Clear();
            }

            Entity.PurgeDead(World.Entities, Player);
            //Particles
            World.Particles.ForEach(Particle => Particle.Update(GameTick));
            World.Particles.RemoveAll(Particle => Particle.ShouldDelete());
            Particle.SpawnParticles(World.Particles, Player.Position, GameTick);

            //Magic Functions
            World.MagicEffects.ForEach(Effect => Effect.Update(Player, World.Entities, GameTick));
            World.MagicEffects.RemoveAll(MagicEffect => MagicEffect.ShouldDelete());
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime GameTime)
    {
        if (!Settings.TranceMode || Settings.TranceModeClearScreen)
        {
            GraphicsDevice.Clear(Settings.BackgroundColor);
        }

        // BEGIN Draw ----
        SpriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp,
            DepthStencilState.None,
            RasterizerState.CullNone
        );


        // Particles
        World.Particles.Where(Particle => !Particle.DrawLater).ToList().ForEach(Particle => Particle.Draw());

        // Grid
        if (GameState == "PlayPage" || Settings.TranceMode)
        {
            RenderUtils.DrawGrid();
        }

        // Ingame
        if (GameState == "PlayPage")
        {
            // Player
            Player.Draw();

            World.Entities.ForEach(Entity => Entity.Draw());
            World.MagicEffects.ForEach(Effect => Effect.Draw());

            Player.DrawBars();
        }

        // Later Particles
        World.Particles.Where(Particle => Particle.DrawLater).ToList().ForEach(Particle => Particle.Draw());


        CurrentPage.RenderElements(Textures);

        // Cursor
        SpriteBatch.Draw(Textures.Cursor,
            MathUtils.CreateRectangle(
                MouseInput.Position - Textures.Cursor.Bounds.Size.ToVector2() / 2F * Settings.CursorSizeMultiplier,
                Textures.Cursor.Bounds.Size.ToVector2() * Settings.CursorSizeMultiplier
            ),
            Color.White
        );

        SpriteBatch.End();
        // END Draw ------

        base.Draw(GameTime);
    }

    #endregion
}