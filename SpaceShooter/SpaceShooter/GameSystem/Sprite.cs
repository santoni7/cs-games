using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameSystem;

namespace SmartPackmans.GameSystem
{
    public enum CollisionDetectionMethod
    {
        Rectangle, Circle
    }
    public class Sprite :IGameObject
    {
        public Texture2D Texture { get; set; }
        public float Rotation { get; set; }
        public float DefaultRotation { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 Origin { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public CollisionDetectionMethod CollisionDetection { get; set; }

        /// <summary>
        /// True, if collision rectangle should be rotated by pi/2
        /// </summary>
        public bool RotatedPiBy2 = false;

#if DEBUG
        private Vector2 CircleTextureOrigin;
#endif 
        /// <summary>
        /// Used only in Circle collision detection
        /// </summary>
        public float Radius = 0f;

        public Sprite(string tag, Texture2D texture, Rectangle sourceRectangle, float defaultRotation = 0)
        {
            Tag = tag;
            this.Texture = texture;
            this.Size = new Vector2(sourceRectangle.Width, sourceRectangle.Height);
            DefaultRotation = defaultRotation;
            Origin = new Vector2((float) ( /*sourceRectangle?.X +*/ sourceRectangle.Width/2f),
                (float) ( /*sourceRectangle?.Y + */sourceRectangle.Height/2f));


            SourceRectangle = sourceRectangle;
            CollisionDetection = CollisionDetectionMethod.Circle;
            Radius = (Size.X + Size.Y)/4.5f;
#if DEBUG
            CircleTextureOrigin = SpriteSheetCollection.Instance.CircleTexture.Bounds.Size.ToVector2()/2;
#endif
        }

        public Sprite(string tag, Texture2D texture, float scaleFactor, Rectangle sourceRectangle,
            float defaultRotation = 0) : this(tag, texture, sourceRectangle, defaultRotation)
        {
            Scale(scaleFactor);
        }

        public Sprite(string tag, Texture2D texture, Vector2 size, Rectangle sourceRectangle,
            float defaultRotation = 0) : this(tag, texture, sourceRectangle, defaultRotation)
        {
            Size = size;
        }

        /// <summary>
        /// Multiplies size by ScaleFactor
        /// </summary>
        /// <param name="scaleFactor"></param>
        public void Scale(float scaleFactor)
        {
            Size *= scaleFactor;
        }

        /// <summary>
        /// Rotates sprite by "angle"
        /// </summary>
        /// <param name="angle"></param>
        public void Rotate(float angle)
        {
            Rotation += angle;
        }

        public string Tag { get; set; }
        public void Draw(Vector2 position, SpriteBatch spriteBatch, GameTime gameTime)
        {
//            Vector2 scale = new Vector2(Size.X/SourceRectangle.Width, Size.Y/SourceRectangle.Height);
//            spriteBatch.Draw(Texture, position, null, SourceRectangle, Origin, DefaultRotation + Rotation, scale,
//                Color.White);


//#if DEBUG
//            DrawDebug(position, spriteBatch);
//#endif
            Draw(position, Rotation, spriteBatch, gameTime);
        }

        public void Draw(Vector2 position, float rotation, SpriteBatch spriteBatch, GameTime gameTime)
        {
            //Vector2 scale = new Vector2(Size.X / SourceRectangle.Width, Size.Y / SourceRectangle.Height);
            //spriteBatch.Draw(Texture, position, null, SourceRectangle, Origin, DefaultRotation + rotation, scale,
            //    Color.White);
            spriteBatch.Draw(Texture, null, GetRectangle(position), SourceRectangle, Origin, DefaultRotation + rotation,
                null, Color.White);

#if DEBUG
            DrawDebug(position, spriteBatch);
#endif
        }

        private void DrawDebug(Vector2 position, SpriteBatch spriteBatch)
        {
#if DEBUG
            spriteBatch.Draw(SpriteSheetCollection.Instance.PointTexture, position, null, null, null,
                DefaultRotation + Rotation, null, Color.Green);
            if(CollisionDetection == CollisionDetectionMethod.Circle)
                spriteBatch.Draw(SpriteSheetCollection.Instance.CircleTexture, null,
                    new Rectangle((int) (position.X), (int) (position.Y), (int) Radius*2, (int) Radius*2), null,
                    SpriteSheetCollection.Instance.CircleTexture.Bounds.Size.ToVector2()/2, DefaultRotation + Rotation);
            if(CollisionDetection == CollisionDetectionMethod.Rectangle)
                spriteBatch.Draw(SpriteSheetCollection.Instance.PointTexture, null, GetRectangleWithOrigin(position), null,
                    null, 0, null, Color.FromNonPremultiplied(100, 255, 100, 125));
            
#endif
        }

        public Rectangle GetRectangle(Vector2 position)
        {
            return new Rectangle((int)position.X, (int)position.Y, (int)Size.X, (int)Size.Y);
            //var rotation = Matrix.CreateRotationZ(DefaultRotation + Rotation);
            //var translateTo = Matrix.CreateTranslation(new Vector3(position, 0));
            //var translateBack = Matrix.CreateTranslation(-new Vector3(position, 0));
            //var combined = translateBack * rotation * translateTo;
            //var topLeft = Vector3.Transform(new Vector3(position.X, position.Y + Size.Y, 0), combined);
            //var downRight = Vector3.Transform(new Vector3(position.X + Size.X, position.Y, 0), combined);
            //return new Rectangle((int)topLeft.X, (int)topLeft.Y, (int)(downRight.X - topLeft.X), (int)(downRight.Y - topLeft.Y));
        }

        public Rectangle GetRectangleWithOrigin(Vector2 position)
        {
            //return new Rectangle((int)(position.X -Origin.X), (int)(position.Y-Origin.Y), (int)Size.X, (int)Size.Y);\


            var rotation = Matrix.CreateRotationZ(DefaultRotation + Rotation);
            var translateTo = Matrix.CreateTranslation(new Vector3(position, 0));
            var translateBack = Matrix.CreateTranslation(-new Vector3(position, 0));
            var combined = translateBack * rotation * translateTo;
            var topLeft = Vector3.Transform(new Vector3(position.X - Origin.X, position.Y -Origin.Y+ Size.Y, 0), combined);
            var downRight = Vector3.Transform(new Vector3(position.X- Origin.X + Size.X, position.Y- Origin.Y, 0), combined);
            return new Rectangle((int)topLeft.X, (int)topLeft.Y, (int)(downRight.X - topLeft.X), (int)(downRight.Y - topLeft.Y));
            
        }
        /// <summary>
        /// Returns true, if current sprite in position1 intersects with secondSprite in position2
        /// </summary>
        /// <param name="secondSprite"></param>
        /// <param name="position1">Position of current sprite</param>
        /// <param name="position2">Position of second sprite</param>
        /// <returns></returns>
        public bool Intersects(Sprite secondSprite, Vector2 position1, Vector2 position2)
        {
            if (CollisionDetection == CollisionDetectionMethod.Circle &&
                secondSprite.CollisionDetection == CollisionDetectionMethod.Circle)
            {
                return Vector2.DistanceSquared(position1, position2) <= Math.Pow(Radius + secondSprite.Radius,2);
            }
            if(CollisionDetection == CollisionDetectionMethod.Rectangle &&
               secondSprite.CollisionDetection == CollisionDetectionMethod.Circle)
            {
                return Intersects(GetRectangleWithOrigin(position1), position2, secondSprite.Radius);
            }
            if (CollisionDetection == CollisionDetectionMethod.Circle &&
                secondSprite.CollisionDetection == CollisionDetectionMethod.Rectangle)
            {
                return Intersects(secondSprite.GetRectangleWithOrigin(position2), position1, Radius);
            }
            return GetRectangleWithOrigin(position1).Intersects(secondSprite.GetRectangleWithOrigin(position2));
        }

        /// <summary>
        /// Checks if rectangle intersects with circle
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private bool Intersects(Rectangle rect, Vector2 circle, float radius)
        {
            // Source: http://stackoverflow.com/questions/401847/circle-rectangle-collision-detection-intersection/402010#402010
            //circleDistance.x = abs(circle.x - rect.x);
            //circleDistance.y = abs(circle.y - rect.y);

            //if (circleDistance.x > (rect.width / 2 + circle.r)) { return false; }
            //if (circleDistance.y > (rect.height / 2 + circle.r)) { return false; }

            //if (circleDistance.x <= (rect.width / 2)) { return true; }
            //if (circleDistance.y <= (rect.height / 2)) { return true; }

            //cornerDistance_sq = (circleDistance.x - rect.width / 2) ^ 2 +
            //                     (circleDistance.y - rect.height / 2) ^ 2;

            //return (cornerDistance_sq <= (circle.r ^ 2));

            Vector2 circleDistance = new Vector2(Math.Abs(circle.X - rect.X - rect.Width/2), Math.Abs(circle.Y - rect.Y - rect.Height/2));
            if (circleDistance.X > rect.Width/2f + radius) return false;
            if (circleDistance.Y > rect.Height/2f + radius) return false;

            if (circleDistance.X <= rect.Width/2f) return true;
            if (circleDistance.Y <= rect.Height/2f) return true;

            double cornerDistSq = Math.Pow(circleDistance.X - rect.X/2f, 2) +
                                  Math.Pow(circleDistance.Y - rect.Height/2f, 2);
            return cornerDistSq <= Math.Pow(radius, 2);

        }
        
    }
}
