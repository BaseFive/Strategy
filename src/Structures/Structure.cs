using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Strategy
{
    public abstract class Structure : Entity, Aggressor
    {
        protected int projectile_speed;

        public Structure(Texture2D spriteSheet, Vector2 pos) : base(spriteSheet, pos)
        {
            sight_radius = 300;
            hit_range = sight_radius;
        }

        public abstract void Attack(GameTime gameTime);
        public abstract void Update(Computer p2, GameTime gameTime);
        public abstract void Draw(SpriteBatch spritebatch);


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