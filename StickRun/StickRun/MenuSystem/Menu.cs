using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MyNewGame.MenuSystem
{
    class Menu
    {
        public List<MenuItem> Items { get; set; }
        SpriteFont font;
        public string additionalMessage = "Привет! Press \"New Game\" to begin";
        Vector2 addpos = new Vector2(400,200);
        float rotation = MathHelper.ToRadians(0.0f);
        float scale = 0.7f;
        Vector2 Origin = new Vector2(100, 10);
        int currentItem;
        KeyboardState oldState;


        public Menu()
        {
            Items = new List<MenuItem>();
        }
        public void Update(GameTime gt)
        {

            addpos += new Vector2((float)Math.Sin(gt.TotalGameTime.TotalSeconds),(float)Math.Cos(gt.TotalGameTime.TotalSeconds));
            rotation -= (float)Math.Sin(gt.TotalGameTime.TotalSeconds)/10;
            scale += (float)Math.Sin(gt.TotalGameTime.TotalSeconds) / 50;
            Origin -= new Vector2((float)Math.Cos(gt.TotalGameTime.TotalSeconds) / 4, (float)Math.Sin(gt.TotalGameTime.TotalSeconds) / 4);

            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Enter))
                Items[currentItem].OnClick();
            int delta = 0;
            if (state.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
                delta = -1;
            if (state.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
                delta = 1;

            currentItem += delta;
            bool isOk = false;
            while(!isOk)
            {
                if (currentItem < 0)
                    currentItem = Items.Count - 1;
                else if (currentItem > Items.Count - 1)
                    currentItem = 0;
                else if (Items[currentItem].Active == false)
                    currentItem += delta;
                else isOk = true;
                    
            }

            oldState = state;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Begin();
            int y = 100;
            spriteBatch.DrawString(font, "Welcome to the StickRun!", addpos, Color.ForestGreen, rotation, Origin, scale, SpriteEffects.None,0);
            foreach (MenuItem item in Items)
            {
                Color color = Color.SaddleBrown;
                if(!item.Active)
                    color = Color.Gray;
                if(item == Items[currentItem])
                    color = Color.White;
                spriteBatch.DrawString(font, item.Name, new Vector2(100, y) + (addpos/50),color
                    ,rotation/50+(MathHelper.ToRadians(3)),Origin/2+new Vector2(0,(float)Math.Sin( MathHelper.ToRadians(y)))
                    ,1,SpriteEffects.None,0);
            y += 40;
            }

            spriteBatch.DrawString(font, "Welcome to the StickRun!", addpos, Color.ForestGreen, rotation, Origin, scale, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, additionalMessage, new Vector2(300, 150), Color.White);
            

            spriteBatch.End();

        }

        public void LoadContent(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("MenuFont");
        }
    }
}
