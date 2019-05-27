using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Strategy
{
    public class Villager : Unit
    {
        Rectangle building_rect;
        Rectangle barracks_rect, a_range_rect, g_tower_rect;

        MouseState oldMouse;

        public Villager(Texture2D spriteSheet, Vector2 pos) : base(spriteSheet, pos)
        {
            unitType = UnitType.Villager;
            HP = 20;
            MaxHP = HP;
            speed = 2;
            attack = 3;
            attack_interval = 2;
            hit_range = 10;
            sight_radius = 300;

            building_rect = new Rectangle(10, 410, 120, 40);
            barracks_rect = new Rectangle(10, 410, 40, 40);
            a_range_rect = new Rectangle(50, 410, 40, 40);
            g_tower_rect = new Rectangle(90, 410, 40, 40);

            oldMouse = Mouse.GetState();
        }

        public override void Update(Computer p2, GameTime gameTime) { }

        public override void Update(Player player, GameTime gameTime)
        {
            GoToDestination();
            CheckHP();

            if (target != null && IsAtUnit(target) && time_since_last_attack >= attack_interval)
                Attack(gameTime);

            MouseState newMouse = Mouse.GetState();

            if (oldMouse.LeftButton == ButtonState.Released && newMouse.LeftButton == ButtonState.Pressed &&
                isSelected && building_rect.Contains(newMouse.Position))
                Build(player);

            oldMouse = newMouse;

            time_since_last_attack += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void Attack(GameTime gameTime)
        {
            if (target.isDead)
            {
                target = null;
                destination = pos;
                vel = Vector2.Zero;
                return;
            }

            target.HP -= attack;
            time_since_last_attack = 0;
        }

        void Build(Player player)
        {
            if (barracks_rect.Contains(Mouse.GetState().Position))
                player.structures.Add(new Barracks(HUD.blue_barracks, new Vector2(10)));
            else if (a_range_rect.Contains(Mouse.GetState().Position))
                player.structures.Add(new ArcheryRange(HUD.blue_archery_range, new Vector2(10, 100)));
            else if (g_tower_rect.Contains(Mouse.GetState().Position))
                player.structures.Add(new GuardTower(HUD.blue_guard_tower, new Vector2(300, 100)));
        }
    }
}