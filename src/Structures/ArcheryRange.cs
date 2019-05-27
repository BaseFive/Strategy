using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Strategy
{
    public class ArcheryRange : Structure
    {
        Rectangle archer_rect;

        MouseState oldMouse;

        public ArcheryRange(Texture2D spriteSheet, Vector2 pos) : base(spriteSheet, pos)
        {
            ID = BuildingID.ArcheryRange;
            HP = 200;
            MaxHP = HP;

            archer_rect = new Rectangle(10, 410, 40, 40);

            oldMouse = Mouse.GetState();
        }

        public override void Update(Player player, Computer p2, GameTime gameTime)
        {
            MouseState newMouse = Mouse.GetState();

            if (oldMouse.LeftButton == ButtonState.Released && newMouse.LeftButton == ButtonState.Pressed &&
                isSelected && archer_rect.Contains(newMouse.Position))
            {
                Archer temp = new Archer(HUD.archer, new Vector2(300, new System.Random().Next() % 380));
                player.units.Add(temp);
                player.soldiers.Add(temp);
            }

            oldMouse = newMouse;
        }
    }
}