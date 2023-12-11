using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;

namespace Bombarder
{
    public class Game1 : Game
    {
        #region Variable Defenition

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D Color_White;

        List<UIPage> UIPages = new List<UIPage>();
        UIPage UIPage_Current;
        string GameState;
        string UIState;

        List<Keys> Keys_BeingPressed = new List<Keys>();
        bool Mouse_isClickingLeft;
        bool Mouse_isClickingRight;
        bool Mouse_isClickingMiddle;


        Player Player;

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
            UIPages = UIPage.GeneratePages();
            UIPage_Current = UIPages[0];
            GameState = "Start";
            UIState = "Default";

            Mouse_isClickingLeft = false;
            Mouse_isClickingRight = false;
            Mouse_isClickingMiddle = false;


            Player = new Player();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Procedurally Creating and Assigning a 1x1 white texture to Color_White
            Color_White = new Texture2D(GraphicsDevice, 1, 1);
            Color_White.SetData(new Color[1] { Color.White });
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

        private void UI_RenderElements(List<UIItem> UIItems)
        {
            foreach (UIItem Item in UIItems)
            {
                int OrientatePosX = _graphics.PreferredBackBufferWidth / 2;
                int OrientatePosY = _graphics.PreferredBackBufferHeight / 2;
                switch (Item.Orientation)
                {
                    case "Bottom Left":
                        OrientatePosX = 0;
                        OrientatePosY = _graphics.PreferredBackBufferHeight;
                        break;
                    case "Left":
                        OrientatePosX = 0;
                        break;
                    case "Top Left":
                        OrientatePosX = 0;
                        OrientatePosY = 0;
                        break;
                    case "Top":
                        OrientatePosY = 0;
                        break;
                    case "Top Right":
                        OrientatePosX = _graphics.PreferredBackBufferWidth;
                        OrientatePosY = 0;
                        break;
                    case "Right":
                        OrientatePosX = _graphics.PreferredBackBufferWidth;
                        break;
                    case "Bottom Right":
                        OrientatePosX = _graphics.PreferredBackBufferWidth;
                        OrientatePosY = _graphics.PreferredBackBufferHeight;
                        break;
                    case "Bottom":
                        OrientatePosY = _graphics.PreferredBackBufferHeight;
                        break;
                }

                int X = OrientatePosX + Item.X;
                int Y = OrientatePosY + Item.Y;
                int CentreX = OrientatePosX + Item.CentreX;
                int CentreY = OrientatePosY + Item.CentreY;

                if (Item.Type == "Text")
                {
                    if (Item.Text != null)
                    {
                        UI_RenderTextElements(Item.Text.Elements, CentreX, CentreY, Item.Text.ElementSize, Item.Text.Color);
                    }
                }
                if (Item.Type == "Button")
                {
                    _spriteBatch.Draw(Color_White, new Rectangle(X, Y, Item.Width, Item.Height), Item.BorderColor);
                    if (!Item.Highlighted)
                    {
                        _spriteBatch.Draw(Color_White, new Rectangle(X + Item.BorderWidth, Y + Item.BorderWidth,
                                                                   Item.Width - Item.BorderWidth * 2, Item.Height - Item.BorderWidth * 2), Item.BaseColor);
                    }
                    else
                    {
                        _spriteBatch.Draw(Color_White, new Rectangle(X + Item.BorderWidth, Y + Item.BorderWidth,
                                                                   Item.Width - Item.BorderWidth * 2, Item.Height - Item.BorderWidth * 2), Item.HighlightedColor);
                    }

                    if (Item.Text != null)
                    {
                        UI_RenderTextElements(Item.Text.Elements, CentreX, CentreY, Item.Text.ElementSize, Item.Text.Color);
                    }
                }
                if (Item.Type == "Fillbar")
                {
                    //Border
                    UI_RenderOutline(Item.BorderColor, X, Y, Item.Width, Item.Height, Item.BorderWidth, Item.BorderTransparency);
                    //Inner
                    _spriteBatch.Draw(Color_White, new Rectangle(X + Item.BorderWidth, Y + Item.BorderWidth,
                                                                   Item.Width - Item.BorderWidth * 2, Item.Height - Item.BorderWidth * 2),
                                                                   Item.SubBorderColor * Item.SubBorderTransparency);
                    //Bar
                    _spriteBatch.Draw(Color_White, new Rectangle(X + Item.BorderWidth, Y + Item.BorderWidth,
                                                                   (int)((Item.Value - Item.MinValue) / (float)Item.MaxValue * (Item.Width - Item.BorderWidth * 2)),
                                                                   Item.Height - Item.BorderWidth * 2), Item.BaseColor * Item.BaseTransparency);
                }
                if (Item.Type == "Container")
                {
                    //Border
                    UI_RenderOutline(Item.BorderColor, X, Y, Item.Width, Item.Height, Item.BorderWidth, Item.BorderTransparency);
                    //Inner
                    _spriteBatch.Draw(Color_White, new Rectangle(X + Item.BorderWidth, Y + Item.BorderWidth,
                                                                   Item.Width - Item.BorderWidth * 2, Item.Height - Item.BorderWidth * 2),
                                                                   Item.SubBorderColor * Item.SubBorderTransparency);
                    if (Item.uIItems.Count > 0)
                    {
                        foreach (UIItem InnerItem in Item.uIItems)
                        {
                            switch (InnerItem.Orientation)
                            {
                                case "Bottom Left":
                                    OrientatePosX = 0;
                                    OrientatePosY = _graphics.PreferredBackBufferHeight;
                                    break;
                                case "Left":
                                    OrientatePosX = 0;
                                    break;
                                case "Top Left":
                                    OrientatePosX = 0;
                                    OrientatePosY = 0;
                                    break;
                                case "Top":
                                    OrientatePosY = 0;
                                    break;
                                case "Top Right":
                                    OrientatePosX = _graphics.PreferredBackBufferWidth;
                                    OrientatePosY = 0;
                                    break;
                                case "Right":
                                    OrientatePosX = _graphics.PreferredBackBufferWidth;
                                    break;
                                case "Bottom Right":
                                    OrientatePosX = _graphics.PreferredBackBufferWidth;
                                    OrientatePosY = _graphics.PreferredBackBufferHeight;
                                    break;
                                case "Bottom":
                                    OrientatePosY = _graphics.PreferredBackBufferHeight;
                                    break;
                            }
                            X = OrientatePosX + InnerItem.X;
                            Y = OrientatePosY + InnerItem.Y;
                            CentreX = OrientatePosX + InnerItem.CentreX;
                            CentreY = OrientatePosY + InnerItem.CentreY;

                            if (InnerItem.Type == "Container Slot")
                            {
                                float BorderTransparency = InnerItem.BorderTransparency;
                                float SubBorderTransparency = InnerItem.SubBorderTransparency;
                                Color BorderColor = InnerItem.BorderColor;
                                Color SubBorderColor = InnerItem.SubBorderColor;
                                if (InnerItem.Highlighted)
                                {
                                    BorderTransparency = InnerItem.BorderHighlightedTransparency;
                                    SubBorderTransparency = InnerItem.SubBorderHighlightedTransparency;
                                    BorderColor = InnerItem.HighlightedBorderColor;
                                    SubBorderColor = InnerItem.HighlightedColor;
                                }


                                //Border
                                UI_RenderOutline(BorderColor, X, Y, InnerItem.Width, InnerItem.Height, InnerItem.BorderWidth, BorderTransparency);
                                //Inner
                                _spriteBatch.Draw(Color_White, new Rectangle(X + InnerItem.BorderWidth, Y + InnerItem.BorderWidth,
                                                                               InnerItem.Width - InnerItem.BorderWidth * 2, InnerItem.Height - InnerItem.BorderWidth * 2),
                                                                               SubBorderColor * SubBorderTransparency);

                                //Hotbar Item
                                if (Item.Data.Contains("Hotbar"))
                                {
                                    if (InnerItem.NumericalData[0] > 0)
                                    {
                                        _spriteBatch.Draw(Color_White, new Rectangle(X + InnerItem.BorderWidth * 2, Y + InnerItem.BorderWidth * 2,
                                                                               InnerItem.Width - InnerItem.BorderWidth * 4, InnerItem.Height - InnerItem.BorderWidth * 4),
                                                                               Color.Purple);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void UI_RenderTextElements(List<List<bool>> Elements, int CentreX, int CentreY, int elementSize, Color elementColor)
        {
            int StartX = CentreX - ((Elements[0].Count * elementSize) / 2);
            int StartY = CentreY - ((Elements.Count * elementSize) / 2);

            for (int y = 0; y < Elements.Count; y++)
            {
                for (int x = 0; x < Elements[0].Count; x++)
                {
                    if (Elements[y][x])
                    {
                        _spriteBatch.Draw(Color_White, new Rectangle(StartX + (x * elementSize), StartY + (y * elementSize), elementSize, elementSize), elementColor);
                    }
                }
            }
        }
        private void UI_RenderOutline(Color color, int X, int Y, int Width, int Height, int BorderWidth, float BorderTransparency)
        {
            _spriteBatch.Draw(Color_White, new Rectangle(X, Y, Width, BorderWidth), color * BorderTransparency);
            _spriteBatch.Draw(Color_White, new Rectangle(X + Width - BorderWidth, Y + BorderWidth, BorderWidth, Height - BorderWidth), color * BorderTransparency);
            _spriteBatch.Draw(Color_White, new Rectangle(X, Y + Height - BorderWidth, Width - BorderWidth, BorderWidth), color * BorderTransparency);
            _spriteBatch.Draw(Color_White, new Rectangle(X, Y + BorderWidth, BorderWidth, Height - (BorderWidth * 2)), color * BorderTransparency);
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
            float Speed = Player.BaseSpeed;
            if (NewPresses.Contains(Keys.LeftShift))
            {
                Speed = Player.BaseSpeed * Player.BoostMultiplier;
            }

            //Upward
            if (NewPresses.Contains(Keys.W) && !Keys_BeingPressed.Contains(Keys.S))
            {
                if (Player.Momentum_Y > -Speed)
                {
                    Player.Momentum_Y -= Player.Acceleration;

                    if (Player.Momentum_Y < -Speed)
                    {
                        Player.Momentum_Y = -Speed;
                    }
                }
            }
            //Downward
            if (NewPresses.Contains(Keys.S) && !Keys_BeingPressed.Contains(Keys.W))
            {
                if (Player.Momentum_Y < Speed)
                {
                    Player.Momentum_Y += Player.Acceleration;

                    if (Player.Momentum_Y > Speed)
                    {
                        Player.Momentum_Y = Speed;
                    }
                }
            }
            //Left
            if (NewPresses.Contains(Keys.A) && !Keys_BeingPressed.Contains(Keys.D))
            {
                if (Player.Momentum_X < Speed)
                {
                    Player.Momentum_X += Player.Acceleration;

                    if (Player.Momentum_X > Speed)
                    {
                        Player.Momentum_X = Speed;
                    }
                }
            }
            //Right
            if (NewPresses.Contains(Keys.D) && !Keys_BeingPressed.Contains(Keys.A))
            {
                if (Player.Momentum_X > -Speed)
                {
                    Player.Momentum_X -= Player.Acceleration;

                    if (Player.Momentum_X < -Speed)
                    {
                        Player.Momentum_X = -Speed;
                    }
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
                if (!Mouse_isClickingLeft)
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
                                    UserControl_ButtonPress(Item.Data);
                                }
                            }
                        }
                    }
                }

                Mouse_isClickingLeft = true;
            }
            else
            {
                Mouse_isClickingLeft = false;
            }

            //Right Click
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {


                Mouse_isClickingRight = true;
            }
            else
            {
                Mouse_isClickingRight = false;
            }

            //Middle Click
            if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
            {


                Mouse_isClickingMiddle = true;
            }
            else
            {
                Mouse_isClickingMiddle = false;
            }
        }

        #endregion

        #region Keyboard

        private void KeyboardHandler()
        {
            List<Keys> Keys_NewlyPressed = Keyboard.GetState().GetPressedKeys().ToList();


            //Toggle Fullscreen
            if (Keys_NewlyPressed.Contains(Keys.F) && !Keys_BeingPressed.Contains(Keys.F))
            {
                Window_ToggleFullscreen();
            }
            //Toggle Pause
            if (Keys_NewlyPressed.Contains(Keys.Escape) && !Keys_BeingPressed.Contains(Keys.Escape))
            {
                TogglePause();
            }

            //Movement
            PlayerMovement_InputHandler(Keys_NewlyPressed);



            Keys_BeingPressed = Keys_NewlyPressed;
        }

        #endregion

        #endregion

        #region General Rendering Functions

        private void DrawGrid()
        {
            Point ScreenStart = new Point((int)Player.X - (_graphics.PreferredBackBufferWidth / 2),
                                              (int)Player.Y - (_graphics.PreferredBackBufferHeight / 2));
            for (int y = 0; y < _graphics.PreferredBackBufferHeight; y++)
            {
                if ((y + ScreenStart.Y) % 300 == 0)
                {
                    _spriteBatch.Draw(Color_White, new Rectangle(0, y - 1, _graphics.PreferredBackBufferWidth, 2), Color.White * 0.7F);
                }
                if ((y + ScreenStart.Y) % 50 == 0)
                {
                    _spriteBatch.Draw(Color_White, new Rectangle(0, y, _graphics.PreferredBackBufferWidth, 1), Color.White * 0.45F);
                }
            }
            for (int x = 0; x < _graphics.PreferredBackBufferWidth; x++)
            {
                if ((x + ScreenStart.X) % 300 == 0)
                {
                    _spriteBatch.Draw(Color_White, new Rectangle(x - 1, 0, 2, _graphics.PreferredBackBufferWidth), Color.White * 0.7F);
                }
                if ((x + ScreenStart.X) % 50 == 0)
                {
                    _spriteBatch.Draw(Color_White, new Rectangle(x, 0, 1, _graphics.PreferredBackBufferWidth), Color.White * 0.45F);
                }
            }
        }

        #endregion

        #region Fundamentals

        protected override void Update(GameTime gameTime)
        {
            KeyboardHandler();
            MouseHandler();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // BEGIN Draw ----
            _spriteBatch.Begin();



            //Ingame
            if (GameState == "Play")
            {
                //Grid
                DrawGrid();

                //Player
                _spriteBatch.Draw(Color_White, new Rectangle(_graphics.PreferredBackBufferWidth / 2 - Player.Width / 2, 
                                                             _graphics.PreferredBackBufferHeight / 2 - Player.Height / 2, 
                                                             Player.Width, Player.Height), Color.Red);
            }

            //UI
            foreach (UIPage page in UIPages)
            {
                if (page.Type == GameState)
                {
                    UI_RenderElements(page.UIItems);
                }
            }

            

            _spriteBatch.End();
            // END Draw ------

            base.Draw(gameTime);
        }

        #endregion
    }
}