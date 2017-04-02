using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyNewGame
{
    class Block
    {
        public Rectangle Rect
        {
            get;
            set;

        }
        Texture2D texture;
        Game1 game;
        public Texture2D leftOverlay;
        public Texture2D rightOverlay;
        public Block(Texture2D Texture, Rectangle rect, Game1 game, Texture2D _leftOverlay, Texture2D _rightOverlay)
        {
            texture = Texture;
            Rect = rect;
            leftOverlay = _leftOverlay;
            rightOverlay = _rightOverlay;
            this.game = game;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle screenRect = game.GetScreenRect(Rect);
            spriteBatch.Draw(texture, screenRect, Color.White);
            if (rightOverlay != null)
                spriteBatch.Draw(rightOverlay, screenRect, Color.White);
            if (leftOverlay != null)
                spriteBatch.Draw(leftOverlay, screenRect, Color.White);
        }
    }
}
