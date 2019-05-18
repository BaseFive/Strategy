using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Strategy
{
    public abstract class Entity
    {
        protected Texture2D spriteSheet;
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public Vector2 pos;
        public int attack;
        public float attack_interval, time_since_last_attack;
        public bool isSelected;
        public float HP, MaxHP;
        protected float hit_range;
        public bool isDead;
        public Unit? target;

        public Vector2 Centre
        {
            get { return new Vector2(pos.X + Width / 2, pos.Y + Height / 2); }
        }

        public Circle Range
        {
            get { return new Circle(Centre, hit_range); }
        }

        public Rectangle Rectangle
        {
            get { return new Rectangle((int)pos.X, (int)pos.Y, Width, Height); }
        }

        public Square Collider
        {
            get { return new Square((int)pos.X, Rectangle.Bottom - Width, Width); }
        }

        public Entity(Texture2D spriteSheet, Vector2 pos)
        {
            this.spriteSheet = spriteSheet;
            Width = spriteSheet.Width;
            Height = spriteSheet.Height;
            this.pos = pos;
            isDead = false;
            target = null;
        }
    }
}