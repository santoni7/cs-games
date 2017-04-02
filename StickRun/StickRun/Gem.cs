using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MyNewGame
{
    class Gem
    {
       public Rectangle Rect
        {
            get;
            set;

        }
       int dy;
        Texture2D texture;
        Game1 game;
        

        public Gem( Texture2D Texture , Rectangle rect , Game1 game)
        {
            texture = Texture;
            Rect = rect;
            this.game = game;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle r = new Rectangle(Rect.X, Rect.Y + dy, Rect.Width, Rect.Height);
            Rectangle screenRect = game.GetScreenRect(r);
            spriteBatch.Draw(texture, screenRect, Color.White);
        }
        public void Update(GameTime gt)
        {
            float t = (float)(gt.TotalGameTime.TotalSeconds*3 + Rect.X);
            dy = (int)(Math.Sin(t) *10);
        }
    }
}

