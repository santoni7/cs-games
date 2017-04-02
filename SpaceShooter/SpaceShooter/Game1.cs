using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.GameSystem;
using SpaceShooter.Screens;

namespace SpaceShooter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class SpaceShooterGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public ScreenManager screenManager;

        public int Width = 1270;
        public int Height = 720;
        /// <summary>
        /// From 1 to 3
        /// </summary>
        public int PlayerTextureNumber = 1;
        /// <summary>
        /// Can be blue, red, orange, green
        /// </summary>
        public string PlayerTextureColor = "blue";
        public int lastScore = 0;

        public bool toOpenPause = true;
        public string PlayerTextureName => "playerShip" + PlayerTextureNumber + "_" + PlayerTextureColor + ".png"; //"playerShip2_blue.png";
        
        public SpaceShooterGame()
        {
            Window.Title = "Space Shooter (SAKHNIUK ANTON)";
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = Width,
                PreferredBackBufferHeight = Height, 
                PreferMultiSampling = true
            };

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            screenManager = new ScreenManager(this);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            SpriteSheetCollection.Instance.LoadContent(Content);
            MainScreen screen = new MainScreen("main", screenManager);
            screen.LoadContent(Content);
            //screenManager.SetActiveScreen(screen);

            MainMenuScreen menuScreen = new MainMenuScreen("menu", screenManager);
            menuScreen.LoadContent(Content);

            PauseScreen pauseScreen = new PauseScreen("pause", screenManager);
            pauseScreen.LoadContent(Content);


            ResultsScreen resultScreen = new ResultsScreen("results", screenManager);
            resultScreen.LoadContent(Content);

            screenManager.SetActiveScreen(menuScreen);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                InputSingletone.Instance.Update(gameTime);
                screenManager.Update(gameTime);
                toOpenPause = true;
            }
            else
            {
                if (toOpenPause)
                {
                    toOpenPause = false;
                    screenManager.SetActiveBackground();
                    screenManager.SetActiveScreen("pause");
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            screenManager.Draw(spriteBatch, gameTime);
            base.Draw(gameTime);
        }
    }
}
