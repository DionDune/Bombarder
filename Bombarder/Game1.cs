using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bombarder
{
    public class Game1 : Game
    {
        #region Variable Defenition

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static Random random = new Random();
        public static uint GameTick;

        List<UIPage> UIPages = new List<UIPage>();
        UIPage UIPage_Current;
        string GameState;
        string UIState;

        Song BackgroundSong;
        Textures Textures;
        Settings Settings;
        InputStates Input;
        Player Player;

        public static List<Entity> Entities = new List<Entity>();
        public static List<Entity> EntitiesToAdd = new List<Entity>();
        public static Object.ObjectContainer Objects = new Object.ObjectContainer();
        public static List<Particle> Particles = new List<Particle>();
        List<MagicEffect> MagicEffects = new List<MagicEffect>();
        List<MagicEffect> SelectedEffects = new List<MagicEffect>();

        #endregion

        #region Initialization

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1800;
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.ApplyChanges();

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
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            BackgroundSong = Content.Load<Song>("Jon Shuemaker - Neurosis");
            MediaPlayer.Play(BackgroundSong);
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
            MediaPlayer.IsRepeating = true;


            //Procedurally Creating and Assigning a 1x1 white texture to Color_White
            Textures.White = new Texture2D(GraphicsDevice, 1, 1);
            Textures.White.SetData(new Color[1] { Color.White });

            Textures.WhiteCircle = Content.Load<Texture2D>("Circle");
            Textures.HalfWhiteCirlce = Content.Load<Texture2D>("HalfCircle");
            Textures.HollowCircle = Content.Load<Texture2D>("HollowCircle");

            Textures.Cursor = Content.Load<Texture2D>("Cursor");
            Textures.HitMarker = Content.Load<Texture2D>("HitMarker");

            //Demon Eye Textures
            Textures.DemonEye = (Content.Load<Texture2D>("DemonEye"), Content.Load<Texture2D>("DemonIris"));
        }

        void MediaPlayer_MediaStateChanged(object sender, System.
                                           EventArgs e)
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

            Objects.GeneralObjects.Clear();
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

        #endregion

        #region UI

        private void Window_ToggleFullscreen()
        {
            if (!_graphics.IsFullScreen)
            {
                _graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
                _graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
                _graphics.ApplyChanges();
            }
            else
            {
                _graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width / 3 * 2;
                _graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height / 3 * 2;
                _graphics.ApplyChanges();
            }

            _graphics.ToggleFullScreen();
        }

        private void UI_ChangePage(string PageType)
        {
            GameState = PageType;

            if (UIPage_Current != null)
            {
                foreach (UIPage page in UIPages)
                {
                    if (page.Type == GameState)
                    {
                        UIPage_Current = page;
                    }
                }
            }
        }
        private void TogglePause()
        {
            if (GameState == "Play")
            {
                UI_ChangePage("Pause");
            }
            else if (GameState == "Pause")
            {
                UI_ChangePage("Play");
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
                System.Environment.Exit(0);
            }
        }

        #endregion

        #region Player

        private void PlayerMovement_InputHandler(List<Keys> NewPresses)
        {
            bool? HeadingUp = null;
            bool? HeadingLeft = null;


            const float UnBoostDivider = 1.05f;

            float Speed = Player.BaseSpeed;
            if (NewPresses.Contains(Keys.LeftShift))
            {
                Speed = Player.BaseSpeed * Player.BoostMultiplier;
            }
            else if (Player.Momentum.LengthSquared() > Player.BaseSpeed * Player.BaseSpeed)
            {
                // Player is now slowing down from boost
                Speed = Player.Momentum.Length() / UnBoostDivider;
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
                if (HeadingUp == true && Player.Momentum.Y > 0)
                {
                    Player.Momentum += new Vector2(0, AccelerationVector.Y - DecelerationVector.Y);
                }
                else if (HeadingUp == false && Player.Momentum.Y < 0)
                {
                    Player.Momentum += new Vector2(0, AccelerationVector.Y - DecelerationVector.Y);
                }

                if (HeadingLeft == true && Player.Momentum.X > 0)
                {
                    Player.Momentum += new Vector2(AccelerationVector.X - DecelerationVector.X, 0);
                }
                else if (HeadingLeft == false && Player.Momentum.X < 0)
                {
                    Player.Momentum += new Vector2(AccelerationVector.X - DecelerationVector.X, 0);
                }
            }

            // Clamp momentum magnitude if it is greater than max speed
            float LengthSquared = Player.Momentum.LengthSquared();
            if (LengthSquared > Speed * Speed)
            {
                Player.Momentum = Vector2.Normalize(Player.Momentum);
                Player.Momentum *= Speed;
            }
        }

        private void PlayerMovement_EnactMomentum()
        {
            Player.Position += Player.Momentum;
        }

        private void CheckEnactPlayerDeath()
        {
            if (Player.IsDead)
            {
                Player.IsDead = false;
                Player.Health = Player.HealthMax;
                UI_ChangePage("Death");
            }
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
                    SpawnEnemy(new Entity.DemonEye(), OnFringe, Vector2.Zero);
                }
                else
                {
                    //Red Cube
                    SpawnEnemy(new Entity.RedCube(), OnFringe, Vector2.Zero);
                }
            }
        }
        private void SpawnEnemy(object EnemyClass, bool RandomLocation, Vector2 Location)
        {
            Vector2 SpawnPoint = Location;

            if (RandomLocation)
            //Spawns randomly from edges of screen
            {
                float SpawnAngle = random.Next(0, 360) * (float)(Math.PI / 180);
                int SpawnDistance = random.Next((int)(_graphics.PreferredBackBufferWidth * 0.6F), (int)(_graphics.PreferredBackBufferWidth * 1.2));
                SpawnPoint = new Vector2(Player.Position.X + (SpawnDistance * (float)Math.Cos(SpawnAngle)),
                                                    Player.Position.Y + (SpawnDistance * (float)Math.Sin(SpawnAngle)));
            }


            switch (EnemyClass)
            {
                case Entity.RedCube:
                    //Red Cube
                    Entities.Add(new Entity(new Entity.RedCube())
                    {
                        X = (int)SpawnPoint.X,
                        Y = (int)SpawnPoint.Y,
                    });
                    break;
                case Entity.DemonEye:
                    //Demon Eye
                    Entities.Add(new Entity(new Entity.DemonEye())
                    {
                        X = (int)SpawnPoint.X,
                        Y = (int)SpawnPoint.Y,
                    });
                    break;
                case Entity.CubeMother:
                    //Cube Mother
                    Entities.Add(new Entity(new Entity.CubeMother())
                    {
                        X = (int)SpawnPoint.X,
                        Y = (int)SpawnPoint.Y
                    });
                    break;
                case Entity.Spider:
                    //Spider
                    Entities.Add(new Entity(new Entity.Spider())
                    {
                        X = (int)SpawnPoint.X,
                        Y = (int)SpawnPoint.Y
                    });
                    break;
            }
        }

        private void EnactEntities()
        {
            if (Settings.RunEntityAI)
            {

                foreach (Entity Entity in Entities)
                {
                    //Enact Inbuilt Function
                    switch (Entity.EntityObj)
                    {
                        case Entity.RedCube:
                            Entity.RedCube.EnactAI(Entity, Player);
                            break;
                        case Entity.DemonEye:
                            Entity.DemonEye.EnactAI(Entity, Player);
                            break;
                        case Entity.CubeMother:
                            Entity.CubeMother.EnactAI(Entity, Player);
                            break;
                        case Entity.Spider:
                            Entity.Spider.EnactAI(Entity, Player);
                            break;
                    }
                }
                
                for (int i = 0; i < EntitiesToAdd.Count(); i++)
                {
                    Entities.Add(EntitiesToAdd.First());
                }
                EntitiesToAdd.Clear();
            }
        }

        #endregion

        /////////////////////////////////////////

        #region Magic Interaction

        private void CreateMagic(int X, int Y, Type MagicType)
        {
            if (MagicType == typeof(MagicEffect.StaticOrb))
            {
                if (Player.CheckUseMana(MagicEffect.StaticOrb.ManaCost))
                {
                    MagicEffects.Add(new MagicEffect()
                    {
                        X = X,
                        Y = Y,
                        Duration = MagicEffect.StaticOrb.DefaultDuration,
                        HasDuration = MagicEffect.StaticOrb.HasDuration
                    });
                }
            }
            else if (MagicType == typeof(MagicEffect.NonStaticOrb))
            {
                //Calculating Angle
                float xDiff = X - Player.Position.X;
                float yDiff = Y - Player.Position.Y;
                float Angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);

                MagicEffects.Add(new MagicEffect()
                {
                    X = (int)Player.Position.X,
                    Y = (int)Player.Position.Y,

                    Duration = MagicEffect.NonStaticOrb.DefaultDuration,
                    MagicObj = new MagicEffect.NonStaticOrb()
                    {
                        Angle = Angle,
                        Velocity = MagicEffect.NonStaticOrb.DefaultVelocity,
                    }
                });
            }
            else if (MagicType == typeof(MagicEffect.DissapationWave))
            {
                if (Player.CheckUseMana(MagicEffect.DissapationWave.ManaCost))
                {
                    MagicEffects.Add(new MagicEffect()
                    {
                        X = X,
                        Y = Y,

                        Duration = MagicEffect.DissapationWave.DefaultDuration,
                        HasDuration = MagicEffect.DissapationWave.HasDuration,
                        MagicObj = new MagicEffect.DissapationWave()
                    });
                }
            }
            else if (MagicType == typeof(MagicEffect.ForceWave))
            {
                if (Player.CheckUseMana(MagicEffect.ForceWave.ManaCost))
                {
                    MagicEffects.Add(new MagicEffect()
                    {
                        X = X,
                        Y = Y,

                        Duration = MagicEffect.ForceWave.DefaultDuration,
                        HasDuration = MagicEffect.ForceWave.HasDuration,
                        MagicObj = new MagicEffect.ForceWave()
                    });
                }
                
            }
            else if (MagicType == typeof(MagicEffect.ForceContainer))
            {
                if (Player.CheckUseMana(MagicEffect.ForceContainer.ManaCost))
                {
                    MagicEffects.Add(new MagicEffect()
                    {
                        X = (int)Player.Position.X,
                        Y = (int)Player.Position.Y,

                        Duration = MagicEffect.ForceContainer.DurationDefault,
                        MagicObj = new MagicEffect.ForceContainer()
                        {
                            CurrentRadius = MagicEffect.ForceContainer.RadiusMoving,
                            Destination = new Point(X, Y)
                        }
                    });
                }
            }
            else if (MagicType == typeof(MagicEffect.WideLazer))
            {
                //Calculating Angle
                float xDiff = X - Player.Position.X;
                float yDiff = Y - Player.Position.Y;
                float Angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);

                MagicEffects.Add(new MagicEffect()
                {
                    X = (int)Player.Position.X,
                    Y = (int)Player.Position.Y,

                    Duration = MagicEffect.WideLazer.DefaultDuration,
                    MagicObj = new MagicEffect.WideLazer()
                    {
                        Angle = Angle
                    }
                });
            }
            else if (MagicType == typeof(MagicEffect.PlayerTeleport))
            {
                if (Player.CheckUseMana(MagicEffect.PlayerTeleport.ManaCost))
                {
                    MagicEffects.Add(new MagicEffect()
                    {
                        X = (int)Player.Position.X,
                        Y = (int)Player.Position.Y,

                        Duration = MagicEffect.PlayerTeleport.DefaultDuration,
                        HasDuration = MagicEffect.PlayerTeleport.HasDuration,
                        MagicObj = new MagicEffect.PlayerTeleport()
                        {
                            Goal = new Vector2(X, Y),
                            GoalReacted = false
                        }
                    });
                    int a = 0;
                }
            }
        }
        private void EnactMagic()
        {
            foreach (MagicEffect Effect in MagicEffects)
            {
                //Enact Inbuilt Function
                switch (Effect.MagicObj)
                {
                    case MagicEffect.StaticOrb:
                        MagicEffect.StaticOrb.EnactEffect(Effect, Entities);
                        break;
                    case MagicEffect.NonStaticOrb:
                        MagicEffect.NonStaticOrb.EnactEffect(Effect, Entities);
                        break;
                    case MagicEffect.DissapationWave:
                        MagicEffect.DissapationWave.EnactEffect(Effect, Entities);
                        break;
                    case MagicEffect.ForceWave:
                        MagicEffect.ForceWave.EnactEffect(Effect, Entities);
                        break;
                    case MagicEffect.ForceContainer:
                        MagicEffect.ForceContainer.EnactEffect(Effect, Entities);
                        break;
                    case MagicEffect.WideLazer:
                        MagicEffect.WideLazer.EnactEffect(Effect, Player, Entities, GameTick);
                        break;
                    case MagicEffect.PlayerTeleport:
                        MagicEffect.PlayerTeleport.EnactEffect(Effect, Player);
                        break;
                    default:
                        break;
                }
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
            if (UIPage_Current != null)
            {
                foreach (UIItem Item in UIPage_Current.UIItems)
                {
                    int X = _graphics.PreferredBackBufferWidth / 2 + Item.X;
                    int Y = _graphics.PreferredBackBufferHeight / 2 + Item.Y;

                    if (Item.Type == "Button")
                    {
                        if (Mouse.GetState().X > X && Mouse.GetState().X < X + Item.Width &&
                                    Mouse.GetState().Y > Y && Mouse.GetState().Y < Y + Item.Height)
                        {
                            Item.SetHighlight(true);
                        }
                        else
                        {
                            Item.SetHighlight(false);
                        }
                    }
                }
            }
        }
        private void CheckMouseClick()
        {
            //Left Click
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (!Input.isClickingLeft)
                {
                    bool UIClicked = false;

                    if (UIPage_Current != null)
                    {
                        foreach (UIItem Item in UIPage_Current.UIItems)
                        {
                            int X = _graphics.PreferredBackBufferWidth / 2 + Item.X;
                            int Y = _graphics.PreferredBackBufferHeight / 2 + Item.Y;

                            if (Mouse.GetState().X > X && Mouse.GetState().X < X + Item.Width &&
                                    Mouse.GetState().Y > Y && Mouse.GetState().Y < Y + Item.Height)
                            {
                                UIClicked = true;

                                if (Item.Type == "Button")
                                {
                                    UserControl_ButtonPress(Item.Data);
                                }
                            }
                        }
                    }

                    if (!UIClicked)
                    {
                        CreateMagic((int)(Mouse.GetState().X - _graphics.PreferredBackBufferWidth / 2 + Player.Position.X),
                                    (int)(Mouse.GetState().Y - _graphics.PreferredBackBufferHeight / 2 + Player.Position.Y),
                                    typeof(MagicEffect.StaticOrb));
                    }
                }

                Input.isClickingLeft = true;
            }
            else
            {
                Input.isClickingLeft = false;
            }

            //Right Click
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                if (!Input.isClickingRight)
                {
                    if (true)
                    {
                        CreateMagic((int)(Mouse.GetState().X - _graphics.PreferredBackBufferWidth / 2 + Player.Position.X),
                                    (int)(Mouse.GetState().Y - _graphics.PreferredBackBufferHeight / 2 + Player.Position.Y),
                                    typeof(MagicEffect.WideLazer));
                        SelectedEffects.Add(MagicEffects.Last());
                    }
                    else
                    {
                        CreateMagic((int)(Mouse.GetState().X - _graphics.PreferredBackBufferWidth / 2 + Player.Position.X),
                                    (int)(Mouse.GetState().Y - _graphics.PreferredBackBufferHeight / 2 + Player.Position.Y),
                                    typeof(MagicEffect.NonStaticOrb));
                    }
                }
                else
                {
                    if (MagicEffects.Count > 0)
                    {
                        foreach(MagicEffect Effect in SelectedEffects)
                        {
                            if (Effect.MagicObj.ToString() == "Bombarder.MagicEffect+WideLazer")
                            {
                                float xDiff = Mouse.GetState().X - (_graphics.PreferredBackBufferWidth / 2);
                                float yDiff = Mouse.GetState().Y - (_graphics.PreferredBackBufferHeight / 2);
                                float Angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);
                                ((MagicEffect.WideLazer)Effect.MagicObj).Angle = Angle;
                                Effect.X = (int)Player.Position.X;
                                Effect.Y = (int)Player.Position.Y;
                            }
                        }
                    }
                }

                Input.isClickingRight = true;
            }
            else
            {
                if (SelectedEffects.Count > 0)
                {
                    List<MagicEffect> ToRemove = new List<MagicEffect>();
                    foreach(MagicEffect Effect in SelectedEffects)
                    {
                        if (Effect.MagicObj.ToString() == "Bombarder.MagicEffect+WideLazer")
                        {
                            ToRemove.Add(Effect);
                        }
                    }

                    foreach(MagicEffect Effect in ToRemove)
                    {
                        MagicEffects.Remove(Effect);
                    }
                }

                Input.isClickingRight = false;
            }

            //Middle Click
            if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
            {
                if (!Input.isClickingMiddle)
                {
                    CreateMagic((int)Player.Position.X, (int)Player.Position.Y,
                                typeof(MagicEffect.DissapationWave));
                }

                Input.isClickingMiddle = true;
            }
            else
            {
                Input.isClickingMiddle = false;
            }
        }

        #endregion

        #region Keyboard
        
        private bool IsNewlyPressed(List<Keys> NewPresses, Keys Key)
        {
            if (NewPresses.Contains(Key) && !Input.PreviouseKeys.Contains(Key))
            {
                return true;
            }
            return false;
        }

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
                    SpawnEnemy(new Entity.CubeMother(), false, new Vector2(0, 0));
                }
                if (IsNewlyPressed(Keys_NewlyPressed, Keys.D2))
                {
                    SpawnEnemy(new Entity.Spider(), false, new Vector2(0, 0));
                }


                //Magic Creation
                if (IsNewlyPressed(Keys_NewlyPressed, Keys.Q))
                {
                    CreateMagic((int)Player.Position.X, (int)Player.Position.Y, typeof(MagicEffect.ForceWave));
                }
                if (IsNewlyPressed(Keys_NewlyPressed, Keys.Tab))
                {
                    CreateMagic((int)(Mouse.GetState().X - _graphics.PreferredBackBufferWidth / 2 + Player.Position.X),
                                (int)(Mouse.GetState().Y - _graphics.PreferredBackBufferHeight / 2 + Player.Position.Y), typeof(MagicEffect.ForceContainer));
                }
                if (IsNewlyPressed(Keys_NewlyPressed, Keys.T))
                {
                    CreateMagic((int)(Mouse.GetState().X - _graphics.PreferredBackBufferWidth / 2 + Player.Position.X),
                                (int)(Mouse.GetState().Y - _graphics.PreferredBackBufferHeight / 2 + Player.Position.Y), typeof(MagicEffect.PlayerTeleport));
                }


                //Settings Changes
                if (IsNewlyPressed(Keys_NewlyPressed, Keys.I))
                {
                    Settings.RunEntityAI = !Settings.RunEntityAI;
                }
                if (IsNewlyPressed(Keys_NewlyPressed, Keys.O))
                {
                    Settings.ShowHitBoxes = !Settings.ShowHitBoxes;
                    Settings.ShowDamageRadii = !Settings.ShowDamageRadii;
                }
            }


            Input.PreviouseKeys = Keys_NewlyPressed;
        }

        #endregion

        #endregion

        #region General Rendering Functions

        private void DrawGrid()
        {
            if (Settings.ShowGrid)
            {
                Point ScreenStart = new Point((int)Player.Position.X - (_graphics.PreferredBackBufferWidth / 2),
                                                              (int)Player.Position.Y - (_graphics.PreferredBackBufferHeight / 2));
                for (int y = 0; y < _graphics.PreferredBackBufferHeight; y++)
                {
                    if ((y + ScreenStart.Y) % (300 * Settings.GridSizeMultiplier) == 0)
                    {
                        _spriteBatch.Draw(Textures.White, new Rectangle(0, y - 1, _graphics.PreferredBackBufferWidth, 2), Settings.GridColor * 0.7F * Settings.GridOpacityMultiplier);
                    }
                    if ((y + ScreenStart.Y) % (100 * Settings.GridSizeMultiplier) == 0)
                    {
                        _spriteBatch.Draw(Textures.White, new Rectangle(0, y, _graphics.PreferredBackBufferWidth, 1), Settings.GridColor * 0.45F * Settings.GridOpacityMultiplier);
                    }
                }
                for (int x = 0; x < _graphics.PreferredBackBufferWidth; x++)
                {
                    if ((x + ScreenStart.X) % (300 * Settings.GridSizeMultiplier) == 0)
                    {
                        _spriteBatch.Draw(Textures.White, new Rectangle(x - 1, 0, 2, _graphics.PreferredBackBufferWidth), Settings.GridColor * 0.7F * Settings.GridOpacityMultiplier);
                    }
                    if ((x + ScreenStart.X) % (100 * Settings.GridSizeMultiplier) == 0)
                    {
                        _spriteBatch.Draw(Textures.White, new Rectangle(x, 0, 1, _graphics.PreferredBackBufferWidth), Settings.GridColor * 0.45F * Settings.GridOpacityMultiplier);
                    }
                }
            }
        }

        void DrawLine(Vector2 point, float Length, float Angle, Color Color, float Thickness)
        {
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(Length, Thickness);

            _spriteBatch.Draw(Textures.White, point, null, Color, Angle, origin, scale, SpriteEffects.None, 0);
        }
        void DrawRotatedTexture(Vector2 Point, Texture2D Texture, float Width, float Height, float Angle, bool Centered, Color Color)
        {
            float AngleRadians = Angle * (float)(Math.PI / 180);

            var origin = new Vector2(0f, 0.5f);
            Vector2 scale;
            scale = new Vector2(Width, Height);

            if (!Centered)
            {
                Point.Y -= ((Texture.Width * scale.Y) / 2) * (float)Math.Cos(AngleRadians);
                Point.X += ((Texture.Height * scale.Y) / 2) * (float)Math.Sin(AngleRadians);
            }

            _spriteBatch.Draw(Texture, Point, null, Color, AngleRadians, origin, scale, SpriteEffects.None, 0);
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
                Particle.SpawnParticles(Particles, Player.Position, _graphics, GameTick);

                //Magic Functions
                EnactMagic();
                MagicEffect.EnactDuration(MagicEffects);
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Settings.BackgroundColor);

            // BEGIN Draw ----
            _spriteBatch.Begin(SpriteSortMode.Deferred,
                                BlendState.AlphaBlend,
                                SamplerState.PointClamp,
                                DepthStencilState.None,
                                RasterizerState.CullNone, null);




            //Particles
            List<Particle> LaterParticles = new List<Particle>();
            foreach (Particle particle in Particles)
            {
                if (particle.ParticleObj is Particle.HitMarker)
                {
                    LaterParticles.Add(particle);
                }
                else if (particle.ParticleObj is Particle.LazerLine)
                {
                    LaterParticles.Add(particle);
                }
                else if (particle.ParticleObj is Particle.TeleportLine)
                {
                    LaterParticles.Add(particle);
                }
                else if (particle.ParticleObj is Particle.Impact)
                {
                    LaterParticles.Add(particle);
                }
                else if (particle.ParticleObj is Particle.Dust)
                {
                    Particle.Dust Dust = (Particle.Dust)particle.ParticleObj;
                    _spriteBatch.Draw(Textures.White, new Rectangle((int)particle.X + (_graphics.PreferredBackBufferWidth / 2) - (int)Player.Position.X,
                                                                    (int)particle.Y + (_graphics.PreferredBackBufferHeight / 2) - (int)Player.Position.Y,
                                                                    Dust.Width, Dust.Height), Dust.Colour * Dust.Opacity);
                }
                else if (particle.ParticleObj is Particle.RedCubeSegment)
                {
                    _spriteBatch.Draw(Textures.White, new Rectangle((int)particle.X + (_graphics.PreferredBackBufferWidth / 2) - (int)Player.Position.X,
                                                                    (int)particle.Y + (_graphics.PreferredBackBufferHeight / 2) - (int)Player.Position.Y,
                                                                    Particle.RedCubeSegment.Width, Particle.RedCubeSegment.Height), Particle.RedCubeSegment.Colour);
                }
            }

            //Ingame
            if (GameState == "Play")
            {
                //Grid
                DrawGrid();

                //Objects
                foreach (Object Obj in Objects.GeneralObjects)
                {
                    string Type = Obj.Type;
                }
                


                //Player
                _spriteBatch.Draw(Textures.White, new Rectangle(_graphics.PreferredBackBufferWidth / 2 - Player.Width / 2, 
                                                             _graphics.PreferredBackBufferHeight / 2 - Player.Height / 2, 
                                                             Player.Width, Player.Height), Color.Red);


                //Entities
                foreach (Entity Entity in Entities)
                {
                    if (Entity.EntityObj is Entity.RedCube)
                    {
                        foreach (EntityBlock Block in Entity.Parts)
                        {
                            Color BlockColor = Block.Color;
                            Texture2D BlockTexture = Textures.White;
                            if (Block.Textures != null)
                            {
                                BlockColor = Color.White;
                                BlockTexture = Block.Textures.First();
                            }

                            _spriteBatch.Draw(BlockTexture, new Rectangle((int)(Entity.X + Block.Offset.X + (_graphics.PreferredBackBufferWidth / 2) - Player.Position.X),
                                                                         (int)(Entity.Y + Block.Offset.Y + (_graphics.PreferredBackBufferHeight / 2) - Player.Position.Y),
                                                                        Block.Width, Block.Height), BlockColor);
                        }
                    }
                    else if (Entity.EntityObj is Entity.DemonEye)
                    {
                        _spriteBatch.Draw(Textures.DemonEye.Item1, new Rectangle((int)(Entity.X - (Textures.DemonEye.Item1.Width * 0.8 / 2)) + (_graphics.PreferredBackBufferWidth / 2) - (int)Player.Position.X,
                                                                      (int)(Entity.Y - (Textures.DemonEye.Item1.Height * 0.8 / 2)) + (_graphics.PreferredBackBufferHeight / 2) - (int)Player.Position.Y,
                                                                      (int)(Textures.DemonEye.Item1.Width * 0.8), (int)(Textures.DemonEye.Item1.Height * 0.8)), Color.White);
                    }
                    else if (Entity.EntityObj is Entity.CubeMother)
                    {
                        foreach (EntityBlock Block in Entity.Parts)
                        {
                            Color BlockColor = Block.Color;
                            Texture2D BlockTexture = Textures.White;
                            if (Block.Textures != null)
                            {
                                BlockColor = Color.White;
                                BlockTexture = Block.Textures.First();
                            }

                            _spriteBatch.Draw(BlockTexture, new Rectangle((int)(Entity.X + Block.Offset.X + (_graphics.PreferredBackBufferWidth / 2) - Player.Position.X),
                                                                         (int)(Entity.Y + Block.Offset.Y + (_graphics.PreferredBackBufferHeight / 2) - Player.Position.Y),
                                                                        Block.Width, Block.Height), BlockColor);
                        }
                    }
                    else if (Entity.EntityObj is Entity.Spider)
                    {
                        _spriteBatch.Draw(Textures.White, new Rectangle((int)Entity.X + (int)Entity.Parts[0].Offset.X + (_graphics.PreferredBackBufferWidth / 2) - (int)Player.Position.X,
                                                                        (int)Entity.Y + (int)Entity.Parts[0].Offset.Y + (_graphics.PreferredBackBufferHeight / 2) - (int)Player.Position.Y,
                                                                        Entity.Parts[0].Width, Entity.Parts[0].Height), Color.MediumPurple);
                    }


                    if (Settings.ShowHitBoxes)
                    {
                        //Top Line
                        _spriteBatch.Draw(Textures.White, new Rectangle((int)(Entity.X + Entity.HitboxOffset.X + (_graphics.PreferredBackBufferWidth / 2) - Player.Position.X),
                                                                     (int)(Entity.Y + Entity.HitboxOffset.Y + (_graphics.PreferredBackBufferHeight / 2) - Player.Position.Y),
                                                                    Entity.HitboxSize.X, 2), Color.White);
                        //Bottom Line
                        _spriteBatch.Draw(Textures.White, new Rectangle((int)(Entity.X + Entity.HitboxOffset.X + (_graphics.PreferredBackBufferWidth / 2) - Player.Position.X),
                                                                     (int)(Entity.Y + Entity.HitboxOffset.Y + Entity.HitboxSize.Y + (_graphics.PreferredBackBufferHeight / 2) - Player.Position.Y),
                                                                    Entity.HitboxSize.X, 2), Color.White);
                        //Left Line
                        _spriteBatch.Draw(Textures.White, new Rectangle((int)(Entity.X + Entity.HitboxOffset.X + (_graphics.PreferredBackBufferWidth / 2) - Player.Position.X),
                                                                     (int)(Entity.Y + Entity.HitboxOffset.Y + (_graphics.PreferredBackBufferHeight / 2) - Player.Position.Y),
                                                                    2, Entity.HitboxSize.Y), Color.White);
                        //Right Line
                        _spriteBatch.Draw(Textures.White, new Rectangle((int)(Entity.X + Entity.HitboxOffset.X + Entity.HitboxSize.X + (_graphics.PreferredBackBufferWidth / 2) - Player.Position.X),
                                                                     (int)(Entity.Y + Entity.HitboxOffset.Y + (_graphics.PreferredBackBufferHeight / 2) - Player.Position.Y),
                                                                    2, Entity.HitboxSize.Y), Color.White);
                    }
                    if (Entity.HealthBarVisible)
                    {
                        _spriteBatch.Draw(Textures.White, new Rectangle((int)(Entity.X + Entity.HealthBarOffset.X + (_graphics.PreferredBackBufferWidth / 2) - Player.Position.X),
                                                                        (int)(Entity.Y + Entity.HealthBarOffset.Y + (_graphics.PreferredBackBufferHeight / 2) - Player.Position.Y),
                                                                        Entity.HealthBarDimentions.X, Entity.HealthBarDimentions.Y), Color.LightGray);
                        _spriteBatch.Draw(Textures.White, new Rectangle((int)(Entity.X + Entity.HealthBarOffset.X + (_graphics.PreferredBackBufferWidth / 2) - Player.Position.X + 2),
                                                                        (int)(Entity.Y + Entity.HealthBarOffset.Y + (_graphics.PreferredBackBufferHeight / 2) - Player.Position.Y + 2),
                                                                        (int)((Entity.HealthBarDimentions.X - 4) * ((float)Entity.Health / Entity.HealthMax)), Entity.HealthBarDimentions.Y - 4), Color.Green);
                    }
                }
                //Magic
                foreach (MagicEffect Effect in MagicEffects)
                {
                    if (Effect.MagicObj is MagicEffect.DissapationWave)
                    {
                        MagicEffect.DissapationWave Wave = (MagicEffect.DissapationWave)Effect.MagicObj;

                        _spriteBatch.Draw(Textures.WhiteCircle, new Rectangle((int)(Effect.X - Wave.Radius) + (_graphics.PreferredBackBufferWidth / 2) - (int)Player.Position.X,
                                                                      (int)(Effect.Y - Wave.Radius) + (_graphics.PreferredBackBufferHeight / 2) - (int)Player.Position.Y,
                                                                      (int)Wave.Radius * 2, (int)Wave.Radius * 2), Wave.Colour * Wave.Opacity);
                    }
                    else if (Effect.MagicObj is MagicEffect.ForceWave)
                    {
                        MagicEffect.ForceWave Wave = (MagicEffect.ForceWave)Effect.MagicObj;

                        _spriteBatch.Draw(Textures.WhiteCircle, new Rectangle((int)(Effect.X - Wave.Radius) + (_graphics.PreferredBackBufferWidth / 2) - (int)Player.Position.X,
                                                                      (int)(Effect.Y - Wave.Radius) + (_graphics.PreferredBackBufferHeight / 2) - (int)Player.Position.Y,
                                                                      (int)(Wave.Radius * 2), (int)(Wave.Radius * 2)), Wave.Colour * 0.3F);
                        for (int i = 0; i < MagicEffect.ForceWave.BorderWidth; i++)
                        {
                            _spriteBatch.Draw(Textures.HollowCircle, new Rectangle((int)(Effect.X - Wave.Radius + i) + (_graphics.PreferredBackBufferWidth / 2) - (int)Player.Position.X,
                                                                      (int)(Effect.Y - Wave.Radius + i) + (_graphics.PreferredBackBufferHeight / 2) - (int)Player.Position.Y,
                                                                      (int)(Wave.Radius * 2) - (i * 2), (int)(Wave.Radius * 2) - (i * 2)), Wave.Colour * 0.7F);
                        }
                    }
                    else if (Effect.MagicObj is MagicEffect.ForceContainer)
                    {
                        MagicEffect.ForceContainer Container = (MagicEffect.ForceContainer)Effect.MagicObj;

                        _spriteBatch.Draw(Textures.WhiteCircle, new Rectangle((int)(Effect.X - Container.CurrentRadius) + (_graphics.PreferredBackBufferWidth / 2) - (int)Player.Position.X,
                                                                      (int)(Effect.Y - Container.CurrentRadius) + (_graphics.PreferredBackBufferHeight / 2) - (int)Player.Position.Y,
                                                                      (int)Container.CurrentRadius * 2, (int)Container.CurrentRadius * 2), Container.Colour * Container.Opacity);
                    }
                    else if (Effect.MagicObj is MagicEffect.WideLazer)
                    {
                        //Old
                        if (Settings.ShowDamageRadii && false)
                        {
                            MagicEffect.WideLazer Lazer = (MagicEffect.WideLazer)Effect.MagicObj;

                            float AngleRadians = Lazer.Angle * (float)(Math.PI / 180);
                            float RightAngleRadians = (Lazer.Angle + 90) * (float)(Math.PI / 180);

                            Vector2 LeftLine = new Vector2((_graphics.PreferredBackBufferWidth / 2) - (MagicEffect.WideLazer.Width / 2 * (float)Math.Cos(RightAngleRadians)),
                                    (_graphics.PreferredBackBufferHeight / 2) - (MagicEffect.WideLazer.Width / 2 * (float)Math.Sin(RightAngleRadians)));
                            LeftLine.X += MagicEffect.WideLazer.InitialDistance * (float)Math.Cos(AngleRadians);
                            LeftLine.Y += MagicEffect.WideLazer.InitialDistance * (float)Math.Sin(AngleRadians);
                            Vector2 RightLine = new Vector2((_graphics.PreferredBackBufferWidth / 2) + ((MagicEffect.WideLazer.Width / 2 - 5) * (float)Math.Cos(RightAngleRadians)),
                                    (_graphics.PreferredBackBufferHeight / 2) + ((MagicEffect.WideLazer.Width / 2 - 5) * (float)Math.Sin(RightAngleRadians)));
                            RightLine.X += MagicEffect.WideLazer.InitialDistance * (float)Math.Cos(AngleRadians);
                            RightLine.Y += MagicEffect.WideLazer.InitialDistance * (float)Math.Sin(AngleRadians);


                            DrawRotatedTexture(LeftLine, Textures.White, MagicEffect.WideLazer.Width, MagicEffect.WideLazer.Range, Lazer.Angle + 90, false, Lazer.PrimaryColor * MagicEffect.WideLazer.Opacity);
                            DrawRotatedTexture(LeftLine, Textures.White, 5, MagicEffect.WideLazer.Range, Lazer.Angle + 90, false, Lazer.SecondaryColor);
                            DrawRotatedTexture(RightLine, Textures.White, 5, MagicEffect.WideLazer.Range, Lazer.Angle + 90, false, Lazer.SecondaryColor);

                            float Scale = (float)MagicEffect.WideLazer.Width / Textures.HalfWhiteCirlce.Width;
                            DrawRotatedTexture(LeftLine, Textures.HalfWhiteCirlce, Scale, Scale, Lazer.Angle + 90, true, Lazer.PrimaryColor * MagicEffect.WideLazer.Opacity);
                        }

                        //New accounting for Lazer Spread
                        if (Settings.ShowDamageRadii)
                        {
                            MagicEffect.WideLazer Lazer = (MagicEffect.WideLazer)Effect.MagicObj;

                            float TrueSpread = MagicEffect.WideLazer.Spread * MagicEffect.WideLazer.TrueSpreadMultiplier;

                            float AngleRadians = Lazer.Angle * (float)(Math.PI / 180);
                            float AngleRadiansLeft = (Lazer.Angle - TrueSpread) * (float)(Math.PI / 180);
                            float AngleRadiansRight = (Lazer.Angle + TrueSpread) * (float)(Math.PI / 180);

                            Vector2 Start = new Vector2(Effect.X + (_graphics.PreferredBackBufferWidth / 2) - (int)Player.Position.X,
                                                        Effect.Y + (_graphics.PreferredBackBufferHeight / 2) - (int)Player.Position.Y);

                            DrawLine(Start, MagicEffect.WideLazer.Range, AngleRadiansLeft, Lazer.SecondaryColor, 10);
                            DrawLine(Start, MagicEffect.WideLazer.Range, AngleRadians, Lazer.MarkerColor, 10);
                            DrawLine(Start, MagicEffect.WideLazer.Range, AngleRadiansRight, Lazer.SecondaryColor, 10);
                        }
                    }
                    
                    if (Settings.ShowDamageRadii)
                    {
                        if (Effect.RadiusIsCircle)
                        {
                            _spriteBatch.Draw(Textures.WhiteCircle, new Rectangle((int)(Effect.X - Effect.DamageRadius) + (_graphics.PreferredBackBufferWidth / 2) - (int)Player.Position.X,
                                                                      (int)(Effect.Y - Effect.DamageRadius) + (_graphics.PreferredBackBufferHeight / 2) - (int)Player.Position.Y,
                                                                      (int)Effect.DamageRadius * 2, (int)Effect.DamageRadius * 2), Color.DarkRed);
                        }
                        else
                        {
                            //Top Line
                            _spriteBatch.Draw(Textures.White, new Rectangle((int)(Effect.X + Effect.RadiusOffset.X + (_graphics.PreferredBackBufferWidth / 2) - Player.Position.X),
                                                                         (int)(Effect.Y + Effect.RadiusOffset.Y + (_graphics.PreferredBackBufferHeight / 2) - Player.Position.Y),
                                                                        Effect.RadiusSize.X * 2, 2), Color.White);
                            //Bottom Line
                            _spriteBatch.Draw(Textures.White, new Rectangle((int)(Effect.X + Effect.RadiusOffset.X + (_graphics.PreferredBackBufferWidth / 2) - Player.Position.X),
                                                                         (int)(Effect.Y + Effect.RadiusOffset.Y + Effect.RadiusSize.Y * 2 + (_graphics.PreferredBackBufferHeight / 2) - Player.Position.Y),
                                                                        Effect.RadiusSize.X * 2, 2), Color.White);
                            //Left Line
                            _spriteBatch.Draw(Textures.White, new Rectangle((int)(Effect.X + Effect.RadiusOffset.X + (_graphics.PreferredBackBufferWidth / 2) - Player.Position.X),
                                                                         (int)(Effect.Y + Effect.RadiusOffset.Y + (_graphics.PreferredBackBufferHeight / 2) - Player.Position.Y),
                                                                        2, Effect.RadiusSize.Y * 2), Color.White);
                            //Right Line
                            _spriteBatch.Draw(Textures.White, new Rectangle((int)(Effect.X + Effect.RadiusOffset.X + Effect.RadiusSize.X * 2 + (_graphics.PreferredBackBufferWidth / 2) - Player.Position.X),
                                                                         (int)(Effect.Y + Effect.RadiusOffset.Y + (_graphics.PreferredBackBufferHeight / 2) - Player.Position.Y),
                                                                        2, Effect.RadiusSize.Y * 2), Color.White);
                        }
                    }
                }
                


                //Health Bar
                if (!Player.HealthInfinite && Player.Health < Player.HealthMax)
                {
                    Point OrientPos = UIItem.GetOritentationPosition(_graphics, Player.ManaBarScreenOrientation);
                    float HealthPercent = (float)Player.Health / Player.HealthMax;

                    Point HealthBarContainerPos = new Point(OrientPos.X + Player.HealthBarOffset.X, OrientPos.Y + Player.HealthBarOffset.Y - Player.HealthBarDimentions.Y);
                    Point HealthBarPos = new Point(OrientPos.X + Player.HealthBarOffset.X,
                                                 OrientPos.Y + Player.HealthBarOffset.Y - (int)(Player.HealthBarDimentions.Y * HealthPercent));



                    _spriteBatch.Draw(Textures.White, new Rectangle(HealthBarContainerPos.X - 2, HealthBarContainerPos.Y - 2,
                                                                    Player.ManaBarDimentions.X + 4, Player.ManaBarDimentions.Y + 4), Color.White * 0.3F);
                    UIPage.RenderOutline(_spriteBatch, Textures.White, Color.White, HealthBarContainerPos.X - 2, HealthBarContainerPos.Y - 2, Player.ManaBarDimentions.X + 4, Player.ManaBarDimentions.Y + 4, 2, 1F);

                    _spriteBatch.Draw(Textures.White, new Rectangle(HealthBarPos.X, HealthBarPos.Y,
                                                                    Player.ManaBarDimentions.X, (int)(Player.ManaBarDimentions.Y * HealthPercent)), Color.Red);
                }
                //Mana Bar
                if (!Player.ManaInfinite && Player.Mana < Player.ManaMax)
                {
                    Point OrientPos = UIItem.GetOritentationPosition(_graphics, Player.ManaBarScreenOrientation);
                    float ManaPercent = (float)Player.Mana / Player.ManaMax;

                    Point ManaContainerPos = new Point(OrientPos.X + Player.ManaBarOffset.X, OrientPos.Y + Player.ManaBarOffset.Y - Player.ManaBarDimentions.Y);
                    Point ManaBarPos = new Point(OrientPos.X + Player.ManaBarOffset.X, 
                                                 OrientPos.Y + Player.ManaBarOffset.Y - (int)(Player.ManaBarDimentions.Y * ManaPercent));

                    

                    _spriteBatch.Draw(Textures.White, new Rectangle(ManaContainerPos.X - 2, ManaContainerPos.Y - 2, 
                                                                    Player.ManaBarDimentions.X + 4, Player.ManaBarDimentions.Y + 4), Color.White * 0.3F);
                    UIPage.RenderOutline(_spriteBatch, Textures.White, Color.White, ManaContainerPos.X - 2, ManaContainerPos.Y - 2, Player.ManaBarDimentions.X + 4, Player.ManaBarDimentions.Y + 4, 2, 1F);

                    _spriteBatch.Draw(Textures.White, new Rectangle(ManaBarPos.X, ManaBarPos.Y, 
                                                                    Player.ManaBarDimentions.X, (int)(Player.ManaBarDimentions.Y * ManaPercent)), Color.Blue);
                }
            }

            //Later Particles
            foreach (Particle particle in LaterParticles)
            {
                if (particle.ParticleObj is Particle.HitMarker)
                {
                    _spriteBatch.Draw(Textures.HitMarker, new Rectangle((int)particle.X + (_graphics.PreferredBackBufferWidth / 2) - (int)Player.Position.X,
                                                                        (int)particle.Y + (_graphics.PreferredBackBufferHeight / 2) - (int)Player.Position.Y,
                                                                        Particle.HitMarker.Width, Particle.HitMarker.Height), Color.White);
                }
                else if (particle.ParticleObj is Particle.LazerLine)
                {
                    Particle.LazerLine Line = (Particle.LazerLine)particle.ParticleObj;
                    Vector2 Position = new Vector2(particle.X + (_graphics.PreferredBackBufferWidth / 2) - (int)Player.Position.X, particle.Y + (_graphics.PreferredBackBufferHeight / 2) - (int)Player.Position.Y);
                    DrawLine(Position, Line.Length, Line.Direction, Line.Colour, Line.Thickness);
                }
                else if (particle.ParticleObj is Particle.TeleportLine)
                {
                    Particle.TeleportLine Line = (Particle.TeleportLine)particle.ParticleObj;
                    Vector2 Position = new Vector2(particle.X + (_graphics.PreferredBackBufferWidth / 2) - (int)Player.Position.X, particle.Y + (_graphics.PreferredBackBufferHeight / 2) - (int)Player.Position.Y);
                    DrawLine(Position, Line.Length, Line.Direction, Line.Colour * Line.Opacity, Line.Thickness);
                }
                else if (particle.ParticleObj is Particle.Impact)
                {
                    Particle.Impact Effect = (Particle.Impact)particle.ParticleObj;

                    _spriteBatch.Draw(Textures.WhiteCircle, new Rectangle((int)(particle.X - Effect.Radius) + (_graphics.PreferredBackBufferWidth / 2) - (int)Player.Position.X,
                                                                        (int)(particle.Y - Effect.Radius) + (_graphics.PreferredBackBufferHeight / 2) - (int)Player.Position.Y,
                                                                        (int)(Effect.Radius * 2), (int)(Effect.Radius * 2)), Particle.Impact.Colour * Effect.Opacity);
                }
            }



            //UI
            foreach (UIPage page in UIPages)
            {
                if (page.Type == GameState)
                {
                    UIPage.RenderElements(_spriteBatch, _graphics, Textures,page.UIItems);
                }
            }
            //Cursor
            _spriteBatch.Draw(Textures.Cursor, new Rectangle(Mouse.GetState().X - (int)((Textures.Cursor.Width / 2) * Settings.CursorSizeMultiplier),
                                                            Mouse.GetState().Y - (int)((Textures.Cursor.Height / 2) * Settings.CursorSizeMultiplier),
                                                            (int)(Textures.Cursor.Width * Settings.CursorSizeMultiplier), (int)(Textures.Cursor.Height * Settings.CursorSizeMultiplier)), Color.White);




            _spriteBatch.End();
            // END Draw ------

            base.Draw(gameTime);
        }

        #endregion
    }
}