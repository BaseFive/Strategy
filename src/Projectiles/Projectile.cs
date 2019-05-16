using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Strategy
{
    public abstract class Projectile : MovableObject
    {
        protected float flight_distance;

        public Projectile(Texture2D spriteSheet, Vector2 pos) : base(spriteSheet, pos)
        {
            hit_range = 10;
        }

        public void Initialize(Vector2 vel, int attack, Unit target)
        {
            this.vel = vel;
            this.attack = attack;
            this.target = target;
        }

        public override void Update(Computer p2, GameTime gameTime) { }

        public override void Update()
        {
            UpdatePosition();
            if (target != null && range.Intersects(target.collider))
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
            range = new Circle(new Vector2(pos.X + Width / 2, pos.Y + Height / 2), hit_range);
            rect = new Rectangle((int)pos.X, (int)pos.Y, Width, Height);
            collider = new Square((int)pos.X, rect.Bottom - Width, Width);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet, pos, Color.White);
        }
    }
}