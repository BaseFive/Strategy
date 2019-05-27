using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Strategy
{
    public class Player
    {
        public List<Unit> units;
        public List<Unit> selectedUnits;
        public List<Villager> villagers;
        public List<Soldier> soldiers;
        public List<Structure> structures;
        public List<Structure> selectedStructures;
        Rectangle selection;

        //Keyboard and mouse states used for single presses and clicks
        KeyboardState oldKeyboard;
        MouseState oldMouse;

        public Player()
        {
            units = new List<Unit>();
            selectedUnits = new List<Unit>();
            villagers = new List<Villager>();
            soldiers = new List<Soldier>();
            structures = new List<Structure>();
            selectedStructures = new List<Structure>();

            oldKeyboard = Keyboard.GetState();
            oldMouse = Mouse.GetState();
        }

        void HandleMouseInput(Computer p2)
        {
            //Only handle clicks inside the main game viewport
            if (!Strategy.viewport.Contains(Mouse.GetState().Position))
                return;

            MouseState newMouse = Mouse.GetState();

            //Toggle selection of units
            if (oldMouse.LeftButton == ButtonState.Released && newMouse.LeftButton == ButtonState.Pressed)
            {
                selection = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 0, 0);
                selectedStructures.Clear();

                //Reset unit selection if <CTRL> key is not held
                if (Keyboard.GetState().IsKeyUp(Keys.LeftControl) && Keyboard.GetState().IsKeyUp(Keys.RightControl))
                {
                    selectedUnits.Clear();

                    foreach (Unit unit in units)
                        unit.isSelected = false;

                    foreach (Structure structure in structures)
                        structure.isSelected = false;
                }

                //Add unit to selected group
                foreach (Unit unit in units)
                    if (unit.Rectangle.Contains(Mouse.GetState().Position))
                    {
                        selectedUnits.Add(unit);
                        unit.isSelected = true;
                        break;
                    }

                foreach (Structure structure in structures)
                    if (structure.Rectangle.Contains(Mouse.GetState().Position))
                    {
                        selectedStructures.Add(structure);
                        structure.isSelected = true;
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
                foreach (Unit unit in units)
                    if (selection.Contains(unit.pos))
                    {
                        selectedUnits.Add(unit);
                        unit.isSelected = true;
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

        void HandleKeyboardInput()
        {
            //Remove units/structures with <DELETE>
            if (Keyboard.GetState().IsKeyDown(Keys.Delete))
            {
                if (selectedUnits.Count > 0)
                    selectedUnits[new Random().Next(selectedUnits.Count)].isDead = true;
                if (selectedStructures.Count > 0)
                    selectedStructures[new Random().Next(selectedStructures.Count)].isDead = true;
            }
        }

        public void UpdateUnits(Computer p2, GameTime gameTime)
        {
            #region For Testing
            KeyboardState newKeyboard = Keyboard.GetState();
            #region Spawn Unit/Structure
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
            if (oldKeyboard.IsKeyUp(Keys.C) && newKeyboard.IsKeyDown(Keys.C) && structures.Count == 0)
            {
                GuardTower temp = new GuardTower(HUD.blue_guard_tower, new Vector2(300, 100));
                structures.Add(temp);
            }
            if (oldKeyboard.IsKeyUp(Keys.V) && newKeyboard.IsKeyDown(Keys.V) && villagers.Count == 0)
            {
                Villager temp = new Villager(HUD.villager, new Vector2(100));
                units.Add(temp);
                villagers.Add(temp);
            }
            #endregion
            #region ReduceHealth
            foreach (Unit unit in selectedUnits)
                if (oldKeyboard.IsKeyUp(Keys.K) && newKeyboard.IsKeyDown(Keys.K))
                    unit.HP--;
            #endregion
            oldKeyboard = newKeyboard;
            #endregion

            HandleMouseInput(p2);
            HandleKeyboardInput();

            foreach (Soldier soldier in soldiers)
                soldier.Update(p2, gameTime);
            foreach (Villager villager in villagers)
                villager.Update(this, gameTime);
            RemoveDeadUnits();

            foreach (Structure structure in structures)
                structure.Update(this, p2, gameTime);
            RemoveDestroyedStructures();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Structure structure in structures)
                structure.Draw(spriteBatch);

            foreach (Unit unit in units)
                unit.Draw(spriteBatch);
        }

        void RemoveDeadUnits()
        {
            for (int i = 0; i < units.Count; i++)
                if (units[i].isDead)
                {
                    selectedUnits.Remove(units[i]);
                    units.Remove(units[i]);
                }
        }

        void RemoveDestroyedStructures()
        {
            for (int i = 0; i < structures.Count; i++)
                if (structures[i].isDead)
                {
                    selectedStructures.Remove(structures[i]);
                    structures.Remove(structures[i]);
                }
        }
    }
}