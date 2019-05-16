using System;
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

        public bool Intersects(Square square)
        {
            return (new Vector2(square.X + square.Width / 2, square.Y + square.Width / 2) - Centre).Length() < Radius + square.Width / 2;
        }
    }

    public struct Square
    {
        public int X, Y, Width;

        public Square(int X, int Y, int Width)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
        }
    }
}