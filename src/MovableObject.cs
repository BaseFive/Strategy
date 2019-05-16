using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Strategy
{
    public abstract class MovableObject : Entity
    {
        public Vector2 vel, origin, centre;
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
            centre = new Vector2(pos.X + Width / 2, pos.Y + Height / 2);
            range = new Circle(new Vector2(collider.X + collider.Width / 2, collider.Y + collider.Width / 2), hit_range);
            rect = new Rectangle((int)pos.X, (int)pos.Y, Width, Height);
            collider = new Square((int)pos.X, rect.Bottom - Width, Width);
        }
    }
}