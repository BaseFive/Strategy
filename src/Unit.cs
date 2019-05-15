using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Strategy
{
    public enum UnitType { Villager, Soldier };

    public abstract class Unit
    {
        protected Texture2D spriteSheet;
        public Rectangle rect { get; protected set; }
        public Rectangle collider { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public Vector2 pos, vel;
        public Vector2 colliderPos;
        public Vector2 origin, destination;
        public UnitType unitType;
        public bool isSelected;
        public float HP, MaxHP;
        public int attack, armor;
        public float attackSpeed, attackTimer;
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
            collider = new Rectangle((int)pos.X, rect.Bottom - 10, Width, 10);
            colliderPos = new Vector2(collider.X, collider.Y);
            isDead = false;
            target = null;
        }

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

            if (IsAtUnit(unit))
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


        protected abstract void Attack(Unit unit);
        public abstract void Update();
        public abstract void Update(Computer p2);
        public abstract void Draw(SpriteBatch spriteBatch);

        #region Helper Functions
        protected void UpdatePosition()
        {
            pos += vel;
            rect = new Rectangle((int)pos.X, (int)pos.Y, Width, Height);
            collider = new Rectangle((int)pos.X, rect.Bottom - 10, Width, 10);
            colliderPos = new Vector2(collider.X, collider.Y);
        }

        public bool IsAtUnit(Unit unit)
        {
            Vector2 TopLeft = colliderPos;
            Vector2 TopRight = new Vector2(collider.Right, collider.Top);
            Vector2 BottomLeft = new Vector2(collider.Left, collider.Bottom);
            Vector2 BottomRight = new Vector2(collider.Right, collider.Bottom);

            return unit.collider.Contains(TopLeft + vel) || unit.collider.Contains(TopRight + vel) ||
                unit.collider.Contains(BottomLeft + vel) || unit.collider.Contains(BottomRight + vel);
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

        public void DrawHealthBar(SpriteBatch spriteBatch)
        {
            int greenBarWidth = (int)((HP / MaxHP) * 16);
            spriteBatch.Draw(HUD.HP_Bar_Green, new Rectangle((int)pos.X - 3, (int)pos.Y - 4, greenBarWidth, 2), Color.White);
            spriteBatch.Draw(HUD.HP_Bar_Red, new Rectangle((int)pos.X - 3 + greenBarWidth, (int)pos.Y - 4, 16 - greenBarWidth, 2), Color.White);
        }
        #endregion
    }
}