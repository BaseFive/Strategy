using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Strategy
{
    public enum Stance { Attack, Defense, StandGround, NoAttack };
    public enum SoldierType { Swordsman, Archer };

    public abstract class Soldier : Unit
    {
        public Stance stance { get; protected set; }
        public SoldierType soldierType { get; protected set; }
        public Rectangle stance_rect { get; protected set; }
        protected Rectangle attack_rect, defense_rect, standGround_rect, noAttack_rect;

        public Soldier(Texture2D spriteSheet, Vector2 pos) : base(spriteSheet, pos)
        {
            unitType = UnitType.Soldier;
            stance = Stance.Attack;

            attack_rect = new Rectangle(10, 410, 40, 40);
            defense_rect = new Rectangle(50, 410, 40, 40);
            standGround_rect = new Rectangle(90, 410, 40, 40);
            noAttack_rect = new Rectangle(130, 410, 40, 40);
        }

        public override void Update()
        {
            //Update based on stance
            switch (stance)
            {
                case Stance.Attack:
                    stance_rect = new Rectangle(0, 0, 160, 40);
                    break;

                case Stance.Defense:
                    stance_rect = new Rectangle(0, 40, 160, 40);
                    break;

                case Stance.StandGround:
                    stance_rect = new Rectangle(0, 80, 160, 40);
                    break;

                case Stance.NoAttack:
                    stance_rect = new Rectangle(0, 120, 160, 40);
                    break;
            }

            //Update stance
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && isSelected)
            {
                if (attack_rect.Contains(Mouse.GetState().Position)) stance = Stance.Attack;
                else if (defense_rect.Contains(Mouse.GetState().Position)) stance = Stance.Defense;
                else if (standGround_rect.Contains(Mouse.GetState().Position)) stance = Stance.StandGround;
                else if (noAttack_rect.Contains(Mouse.GetState().Position)) stance = Stance.NoAttack;
            }

            GoToDestination();
            CheckHP();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet, pos, Color.White);

            if (isSelected)
            {
                int greenBarWidth = (int)((HP / MaxHP) * 16);
                spriteBatch.Draw(HUD.HP_Bar_Green, new Rectangle((int)pos.X - 3, (int)pos.Y - 4, greenBarWidth, 2), Color.White);
                spriteBatch.Draw(HUD.HP_Bar_Red, new Rectangle((int)pos.X - 3 + greenBarWidth, (int)pos.Y - 4, 16 - greenBarWidth, 2), Color.White);
            }
        }
    }
}