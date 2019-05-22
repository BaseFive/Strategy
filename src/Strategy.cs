using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Griddle;

namespace Strategy
{
    public class Strategy : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Rectangle viewport;

        Map map;

        Player player;
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

            Camera.Initialize(GraphicsDevice.Viewport);

            map = new Map("Grassland.xml");

            player = new Player();
            p2 = new Computer();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            HUD.LoadContent(Content);
            map.LoadContent(Content);
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Camera.Update(map);

            player.UpdateUnits(p2, gameTime);
            p2.UpdateUnits(player, gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGreen);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera.transform);
            map.Draw(spriteBatch);
            player.Draw(spriteBatch);
            p2.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
            HUD.Draw(spriteBatch, player);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
