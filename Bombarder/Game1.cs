using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Authentication;
using static Bombarder.MagicEffect;

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

        Textures Textures;
        Settings Settings;
        InputStates Input;
        Player Player;

        List<Entity> Entities = new List<Entity>();
        public static Object.ObjectContainer Objects = new Object.ObjectContainer();
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

            //Procedurally Creating and Assigning a 1x1 white texture to Color_White
            Textures.White = new Texture2D(GraphicsDevice, 1, 1);
            Textures.White.SetData(new Color[1] { Color.White });

            Textures.WhiteCircle = Content.Load<Texture2D>("Circle");
            Textures.Cursor = Content.Load<Texture2D>("Cursor");
            Textures.HitMarker = Content.Load<Texture2D>("HitMarker");

            //Demon Eye Textures
            Textures.DemonEye = (Content.Load<Texture2D>("DemonEye"), Content.Load<Texture2D>("DemonIris"));
        }

        #endregion

        /////////////////////////////////////////

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
                UI_ChangePage("Play");
            }
            else if (Data.Contains("Resume"))
            {
                UI_ChangePage("Play");
            }
            else if (Data.Contains("Quit"))
            {
                System.Environment.Exit(0);
            }
        }

        #endregion

        #region Player Movement

        private void PlayerMovement_InputHandler(List<Keys> NewPresses)
        {
            const int UnBoostDivider = 3;
            const float SwitchDirectionMult = 1F;

            float Speed = Player.BaseSpeed;
            if (NewPresses.Contains(Keys.LeftShift))
            {
                Speed = Player.BaseSpeed * Player.BoostMultiplier;
            }

            //Upward
            if (NewPresses.Contains(Keys.W) && !Input.PreviouseKeys.Contains(Keys.S))
            {
                if (Player.Momentum_Y > -Speed)
                {
                    Player.Momentum_Y -= Player.Acceleration;
                    if (Player.Momentum_Y > 0)
                    {
                        Player.Momentum_Y -= Player.Acceleration * SwitchDirectionMult;
                    }


                    if (Player.Momentum_Y < -Speed)
                    {
                        Player.Momentum_Y = -Speed;
                    }
                }
                if (Player.Momentum_Y < -Speed)
                {
                    Player.Momentum_Y += Player.Acceleration / UnBoostDivider;
                }
            }
            //Downward
            if (NewPresses.Contains(Keys.S) && !Input.PreviouseKeys.Contains(Keys.W))
            {
                if (Player.Momentum_Y < Speed)
                {
                    Player.Momentum_Y += Player.Acceleration;
                    if (Player.Momentum_Y < 0)
                    {
                        Player.Momentum_Y += Player.Acceleration * SwitchDirectionMult;
                    }


                    if (Player.Momentum_Y > Speed)
                    {
                        Player.Momentum_Y = Speed;
                    }
                }
                if (Player.Momentum_Y > Speed)
                {
                    Player.Momentum_Y -= Player.Acceleration / UnBoostDivider;
                }
            }
            //Left
            if (NewPresses.Contains(Keys.A) && !Input.PreviouseKeys.Contains(Keys.D))
            {
                if (Player.Momentum_X > -Speed)
                {
                    Player.Momentum_X -= Player.Acceleration;
                    if (Player.Momentum_X > 0)
                    {
                        Player.Momentum_X -= Player.Acceleration * SwitchDirectionMult;
                    }


                    if (Player.Momentum_X < -Speed)
                    {
                        Player.Momentum_X = -Speed;
                    }
                }
                if (Player.Momentum_X < -Speed)
                {
                    Player.Momentum_X += Player.Acceleration / UnBoostDivider;
                }
            }
            //Right
            if (NewPresses.Contains(Keys.D) && !Input.PreviouseKeys.Contains(Keys.A))
            {
                if (Player.Momentum_X < Speed)
                {
                    Player.Momentum_X += Player.Acceleration;
                    if (Player.Momentum_X < 0)
                    {
                        Player.Momentum_X += Player.Acceleration * SwitchDirectionMult;
                    }

                    if (Player.Momentum_X > Speed)
                    {
                        Player.Momentum_X = Speed;
                    }
                }
                if (Player.Momentum_X > -Speed)
                {
                    Player.Momentum_X -= Player.Acceleration / UnBoostDivider;
                }
            }

            //Slowdown
            if (!NewPresses.Contains(Keys.W) && !NewPresses.Contains(Keys.S))
            {
                if (Player.Momentum_Y > 0)
                {
                    Player.Momentum_Y -= Player.Slowdown;

                    if (Player.Momentum_Y < 0)
                    {
                        Player.Momentum_Y = 0;
                    }
                }
                else if (Player.Momentum_Y < 0)
                {
                    Player.Momentum_Y += Player.Slowdown;

                    if (Player.Momentum_Y > 0)
                    {
                        Player.Momentum_Y = 0;
                    }
                }
            }
            if (!NewPresses.Contains(Keys.A) && !NewPresses.Contains(Keys.D))
            {
                if (Player.Momentum_X > 0)
                {
                    Player.Momentum_X -= Player.Slowdown;

                    if (Player.Momentum_X < 0)
                    {
                        Player.Momentum_X = 0;
                    }
                }
                else if (Player.Momentum_X < 0)
                {
                    Player.Momentum_X += Player.Slowdown;

                    if (Player.Momentum_X > 0)
                    {
                        Player.Momentum_X = 0;
                    }
                }
            }
        }

        private void PlayerMovement_EnactMomentum()
        {
            Player.X += Player.Momentum_X;
            Player.Y += Player.Momentum_Y;
        }

        #endregion

        #region Entity Interaction

        private void SpawnRandomEnemy(bool OnFringe, bool IsEnemy)
        {
            if (OnFringe)
            //Spawns randomly from edges of screen
            {
                float SpawnAngle = random.Next(0, 360) * (float)(Math.PI / 180);
                int SpawnDistance = random.Next((int)(_graphics.PreferredBackBufferWidth * 0.6F), (int)(_graphics.PreferredBackBufferWidth * 1.2));
                Vector2 SpawnPoint = new Vector2(Player.X + (SpawnDistance * (float)Math.Cos(SpawnAngle)),
                                                 Player.Y + (SpawnDistance * (float)Math.Sin(SpawnAngle)));

                if (IsEnemy)
                {
                    if (random.Next(0, 4) == 0)
                    {
                        //Demon Eye
                        Entities.Add(new Entity()
                        {
                            X = (int)SpawnPoint.X,
                            Y = (int)SpawnPoint.Y,
                            
                            Type = "DemonEye",

                            Peices = new List<EntityBlock>() { new EntityBlock() }
                        });
                        Entities.Last().Peices[0].Textures = new List<Texture2D>() { Textures.DemonEye.Item1, Textures.DemonEye.Item2 };
                        Entities.Last().Peices[0].Width = Entities.Last().Peices[0].Textures[0].Width / 3 * 2;
                        Entities.Last().Peices[0].Height = Entities.Last().Peices[0].Textures[0].Height / 3 * 2;
                        Entities.Last().Peices[0].Offset = new Vector2( -Entities.Last().Peices[0].Width / 2, -Entities.Last().Peices[0].Height / 2 );
                        Entities.Last().HitboxOffset = new Point( -(int)(Entities.Last().Peices[0].Width / 2 * 0.8), -(int)(Entities.Last().Peices[0].Height / 2 * 0.8) );
                        Entities.Last().HitboxSize = new Point( (int)(Entities.Last().Peices[0].Width * 0.8), (int)(Entities.Last().Peices[0].Height * 0.8) );
                        Entities.Last().HealthBarVisible = true;
                        Entities.Last().HealthBarDimentions = new Point(80, 16);
                        Entities.Last().HealthBarOffset = new Point(-40, -Entities.Last().HitboxOffset.Y + 5);
                    }
                    else
                    {
                        //Default
                        Entities.Add(new Entity()
                        {
                            X = (int)SpawnPoint.X,
                            Y = (int)SpawnPoint.Y,
                        });
                    }
                }
            }
        }

        private void EnactEnemyChase()
        {
            if (Settings.RunEntityAI)
            {
                foreach (Entity Entity in Entities)
                {
                    if (Entity.ChasesPlayer)
                    {
                        Entity.MoveTowards(new Vector2(Player.X, Player.Y));
                    }
                }
            }
        }

        private void PurgeDeadEntities()
        {
            List<Entity> DeadEntities = new List<Entity>();

            foreach (Entity Entity in Entities)
            {
                if (Entity.IsDead)
                {
                    DeadEntities.Add(Entity);
                }

                //Temporarily Here. Need an Enemy Interation handler
                Object.HitMarker.PurgeDead(Entity);
            }

            foreach (Entity Entity in DeadEntities)
            {
                Entities.Remove(Entity);
            }
        }


        #endregion

        /////////////////////////////////////////

        #region Magic Interaction

        private void CreateMagic(int X, int Y, object MagicType)
        {
            if (MagicType.ToString() == "Bombarder.MagicEffect+StaticOrb")
            {
                MagicEffects.Add(new MagicEffect()
                {
                    X = X,
                    Y = Y,
                    Duration = MagicEffect.StaticOrb.DefaultDuration
                });
            }
            else if (MagicType.ToString() == "Bombarder.MagicEffect+NonStaticOrb")
            {
                //Calculating Angle
                float xDiff = X - Player.X;
                float yDiff = Y - Player.Y;
                float Angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);

                MagicEffects.Add(new MagicEffect()
                {
                    X = (int)Player.X,
                    Y = (int)Player.Y,

                    Duration = MagicEffect.NonStaticOrb.DefaultDuration,
                    MagicObj = new NonStaticOrb()
                    {
                        Angle = Angle,
                        Velocity = NonStaticOrb.DefaultVelocity,
                    }
                });
            }
            else if (MagicType.ToString() == "Bombarder.MagicEffect+DissapationWave")
            {
                MagicEffects.Add(new MagicEffect()
                {
                    X = X,
                    Y = Y,

                    Duration = MagicEffect.DissapationWave.DefaultDuration,
                    MagicObj = new DissapationWave()
                });
            }
            else if (MagicType.ToString() == "Bombarder.MagicEffect+WideLazer")
            {
                //Calculating Angle
                float xDiff = X - Player.X;
                float yDiff = Y - Player.Y;
                float Angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);

                MagicEffects.Add(new MagicEffect()
                {
                    X = (int)Player.X,
                    Y = (int)Player.Y,

                    Duration = MagicEffect.WideLazer.DefaultDuration,
                    MagicObj = new WideLazer()
                    {
                        Angle = Angle
                    }
                });
            }
        }
        private void EnactMagic()
        {
            foreach (MagicEffect Effect in MagicEffects)
            {
                //Enact Inbuilt Function
                if (Effect.MagicObj.ToString() == "Bombarder.MagicEffect+StaticOrb")
                {
                    MagicEffect.StaticOrb.EnactEffect(Effect, Entities);
                }
                else if (Effect.MagicObj.ToString() == "Bombarder.MagicEffect+NonStaticOrb")
                {
                    MagicEffect.NonStaticOrb.EnactEffect(Effect, Entities);
                }
                else if (Effect.MagicObj.ToString() == "Bombarder.MagicEffect+DissapationWave")
                {
                    MagicEffect.DissapationWave.EnactEffect(Effect, Entities);
                }
                else if (Effect.MagicObj.ToString() == "Bombarder.MagicEffect+WideLazer")
                {
                    MagicEffect.WideLazer.EnactEffect(Effect, Player, Entities, GameTick);
                }
            }

            //Enact Lifespan
            List<MagicEffect> DeadEffects = new List<MagicEffect>();
            foreach (MagicEffect Effect in MagicEffects)
            {
                Effect.EnactLifespan();
                
                if (Effect.Duration == 0)
                {
                    DeadEffects.Add(Effect);
                }
            }
            foreach(MagicEffect Effect in DeadEffects)
            {
                MagicEffects.Remove(Effect);
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
                        CreateMagic((int)(Mouse.GetState().X - _graphics.PreferredBackBufferWidth / 2 + Player.X),
                                    (int)(Mouse.GetState().Y - _graphics.PreferredBackBufferHeight / 2 + Player.Y),
                                    new StaticOrb());
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
                        CreateMagic((int)(Mouse.GetState().X - _graphics.PreferredBackBufferWidth / 2 + Player.X),
                                    (int)(Mouse.GetState().Y - _graphics.PreferredBackBufferHeight / 2 + Player.Y),
                                    new WideLazer());
                        SelectedEffects.Add(MagicEffects.Last());
                    }
                    else
                    {
                        CreateMagic((int)(Mouse.GetState().X - _graphics.PreferredBackBufferWidth / 2 + Player.X),
                                    (int)(Mouse.GetState().Y - _graphics.PreferredBackBufferHeight / 2 + Player.Y),
                                    new NonStaticOrb());
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
                                ((WideLazer)Effect.MagicObj).Angle = Angle;
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
                    CreateMagic((int)Player.X, (int)Player.Y,
                                new DissapationWave());
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

                if (IsNewlyPressed(Keys_NewlyPressed, Keys.V))
                {
                    SpawnRandomEnemy(true, true);
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
                Point ScreenStart = new Point((int)Player.X - (_graphics.PreferredBackBufferWidth / 2),
                                                              (int)Player.Y - (_graphics.PreferredBackBufferHeight / 2));
                for (int y = 0; y < _graphics.PreferredBackBufferHeight; y++)
                {
                    if ((y + ScreenStart.Y) % 300 == 0)
                    {
                        _spriteBatch.Draw(Textures.White, new Rectangle(0, y - 1, _graphics.PreferredBackBufferWidth, 2), Color.White * 0.7F);
                    }
                    if ((y + ScreenStart.Y) % 50 == 0)
                    {
                        _spriteBatch.Draw(Textures.White, new Rectangle(0, y, _graphics.PreferredBackBufferWidth, 1), Color.White * 0.45F);
                    }
                }
                for (int x = 0; x < _graphics.PreferredBackBufferWidth; x++)
                {
                    if ((x + ScreenStart.X) % 300 == 0)
                    {
                        _spriteBatch.Draw(Textures.White, new Rectangle(x - 1, 0, 2, _graphics.PreferredBackBufferWidth), Color.White * 0.7F);
                    }
                    if ((x + ScreenStart.X) % 50 == 0)
                    {
                        _spriteBatch.Draw(Textures.White, new Rectangle(x, 0, 1, _graphics.PreferredBackBufferWidth), Color.White * 0.45F);
                    }
                }
            }
        }

        void DrawLine(Vector2 point, float Length, float Angle, Color Color, float Thickness = 1F)
        {
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(Length, Thickness);

            _spriteBatch.Draw(Textures.White, point, null, Color, Angle, origin, scale, SpriteEffects.None, 0);
        }
        void DrawRotatedTexture(Vector2 Point, Texture2D Texture, float Width, float Height, float Angle, Color Color)
        {
            float AngleRadians = Angle * (float)(Math.PI / 180);

            var origin = new Vector2(0f, 0.5f);
            Vector2 scale;
            scale = new Vector2(Width, Height);

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

                //Entity Functions
                if (Settings.RunEntityAI)
                {
                    EnactEnemyChase();
                }
                PurgeDeadEntities();

                //Magic Functions
                EnactMagic();
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // BEGIN Draw ----
            _spriteBatch.Begin(SpriteSortMode.Deferred,
                                BlendState.AlphaBlend,
                                SamplerState.PointClamp,
                                DepthStencilState.None,
                                RasterizerState.CullNone, null);



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
                //Player Health Bar
                _spriteBatch.Draw(Textures.White, new Rectangle(_graphics.PreferredBackBufferWidth / 2 + Player.HealthBarOffset.X,
                                                             _graphics.PreferredBackBufferHeight / 2 + Player.HealthBarOffset.Y,
                                                             Player.HealthBarDimentions.X, Player.HealthBarDimentions.Y), Color.LightGray);
                //Player Health Bar
                _spriteBatch.Draw(Textures.White, new Rectangle(_graphics.PreferredBackBufferWidth / 2 + Player.HealthBarOffset.X + 2,
                                                             _graphics.PreferredBackBufferHeight / 2 + Player.HealthBarOffset.Y + 2,
                                                             (int)((Player.HealthBarDimentions.X - 4) * ((float)Player.Health / Player.HealthMax)), Player.HealthBarDimentions.Y - 4), Color.Green);


                //Entities
                foreach (Entity Entity in Entities)
                {
                    foreach (EntityBlock Block in Entity.Peices)
                    {
                        Color BlockColor = Block.Color;
                        Texture2D BlockTexture = Textures.White;
                        if (Block.Textures != null)
                        {
                            BlockColor = Color.White;
                            BlockTexture = Block.Textures.First();
                        }

                        _spriteBatch.Draw(BlockTexture, new Rectangle((int)(Entity.X + Block.Offset.X + (_graphics.PreferredBackBufferWidth / 2) - Player.X),
                                                                     (int)(Entity.Y + Block.Offset.Y + (_graphics.PreferredBackBufferHeight / 2) - Player.Y),
                                                                    Block.Width, Block.Height), BlockColor);
                    }

                    if (Settings.ShowHitBoxes)
                    {
                        //Top Line
                        _spriteBatch.Draw(Textures.White, new Rectangle((int)(Entity.X + Entity.HitboxOffset.X + (_graphics.PreferredBackBufferWidth / 2) - Player.X),
                                                                     (int)(Entity.Y + Entity.HitboxOffset.Y + (_graphics.PreferredBackBufferHeight / 2) - Player.Y),
                                                                    Entity.HitboxSize.X, 2), Color.White);
                        //Bottom Line
                        _spriteBatch.Draw(Textures.White, new Rectangle((int)(Entity.X + Entity.HitboxOffset.X + (_graphics.PreferredBackBufferWidth / 2) - Player.X),
                                                                     (int)(Entity.Y + Entity.HitboxOffset.Y + Entity.HitboxSize.Y + (_graphics.PreferredBackBufferHeight / 2) - Player.Y),
                                                                    Entity.HitboxSize.X, 2), Color.White);
                        //Left Line
                        _spriteBatch.Draw(Textures.White, new Rectangle((int)(Entity.X + Entity.HitboxOffset.X + (_graphics.PreferredBackBufferWidth / 2) - Player.X),
                                                                     (int)(Entity.Y + Entity.HitboxOffset.Y + (_graphics.PreferredBackBufferHeight / 2) - Player.Y),
                                                                    2, Entity.HitboxSize.Y), Color.White);
                        //Right Line
                        _spriteBatch.Draw(Textures.White, new Rectangle((int)(Entity.X + Entity.HitboxOffset.X + Entity.HitboxSize.X + (_graphics.PreferredBackBufferWidth / 2) - Player.X),
                                                                     (int)(Entity.Y + Entity.HitboxOffset.Y + (_graphics.PreferredBackBufferHeight / 2) - Player.Y),
                                                                    2, Entity.HitboxSize.Y), Color.White);
                    }

                    //HitMarkers
                    foreach (Object Obj in Entity.HitMarkers)
                    {
                        _spriteBatch.Draw(Textures.HitMarker, new Rectangle(Obj.X + (_graphics.PreferredBackBufferWidth / 2) - (int)Player.X,
                                                                        Obj.Y + (_graphics.PreferredBackBufferHeight / 2) - (int)Player.Y,
                                                                        Obj.Width, Obj.Height), Color.White);
                    }

                    if (Entity.HealthBarVisible)
                    {
                        _spriteBatch.Draw(Textures.White, new Rectangle((int)(Entity.X + Entity.HealthBarOffset.X + (_graphics.PreferredBackBufferWidth / 2) - Player.X),
                                                                        (int)(Entity.Y + Entity.HealthBarOffset.Y + (_graphics.PreferredBackBufferHeight / 2) - Player.Y),
                                                                        Entity.HealthBarDimentions.X, Entity.HealthBarDimentions.Y), Color.LightGray);
                        _spriteBatch.Draw(Textures.White, new Rectangle((int)(Entity.X + Entity.HealthBarOffset.X + (_graphics.PreferredBackBufferWidth / 2) - Player.X + 2),
                                                                        (int)(Entity.Y + Entity.HealthBarOffset.Y + (_graphics.PreferredBackBufferHeight / 2) - Player.Y + 2),
                                                                        (int)((Entity.HealthBarDimentions.X - 4) * ((float)Entity.Health / Entity.HealthMax)), Entity.HealthBarDimentions.Y - 4), Color.Green);
                    }
                }
                //Magic
                foreach (MagicEffect Effect in MagicEffects)
                {
                    string MagicType = Effect.MagicObj.ToString();


                    if (MagicType == "Bombarder.MagicEffect+DissapationWave")
                    {
                        DissapationWave Wave = (DissapationWave)Effect.MagicObj;

                        _spriteBatch.Draw(Textures.WhiteCircle, new Rectangle((int)(Effect.X - Wave.Radius) + (_graphics.PreferredBackBufferWidth / 2) - (int)Player.X,
                                                                      (int)(Effect.Y - Wave.Radius) + (_graphics.PreferredBackBufferHeight / 2) - (int)Player.Y,
                                                                      (int)Wave.Radius * 2, (int)Wave.Radius * 2), Wave.Colour * Wave.Opacity);
                    }
                    else if (MagicType == "Bombarder.MagicEffect+WideLazer")
                    {
                        WideLazer Lazer = (WideLazer)Effect.MagicObj;

                        float AngleRadians = Lazer.Angle * (float)(Math.PI / 180);
                        float RightAngleRadians = (Lazer.Angle + 90) * (float)(Math.PI / 180);

                        float X = (float)Player.X;
                        float Y = (float)Player.Y;

                        Vector2 LeftLine = new Vector2((_graphics.PreferredBackBufferWidth / 2) - (WideLazer.Width / 2 * (float)Math.Cos(RightAngleRadians)),
                                (_graphics.PreferredBackBufferHeight / 2) - (WideLazer.Width / 2 * (float)Math.Sin(RightAngleRadians)));
                        LeftLine.X += WideLazer.InitialDistance * (float)Math.Cos(AngleRadians);
                        LeftLine.Y += WideLazer.InitialDistance * (float)Math.Sin(AngleRadians);
                        Vector2 RightLine = new Vector2((_graphics.PreferredBackBufferWidth / 2) + (WideLazer.Width / 2 * (float)Math.Cos(RightAngleRadians)),
                                (_graphics.PreferredBackBufferHeight / 2) + (WideLazer.Width / 2 * (float)Math.Sin(RightAngleRadians)));
                        RightLine.X += WideLazer.InitialDistance * (float)Math.Cos(AngleRadians);
                        RightLine.Y += WideLazer.InitialDistance * (float)Math.Sin(AngleRadians);

                        for (int i = 0; i < WideLazer.Range / WideLazer.MarkerDistance; i++)
                        {
                            X += (int)(WideLazer.MarkerDistance * (float)Math.Cos(AngleRadians));
                            Y += (int)(WideLazer.MarkerDistance * (float)Math.Sin(AngleRadians));

                            _spriteBatch.Draw(Textures.WhiteCircle, new Rectangle((int)(X - 2) + (_graphics.PreferredBackBufferWidth / 2) - (int)Player.X,
                                                                      (int)(Y - 2) + (_graphics.PreferredBackBufferHeight / 2) - (int)Player.Y,
                                                                      4, 4), Lazer.MarkerColor);
                        }
                        DrawLine(LeftLine, WideLazer.Range, AngleRadians, Lazer.MarkerColor);
                        DrawLine(RightLine, WideLazer.Range, AngleRadians, Lazer.MarkerColor);
                    }
                    else
                    {
                        foreach (MagicEffectPiece Piece in Effect.Pieces)
                        {
                            Texture2D EffectTexture = Textures.White;
                            if (Piece.BaseShape == "Circle")
                            {
                                EffectTexture = Textures.WhiteCircle;
                            }

                            _spriteBatch.Draw(EffectTexture, new Rectangle(Effect.X + Piece.Offset.X + (_graphics.PreferredBackBufferWidth / 2) - (int)Player.X,
                                                                         Effect.Y + Piece.Offset.Y + (_graphics.PreferredBackBufferHeight / 2) - (int)Player.Y,
                                                                         Piece.Width, Piece.Height), Piece.Color);
                        }
                    }
                    
                    if (Settings.ShowDamageRadii)
                    {
                        if (Effect.RadiusIsCircle)
                        {
                            _spriteBatch.Draw(Textures.WhiteCircle, new Rectangle((int)(Effect.X - Effect.DamageRadius) + (_graphics.PreferredBackBufferWidth / 2) - (int)Player.X,
                                                                      (int)(Effect.Y - Effect.DamageRadius) + (_graphics.PreferredBackBufferHeight / 2) - (int)Player.Y,
                                                                      (int)Effect.DamageRadius * 2, (int)Effect.DamageRadius * 2), Color.DarkRed);
                        }
                        else
                        {
                            //Top Line
                            _spriteBatch.Draw(Textures.White, new Rectangle((int)(Effect.X + Effect.RadiusOffset.X + (_graphics.PreferredBackBufferWidth / 2) - Player.X),
                                                                         (int)(Effect.Y + Effect.RadiusOffset.Y + (_graphics.PreferredBackBufferHeight / 2) - Player.Y),
                                                                        Effect.RadiusSize.X * 2, 2), Color.White);
                            //Bottom Line
                            _spriteBatch.Draw(Textures.White, new Rectangle((int)(Effect.X + Effect.RadiusOffset.X + (_graphics.PreferredBackBufferWidth / 2) - Player.X),
                                                                         (int)(Effect.Y + Effect.RadiusOffset.Y + Effect.RadiusSize.Y * 2 + (_graphics.PreferredBackBufferHeight / 2) - Player.Y),
                                                                        Effect.RadiusSize.X * 2, 2), Color.White);
                            //Left Line
                            _spriteBatch.Draw(Textures.White, new Rectangle((int)(Effect.X + Effect.RadiusOffset.X + (_graphics.PreferredBackBufferWidth / 2) - Player.X),
                                                                         (int)(Effect.Y + Effect.RadiusOffset.Y + (_graphics.PreferredBackBufferHeight / 2) - Player.Y),
                                                                        2, Effect.RadiusSize.Y * 2), Color.White);
                            //Right Line
                            _spriteBatch.Draw(Textures.White, new Rectangle((int)(Effect.X + Effect.RadiusOffset.X + Effect.RadiusSize.X * 2 + (_graphics.PreferredBackBufferWidth / 2) - Player.X),
                                                                         (int)(Effect.Y + Effect.RadiusOffset.Y + (_graphics.PreferredBackBufferHeight / 2) - Player.Y),
                                                                        2, Effect.RadiusSize.Y * 2), Color.White);
                        }
                    }
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