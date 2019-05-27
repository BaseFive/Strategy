using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Strategy
{
    public class Barracks : Structure
    {
        Rectangle swordsman_rect;

        MouseState oldMouse;

        public Barracks(Texture2D spriteSheet, Vector2 pos) : base(spriteSheet, pos)
        {
            ID = BuildingID.Barracks;
            HP = 200;
            MaxHP = HP;

            swordsman_rect = new Rectangle(10, 410, 40, 40);

            oldMouse = Mouse.GetState();
        }

        public override void Update(Player player, Computer p2, GameTime gameTime)
        {
            MouseState newMouse = Mouse.GetState();

            if (oldMouse.LeftButton == ButtonState.Released && newMouse.LeftButton == ButtonState.Pressed &&
                isSelected && swordsman_rect.Contains(newMouse.Position))
            {
                Swordsman temp = new Swordsman(HUD.swordsman, new Vector2(330, new System.Random().Next() % 380));
                player.units.Add(temp);
                player.soldiers.Add(temp);
            }

            oldMouse = newMouse;
        }
    }
}