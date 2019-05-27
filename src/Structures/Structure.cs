using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Strategy
{
    public enum BuildingID { Barracks, ArcheryRange, GuardTower };

    public abstract class Structure : Entity, Aggressor
    {
        public BuildingID ID { get; protected set; }
        protected int projectile_speed;

        public Structure(Texture2D spriteSheet, Vector2 pos) : base(spriteSheet, pos)
        {
            sight_radius = 300;
            hit_range = sight_radius;
        }

        public abstract void Update(Player player, Computer p2, GameTime gameTime);

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Projectile proj in projectiles)
                proj.Draw(spriteBatch);

            spriteBatch.Draw(spriteSheet, pos, Color.White);

            if (isSelected || HP < MaxHP)
                DrawHealthBar(spriteBatch);
        }

        public void LookForTarget(Computer p2)
        {
            if (target == null)
            {
                List<Unit> enemiesInSight = new List<Unit>();

                foreach (Unit enemy in p2.units)
                    if (Sight.Contains(enemy.pos))
                        enemiesInSight.Add(enemy);

                if (enemiesInSight.Count == 0)
                    return;

                float shortest_distance = Sight.Radius;
                foreach (Unit enemy in enemiesInSight)
                {
                    float distance = (enemy.pos - pos).Length();
                    if (distance < shortest_distance)
                    {
                        shortest_distance = distance;
                        target = enemy;
                    }
                }
            }
        }

        public void LookForTarget(Player player)
        {
            if (target == null)
            {
                List<Unit> enemiesInSight = new List<Unit>();

                foreach (Unit enemy in player.units)
                    if (Sight.Contains(enemy.pos))
                        enemiesInSight.Add(enemy);

                if (enemiesInSight.Count == 0)
                    return;

                float shortest_distance = Sight.Radius;
                foreach (Unit enemy in enemiesInSight)
                {
                    float distance = (enemy.pos - pos).Length();
                    if (distance < shortest_distance)
                    {
                        shortest_distance = distance;
                        target = enemy;
                    }
                }
            }
        }
    }
}