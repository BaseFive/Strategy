using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Strategy
{
    public class Strategy : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Rectangle viewport;
        Computer p2;

        public Strategy()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            IsFixedTimeStep = false;
        }

        protected override void Initialize()
        {
            viewport = new Rectangle(0, 0, 800, 400);

            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();

            Player.Initialize();
            p2 = new Computer();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            HUD.LoadContent(Content);
            Player.LoadContent(Content);
            p2.LoadContent(Content);
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Player.UpdateUnits(p2, gameTime);
            p2.UpdateUnits();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGreen);

            spriteBatch.Begin();
            Player.Draw(spriteBatch);
            p2.Draw(spriteBatch);
            HUD.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
