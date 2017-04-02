using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SmartPackmansWindows;

namespace SpaceShooter.GameSystem
{
    /// <summary>
    /// Screen manager holds screens and draws/updates active screen when needed
    /// </summary>
    public class ScreenManager:IDrawableGameObject, IUpdatableGameObject
    {
        /// <summary>
        /// Default: ScreenManager
        /// Caution: should not be changed, though can be
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// Not used, leave null
        /// </summary>
        public Texture2D Texture { get; set; }

        public SpaceShooterGame Game { get; }

        private List<Screen> allScreens = new List<Screen>();
        private Screen backgroundScreen;
        private Screen activeScreen;   

        /// <summary>
        /// Initializes ScreenManager with "ScreenManager" tag
        /// </summary>
        public ScreenManager(SpaceShooterGame game):this("ScreenManager", game)
        {
        }

        public ScreenManager(string tag, SpaceShooterGame game)
        {
            Tag = tag;
            Game = game;
        }

        public void AddScreen(Screen screen)
        {
            allScreens.Add(screen);
        }

        public void ClearBackground()
        {
            if (backgroundScreen != null) backgroundScreen.ScreenState = ScreenState.NonActive;
        }


        public void SetActiveScreen(Screen screen, bool shouldInit = true)
        {
            if(screen == null) throw new ArgumentNullException(nameof(screen));
            if (activeScreen != null)
            {
                activeScreen.ScreenState = ScreenState.NonActive;
            }
            if(shouldInit)
                screen.Initialize();
            screen.ScreenState = ScreenState.Active;
            activeScreen = screen;
            //SetActiveBackground();
        }

        public void SetActiveScreen(string tag, bool shouldInit = true)
        {
            SetActiveScreen(allScreens.Find(x => x.Tag == tag), shouldInit);
        }

        /// <summary>
        /// Sets background screen
        /// </summary>
        /// <param name="screen"></param>
        /// <exception cref="ArgumentNullException">Screen cannot be null</exception>
        public void SetBackgroundScreen(Screen screen)
        {
            if (screen == null) throw new ArgumentNullException(nameof(screen));
            if( backgroundScreen!= null) backgroundScreen.ScreenState = ScreenState.NonActive;
            screen.ScreenState = ScreenState.InBackground;
            backgroundScreen = screen;
        }
        /// <summary>
        /// Moves active screen to background layer
        /// </summary>
        /// <exception cref="NullReferenceException">Throws nullreference if active screen is null</exception>
        public void SetActiveBackground()
        {
            if (activeScreen == null) throw new NullReferenceException("Active screen is null");
            if (backgroundScreen != null) backgroundScreen.ScreenState = ScreenState.NonActive;
            activeScreen.ScreenState = ScreenState.InBackground;
            backgroundScreen = activeScreen;
            activeScreen = null;
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // TODO: Update draw, draw background layer to rendertarger etc
            backgroundScreen?.Draw(spriteBatch, gameTime);
            activeScreen?.Draw(spriteBatch, gameTime);
        }

        public void Update(GameTime gameTime)
        {
            // TODO: Update "update" method
            
            activeScreen?.Update(gameTime);
        }
        public override int GetHashCode()
        {
            return Tag.GetHashCode();
        }
        
    }
}
