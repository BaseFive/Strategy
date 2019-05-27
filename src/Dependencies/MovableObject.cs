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

        public virtual void Update() { }

        protected virtual void UpdatePosition()
        {
            pos += vel;
        }
    }
}