using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.GameSystem
{
    public interface IGameObject
    {
        string Tag { get; set; }
        int GetHashCode();
    }
    public interface IUpdatableGameObject :IGameObject
    {
        void Update(GameTime gameTime);
    }

    public interface IMovableGameObject : IUpdatableGameObject
    {
        Vector2 Velocity { get; set; }
        Vector2 Position { get; set; }
        
    }
    public interface IDrawableGameObject : IGameObject
    {
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}