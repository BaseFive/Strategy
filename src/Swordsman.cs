using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Strategy
{
    public class Swordsman : Soldier
    {
        public Swordsman(Texture2D spriteSheet, Vector2 pos) : base(spriteSheet, pos)
        {
            soldierType = SoldierType.Swordsman;
            sightRadius = 100;
            HP = 20;
            MaxHP = HP;
            speed = 2;
            attack = 5;
            armor = 2;
        }

        protected override void Attack(Unit unit)
        {
            //Yet to be implemented
        }
    }
}