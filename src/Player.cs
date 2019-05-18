﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
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
        static Rectangle selection;

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

        static void HandleMouseInput(Computer p2)
        {
            //Only handle clicks inside the main game viewport
            if (!Strategy.viewport.Contains(Mouse.GetState().Position))
                return;

            MouseState newMouse = Mouse.GetState();

            //Toggle selection of units
            if (oldMouse.LeftButton == ButtonState.Released && newMouse.LeftButton == ButtonState.Pressed)
            {
                selection = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 0, 0);
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
                    if (soldier.Rectangle.Contains(Mouse.GetState().Position))
                    {
                        selectedSoldiers.Add(soldier);
                        selectedUnits.Add(soldier);
                        soldier.isSelected = true;
                        break;
                    }
            }

            //Select multiple units by dragging mouse while holding LMB
            else if (oldMouse.LeftButton == ButtonState.Pressed && newMouse.LeftButton == ButtonState.Pressed)
            {
                selection.Width = Mouse.GetState().X - selection.X;
                selection.Height = Mouse.GetState().Y - selection.Y;
            }

            else if (oldMouse.LeftButton == ButtonState.Pressed && newMouse.LeftButton == ButtonState.Released)
            {
                foreach (Soldier soldier in soldiers)
                {
                    if (selection.Contains(soldier.pos))
                    {
                        selectedSoldiers.Add(soldier);
                        selectedUnits.Add(soldier);
                        soldier.isSelected = true;
                    }
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
                    unit.origin = unit.destination;

                    foreach (Unit enemy in p2.units)
                        if (enemy.Rectangle.Contains(Mouse.GetState().Position))
                            unit.target = enemy;
                }
            }

            oldMouse = newMouse;
        }

        static void HandleKeyboardInput()
        {
            //Kill units with <DELETE>
            if (Keyboard.GetState().IsKeyDown(Keys.Delete) && selectedUnits.Count > 0)
                selectedUnits[new Random().Next(selectedUnits.Count)].isDead = true;
        }

        public static void UpdateUnits(Computer p2, GameTime gameTime)
        {
            #region For Testing
            KeyboardState newKeyboard = Keyboard.GetState();
            #region SpawnUnit
            if (oldKeyboard.IsKeyUp(Keys.Z) && newKeyboard.IsKeyDown(Keys.Z))
            {
                Swordsman temp = new Swordsman(HUD.swordsman, new Vector2(new Random().Next() % 380, new Random().Next() % 380));
                units.Add(temp);
                soldiers.Add(temp);
            }
            if (oldKeyboard.IsKeyUp(Keys.A) && newKeyboard.IsKeyDown(Keys.A))
            {
                Archer temp = new Archer(HUD.archer, new Vector2(new Random().Next() % 380, new Random().Next() % 380));
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

            HandleMouseInput(p2);
            HandleKeyboardInput();

            foreach (Unit unit in units)
                unit.Update(p2, gameTime);

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