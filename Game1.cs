using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monogame_Lesson_6___Keyboard_and_Mouse_Events
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        MouseState mouseState, prevMousestate;
        KeyboardState keyboardState;

        //Textures
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
        Rectangle barrierRect1, barrierRect2;

        Texture2D coinTexture;
        Rectangle coinRect;

        enum Screen
        {
            intro,
            pacman,
            result
        }
        Screen screen;
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



            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
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
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}