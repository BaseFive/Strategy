using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Strategy
{
    public enum UnitType { Villager, Soldier };

    public abstract class Unit
    {
        protected Texture2D spriteSheet;
        public Rectangle rect { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public Vector2 pos, destination;
        public UnitType unitType;
        public bool isSelected;
        public float HP, MaxHP;
        public int attack, armor;
        public float speed;
        public bool isDead;

        public Unit(Texture2D spriteSheet, Vector2 pos)
        {
            this.spriteSheet = spriteSheet;
            Width = spriteSheet.Width;
            Height = spriteSheet.Height;
            this.pos = pos;
            destination = pos;
            rect = new Rectangle((int)pos.X, (int)pos.Y, Width, Height);
            isDead = false;
        }

        protected void GoToDestination()
        {
            if (pos == destination)
                return;

            //Prevent unit speed from doubling when moving diagonally
            if (pos.X < destination.X - speed + 1 && pos.Y < destination.Y - speed + 1)
            {
                pos.X += speed / 2f;
                pos.Y += speed / 2f;
            }

            else if (pos.X < destination.X - speed + 1 && pos.Y > destination.Y + speed - 1)
            {
                pos.X += speed / 2f;
                pos.Y -= speed / 2f;
            }

            else if (pos.X > destination.X + speed - 1 && pos.Y < destination.Y - speed + 1)
            {
                pos.X -= speed / 2f;
                pos.Y += speed / 2f;
            }

            else if (pos.X > destination.X + speed - 1 && pos.Y > destination.Y + speed - 1)
            {
                pos.X -= speed / 2f;
                pos.Y -= speed / 2f;
            }

            else
            {
                //Handle single direction movement
                if (pos.X < destination.X - speed + 1)
                    pos.X += speed;

                else if (pos.X > destination.X + speed - 1)
                    pos.X -= speed;

                if (pos.Y < destination.Y - speed + 1)
                    pos.Y += speed;

                else if (pos.Y > destination.Y + speed - 1)
                    pos.Y -= speed;
            }

            rect = new Rectangle((int)pos.X, (int)pos.Y, Width, Height);
        }

        protected void CheckHP()
        {
            if (HP <= 0)
                isDead = true;
        }

        public abstract void Update();
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}