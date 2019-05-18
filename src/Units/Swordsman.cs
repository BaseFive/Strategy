using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Strategy
{
    public class Swordsman : Soldier
    {
        public Swordsman(Texture2D spriteSheet, Vector2 pos) : base(spriteSheet, pos)
        {
            soldierType = SoldierType.Swordsman;
            HP = 20;
            MaxHP = HP;
            speed = 2;
            attack = 7;
            armor = 2;
            attack_interval = 1.5f;
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

            target.HP -= attack;
            time_since_last_attack = 0;
        }
    }
}