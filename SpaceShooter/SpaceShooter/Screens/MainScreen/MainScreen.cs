using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SmartPackmans.GameSystem;
using SpaceShooter.GameSystem;

namespace SpaceShooter.Screens
{
    public class MainScreen:Screen
    {
        private const float BackgroundMoveSpeed = 0.05f;
        private const int StartingDifficulty = 7;
        private const int MaxDifficulty = 25;
        private Random random;
        private Player player;
        private Texture2D _backgroundTexture;
        private float _bgDeltaX = 0;

        private SpriteFont font;
        private DateTime lastInit;
        private DateTime lastDifficultyUpdate;
        /// <summary>
        /// From 1 to 10
        /// </summary>
        public float Difficulty = StartingDifficulty;
        /// <summary>
        /// Time, elapsed after last asteroid was spawned (in milliseconds)
        /// </summary>
        private int _lastAsteroidTimeElapsed = 0;
        public List<Asteroid> Asteroids { get; set; }
        private Sprite _asteroidSprite;

        private int score = 0;
        
        public MainScreen(string name, ScreenManager screenManager) : base(name, screenManager)
        {
            player = new Player(this);
            Asteroids = new List<Asteroid>();
            random = new Random();
        }
        /// <summary>
        /// Should be called in order to reset defaults
        /// </summary>
        public override void Initialize()
        {
            player.Initialize();
            Asteroids = new List<Asteroid>();
            Difficulty = StartingDifficulty;
            lastDifficultyUpdate = DateTime.Now;
            lastInit = DateTime.Now;
            score = 0;

        }
        public override void LoadContent(ContentManager contentManager)
        {
            _backgroundTexture = contentManager.Load<Texture2D>("textures/background/darkPurple");
            _asteroidSprite = new Sprite("meteorBrown_big1", SpriteSheetCollection.Instance.Atlas,
                SpriteSheetCollection.Instance["meteorBrown_big1.png"]);
            _asteroidSprite.CollisionDetection = CollisionDetectionMethod.Circle;
            font = contentManager.Load<SpriteFont>("fonts/Calibri16");
            player.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            score += gameTime.ElapsedGameTime.Milliseconds;
            if ((DateTime.Now - lastDifficultyUpdate).TotalMilliseconds > 5000)
            {
                if (Difficulty < MaxDifficulty)
                {
                    Difficulty++;
                }
                lastDifficultyUpdate = DateTime.Now;
            }
        

            _bgDeltaX = _bgDeltaX + gameTime.ElapsedGameTime.Milliseconds* BackgroundMoveSpeed;
            if (_bgDeltaX <= -256) _bgDeltaX = 0;
            if(InputSingletone.Instance.IsKeyPress(Keys.Escape))
            {
                ScreenManager.SetActiveBackground();
                ScreenManager.SetActiveScreen("pause");
            }
            List<Asteroid> asteroidsToDel = new List<Asteroid>();
            List<Bullet> bulletsToDel = new List<Bullet>();
            foreach (var asteroid in Asteroids)
            {
                asteroid.Update(gameTime);
                
                if (asteroid.Sprite.Intersects(player.Sprite, asteroid.Position, player.Position))
                {
                    ScreenManager.SetActiveBackground();
                    Game.lastScore = score/500;//(int)(DateTime.Now - lastInit).TotalSeconds;
                    ScreenManager.SetActiveScreen("results");
                }
                if (asteroid.Position.X < -asteroid.Sprite.Size.X) asteroidsToDel.Add(asteroid);
                else
                {
                    foreach (var bullet in player.Bullets)
                    {
                        if(bullet.Sprite.Intersects(asteroid.Sprite, bullet.Position, asteroid.Position))
                        {
                            asteroidsToDel.Add(asteroid);
                            bulletsToDel.Add(bullet);
                        }
                    }
                }
            }
            asteroidsToDel.ForEach(x=>Asteroids.Remove(x));
            try
            {
                bulletsToDel.ForEach(x => player.Bullets.Remove(x));
            }
            catch (Exception)
            {
                
                throw;
            }

            _lastAsteroidTimeElapsed += gameTime.ElapsedGameTime.Milliseconds;
            if (_lastAsteroidTimeElapsed > (5000/Difficulty)*(random.NextDouble()/2f + 1))
            {
                _lastAsteroidTimeElapsed = 0;
                Asteroid asteroid = new Asteroid(_asteroidSprite, new Vector2(Game.Width + _asteroidSprite.Size.X, random.Next(0, Game.Height)), -Vector2.UnitX,
                    ((float)random.NextDouble()/10f+0.1f)*0.25f*Difficulty );//
                asteroid.RotationSpeed = ((float) random.NextDouble() - 0.5f)*0.02f;
                Asteroids.Add(asteroid);
            }

            player.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //DRAW BACKGROUND
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
            spriteBatch.Draw(_backgroundTexture, null,
                new Rectangle(-(int)_bgDeltaX, 0, Game.GraphicsDevice.Viewport.Width+(int)_bgDeltaX, Game.GraphicsDevice.Viewport.Height),
                new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width+(int)_bgDeltaX, Game.GraphicsDevice.Viewport.Height) 
                );
            spriteBatch.End();
            //END BACKGROUND

            spriteBatch.Begin();
            player.Draw(spriteBatch, gameTime);
            Asteroids.ForEach(x=>x.Draw(spriteBatch, gameTime));
            spriteBatch.End();
        }
    }
    
}
