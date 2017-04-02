using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MyNewGame
{
    class AnimatedSprite
    {
        Texture2D idleText;
        Texture2D runText;
        public Rectangle rect;
        int frameWidth;
        int frameHeight;
        int currentFrame;
        int timeElapsed; // Сколько времени прошло с начала показа текущего кадра
        int timeForFrame=50;
        bool isRunning = false;
        bool toRight;
        bool isJumping;
        public double SpeedK = 2;
        public double AISpeedK = 1;


        int timeForJump = (int)(2000*rand.NextDouble() + 3000);
        public static Random rand = new Random();

        float ySpeed;
        float maxYSpeed = 10;
        public float g = 0.2f;

        
        Game1 game;
        public int frameCount
        {
            get 
            {
                return runText.Width / frameWidth;    
            }
        }
        public void Run(bool Right)
        {
            if (!isRunning)
            {
                currentFrame = 0;
                timeElapsed = 0;
                isRunning = true;
            }
            toRight = Right;
            
        }
        public void Jump()
        {
            if (!isJumping && ySpeed == 0)
            {
                ySpeed = maxYSpeed;
                currentFrame = 0;
                timeElapsed = 0;
                isJumping = true;
            }
        }
        public void Stop()
        {
            isRunning = false;
            currentFrame = 0;
            timeElapsed = 0;
        }
        public AnimatedSprite(Rectangle rect, Texture2D idle, Texture2D run,Game1 game)
        {
            this.rect = rect;
            idleText = idle;
            runText = run;
            frameWidth = frameHeight = runText.Height;
            this.game = game;
        }
        public void ApplyGravity(GameTime gameTime)
        {
            ySpeed = ySpeed - g * gameTime.ElapsedGameTime.Milliseconds / 10;
            float dy = ySpeed * gameTime.ElapsedGameTime.Milliseconds / 10;
            Rectangle nextPosition = rect;
            nextPosition.Offset(0, -(int)Math.Floor(dy));
            Rectangle bounded = GetBoundingRect(nextPosition);
            Rectangle ScreenRect = game.GetScreenRect(bounded);
            if (ScreenRect.Top > 0
               // && ScreenRect.Bottom < game.Height 
                && !game.CollidesWithLevel(bounded))
                rect = nextPosition;
            bool collidesOnFallDown = game.CollidesWithLevel(bounded)&&ySpeed<0;
            if (
                //ScreenRect.Bottom > game.Height 
                //|| 
                collidesOnFallDown)
            {
                 isJumping = false;
                 ySpeed = 0;
            }
        }

        public void UpdateAI(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime.Milliseconds;
            if (timeElapsed > timeForFrame)
            {
                timeElapsed = 0;
                currentFrame = (currentFrame + 1) % frameCount;
            }
            if (isRunning)
            {
                timeElapsed += gameTime.ElapsedGameTime.Milliseconds;
                if (timeElapsed > timeForFrame)
                {
                    timeElapsed = 0;
                    currentFrame = (currentFrame + 1) % frameCount;
                }
                int dx = (int)(AISpeedK * gameTime.ElapsedGameTime.Milliseconds / 10);
                if (!toRight)
                    dx = -dx;
                Rectangle nextPosition = rect;
                nextPosition.Offset(dx, 0);
                Rectangle BoundingRect = GetBoundingRect(nextPosition);
                /*
                if (BoundingRect.Left < 0 || BoundingRect.Right > game.levelLength)
                    toRight = !toRight;
                else if (game.CollidesWithLevel(BoundingRect))
                    toRight = !toRight;
                else if (game.WillFallDown(BoundingRect))
                    toRight = !toRight;
                else
                    rect = nextPosition;
                */
                if (!game.CollidesWithLevel(BoundingRect))
                    rect = nextPosition;
               

            }
            if (game.HeroRect.Left < rect.Left)
                Run(false);
            if (game.HeroRect.Left > rect.Left)
                Run(true);
            if(game.HeroRect.Left == rect.Left)
                Stop();
            timeForJump -= gameTime.ElapsedGameTime.Milliseconds;
            if (timeForJump <= 0)
            {
                Jump();
                timeForJump = (int)(2000 * rand.NextDouble() + 3000);
            }
            ApplyGravity(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime.Milliseconds;
            if (timeElapsed > timeForFrame)
            {
                timeElapsed = 0;
                currentFrame = (currentFrame + 1) % frameCount;
            }
            if (isRunning)
            {
                timeElapsed += gameTime.ElapsedGameTime.Milliseconds;
                if (timeElapsed > timeForFrame)
                {
                    timeElapsed = 0;
                    currentFrame = (currentFrame+1)%frameCount;
                }
                int dx = (int)(SpeedK * gameTime.ElapsedGameTime.Milliseconds / 10);
                if (!toRight)
                    dx = -dx;
                Rectangle nextPosition = rect;
                nextPosition.Offset(dx,0);
                Rectangle BoundingRect = GetBoundingRect(nextPosition);
                Rectangle ScreenRect = game.GetScreenRect(BoundingRect);

                if (ScreenRect.Left > 0 && ScreenRect.Right < game.Width && !game.CollidesWithLevel(BoundingRect))
                   rect = nextPosition ;
            }
            ApplyGravity(gameTime);
        }
        public Rectangle GetBoundingRect(Rectangle rect)
        {
            int width = (int)(rect.Width * 0.4f);
            int dx = (int)(rect.Width*0.3f);
            return new Rectangle(rect.X + dx, rect.Y, width, rect.Height);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
           Rectangle sourceRect = new Rectangle(frameWidth * currentFrame, 0, frameWidth, frameHeight);
            Rectangle screenRect = game.GetScreenRect(rect);
            SpriteEffects effect = SpriteEffects.None;
            if (toRight)
                effect = SpriteEffects.FlipHorizontally;
            if (isJumping)
            {
                spriteBatch.Draw(runText, screenRect, sourceRect, Color.White, 0, Vector2.Zero, effect, 0);
                
            }
            else
            if (isRunning)
            {

                spriteBatch.Draw(runText, screenRect, sourceRect, Color.White, 0, Vector2.Zero, effect, 0);
            }
            else
            {
                spriteBatch.Draw(idleText, screenRect, Color.White);

            }
            
        }
    }
}
