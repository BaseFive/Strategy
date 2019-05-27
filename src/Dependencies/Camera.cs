using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Strategy
{
    public static class Camera
    {
        public static Matrix transform;
        static Vector2 centre;
        static Viewport viewport;
        static int MovementSpeed;

        public static void Initialize(Viewport Viewport)
        {
            viewport = Viewport;
            centre = new Vector2(viewport.Width / 2, viewport.Height / 2);
            MovementSpeed = 5;
        }

        public static void Update(Map map)
        {
            MouseState Pos = Mouse.GetState();

            if ((Pos.X < 0 || Keyboard.GetState().IsKeyDown(Keys.Left)) && centre.X > viewport.Width / 2)
                centre.X -= MovementSpeed;

            else if ((Pos.X > viewport.Width || Keyboard.GetState().IsKeyDown(Keys.Right)) && centre.X < map.Width - viewport.Width / 2)
                centre.X += MovementSpeed;

            if ((Pos.Y < 0 || Keyboard.GetState().IsKeyDown(Keys.Up)) && centre.Y > viewport.Height / 2)
                centre.Y -= MovementSpeed;

            else if ((Pos.Y > viewport.Height || Keyboard.GetState().IsKeyDown(Keys.Down)) && centre.Y < map.Height - viewport.Height / 2)
                centre.Y+= MovementSpeed;

            transform = Matrix.CreateTranslation(new Vector3(-centre.X + viewport.Width / 2, -centre.Y + viewport.Height / 2, 0));
        }
    }
}