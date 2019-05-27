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
            HP = 40;
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
            if (target.isDead || (projectiles.Count > 0 && target.HP - projectiles.Count * attack <= 0))
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
            Vector2 direction = target.Centre - Fire_Position;
            float distance = direction.Length();
            Vector2 velocity = (direction / distance) * arrow_speed;
            float rotation = (float)System.Math.Atan(direction.Y / direction.X);

            if (target.Centre.Y <= Fire_Position.Y && target.Centre.X <= Fire_Position.X)
                rotation += 3.142f;
            else if (target.Centre.Y >= Fire_Position.Y && target.Centre.X <= Fire_Position.X)
                rotation -= 3.142f;

            projectiles.Add(new Arrow(HUD.arrow, Fire_Position));
            projectiles[projectiles.Count - 1].Initialize(velocity, attack, target, rotation);
            time_since_last_attack = 0;
        }
    }
}