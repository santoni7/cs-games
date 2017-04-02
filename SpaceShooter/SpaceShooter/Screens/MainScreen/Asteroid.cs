using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SmartPackmans.GameSystem;
using SpaceShooter.GameSystem;

namespace SpaceShooter.Screens
{
    public class Asteroid : IDrawableGameObject, IUpdatableGameObject
    {

        public string Tag { get; set; }
        public Sprite Sprite { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }

        public float Speed = -0.05f;
        public float RotationSpeed = 0.05f;
        private float _rotation = 0;
        public Asteroid(Sprite sprite, Vector2 position, Vector2 direction, float speed)
        {
            Sprite = sprite;
            Position = position;
            Direction = direction;
            Speed = speed;
            
        }

        public Asteroid(Sprite sprite, Vector2 position)
        {
            Sprite = sprite;
            Position = position;
        }

        public void Update(GameTime gameTime)
        {
            Position += Direction * Speed * gameTime.ElapsedGameTime.Milliseconds;
            _rotation += RotationSpeed*gameTime.ElapsedGameTime.Milliseconds;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Sprite.Draw(Position, _rotation, spriteBatch, gameTime);
        }
    }
}
