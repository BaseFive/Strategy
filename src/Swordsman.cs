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
            attack = 5;
            armor = 2;
        }
    }
}