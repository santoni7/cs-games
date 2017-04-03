using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SmartPackmans.GameSystem;
using SpaceShooter.GameSystem;

namespace SpaceShooter.Screens
{
    public class Player : IDrawableGameObject, IUpdatableGameObject
    {
        public string Tag { get; set; }

        public Sprite Sprite
        {
            get { return playerSprite; }
            set { playerSprite = value; }
        }

        private Sprite playerSprite;
        private Sprite bulletSprite;

        private Screen parentScreen;

        public Vector2 Position = new Vector2(50, 100);

        public Vector2 Size = new Vector2(100, 50);
        public List<Bullet> Bullets = new List<Bullet>();

        public float BulletsPerMinute = 45;
        private DateTime lastFired = DateTime.Now;

        private SoundEffect _laser1Sound;
        public Player(Screen screen)
        {
            this.parentScreen = screen;
        }
        public void LoadContent(ContentManager content)
        {
            playerSprite = 
                new Sprite("mainPlayerSprite", SpriteSheetCollection.Instance.Atlas, 1f, SpriteSheetCollection.Instance[parentScreen.Game.PlayerTextureName],(float) Math.PI/2);
            bulletSprite = new Sprite("mainPlayerBulletSprite", SpriteSheetCollection.Instance.Atlas,SpriteSheetCollection.Instance["laserBlue16.png"], (float) Math.PI/2);
            bulletSprite.CollisionDetection = CollisionDetectionMethod.Rectangle;
            _laser1Sound = content.Load<SoundEffect>("audio/sfx_laser1");
        }

        public void Initialize()
        {
            playerSprite =
                new Sprite("mainPlayerSprite", SpriteSheetCollection.Instance.Atlas, 1f, SpriteSheetCollection.Instance[parentScreen.Game.PlayerTextureName], (float)Math.PI / 2);
            bulletSprite = new Sprite("mainPlayerBulletSprite", SpriteSheetCollection.Instance.Atlas, SpriteSheetCollection.Instance["laserBlue16.png"], (float)Math.PI / 2);
            Bullets = new List<Bullet>();

        }
        public void Update(GameTime gameTime)
        {
            int mY = InputSingletone.Instance.MouseState.Y;
            Position.Y = MathHelper.Clamp( MathHelper.SmoothStep(Position.Y, mY, 0.05f * gameTime.ElapsedGameTime.Milliseconds),0, parentScreen.Game.Height); //TODO: change hardcoded val
            

            if(InputSingletone.Instance.MouseState.LeftButton == ButtonState.Pressed && InputSingletone.Instance.LastMouseState.LeftButton == ButtonState.Released)
            {
                if ((DateTime.Now - lastFired).TotalMilliseconds > 1000*BulletsPerMinute/60)
                {
                    var soundInstance = _laser1Sound.CreateInstance();
                    soundInstance.IsLooped = false;
                    soundInstance.Play();
                    var newBullet = new Bullet()
                    {
                        Direction = new Vector2(1, 0),
                        Position = Position,
                        Speed = 0.5f,
                        Sprite = bulletSprite,
                        TTL = 2000
                    };
                    Bullets.Add(newBullet);
                    lastFired = DateTime.Now;
                }
            }
            foreach (var bullet in Bullets)
            {
                bullet.Update(gameTime);
            }
            
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            playerSprite.Draw(Position, spriteBatch, gameTime);
            Bullets.ForEach(x => x.Draw(spriteBatch, gameTime));
        }
    }

    public class Bullet :IDrawableGameObject, IUpdatableGameObject
    {
        public string Tag { get; set; }
        public Vector2 Position;
        public Vector2 Direction;
        public float Speed = 0.5f;
        public Sprite Sprite;
        public int TTL;
        public void Update(GameTime gameTime)
        {
            Position += Direction*Speed*gameTime.ElapsedGameTime.Milliseconds;
            TTL -= gameTime.ElapsedGameTime.Milliseconds;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Sprite.Draw(Position, spriteBatch, gameTime);
        }
    }
}
