using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SmartPackmansWindows;

namespace SpaceShooter.GameSystem
{
    public enum ScreenState
    {
        Active, NonActive, InBackground
    }
    public abstract class Screen : IUpdatableGameObject, IDrawableGameObject
    {
        public string Tag { get; set; }
        public Texture2D Texture { get; set; }
        public ScreenManager ScreenManager { get; }
        public ScreenState ScreenState { get; set; }
        public SpaceShooterGame Game => ScreenManager.Game;
        

        public Screen(string name, ScreenManager screenManager)
        {
            
            Tag = name;
            ScreenManager = screenManager;
            ScreenState = ScreenState.NonActive;
            screenManager.AddScreen(this);
        }

        public abstract void Initialize();

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        public abstract void Update(GameTime gameTime);

        public abstract void LoadContent(ContentManager contentManager);

        public override int GetHashCode()
        {
            return Tag.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var gameObject = obj as IGameObject;
            return gameObject != null && Tag == gameObject.Tag;
        }
    }
}
