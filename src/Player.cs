using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Strategy
{
    public static class Player
    {
        public static List<Unit> units;
        public static List<Soldier> soldiers;
        public static List<Unit> selectedUnits;
        public static List<Soldier> selectedSoldiers;
        public static Texture2D soldier, stances;

        //Keyboard and mouse states used for single presses and clicks
        static KeyboardState oldKeyboard;
        static MouseState oldMouse;

        public static void Initialize()
        {
            units = new List<Unit>();
            selectedUnits = new List<Unit>();
            soldiers = new List<Soldier>();
            selectedSoldiers = new List<Soldier>();

            oldKeyboard = Keyboard.GetState();
            oldMouse = Mouse.GetState();
        }

        public static void LoadContent(ContentManager Content)
        {
            soldier = Content.Load<Texture2D>("Soldiers/Soldier");
            stances = Content.Load<Texture2D>("HUD/Stances");
        }

        static void HandleMouseInput()
        {
            //Only handle clicks inside the main game viewport
            if (!Game1.viewport.Contains(Mouse.GetState().Position))
                return;

            MouseState newMouse = Mouse.GetState();

            //Toggle selection of units
            if (oldMouse.LeftButton == ButtonState.Released && newMouse.LeftButton == ButtonState.Pressed)
            {
                //Reset selection if <CTRL> key is not held
                if (Keyboard.GetState().IsKeyUp(Keys.LeftControl) && Keyboard.GetState().IsKeyUp(Keys.RightControl))
                {
                    selectedUnits.Clear();
                    selectedSoldiers.Clear();

                    foreach (Unit unit in units)
                        unit.isSelected = false;
                }

                //Add unit to selected group
                foreach (Soldier soldier in soldiers)
                    if (soldier.rect.Contains(Mouse.GetState().Position))
                    {
                        selectedSoldiers.Add(soldier);
                        selectedUnits.Add(soldier);
                        soldier.isSelected = true;
                        break;
                    }
            }

            //Tell units where to go
            if (oldMouse.RightButton == ButtonState.Released && newMouse.RightButton == ButtonState.Pressed)
            {
                foreach (Unit unit in selectedUnits)
                {
                    unit.target = null;
                    unit.vel = Vector2.One;
                    unit.destination = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                }
            }

            oldMouse = newMouse;
        }

        static void HandleKeyboardInput()
        {
            //Kill units with <DELETE>
            if (Keyboard.GetState().IsKeyDown(Keys.Delete) && selectedUnits.Count > 0)
                selectedUnits[new Random().Next() % selectedUnits.Count].isDead = true;
        }

        public static void UpdateUnits(Computer p2)
        {
            #region For Testing
            KeyboardState newKeyboard = Keyboard.GetState();
            #region SpawnUnit
            if (oldKeyboard.IsKeyUp(Keys.Z) && newKeyboard.IsKeyDown(Keys.Z))
            {
                Swordsman temp = new Swordsman(soldier, new Vector2(new Random().Next() % 380, new Random().Next() % 380));
                units.Add(temp);
                soldiers.Add(temp);
            }
            #endregion
            #region ReduceHealth
            foreach (Unit unit in selectedUnits)
                if (oldKeyboard.IsKeyUp(Keys.K) && newKeyboard.IsKeyDown(Keys.K))
                    unit.HP--;
            #endregion
            oldKeyboard = newKeyboard;
            #region MoveEnemyUnit
            if (p2.units.Count > 0 && Keyboard.GetState().IsKeyDown(Keys.Space))
                p2.units[0].destination = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            #endregion
            #endregion

            HandleMouseInput();
            HandleKeyboardInput();

            foreach (Unit unit in units)
                unit.Update(p2);

            //Remove and disselect dead soldiers
            for (int i = 0; i < soldiers.Count; i++)
                if (soldiers[i].isDead)
                {
                    selectedUnits.Remove(soldiers[i]);
                    units.Remove(soldiers[i]);
                    selectedSoldiers.Remove(soldiers[i]);
                    soldiers.Remove(soldiers[i]);
                }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Unit unit in units)
                unit.Draw(spriteBatch);
        }
    }
}