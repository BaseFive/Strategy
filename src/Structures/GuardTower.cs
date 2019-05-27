using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Strategy
{
    public class GuardTower : DefenseStructure
    {
        public GuardTower(Texture2D spriteSheet, Vector2 pos) : base(spriteSheet, pos)
        {
            ID = BuildingID.GuardTower;
            HP = 500;
            MaxHP = HP;
            attack = 10;
            attack_interval = 0.5f;
            projectile_speed = 4;
            Fire_Position = new Vector2(pos.X + Width / 2, pos.Y + Height / 5);
        }

        public override void Update(Player player, Computer p2, GameTime gameTime)
        {
            LookForTarget(p2);
            if (target != null && time_since_last_attack >= attack_interval)
                Attack(gameTime);

            UpdateProjectiles();

            time_since_last_attack += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Attack(GameTime gameTime)
        {
            if (target.isDead || (projectiles.Count > 0 && target.HP - projectiles.Count * attack <= 0))
            {
                target = null;
                return;
            }

            //Calculate direction, velocity and rotation of arrows
            Vector2 direction = target.Centre - Fire_Position;
            float distance = direction.Length();
            Vector2 velocity = (direction / distance) * projectile_speed;
            float rotation = (float)System.Math.Atan(direction.Y / direction.X);

            if (target.Centre.Y <= Fire_Position.Y && target.Centre.X <= Fire_Position.X)
                rotation += 3.142f;
            else if (target.Centre.Y >= Fire_Position.Y && target.Centre.X <= Fire_Position.X)
                rotation -= 3.142f;

            projectiles.Add(new TowerArrow(HUD.arrow, Fire_Position));
            projectiles[projectiles.Count - 1].Initialize(velocity, attack, target, rotation);
            time_since_last_attack = 0;
        }
    }
}