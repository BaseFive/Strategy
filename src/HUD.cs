﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Strategy
{
    public static class HUD
    {
        static Texture2D Base, Stances;
        public static Texture2D HP_Bar_Green, HP_Bar_Red;

        public static Texture2D arrow;

        public static Texture2D swordsman, enemy_swordsman;
        public static Texture2D archer, enemy_archer;

        public static void LoadContent(ContentManager Content)
        {
            Base = Content.Load<Texture2D>("HUD/HUD_Base");
            Stances = Content.Load<Texture2D>("HUD/Stances");
            HP_Bar_Green = Content.Load<Texture2D>("HUD/HP_Bar_Green");
            HP_Bar_Red = Content.Load<Texture2D>("HUD/HP_Bar_Red");

            arrow = Content.Load<Texture2D>("Projectiles/Arrow");

            swordsman = Content.Load<Texture2D>("Soldiers/Swordsman");
            archer = Content.Load<Texture2D>("Soldiers/Archer");
            enemy_swordsman = Content.Load<Texture2D>("Soldiers/Enemy_Swordsman");
            enemy_archer = Content.Load<Texture2D>("Soldiers/Enemy_Archer");
        }

        static bool SelectedSoldiersHaveSameStance()
        {
            Stance temp = Player.selectedSoldiers[0].stance;
            foreach (Soldier soldier in Player.soldiers)
                if (soldier.stance != temp)
                    return false;
            return true;
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Base, Vector2.Zero, Color.White);

            //Displays the stances of selected soldiers if they are the same
            if (Player.selectedUnits.Count == Player.selectedSoldiers.Count)
            {
                if (Player.selectedSoldiers.Count == 1)
                    spriteBatch.Draw(Stances, new Rectangle(10, 410, 160, 40), Player.selectedSoldiers[0].stance_rect, Color.White);

                else if (Player.selectedSoldiers.Count > 1)
                    if (SelectedSoldiersHaveSameStance())
                        spriteBatch.Draw(Stances, new Rectangle(10, 410, 160, 40), Player.selectedSoldiers[0].stance_rect, Color.White);
                    else
                        spriteBatch.Draw(Stances, new Rectangle(10, 410, 160, 40), new Rectangle(0, 160, 160, 40), Color.White);
            }
        }
    }
}