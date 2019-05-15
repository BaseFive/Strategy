using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Strategy
{
    public class Swordsman : Soldier
    {
        public Swordsman(Texture2D spriteSheet, Vector2 pos) : base(spriteSheet, pos)
        {
            soldierType = SoldierType.Swordsman;
            sightRadius = 200;
            HP = 20;
            MaxHP = HP;
            speed = 2;
            attack = 3;
            armor = 2;
            attackSpeed = 2;
        }

        protected override void Attack(Unit unit)
        {
            //destination = pos;
            unit.HP -= 0.05f * attack;

            if (unit.isDead)
            {
                target = null;
                if (stance == Stance.Defensive)
                    destination = origin;
            }
        }
    }
}