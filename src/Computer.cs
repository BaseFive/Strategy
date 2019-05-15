using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Strategy
{
    //Temporary class for testing units against enemies
    public class Computer
    {
        public List<Unit> units;
        public List<Soldier> soldiers;
        public Texture2D soldier, stances;

        KeyboardState oldKeyboard;

        public Computer()
        {
            units = new List<Unit>();
            soldiers = new List<Soldier>();

            oldKeyboard = Keyboard.GetState();
        }

        public void LoadContent(ContentManager Content)
        {
            soldier = Content.Load<Texture2D>("Soldiers/Enemy");
            stances = Content.Load<Texture2D>("HUD/Stances");
        }

        public void UpdateUnits()
        {
            #region SpawnUnit
            KeyboardState newKeyboard = Keyboard.GetState();
            if (oldKeyboard.IsKeyUp(Keys.X) && newKeyboard.IsKeyDown(Keys.X))
            {
                Swordsman temp = new Swordsman(soldier, new Vector2(400 + new Random().Next() % 380, new Random().Next() % 380));
                units.Add(temp);
                soldiers.Add(temp);
            }
            oldKeyboard = newKeyboard;
            #endregion

            foreach (Unit unit in units)
                unit.Update();

            //Remove and disselect dead soldiers
            for (int i = 0; i < soldiers.Count; i++)
                if (soldiers[i].isDead)
                {
                    units.Remove(soldiers[i]);
                    soldiers.Remove(soldiers[i]);
                }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Unit unit in units)
            {
                unit.Draw(spriteBatch);
                if (unit.HP < unit.MaxHP)
                    unit.DrawHealthBar(spriteBatch);
            }
        }
    }
}