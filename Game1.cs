using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Threading;

namespace Monogame_Lesson_6___Keyboard_and_Mouse_Events
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        MouseState mouseState, prevMousestate;
        KeyboardState keyboardState, prevKeyboardState;

        //Textures
        Texture2D pacmanIntroTexture;

        Texture2D pacLeftTexture;
        Texture2D pacRightTexture;
        Texture2D pacUpTexture;
        Texture2D pacDownTexture;
        Texture2D pacSleepTexture;

        Texture2D currentPacTexture; // Current Pacman texture to draw
        Rectangle pacRect; // This rectangle will track where Pacman is and his size\

        Texture2D exitTexture;
        Rectangle exitRect;

        Texture2D barrierTexture;
        Texture2D coinTexture;

        int pacSpeed, coinUpandDown;
        float idleSeconds,gametimeSeconds;

        enum Screen
        {
            intro,
            pacman,
            result
        }
        Screen screen;

        List<Rectangle> coins = new List<Rectangle>();
        List<Rectangle> barriers = new List<Rectangle> ();

        Random gen = new Random ();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            screen = Screen.intro;
            _graphics.PreferredBackBufferWidth = 640;
            _graphics.PreferredBackBufferHeight = 640;
            _graphics.ApplyChanges();
            
            pacSpeed = 3;
            pacRect = new Rectangle(10, 10, 60, 60);
            exitRect = new Rectangle(550, 500, 100, 100);
            coinUpandDown = 0;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            pacmanIntroTexture = Content.Load<Texture2D>("pacmanIntro");
            // Pacman
            pacDownTexture = Content.Load<Texture2D>("pacdown");
            pacUpTexture = Content.Load<Texture2D>("pacup");
            pacRightTexture = Content.Load<Texture2D>("pacright");
            pacLeftTexture = Content.Load<Texture2D>("pacleft");
            pacSleepTexture = Content.Load<Texture2D>("pacsleep");
            currentPacTexture = pacRightTexture;
            // Barrier
            barrierTexture = Content.Load<Texture2D>("rock_barrier");
            // Exit
            exitTexture = Content.Load<Texture2D>("hobbit_door");
            // Coin
            coinTexture = Content.Load<Texture2D>("coin");

            coins.Add(new Rectangle(400, 50, coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(475, 50, coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(200, 300, coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(400, 300, coinTexture.Width, coinTexture.Height));
         
            barriers.Add(new Rectangle(0, 200, 350, 75));
            barriers.Add(new Rectangle(450, 200, 350, 75));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            prevMousestate = mouseState;
            prevKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            idleSeconds = (float)gameTime.TotalGameTime.TotalSeconds-gametimeSeconds;

            if (screen == Screen.intro)
            {
                if (mouseState.LeftButton == ButtonState.Released && prevMousestate.LeftButton == ButtonState.Pressed)
                {
                    screen = Screen.pacman;
                    currentPacTexture = pacSleepTexture;
                    gametimeSeconds = (float)gameTime.TotalGameTime.TotalSeconds;
                }
            }
            else if (screen == Screen.pacman)
            {
                if (keyboardState.IsKeyDown(Keys.W))
                {
                    pacRect.Y -= pacSpeed;
                    currentPacTexture = pacUpTexture;
                    gametimeSeconds = (float)gameTime.TotalGameTime.TotalSeconds;
                    foreach (Rectangle barrier in barriers)
                        if (pacRect.Intersects(barrier))
                        {
                            pacRect.Y = barrier.Bottom;
                        }
                    if (pacRect.Y <= 0)
                    {
                        pacRect.Y = 0;
                    }

                }
                if (keyboardState.IsKeyDown(Keys.S))
                {
                    pacRect.Y += pacSpeed;
                    currentPacTexture = pacDownTexture;
                    gametimeSeconds = (float)gameTime.TotalGameTime.TotalSeconds;
                    foreach (Rectangle barrier in barriers)
                        if (pacRect.Intersects(barrier))
                        {
                            pacRect.Y = barrier.Top - pacRect.Height;
                        }
                    if (pacRect.Bottom >= _graphics.PreferredBackBufferHeight)
                    {
                        pacRect.Y = _graphics.PreferredBackBufferHeight - pacRect.Height;
                    }
                }
                if (keyboardState.IsKeyDown(Keys.A))
                {
                    pacRect.X -= pacSpeed;
                    currentPacTexture = pacLeftTexture;
                    gametimeSeconds = (float)gameTime.TotalGameTime.TotalSeconds;
                    foreach (Rectangle barrier in barriers)
                        if (pacRect.Intersects(barrier))
                        {
                            pacRect.X = barrier.Right;
                        }
                    if (pacRect.X <= 0)
                    {
                        pacRect.X = 0;
                    }
                }
                if (keyboardState.IsKeyDown(Keys.D))
                {
                    pacRect.X += pacSpeed;
                    currentPacTexture = pacRightTexture;
                    gametimeSeconds = (float)gameTime.TotalGameTime.TotalSeconds;
                    foreach (Rectangle barrier in barriers)
                        if (pacRect.Intersects(barrier))
                        {
                            pacRect.X = barrier.Left - pacRect.Width;
                        }
                    if (pacRect.Right >= _graphics.PreferredBackBufferWidth)
                    {
                        pacRect.X = _graphics.PreferredBackBufferWidth - pacRect.Width;
                    }
                }
                if (keyboardState.IsKeyDown(Keys.F) && prevKeyboardState.IsKeyUp(Keys.F))
                {
                    if (pacSpeed == 2)
                    {
                        pacSpeed = 4;
                    }
                    else if (pacSpeed == 4)
                    {
                        pacSpeed = 2;
                    }
                }
                if (idleSeconds >= 10)
                {
                    currentPacTexture = pacSleepTexture;
                }
                for (int i = 0; i < coins.Count; i++)
                {
                    if (pacRect.Intersects(coins[i]))
                    {
                        coins.RemoveAt(i);
                        i--;
                    }
                }
                if (keyboardState.IsKeyDown(Keys.E))
                    if (exitRect.Contains(pacRect))
                    {
                        gametimeSeconds = (float)gameTime.TotalGameTime.TotalSeconds;
                        pacRect = new Rectangle(10, 10, 60, 60);
                        exitRect = new Rectangle(550, 500, 100, 100);
                        coins.Clear();
                        Rectangle newCoin;
                        for (int i = 0; i < 10; i++)
                        {
                            bool done = true;
                            do
                            {
                                done = true;
                                newCoin = new Rectangle(gen.Next(0, 640 - coinTexture.Width), gen.Next(0, 640 - coinTexture.Height), coinTexture.Width, coinTexture.Height);
                                if (pacRect.Intersects(newCoin) || exitRect.Intersects(newCoin))
                                    done = false;

                                foreach (Rectangle coin in coins)
                                    if (coin.Intersects(newCoin))
                                        done = false;


                                foreach (Rectangle barrier in barriers)
                                    if (barrier.Intersects(newCoin))
                                        done = false;



                            } while (!done);
                            coins.Add(newCoin);
                        }


                    }
             


                
                for (int i = 0; i < coins.Count; i++)
                {
                    if (coinUpandDown % 2 == 0)
                    {
                        if (coinUpandDown < 20)
                        {
                            Rectangle coinTemp = new Rectangle();
                            coinTemp.Width = coins[i].Width;
                            coinTemp.Height = coins[i].Height;
                            coinTemp.X = coins[i].X;
                            coinTemp.Y = coins[i].Y + 1;
                            coins[i] = coinTemp;
                            coinUpandDown += 1;
                        }
                        else if (coinUpandDown < 40)
                        {
                            Rectangle coinTemp = new Rectangle();
                            coinTemp.Width = coins[i].Width;
                            coinTemp.Height = coins[i].Height;
                            coinTemp.X = coins[i].X;
                            coinTemp.Y = coins[i].Y - 1;
                            coins[i] = coinTemp;
                            coinUpandDown += 1;
                        }
                        else if (coinUpandDown >= 40)
                        {
                            coinUpandDown = 0;
                        }
                    }
                    else
                        coinUpandDown += 1;

                    
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            if (screen == Screen.intro)
            {
                _spriteBatch.Draw(pacmanIntroTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight),Color.White);
            }
            else if (screen == Screen.pacman)
            {
                foreach (Rectangle barrier in barriers)
                    _spriteBatch.Draw(barrierTexture, barrier, Color.White);
                _spriteBatch.Draw(exitTexture, exitRect, Color.White);
                _spriteBatch.Draw(currentPacTexture, pacRect, Color.White);
                foreach (Rectangle coin in coins)
                    _spriteBatch.Draw(coinTexture, coin, Color.White);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}