using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Strategy
{
    public class Arrow : Projectile
    {
        public Arrow(Texture2D spriteSheet, Vector2 pos) : base(spriteSheet, pos)
        {
            hit_range = 10;
            flight_distance = 150;
        }
    }

    public class TowerArrow : Projectile
    {
        public TowerArrow(Texture2D spriteSheet, Vector2 pos) : base(spriteSheet, pos)
        {
            hit_range = 10;
            flight_distance = 300;
        }
    }
}