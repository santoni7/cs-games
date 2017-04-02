using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.IO.Compression;
using System.Collections.Specialized;
using Ionic.Zip;
using System.Xml;
using MyNewGame.MenuSystem;
using System.Text;

namespace MyNewGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        AnimatedSprite hero;
        Texture2D runTexture;
        Texture2D idleTexture;
        Texture2D groundTexture;
        Texture2D groundTextureT;
        Texture2D groundTextureR;
        Texture2D groundTextureL;


        Texture2D platformTexture;

        public SpriteFont font;

        List<Block> blocks;
        List<Gem> gems;
        List<AnimatedSprite> enemies;

        int currentLevel = 1;
        public int Width;
        public int Height;
        int maxLevel = 8;
        KeyboardState oldState;
        byte[,] levelMap;

        public Rectangle HeroRect
        {
            get
            {
                return hero.rect;
            }
        }

        Texture2D lampTexture;

        Texture2D enemyIdleTexture;
        Texture2D enemyRunTexture;

        int Score = 0;

        string levelName;
        int ShowTimeElapsed;

        string filesFolder;

        int scrollX;
        public int levelLength;
        int scrollY;
        public int levelLengthY;

        int backScrollX;

        Color backgroundColor = Color.CornflowerBlue;
        Texture2D background;

        int Health = 100;
        Texture2D healthLine;
        Texture2D healthLineB;
        TimeSpan lastTouch = new TimeSpan(0, 0, 0);

        Menu menu;

        GameState gameState = GameState.Menu;

        SoundEffect sound;
        SoundEffect music;
        SoundEffectInstance instance;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Width = this.graphics.PreferredBackBufferWidth = 800;
            Height = this.graphics.PreferredBackBufferHeight = 500;
            //GetLevelParams(1);
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

            menu = new Menu();
            MenuItem newGame = new MenuItem("New Game");
            MenuItem resumeGame = new MenuItem("Resume");
            MenuItem soundControl = new MenuItem("Sound on/off");
            MenuItem exitGame = new MenuItem("Exit");

            resumeGame.Active = false;

            newGame.Click += new EventHandler(newGame_Click);
            resumeGame.Click += new EventHandler(resumeGame_Click);
            soundControl.Click += new EventHandler(soundControl_Click);
            exitGame.Click += new EventHandler(exitGame_Click);

            menu.Items.Add(newGame);
            menu.Items.Add(resumeGame);
            menu.Items.Add(soundControl);
            menu.Items.Add(exitGame);

            base.Initialize();


        }

        void soundControl_Click(object sender, EventArgs e)
        {
            if (oldState.IsKeyUp(Keys.Enter))
            {
                if (instance.State == SoundState.Playing)
                    instance.Pause();
                else instance.Play();
            }
        }

        void exitGame_Click(object sender, EventArgs e)
        {
            this.Exit();
        }

        void newGame_Click(object sender, EventArgs e)
        {
            menu.Items[1].Active = true;
            gameState = GameState.Game;
            Rectangle rect = new Rectangle(0, 0, 35, 35);
            hero = new AnimatedSprite(rect, idleTexture, runTexture, this);
            Score = 0;
            currentLevel = 1;
            CreateLevel();
        }

        void resumeGame_Click(object sender, EventArgs e)
        {
            gameState = GameState.Game;
        }

        public bool WillFallDown(Rectangle rect)
        {
            Rectangle r = rect;
            r.Offset(0, 5);
            if (!CollidesWithLevel(r))
                return true;
            else
                return false;
        }

        public Rectangle GetScreenRect(Rectangle rect)
        {
            Rectangle r = rect;
            r.Offset(-scrollX, -scrollY);
            return r;
        }
        public void Scroll(int dx)
        {
            if (scrollX + dx > 0 && scrollX + dx < levelLength - Width)
                scrollX += dx;
        }
        public void ScrollXBack(int dx)
        {
            if (backScrollX + dx + Width > 0 && backScrollX + dx + Width < levelLength - Width)
                backScrollX += dx;
        }
        public void ScrollY(int dy)
        {
            if (scrollY + dy > 0 && scrollY + dy < levelLengthY - Height)
                scrollY += dy;
        }
        public void CreateLevel()
        {
            blocks = new List<Block>();
            Health = 100;
            ShowTimeElapsed = 0;
            gems = new List<Gem>();
            enemies = new List<AnimatedSprite>();
            XmlDocument xml =  GetLevelParams(currentLevel);
            hero.g = (float)Convert.ToDouble(xml.SelectSingleNode("params/LevelGravity").InnerText);
            hero.SpeedK = (double)(Convert.ToDouble(xml.SelectSingleNode("params/HeroSpeedK").InnerText));
            string[] lines =  File.ReadAllLines(filesFolder+"level.lvl");
            hero.rect = new Rectangle(Convert.ToInt32(xml.SelectSingleNode("params/SpawnPoint/X").InnerText),
                Convert.ToInt32(xml.SelectSingleNode("params/SpawnPoint/Y").InnerText), 35, 35);
            
            backgroundColor = Color.FromNonPremultiplied(Convert.ToInt32(xml.SelectSingleNode("params/BackgroundColor/Red").InnerText),
                Convert.ToInt32(xml.SelectSingleNode("params/BackgroundColor/Green").InnerText),
                Convert.ToInt32(xml.SelectSingleNode("params/BackgroundColor/Blue").InnerText),
                Convert.ToInt32(xml.SelectSingleNode("params/BackgroundColor/Alpha").InnerText));
            levelName = xml.SelectSingleNode("params/LevelName").InnerText;

            string str = xml.SelectSingleNode("params/LevelDesc").InnerText;
            Encoding srcEncodingFormat = Encoding.UTF8;
            Encoding dstEncodingFormat = Encoding.GetEncoding("windows-1251");
            byte[] originalByteString = srcEncodingFormat.GetBytes(str);
            byte[] convertedByteString = Encoding.Convert(srcEncodingFormat,
            dstEncodingFormat, originalByteString);
            menu.additionalMessage =
                //dstEncodingFormat.GetString(convertedByteString);
                "Чтобы начать новую игру выберите \r\n \"New Game\" или \"Resume\",\r\n чтобы продолжить.\r\n  \r\nБегите с помощью стрелок, \r\nпрыгайте с помощью стрелки вверх. \r\nСобирайте алмазы и избегайте врагов!";
             
            
            levelLength = 40 * lines[0].Length;
            levelLengthY = 40 * lines.Length;
            scrollX = 0;
            levelMap = new byte[lines[0].Length, lines.Length];
            int x = 0;
            int y = 0;
            int i = 0;
            int j = 0;
            foreach(string item in lines)
            {
                foreach (char c in item)
                {
                    if (c == 'X')
                    {
                        Rectangle rect = new Rectangle(x,y,40,40);
                       //Block block = new Block(platformTexture, rect, this);
                        Block block;
                        /*if (levelMap[i, j - 1] == 1)
                            block = new Block(groundTexture, rect, this);
                        else
                            block = new Block(groundTextureT, rect, this);*/
                        block = new Block(platformTexture, rect, this, null, null);
                        blocks.Add(block);
                        levelMap[i, j] = 1;
                    }
                    else if (c == 'Y')
                    {
                        Rectangle rect = new Rectangle(x, y, 40, 40);
                        Block block;
                        if (j != 0)
                        {
                            if (levelMap[i, j - 1] == 1)
                                block = new Block(groundTexture, rect, this, null, null);
                            else
                                block = new Block(groundTextureT, rect, this, null, null);

                        }
                        else
                            block = new Block(groundTexture, rect, this, null, null);

                        if (i - 1 > -1)
                        {
                            if (levelMap[i - 1, j] ==0)
                                block.leftOverlay = groundTextureL;
                        }
                        if (i + 1 < lines.Length)
                        {
                            if (item[i+1]!='Y')
                                block.rightOverlay = groundTextureR;
                        }
                        blocks.Add(block);
                        levelMap[i, j] = 1;
                    }
                    else if (c == 'A')
                    {
                        Rectangle gemRect = new Rectangle(x, y, 20, 20);
                        Gem gem = new Gem(lampTexture, gemRect, this);
                        gems.Add(gem);

                    }
                    else if (c == 'E')
                    {
                        Rectangle enemyR = new Rectangle(x, y, 20, 20);
                        AnimatedSprite enemy = new AnimatedSprite(enemyR, enemyIdleTexture, enemyRunTexture, this);
                        enemies.Add(enemy);
                        enemy.AISpeedK = (double)(Convert.ToDouble(xml.SelectSingleNode("params/EnemySpeedK").InnerText));
                        enemy.g = (float)Convert.ToDouble(xml.SelectSingleNode("params/EnemyGravity").InnerText);
                        
                        enemy.Run(true);

                    }
                    
                    x += 40;
                    i++;
                }
                x = 0;
                i = 0;
                j++;
                y += 40;
                scrollY = 0;
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("BackGround");

            healthLine = Content.Load<Texture2D>("HealthLine");
            healthLineB = Content.Load<Texture2D>("HealthLineB");

            runTexture = Content.Load<Texture2D>("Run");
            idleTexture = Content.Load<Texture2D>("idle");
            Rectangle rect = new Rectangle(0, 0, 35, 35);
            hero = new AnimatedSprite(rect,idleTexture,runTexture, this);
            groundTexture = Content.Load<Texture2D>("ground");
            groundTextureT = Content.Load<Texture2D>("groundT");
            groundTextureR = Content.Load<Texture2D>("groundR");
            groundTextureL = Content.Load<Texture2D>("groundL");
            platformTexture = Content.Load<Texture2D>("platform");
            lampTexture = Content.Load<Texture2D>("almaz");
            font = Content.Load<SpriteFont>("GameFont");

            menu.LoadContent(Content);

            sound = Content.Load<SoundEffect>("Sound/Catch");
            music = Content.Load<SoundEffect>("Sound/Music");

            instance = music.CreateInstance();
            instance.IsLooped = true;

            instance.Play();

            
            

            enemyIdleTexture = Content.Load<Texture2D>("Enemy/enemyIdle");
            enemyRunTexture = Content.Load<Texture2D>("Enemy/EnemyRun");
            CreateLevel();
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        public XmlDocument GetLevelParams(int n)
        {
            #region ZipUnpack
            string zipToUnpack = "Content/Levels/level"+n+"z.lvl";
            string tempFolder = Path.GetTempPath(); ;
            string unpackDirectory =
                filesFolder = tempFolder + "GameJump/";
            using (ZipFile zip1 = ZipFile.Read(zipToUnpack))
            {
                // here, we extract every entry, but we could extract conditionally
                // based on entry name, size, date, checkbox status, etc.  
                foreach (ZipEntry e in zip1)
                {
                    e.Extract(unpackDirectory, ExtractExistingFileAction.OverwriteSilently);
                }
            }
            #endregion
            #region LoadParams

           XmlDocument xml = new XmlDocument();
           string lines = File.ReadAllText(filesFolder + "params.xml");
    
            xml.LoadXml( lines.ToString());
            

            #endregion
            return xml;

        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            if (gameState == GameState.Game)
                UpdateGameLogic(gameTime);
            else menu.Update(gameTime);

            

            oldState = state;
            base.Update(gameTime);
        }

        private void UpdateGameLogic(GameTime gameTime)
        {

            // Allows the game to exit
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape))
                gameState = GameState.Menu;

            ShowTimeElapsed += gameTime.ElapsedGameTime.Milliseconds;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (state.IsKeyDown(Keys.Left))
            {
                hero.Run(false);
                ScrollXBack((int)(levelLength / background.Width) * gameTime.ElapsedGameTime.Milliseconds / 70);
            }
            else if (state.IsKeyDown(Keys.Right))
            {
                hero.Run(true);
                ScrollXBack((int)-((levelLength / Width) * gameTime.ElapsedGameTime.Milliseconds / 70));
            }

            else
                hero.Stop();
            if (state.IsKeyDown(Keys.Up))
                hero.Jump();
            hero.Update(gameTime);
            foreach (AnimatedSprite enemy in enemies)
            {
                enemy.UpdateAI(gameTime);
            }

            if (gems.Count < 1)
            {
                currentLevel++;
                if (currentLevel > maxLevel)
                    currentLevel = 1;
                CreateLevel();

            }



            Rectangle heroScreenRect = GetScreenRect(hero.rect);
            if (heroScreenRect.Right < Width / 2)
            {
                Scroll((int)-2 * gameTime.ElapsedGameTime.Milliseconds / 10);
            }
            if (heroScreenRect.Left > Width / 2)
            {
                Scroll((int)(2 * gameTime.ElapsedGameTime.Milliseconds / 10));
            }
            /*
            if (heroScreenRect.Right < Width / 2)
            {
                ScrollXBack((int)-(levelLength/Width) * gameTime.ElapsedGameTime.Milliseconds / 20);
            }
            if (heroScreenRect.Left > Width / 2)
            {
                ScrollXBack((int)((levelLength/Width) * gameTime.ElapsedGameTime.Milliseconds / 20));
            }
              */
            if (heroScreenRect.Top < Height / 2)
                ScrollY(-2 * gameTime.ElapsedGameTime.Milliseconds / 10);
            if (heroScreenRect.Bottom > Height / 2)
                ScrollY(2 * gameTime.ElapsedGameTime.Milliseconds / 10);

            /*
            if (state.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
            {
                currentLevel++;
                if (currentLevel > maxLevel)
                    currentLevel = 1;
                CreateLevel();

            }
             * */
            foreach (Gem gem in gems)
            {
                gem.Update(gameTime);
            }
            // TODO: Add your update logic here
            oldState = state;
            int i = 0;
            Rectangle bounding = hero.GetBoundingRect(hero.rect);
            while (i < gems.Count)
            {
                if (gems[i].Rect.Intersects(bounding))
                {
                    gems.RemoveAt(i);
                    Score += 10;
                    
                    sound.Play(0.5f,0,0);
                }
                else
                {
                    i++;
                }
            }

            foreach (AnimatedSprite enemy in enemies)
            {
                Rectangle enemyBoundingRect = enemy.GetBoundingRect(enemy.rect);
                if (enemyBoundingRect.Intersects(bounding))
                {
                    //Score = 0;
                    if ((gameTime.TotalGameTime - lastTouch).Milliseconds > 200)
                    {
                        Health -= 30;
                        lastTouch = gameTime.TotalGameTime;
                    }
                    //CreateLevel();
                }
            }
            if (Health <= 0)
            {
                Score = 0;
                CreateLevel();
            }
            base.Update(gameTime);
        }

        public bool CollidesWithLevel(Rectangle rect)
        {
            /*
            foreach (Block block in blocks)
            {
                if (block.Rect.Intersects(rect))
                    return true;
            }

            return false;
              */

            int minX = rect.Left / 40;
            int minY = rect.Top / 40;
            int maxX = rect.Right / 40;
            int maxY = rect.Bottom / 40;
            for(int i =minX;i<=maxX;i++)
            {
                for(int j=minY; j<=maxY;j++)
                {
                    if (levelMap[i,j] == 1) return true;
                }
            }
            return false;

        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);
            spriteBatch.Begin();
            Rectangle r = new Rectangle(0, 0, Width, Height);
            //r.Offset(-backScrollX, 0);
            spriteBatch.Draw(background, r,
               new Rectangle(0, 0, background.Width, background.Height),
                Color.White);
            spriteBatch.End();
            if (gameState == GameState.Game)
                DrawGame();
            else menu.Draw(spriteBatch);
            base.Draw(gameTime);
        }

        private void DrawGame()
        {
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            Rectangle r = new Rectangle(0, 0, Width, Height);
            //r.Offset(-backScrollX, 0);
            spriteBatch.Draw(background, r,
               new Rectangle(0, 0, background.Width, background.Height),
                Color.White);
            foreach (Block block in blocks)
            {
                block.Draw(spriteBatch);
            }

            foreach (Gem gem in gems)
            {
                gem.Draw(spriteBatch);
            }
            foreach (AnimatedSprite enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }
            spriteBatch.DrawString(font, "Score:" + Score
                + "\r\nHealth:" + Health
                , Vector2.One, Color.White);
            spriteBatch.Draw(healthLineB, new Rectangle(1, 50, 100, 10), Color.White);
            spriteBatch.Draw(healthLine, new Rectangle(1, 50, Health, 10), Color.White);
            if (ShowTimeElapsed <= 5000)
                spriteBatch.DrawString(font, levelName, new Vector2(350, 2), Color.White);


            hero.Draw(spriteBatch);
            spriteBatch.End();
        }
    }

    enum GameState
    {
        Game,
        Menu
    }

}
