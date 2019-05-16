using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Strategy
{
    public enum Stance { Aggressive, Defensive, StandGround, NoAttack };
    public enum SoldierType { Swordsman, Archer };

    public abstract class Soldier : Unit
    {
        public Stance stance { get; protected set; }
        public SoldierType soldierType { get; protected set; }
        protected float sightRadius;
        public Rectangle stance_rect { get; protected set; }
        protected Rectangle aggressive_rect, defensive_rect, standGround_rect, noAttack_rect;
        public List<Projectile> projectiles;

        public Soldier(Texture2D spriteSheet, Vector2 pos) : base(spriteSheet, pos)
        {
            projectiles = new List<Projectile>();
            unitType = UnitType.Soldier;
            stance = Stance.Aggressive;
            hit_range = 10;
            sightRadius = 300;

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
                            Attack(target, gameTime);
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
                            Attack(target, gameTime);

                        Circle range = new Circle(origin, 500);
                        if (!range.Contains(pos))
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
                        Attack(target, gameTime);

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

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet, pos, Color.White);

            if (isSelected)
                DrawHealthBar(spriteBatch);

            foreach (Projectile proj in projectiles)
                proj.Draw(spriteBatch);
        }

        #region Helper Functions
        protected Circle Sight()
        {
            return new Circle(new Vector2(pos.X + Width / 2, pos.Y + Height / 2), sightRadius);
        }

        protected void LookForTarget(Computer p2)
        {
            if (!IsMoving() && target == null)
            {
                Circle sight = Sight();
                foreach (Unit unit in p2.units)
                    if (sight.Contains(unit.pos))
                    {
                        target = unit;
                        break;
                    }
            }
        }

        protected bool EnemyInSight(Computer p2)
        {
            Circle sight = Sight();
            foreach (Unit unit in p2.units)
                if (sight.Contains(unit.pos))
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