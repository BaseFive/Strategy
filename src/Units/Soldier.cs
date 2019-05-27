using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Strategy
{
    public enum Stance { Aggressive, Defensive, StandGround, NoAttack };
    public enum SoldierType { Swordsman, Archer };

    public abstract class Soldier : Unit, Aggressor
    {
        public Stance stance { get; protected set; }
        public SoldierType soldierType { get; protected set; }
        public Rectangle stance_rect { get; protected set; }
        protected Rectangle aggressive_rect, defensive_rect, standGround_rect, noAttack_rect;

        Circle Movement_Range { get { return new Circle(origin, 500); } }

        public Soldier(Texture2D spriteSheet, Vector2 pos) : base(spriteSheet, pos)
        {
            unitType = UnitType.Soldier;
            stance = Stance.Aggressive;
            hit_range = 10;
            sight_radius = 300;

            aggressive_rect = new Rectangle(10, 410, 40, 40);
            defensive_rect = new Rectangle(50, 410, 40, 40);
            standGround_rect = new Rectangle(90, 410, 40, 40);
            noAttack_rect = new Rectangle(130, 410, 40, 40);
        }

        public override void Update()
        {
            GoToDestination();
            CheckHP();
        }

        public override void Update(Computer p2, GameTime gameTime)
        {
            //Update based on stance
            switch (stance)
            {
                case Stance.Aggressive:
                    //Continuously follow a target
                    stance_rect = new Rectangle(0, 0, 160, 40);

                    LookForTarget(p2);

                    if (target != null)
                    {
                        FollowUnit(target);
                        if (IsAtUnit(target) && time_since_last_attack >= attack_interval)
                            Attack(gameTime);
                    }

                    break;

                case Stance.Defensive:
                    //Follow a target within a limited range
                    stance_rect = new Rectangle(0, 40, 160, 40);

                    LookForTarget(p2);

                    if (target != null)
                    {
                        FollowUnit(target);
                        if (IsAtUnit(target) && time_since_last_attack >= attack_interval)
                            Attack(gameTime);

                        if (!Movement_Range.Contains(pos))
                        {
                            target = null;
                            destination = origin;
                        }
                    }

                    break;

                case Stance.StandGround:
                    //Only attack units on the spot
                    stance_rect = new Rectangle(0, 80, 160, 40);

                    LookForTarget(p2);

                    if (target != null && IsAtUnit(target) && time_since_last_attack >= attack_interval)
                        Attack(gameTime);

                    break;

                case Stance.NoAttack:
                    //No movement or attacking
                    stance_rect = new Rectangle(0, 120, 160, 40);
                    break;
            }

            //Change stance based on clicks or <Q,W,E,R> hotkeys
            if (isSelected && (Mouse.GetState().LeftButton == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Q) ||
                Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.E) || Keyboard.GetState().IsKeyDown(Keys.R)))
                ChangeStance();

            //Update any projectiles fired by the soldier
            foreach (Projectile proj in projectiles)
                proj.Update();

            for (int i = 0; i < projectiles.Count; i++)
                if (projectiles[i].isDead)
                    projectiles.Remove(projectiles[i]);

            time_since_last_attack += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (target == null)
                GoToDestination();
            CheckHP();
        }

        public override void Update(Player player, GameTime gameTime)
        {
            switch (stance)
            {
                case Stance.Aggressive:
                    stance_rect = new Rectangle(0, 0, 160, 40);

                    LookForTarget(player);

                    if (target != null)
                    {
                        FollowUnit(target);
                        if (IsAtUnit(target) && time_since_last_attack >= attack_interval)
                            Attack(gameTime);
                    }

                    break;

                case Stance.Defensive:
                    stance_rect = new Rectangle(0, 40, 160, 40);

                    LookForTarget(player);

                    if (target != null)
                    {
                        FollowUnit(target);
                        if (IsAtUnit(target) && time_since_last_attack >= attack_interval)
                            Attack(gameTime);

                        if (!Movement_Range.Contains(pos))
                        {
                            target = null;
                            destination = origin;
                        }
                    }

                    break;

                case Stance.StandGround:
                    stance_rect = new Rectangle(0, 80, 160, 40);

                    LookForTarget(player);

                    if (target != null && IsAtUnit(target) && time_since_last_attack >= attack_interval)
                        Attack(gameTime);

                    break;

                case Stance.NoAttack:
                    stance_rect = new Rectangle(0, 120, 160, 40);
                    break;
            }

            UpdateProjectiles();

            time_since_last_attack += (float)gameTime.ElapsedGameTime.TotalSeconds;
            CheckHP();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Projectile proj in projectiles)
                proj.Draw(spriteBatch);

            spriteBatch.Draw(spriteSheet, pos, Color.White);

            if (isSelected || HP < MaxHP)
                DrawHealthBar(spriteBatch);
        }

        #region Helper Functions
        public void LookForTarget(Computer p2)
        {
            if (!IsMoving() && target == null)
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
            if (!IsMoving() && target == null)
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

        protected bool EnemyInSight(Computer p2)
        {
            foreach (Unit unit in p2.units)
                if (Sight.Contains(unit.pos))
                    return true;
            return false;
        }

        protected void ChangeStance()
        {
            if (aggressive_rect.Contains(Mouse.GetState().Position) || Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                stance = Stance.Aggressive;
                vel = Vector2.Zero;
            }

            else if (defensive_rect.Contains(Mouse.GetState().Position) || Keyboard.GetState().IsKeyDown(Keys.W))
            {
                stance = Stance.Defensive;
                vel = Vector2.Zero;
            }

            else if (standGround_rect.Contains(Mouse.GetState().Position) || Keyboard.GetState().IsKeyDown(Keys.E))
            {
                stance = Stance.StandGround;
            }

            else if (noAttack_rect.Contains(Mouse.GetState().Position) || Keyboard.GetState().IsKeyDown(Keys.R))
            {
                stance = Stance.NoAttack;
                if (target != null)
                {
                    target = null;
                    destination = pos;
                }
            }
        }
        #endregion
    }
}