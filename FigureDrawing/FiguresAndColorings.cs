using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FigureDrawing
{
    static class FiguresAndColorings
    {

            
        public static void drawLine(Point p1, Point p2, Bitmap bmp) {
            bool check = false;

            if(Math.Abs(p1.X - p2.X) < Math.Abs(p1.Y - p2.Y))
            {
                check = Math.Abs(p1.X - p2.X) < Math.Abs(p1.Y - p2.Y);

                int temp = p1.Y;
                p1.Y = p1.X;
                p1.X = temp;

                temp = p2.Y;
                p2.Y = p2.X;
                p2.X = temp;

            }
            if(p1.X > p2.X)
            {
                Point temp = new Point(p1.X, p1.Y);
                p1 = p2;
                p2 = temp;
            }


            double dx = p2.X - p1.X;
            double dy = Math.Abs(p2.Y - p1.Y);
            double m = dy / dx;
            double e = m - 0.5;
            int y = p1.Y;
            int ystep;
            if (p1.Y < p2.Y)
            {
                ystep = 1;
            }
            else
            {
                ystep = -1;
            }
            for (int x = p1.X; x <= p2.X; x++)
            {
                if (check)
                {
                    bmp.SetPixel(y, x, Color.Black);
                }
                else
                {
                    bmp.SetPixel(x, y, Color.Black);
                }
                if (e >= 0)
                {
                    y += ystep;
                    e += m - 1;
                }
                else e += m;
            }

        }
        static void drawBresenhamLine(PointF[] points, Bitmap bmp, Color color)
        {
            int n = points.Length;
            int countPoints = n * 64;
            PointF[] drawingPoints = new PointF[countPoints + 1];
            float dt = 1f / countPoints;
            float t = 0f;

            int i = 0;
            float mt;
            while (t <= 1f)
            {
                mt = 1 - t;
                drawingPoints[i].X = points[0].X * mt * mt * mt + 3 * points[1].X * mt * mt * t + 3 * points[2].X * mt * t * t + points[3].X * t * t * t;
                drawingPoints[i].Y = points[0].Y * mt * mt * mt + 3 * points[1].Y * mt * mt * t + 3 * points[2].Y * mt * t * t + points[3].Y * t * t * t;
                i++;
                t += dt;
            }

            for (i = 1; i < countPoints + 1; i++)
            {
                drawLine(new Point((int)drawingPoints[i - 1].X, (int)drawingPoints[i - 1].Y), new Point((int)drawingPoints[i].X, (int)drawingPoints[i].Y), bmp);
            }
        }
        private static void Draw8Pixels(int x, int y, int x0, int y0, Bitmap bmp)
        {

            bmp.SetPixel(x + x0, y + y0, Color.Black);
            bmp.SetPixel(x + x0, -y + y0, Color.Black);
            bmp.SetPixel(-x + x0, y + y0, Color.Black);
            bmp.SetPixel(-x + x0, -y + y0, Color.Black);
            bmp.SetPixel(y + x0, x + y0, Color.Black);
            bmp.SetPixel(y + x0, -x + y0, Color.Black);
            bmp.SetPixel(-y + x0, x + y0, Color.Black);
            bmp.SetPixel(-y + x0, -x + y0, Color.Black);
        }
        public static void drawCircle(Point p1, int R, Bitmap bmp) {
            int x = 0;
            int y = R;
            int d = 3 - 2 * R;
            while (y >= x)
            {
                Draw8Pixels(x, y, p1.X, p1.Y, bmp);
                if (d <= 0)
                {
                    d += 4 * x + 6;
                }
                else
                {
                    d += 4 * (x - y) + 10;
                    y--;
                }
                x++;
            }

        }
        public static void recColorise(Point pt, Bitmap bmp) {

            Stack<Point> pixels = new Stack<Point>();
            Color targetColor = bmp.GetPixel(pt.X, pt.Y);
            pixels.Push(pt);

            while (pixels.Count > 0)
            {
                Point a = pixels.Pop();
                if (a.X < bmp.Width && a.X > 0 &&
                a.Y < bmp.Height && a.Y > 0)
                {

                    if (bmp.GetPixel(a.X, a.Y) == targetColor)
                    {
                        bmp.SetPixel(a.X, a.Y, Color.Black);
                        pixels.Push(new Point(a.X - 1, a.Y));
                        pixels.Push(new Point(a.X + 1, a.Y));
                        pixels.Push(new Point(a.X, a.Y - 1));
                        pixels.Push(new Point(a.X, a.Y + 1));
                    }
                }
            }
            return;
        }
        public static void patternColorise(int x, int y, Bitmap bmp, Bitmap pattern) {
            int XLeft = x, XRight = x + 1;
            Color backcolor = bmp.GetPixel(x, y);

            while ((XLeft >= 0) && (bmp.GetPixel(XLeft, y) == backcolor))
            {
                bmp.SetPixel(XLeft, y, pattern.GetPixel(XLeft % 3, y % 3));
                XLeft--;
            }
            XLeft++;

            while ((XRight < bmp.Width) && (bmp.GetPixel(XRight, y) == backcolor))
            {
                bmp.SetPixel(XRight, y, pattern.GetPixel(XRight % 3, y % 3));
                XRight++;
            }
            XRight--;

            int tmpx;
            tmpx = XRight;
            while ((tmpx > XLeft) && (y != 0) && ((y - 1) >= 0))
            {
                while ((tmpx > XLeft) && (bmp.GetPixel(tmpx, y - 1) != backcolor))
                {
                    tmpx--;
                }
                if (tmpx > XLeft)
                {
                    patternColorise(tmpx, y - 1, bmp, pattern);
                }
                tmpx--;
            }
            tmpx = XRight;
            while ((tmpx > XLeft) && (y != 0) && ((y + 1) < bmp.Height))
            {
                while ((tmpx > XLeft) && (bmp.GetPixel(tmpx, y + 1) != backcolor))
                {
                    tmpx--;
                }
                if (tmpx > XLeft)
                {
                    patternColorise(tmpx, y + 1, bmp, pattern);
                }
                tmpx--;
            }
        }

    }
}
