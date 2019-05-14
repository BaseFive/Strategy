using Microsoft.Xna.Framework;

namespace Strategy
{
    public struct Circle
    {
        public Vector2 Centre { get; set; }
        public float Radius { get; set; }

        public Circle(Vector2 Centre, float Radius)
        {
            this.Centre = Centre;
            this.Radius = Radius;
        }

        public bool Contains(Vector2 point)
        {
            return (point - Centre).Length() <= Radius;
        }
    }
}