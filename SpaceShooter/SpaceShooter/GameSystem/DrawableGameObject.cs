using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.GameSystem
{
    public class DrawableGameObject : IDrawableGameObject, IMovableGameObject
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Velocity { get; set; }
        public string Tag { get; set; }
        public Action<DrawableGameObject, GameTime> OnUpdateAction; 
        #region Constructors
        public DrawableGameObject(Vector2 position, Texture2D texture, Vector2 velocity, string tag)
        {
            Position = position;
            Texture = texture;
            Velocity = velocity;
            Tag = tag;
        }

        public DrawableGameObject(Vector2 position, Texture2D texture)
        {
            Position = position;
            Texture = texture;
        }

        public DrawableGameObject(Vector2 position, Texture2D texture,  string tag)
        {
            Position = position;
            Texture = texture;
            Tag = tag;
        }

        public DrawableGameObject(string tag)
        {
            Tag = tag;
        }

        public DrawableGameObject(Texture2D texture, string tag)
        {
            Texture = texture;
            Tag = tag;
        }

        public DrawableGameObject(Vector2 position, string tag)
        {
            Position = position;
            Tag = tag;
        }
        #endregion

        public void Update(GameTime gameTime)
        {
            OnUpdateAction?.Invoke(this,gameTime);
            Position += Velocity*gameTime.ElapsedGameTime.Milliseconds;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (Texture == null) throw new NullReferenceException("Texture cannot be null");
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
