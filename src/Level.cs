using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Strategy
{
    public class Level
    {
        Texture2D map;

        public void LoadContent(ContentManager Content)
        {
            map = Content.Load<Texture2D>("Maps/Map5");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(map, Vector2.Zero, Color.White);
        }
    }
}