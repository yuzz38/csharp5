using csharp5.Objects;
using static System.Formats.Asn1.AsnWriter;

namespace csharp5
{
    public partial class Form1 : Form
    {
        MyRectangle myRect;
        List<BaseObject> objects = new();
        Player player;
        Marker marker;
        List<Circle> circles = new();
        int score = 0;
        public Form1()
        {
            InitializeComponent();
          
            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);
            // добавл€ю реакцию на пересечение
            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] »грок пересекс€ с {obj}\n" + txtLog.Text;
            };

            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };
            marker = new Marker(pbMain.Width / 2 + 50, pbMain.Height / 2 + 50, 0);
            for (int i = 0; i <2; i++)
            {
                Circle circle = new Circle(200 + i * 50, 200 + i * 50, 0); 
                circles.Add(circle);
                objects.Add(circle);  
            }
            player.OnOverlap += (p, obj) =>
            {
                if (obj is Circle)
                {
                    player_OverlapCircle(obj); 
                }
                else
                {
                    txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] »грок пересекс€ с {obj}\n" + txtLog.Text;
                }
            };
            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };
            objects.Add(marker);
            objects.Add(player);
            txtScore.Text = $"Score: {score}";
        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            g.Clear(Color.White);

            
            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);
                    obj.Overlap(player);
                }

            }

            // рендерим объекты
            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
        }

        private void updatePlayer()
        {
            if (marker != null)
            {
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;
                float length = MathF.Sqrt(dx * dx + dy * dy);
                dx /= length;
                dy /= length;


                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;


                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }


            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;


            player.X += player.vX;
            player.Y += player.vY;

            foreach (var circle in circles)
            {
                circle.Shrink();
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            updatePlayer();
            pbMain.Invalidate();
        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            // тут добавил создание маркера по клику если он еще не создан
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker); 
            }

            marker.X = e.X;
            marker.Y = e.Y;
        }
        private void player_OverlapCircle(BaseObject obj)
        {
            if (obj is Circle circle)
            {
               
                score++;
                txtScore.Text = $"Score: {score}";
                circle.Respawn(); 
            }
        }

    }
}
