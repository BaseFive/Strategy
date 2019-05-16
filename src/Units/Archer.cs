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

        protected override void Attack(Unit target, GameTime gameTime)
        {
            //Calculate direction and velocity of arrow
            Vector2 dir = new Vector2(target.pos.X + target.Width / 2, target.pos.Y + target.Height / 2) - pos;
            float distance = dir.Length();
            Vector2 velocity = (dir / distance) * arrow_speed;

            projectiles.Add(new Arrow(HUD.HP_Bar_Red, pos));
            projectiles[projectiles.Count - 1].Initialize(velocity, attack, target);
            time_since_last_attack = 0;

            if (target.isDead)
            {
                base.target = null;
                if (stance == Stance.Defensive)
                    destination = origin;
                else
                {
                    destination = pos;
                    vel = Vector2.Zero;
                }
            }
        }
    }
}