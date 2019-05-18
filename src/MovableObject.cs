using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Strategy
{
    public abstract class MovableObject : Entity
    {
        public Vector2 vel, origin;
        public float speed;

        public MovableObject(Texture2D spriteSheet, Vector2 pos) : base(spriteSheet, pos)
        {
            origin = pos;
        }

        public abstract void Update();
        public abstract void Update(Computer p2, GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        protected virtual void UpdatePosition()
        {
            pos += vel;
        }
    }
}