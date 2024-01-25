using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FigureDrawing
{
    public partial class Form1 : Form
    {

        private enum Modes {line, circle, coloring, patternColoring};
        private Modes mode;
        Bitmap bmp;
        Bitmap pattern;
        Stack <Point> points = new Stack<Point>(2);
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pattern = new Bitmap(3, 3);
            pattern.SetPixel(0, 0, Color.White);
            pattern.SetPixel(0, 1, Color.Yellow);
            pattern.SetPixel(1, 0, Color.Red);
            pattern.SetPixel(1, 1, Color.Blue);

            pattern.SetPixel(2, 2, Color.White);


            pattern.SetPixel(0, 2, Color.Black);
            pattern.SetPixel(1, 2, Color.Yellow);

            pattern.SetPixel(2, 0, Color.Red);
            pattern.SetPixel(2, 1, Color.Blue);
        }
        private void lineMode(object sender, EventArgs e)
        {
            mode = Modes.line;
        }

        private void coloring(object sender, EventArgs e)
        {
            mode = Modes.coloring;
            
        }

        private void recursiveColoring(object sender, EventArgs e)
        {
            mode = Modes.patternColoring;
        }

        private void circleMode(object sender, EventArgs e)
        {
            mode = Modes.circle;
        }


        private void setDots(object sender, MouseEventArgs e)
        {
            

            switch (mode)
            {
                case (Modes.line):
                    points.Push(new Point(e.X, e.Y));

                    if(points.Count() == 2)
                    {
                        FiguresAndColorings.drawLine(points.Pop(), points.Pop(), bmp);
                    }
                    break;
                case (Modes.circle):
                    points.Push(new Point(e.X, e.Y));
                    if (points.Count() == 2)
                    {
                        Point p2 = points.Pop();
                        Point p1 = points.Pop();

                        int R =(int)Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y));
                        FiguresAndColorings.drawCircle(p1, R, bmp);
                    }
                    break;
                case (Modes.coloring):
                    FiguresAndColorings.recColorise(new Point(e.X, e.Y), bmp);
                    break;
                case (Modes.patternColoring):
                    FiguresAndColorings.patternColorise(e.X, e.Y, bmp, pattern);
                    break;
            }

            pictureBox1.Image = bmp;
        }
    }
}
