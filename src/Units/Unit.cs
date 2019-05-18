using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Strategy
{
    public enum UnitType { Villager, Soldier };

    public abstract class Unit : MovableObject
    {
        public Vector2 destination;
        public UnitType unitType;
        public int armor;

        public Unit(Texture2D spriteSheet, Vector2 pos) : base(spriteSheet, pos)
        {
            destination = pos;
        }

        protected abstract void Attack(GameTime gameTime);

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

        #region Helper Functions
        public bool IsAtUnit(Unit unit)
        {
            return Range.Intersects(unit.Collider);
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