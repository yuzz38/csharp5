using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp5.Objects
{
    internal class Circle : BaseObject
    {
        public float Size { get; set; } = 50;
        private const float MinSize = 5f;
        public Circle(float x, float y, float angle) : base(x, y, angle)
        {
        }
        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Green), -Size / 2, -Size / 2, Size, Size);
            g.DrawEllipse(new Pen(Color.Black, 2), -Size / 2, -Size / 2, Size, Size);
        }
        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-Size / 2, -Size / 2, Size, Size); 
            return path;
        }
        public void Respawn()
        {
         
            Random rand = new Random();
            this.X = rand.Next(10, 500); 
            this.Y = rand.Next(10, 400);
            this.Size = 50;
        }
        public void Shrink()
        {
            if (Size > MinSize)
            {
                Size -= 0.4f; 
            }
            else
            {
               
                Respawn();
            }
        }
        public override void Overlap(BaseObject obj)
        {
            base.Overlap(obj);
            if (obj is Player)
            {
               
                Respawn();
            }
        }
    }
}
