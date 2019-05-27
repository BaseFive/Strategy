using System.Collections.Generic;
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
        protected float sight_radius;
        protected float hit_range;
        public bool isDead;
        public Unit? target;
        public List<Projectile> projectiles;

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

        protected Circle Sight
        {
            get { return new Circle(new Vector2(pos.X + Width / 2, pos.Y + Height / 2), sight_radius); }
        }

        protected Vector2 Fire_Position
        {
            get { return new Vector2(pos.X + Width / 2, pos.Y + Height / 4); }
            set { }
        }

        public Entity(Texture2D spriteSheet, Vector2 pos)
        {
            this.spriteSheet = spriteSheet;
            Width = spriteSheet.Width;
            Height = spriteSheet.Height;
            this.pos = pos;
            isDead = false;
            target = null;
            time_since_last_attack = 0;
            projectiles = new List<Projectile>();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet, Rectangle, Color.White);

            if (isSelected || HP < MaxHP)
                DrawHealthBar(spriteBatch);
        }

        public void UpdateProjectiles()
        {
            foreach (Projectile proj in projectiles)
                proj.Update();

            for (int i = 0; i < projectiles.Count; i++)
                if (projectiles[i].isDead)
                    projectiles.Remove(projectiles[i]);
        }

        public void DrawHealthBar(SpriteBatch spriteBatch)
        {
            int greenBarWidth = (int)((HP / MaxHP) * 16);
            int xPos = (int)pos.X + Width / 2 - HUD.HP_Bar_Green.Width / 2;
            spriteBatch.Draw(HUD.HP_Bar_Green, new Rectangle(xPos, (int)pos.Y - 4, greenBarWidth, 2), Color.White);
            spriteBatch.Draw(HUD.HP_Bar_Red, new Rectangle(xPos + greenBarWidth, (int)pos.Y - 4, 16 - greenBarWidth, 2), Color.White);
        }
    }
}