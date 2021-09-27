using System;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace RefactorMe
{
    class Drawer
    {
        public const float LengthMultiplier = 0.375f;
        public const float WidthMultiplier = 0.04f;

        static float x, y;
        static Graphics graphics;

        public static void Initialize(Graphics newGraphics)
        {
            graphics = newGraphics;
            graphics.SmoothingMode = SmoothingMode.None;
            graphics.Clear(Color.Black);
        }

        public static void SetPosition(float x0, float y0)
        {
            x = x0; 
            y = y0;
        }

        public static void DrawLinePolar(Pen pen, double length, double angle) 
        {
        // Polar - аналогия с полярными координатами
        // Делает шаг длиной length в направлении angle и рисует пройденную траекторию 
            var x1 = (float)(x + length * Math.Cos(angle));
            var y1 = (float)(y + length * Math.Sin(angle));
            graphics.DrawLine(pen, x, y, x1, y1);
            SetPosition(x1, y1);
        }

        public static void MoveToPolar(double length, double angle)
        {
            x = (float)(x + length * Math.Cos(angle)); 
            y = (float)(y + length * Math.Sin(angle));
        }

        public static void DrawSide(Pen pen, float size, double angle)
        {
            DrawLinePolar(pen, size * LengthMultiplier, angle);
            DrawLinePolar(pen, size * WidthMultiplier * Math.Sqrt(2), angle + Math.PI / 4);
            DrawLinePolar(pen, size * LengthMultiplier, angle + Math.PI);
            DrawLinePolar(pen, size * LengthMultiplier - size * WidthMultiplier, angle + Math.PI / 2);

            MoveToPolar(size * WidthMultiplier, angle - Math.PI);
            MoveToPolar(size * WidthMultiplier * Math.Sqrt(2), angle + 3 * Math.PI / 4);
        }

        public static void SetDefaultPosition(double diagonalLength, int width, int height)
        {
            var x0 = (float)(diagonalLength * Math.Cos(Math.PI / 4 + Math.PI)) + width / 2f;
            var y0 = (float)(diagonalLength * Math.Sin(Math.PI / 4 + Math.PI)) + height / 2f;
            Drawer.SetPosition(x0, y0);
        }
    }

    public class ImpossibleSquare
    {
        public static void Draw(int width, int height, double rotationAngle, Graphics graphics)
        {
            // rotationAngle пока не используется, но будет использоваться в будущем
            Drawer.Initialize(graphics);
            
            var size = Math.Min(width, height);
            var diagonalLength = Math.Sqrt(2) * (size * Drawer.LengthMultiplier + size * Drawer.WidthMultiplier) / 2;

            Drawer.SetDefaultPosition(diagonalLength, width, height);

            for (var i = 0; i > -4; i--)
                Drawer.DrawSide(Pens.Yellow, size, Math.PI * i / 2);
        }
    }
}