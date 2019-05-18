using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Strategy
{
    public class Archer : Soldier
    {
        int arrow_speed;

        public Archer(Texture2D spriteSheet, Vector2 pos) : base(spriteSheet, pos)
        {
            soldierType = SoldierType.Archer;
            HP = 20;
            MaxHP = HP;
            speed = 2;
            attack = 5;
            armor = 2;
            attack_interval = 1;
            hit_range = 100;
            arrow_speed = 4;
        }

        protected override void Attack(GameTime gameTime)
        {
            if (target.isDead)
            {
                target = null;
                if (stance == Stance.Defensive)
                    destination = origin;
                else
                {
                    destination = pos;
                    vel = Vector2.Zero;
                }
                return;
            }

            //Calculate direction, velocity and rotation of arrows
            Vector2 direction = target.Centre - Centre;
            float distance = direction.Length();
            Vector2 velocity = (direction / distance) * arrow_speed;
            float rotation = (float)System.Math.Atan(direction.Y / direction.X);

            if (target.Centre.Y <= pos.Y && target.Centre.X <= pos.X)
                rotation += 3.142f;
            else if (target.Centre.Y >= pos.Y && target.Centre.X <= pos.X)
                rotation -= 3.142f;

            projectiles.Add(new Arrow(HUD.arrow, Centre));
            projectiles[projectiles.Count - 1].Initialize(velocity, attack, target, rotation);
            time_since_last_attack = 0;
        }
    }
}