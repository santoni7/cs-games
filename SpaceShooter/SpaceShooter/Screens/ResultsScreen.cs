using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SmartPackmans.GameSystem;
using SpaceShooter.GameSystem;
using SpaceShooter.Screens;

namespace SpaceShooter.Screens
{
    class ResultsScreen : Screen
    {
        private SpriteFont font;
        private List<MenuItem> items = new List<MenuItem>();
        private int currentItem = 0;
        public ResultsScreen(string name, ScreenManager screenManager) : base(name, screenManager)
        {
            //items.Add(new MenuItem()
            //{
            //    Caption = "Continue",
            //    IsHovered = true,
            //    OnClicked =
            //       () => { screenManager.ClearBackground(); screenManager.SetActiveScreen("main"); }
            //});
            items.Add(new MenuItem()
            {
                Caption = "Restart",
                IsHovered = true,
                OnClicked =
                    () =>
                    {
                        screenManager.ClearBackground();
                        screenManager.SetActiveScreen("main");
                    }
            });
            items.Add(new MenuItem()
            {
                Caption = "Main menu",
                IsHovered = true,
                OnClicked =
                    () =>
                    {
                        screenManager.ClearBackground();
                        screenManager.SetActiveScreen("menu");
                    }
            });


            items.Add(new MenuItem()
            {
                Caption = "Exit",
                IsHovered = true,
                OnClicked =
                   () => { Game.Exit(); }
            });

        }

        public override void Initialize()
        {
            //throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            int dy = 50;
            int i = 0;
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Game over! Your score: " + Game.lastScore, new Vector2(200, 100), Color.Green);
            spriteBatch.Draw(SpriteSheetCollection.Instance.PointTexture, null, new Rectangle(0, 0, Game.Width, Game.Height), null, null, 0, null, Color.FromNonPremultiplied(120, 120, 120, 120));
            foreach (var item in items)
            {
                if (i == currentItem) spriteBatch.DrawString(font, item.Caption, new Vector2(200, 300 + i * dy), Color.Red);
                else spriteBatch.DrawString(font, item.Caption, new Vector2(200, 300 + i * dy), Color.Green);
                i++;
            }
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputSingletone.Instance.IsKeyPress(Keys.Down)) currentItem += 1;
            if (InputSingletone.Instance.IsKeyPress(Keys.Up)) currentItem -= 1;
            if (InputSingletone.Instance.IsKeyPress(Keys.Enter)) items[currentItem].OnClicked();
            if (InputSingletone.Instance.IsKeyPress(Keys.Right)) items[currentItem].OnRightPress();
            if (InputSingletone.Instance.IsKeyPress(Keys.Left)) items[currentItem].OnLeftPress();
            if (currentItem < 0) currentItem = items.Count - 1;
            currentItem = currentItem % (items.Count);
        }

        public override void LoadContent(ContentManager contentManager)
        {
            font = contentManager.Load<SpriteFont>("fonts/Calibri16");
        }
    }
}
