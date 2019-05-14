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
        public Vector2 pos, vel;
        public Vector2 origin, destination;
        public UnitType unitType;
        public bool isSelected;
        public float HP, MaxHP;
        public int attack, armor;
        public float speed;
        public bool isDead;
        public Unit? target;

        public Unit(Texture2D spriteSheet, Vector2 pos)
        {
            this.spriteSheet = spriteSheet;
            Width = spriteSheet.Width;
            Height = spriteSheet.Height;
            this.pos = pos;
            destination = pos;
            rect = new Rectangle((int)pos.X, (int)pos.Y, Width, Height);
            isDead = false;
            target = null;
        }

        protected abstract void Attack(Unit unit);

        protected void GoToDestination()
        {
            if (pos == destination)
                return;

            //Prevent unit speed from doubling when moving diagonally
            if (pos.X < destination.X - speed + 1 && pos.Y < destination.Y - speed + 1)
            {
                vel.X = speed / 2f;
                vel.Y = speed / 2f;
            }

            else if (pos.X < destination.X - speed + 1 && pos.Y > destination.Y + speed - 1)
            {
                vel.X = speed / 2f;
                vel.Y = -speed / 2f;
            }

            else if (pos.X > destination.X + speed - 1 && pos.Y < destination.Y - speed + 1)
            {
                vel.X = -speed / 2f;
                vel.Y = speed / 2f;
            }

            else if (pos.X > destination.X + speed - 1 && pos.Y > destination.Y + speed - 1)
            {
                vel.X = -speed / 2f;
                vel.Y = -speed / 2f;
            }

            else
            {
                //Handle single direction movement
                if (pos.X < destination.X - speed + 1)
                    vel.X = speed;

                else if (pos.X > destination.X + speed - 1)
                    vel.X = -speed;

                else vel.X = 0;

                if (pos.Y < destination.Y - speed + 1)
                    vel.Y = speed;

                else if (pos.Y > destination.Y + speed - 1)
                    vel.Y = -speed;

                else vel.Y = 0;
            }

            UpdatePosition();
        }

        protected void FollowUnit(Unit unit)
        {
            //Temporarily follows the same process as GoToDestination()
            destination = unit.pos;

            if (pos == destination)
                return;

            if (pos.X < destination.X - speed + 1 && pos.Y < destination.Y - speed + 1)
            {
                vel.X = speed / 2f;
                vel.Y = speed / 2f;
            }

            else if (pos.X < destination.X - speed + 1 && pos.Y > destination.Y + speed - 1)
            {
                vel.X = speed / 2f;
                vel.Y = -speed / 2f;
            }

            else if (pos.X > destination.X + speed - 1 && pos.Y < destination.Y - speed + 1)
            {
                vel.X = -speed / 2f;
                vel.Y = speed / 2f;
            }

            else if (pos.X > destination.X + speed - 1 && pos.Y > destination.Y + speed - 1)
            {
                vel.X = -speed / 2f;
                vel.Y = -speed / 2f;
            }

            else
            {
                if (pos.X < destination.X - speed + 1)
                    vel.X = speed;

                else if (pos.X > destination.X + speed - 1)
                    vel.X = -speed;

                else vel.X = 0;

                if (pos.Y < destination.Y - speed + 1)
                    vel.Y = speed;

                else if (pos.Y > destination.Y + speed - 1)
                    vel.Y = -speed;

                else vel.Y = 0;
            }

            UpdatePosition();
        }



        public abstract void Update();
        public abstract void Update(Computer p2);
        public abstract void Draw(SpriteBatch spriteBatch);

        #region Helper Functions
        protected void UpdatePosition()
        {
            pos += vel;
            rect = new Rectangle((int)pos.X, (int)pos.Y, Width, Height);
        }

        public bool IsMoving()
        {
            return vel != Vector2.Zero;
        }

        protected void CheckHP()
        {
            if (HP <= 0)
                isDead = true;
        }
        #endregion
    }
}