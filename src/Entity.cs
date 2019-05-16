using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Strategy
{
    public abstract class Entity
    {
        protected Texture2D spriteSheet;
        public Rectangle rect { get; protected set; }
        public Square collider { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public Vector2 pos;
        public int attack;
        public float attack_interval, time_since_last_attack;
        public bool isSelected;
        public float HP, MaxHP;
        protected Circle range;
        protected float hit_range;
        public bool isDead;
        public Unit? target;

        public Entity(Texture2D spriteSheet, Vector2 pos)
        {
            this.spriteSheet = spriteSheet;
            Width = spriteSheet.Width;
            Height = spriteSheet.Height;
            this.pos = pos;
            rect = new Rectangle((int)pos.X, (int)pos.Y, Width, Height);
            collider = new Square((int)pos.X, rect.Bottom - Width, Width);
            isDead = false;
            target = null;
        }
    }
}