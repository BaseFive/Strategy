using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Strategy
{
    public abstract class Projectile : MovableObject
    {
        protected float flight_distance;
        public float rotation;

        public Projectile(Texture2D spriteSheet, Vector2 pos) : base(spriteSheet, pos)
        {
            hit_range = 10;
            rotation = 0;
        }

        public void Initialize(Vector2 vel, int attack, Unit target, float rotation)
        {
            this.vel = vel;
            this.attack = attack;
            this.target = target;
            this.rotation = rotation;
        }

        public override void Update(Computer p2, GameTime gameTime) { }

        public override void Update()
        {
            UpdatePosition();
            if (target != null && Range.Intersects(target.Collider))
            {
                target.HP -= attack;
                isDead = true;
            }

            if ((pos - origin).Length() > flight_distance)
                isDead = true;
        }

        protected override void UpdatePosition()
        {
            pos += vel;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle srcRect = new Rectangle(0, 0, Width, Height);
            spriteBatch.Draw(spriteSheet, Rectangle, srcRect, Color.White, rotation, Vector2.Zero, SpriteEffects.None, 1f);
        }
    }
}