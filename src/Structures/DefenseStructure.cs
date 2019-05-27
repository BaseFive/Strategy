using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Strategy
{
    public abstract class DefenseStructure : Structure
    {
        public DefenseStructure(Texture2D spriteSheet, Vector2 pos) : base(spriteSheet, pos)
        {

        }

        public abstract void Attack(GameTime gameTime);
    }
}