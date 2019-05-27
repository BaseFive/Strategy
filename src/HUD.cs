using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Strategy
{
    public static class HUD
    {
        public static Texture2D Base, Stances, Buildings;
        public static Texture2D infantry, archers;

        public static Texture2D HP_Bar_Green, HP_Bar_Red;

        public static Texture2D arrow;

        public static Texture2D villager;
        public static Texture2D swordsman, enemy_swordsman;
        public static Texture2D archer, enemy_archer;

        public static Texture2D blue_guard_tower, red_guard_tower;

        public static Texture2D blue_barracks, red_barracks;
        public static Texture2D blue_archery_range, red_archery_range;

        public static void LoadContent(ContentManager Content)
        {
            Base = Content.Load<Texture2D>("HUD/HUD_Base");
            Stances = Content.Load<Texture2D>("HUD/Stances");
            Buildings = Content.Load<Texture2D>("HUD/Buildings");

            infantry = Content.Load<Texture2D>("HUD/Infantry");
            archers = Content.Load<Texture2D>("HUD/Archers");

            HP_Bar_Green = Content.Load<Texture2D>("HUD/HP_Bar_Green");
            HP_Bar_Red = Content.Load<Texture2D>("HUD/HP_Bar_Red");

            arrow = Content.Load<Texture2D>("Projectiles/Arrow");

            blue_guard_tower = Content.Load<Texture2D>("Structures/Blue_Guard_Tower");
            red_guard_tower = Content.Load<Texture2D>("Structures/Red_Guard_Tower");

            blue_barracks = Content.Load<Texture2D>("Structures/Blue_Barracks");
            red_barracks = Content.Load<Texture2D>("Structures/Red_Barracks");
            blue_archery_range = Content.Load<Texture2D>("Structures/Blue_Archery_Range");
            red_archery_range = Content.Load<Texture2D>("Structures/Red_Archery_Range");

            villager = Content.Load<Texture2D>("Soldiers/Villager");
            swordsman = Content.Load<Texture2D>("Soldiers/Swordsman");
            archer = Content.Load<Texture2D>("Soldiers/Archer");
            enemy_swordsman = Content.Load<Texture2D>("Soldiers/Enemy_Swordsman");
            enemy_archer = Content.Load<Texture2D>("Soldiers/Enemy_Archer");
        }

        static bool SelectedSoldiersHaveSameStance(Player player)
        {
            Stance temp = Stance.Aggressive;

            foreach (Soldier soldier in player.soldiers)
                if (soldier.isSelected)
                {
                    temp = soldier.stance;
                    break;
                }

            foreach (Soldier soldier in player.soldiers)
                if (soldier.stance != temp)
                    return false;

            return true;
        }

        static bool SelectedUnitsAreSoldiers(Player player)
        {
            int selectedSoldiers = 0;

            foreach (Unit unit in player.selectedUnits)
            {
                selectedSoldiers++;
                if (unit.unitType != UnitType.Soldier)
                    return false;
            }

            return selectedSoldiers != 0;
        }

        static bool SelectedUnitsAreVillagers(Player player)
        {
            int selectedVillagers = 0;

            foreach (Unit unit in player.selectedUnits)
            {
                selectedVillagers++;
                if (unit.unitType != UnitType.Villager)
                    return false;
            }
            
            return selectedVillagers != 0;
        }

        public static void Draw(SpriteBatch spriteBatch, Player player)
        {
            spriteBatch.Draw(Base, Vector2.Zero, Color.White);

            //Displays the stances of selected soldiers if they are the same
            if (SelectedUnitsAreSoldiers(player))
            {
                int numSoldiersSelected = 0;
                foreach (Unit unit in player.selectedUnits)
                    if (unit.unitType == UnitType.Soldier)
                        numSoldiersSelected++;

                Soldier selectedSoldier = player.soldiers[0];
                foreach (Soldier soldier in player.soldiers)
                    if (soldier.isSelected)
                    {
                        selectedSoldier = soldier;
                        break;
                    }

                if (numSoldiersSelected == 1)
                    spriteBatch.Draw(Stances, new Rectangle(10, 410, 160, 40), selectedSoldier.stance_rect, Color.White);

                else if (numSoldiersSelected > 1)
                    if (SelectedSoldiersHaveSameStance(player))
                        spriteBatch.Draw(Stances, new Rectangle(10, 410, 160, 40), selectedSoldier.stance_rect, Color.White);
                    else
                        spriteBatch.Draw(Stances, new Rectangle(10, 410, 160, 40), new Rectangle(0, 160, 160, 40), Color.White);
            }

            else if (SelectedUnitsAreVillagers(player))
                spriteBatch.Draw(Buildings, new Rectangle(10, 410, 120, 40), Color.White);

            else if (player.selectedUnits.Count < 1 && player.selectedStructures.Count == 1)
                switch (player.selectedStructures[0].ID)
                {
                    case BuildingID.Barracks: spriteBatch.Draw(infantry, new Rectangle(10, 410, 40, 40), Color.White); break;
                    case BuildingID.ArcheryRange: spriteBatch.Draw(archers, new Rectangle(10, 410, 40, 40), Color.White); break;
                }
        }
    }
}