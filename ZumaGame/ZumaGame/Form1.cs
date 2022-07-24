using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Media;

namespace ZumaGame
{
    public class HoldBall
    {
        public List<Bitmap> imgs = new List<Bitmap>();
        public int iCurrFrame;
        public int w = 30, h = 30;
        public PointF BallPoint;
    }
    public class Ball
    {

        public List<Bitmap> imgs = new List<Bitmap>();
        public int iCurrFrame;
        public int w = 40, h = 40;
        public int hint = 1;
        public float speed = (0.0009f) * 5;
        public PointF BallPoint;


        public float my_t_inForm = 0.0001f;
        //public float my_t2_inForm = 0.0001f;
        //public float my_t3_inForm = 0.0001f;
        //public float my_t4_inForm = 0.0001f;
    }
    public class ShootingBall
    {

        public List<Bitmap> imgs = new List<Bitmap>();
        public int iCurrFrame;
        public int w = 40, h = 40;
        public PointF EndPoint;
        public PointF MovingBall;
        public PointF BallPoint;
        public bool dead = false;
        public bool fired = false;

        public bool Move()
        {
            float Speed = 20;
            float dx = EndPoint.X - BallPoint.X;
            float dy = EndPoint.Y - BallPoint.Y;

            float m = dy / dx;
            if (MovingBall.X > 2100 || MovingBall.Y > 1200 || (MovingBall.X < -200) || MovingBall.Y < -200)
            {
                Speed = 0;
            }
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                if (BallPoint.X < EndPoint.X && BallPoint.Y < EndPoint.Y)
                {
                    MovingBall.X += Speed;
                    MovingBall.Y += m * Speed;


                }
                else if (BallPoint.X < EndPoint.X && BallPoint.Y > EndPoint.Y)
                {
                    MovingBall.X += Speed;
                    MovingBall.Y += m * Speed;


                }
                else if (BallPoint.X > EndPoint.X && BallPoint.Y < EndPoint.Y)
                {
                    MovingBall.X -= Speed;
                    MovingBall.Y -= m * Speed;


                }

                else if (BallPoint.X > EndPoint.X && BallPoint.Y > EndPoint.Y)
                {
                    MovingBall.X -= Speed;
                    MovingBall.Y -= m * Speed;


                }
            }
            else
            {
                if (BallPoint.X < EndPoint.X && BallPoint.Y < EndPoint.Y)
                {
                    MovingBall.Y += Speed;
                    MovingBall.X += 1 / m * Speed;


                }
                else if (BallPoint.X < EndPoint.X && BallPoint.Y > EndPoint.Y)
                {
                    MovingBall.Y -= Speed;
                    MovingBall.X -= 1 / m * Speed;


                }
                else if (BallPoint.X > EndPoint.X && BallPoint.Y < EndPoint.Y)
                {
                    MovingBall.Y += Speed;
                    MovingBall.X += 1 / m * Speed;


                }

                else if (BallPoint.X > EndPoint.X && BallPoint.Y > EndPoint.Y)
                {
                    MovingBall.Y -= Speed;
                    MovingBall.X -= 1 / m * Speed;


                }
            }
            return false;
        }

    }
    public class Frog
    {

        public Bitmap image;
        public float x;
        public float y;
        public float w;
        public float h;
    }
    public partial class Form1 : Form
    {
        Bitmap off;
        public int removect = 0;
        List<Ball> Balls = new List<Ball>();
        List<ShootingBall> ShootingBalls = new List<ShootingBall>();
        SoundPlayer SoundPlayer = new SoundPlayer("background.wav");
        enum Modes { CTRL_POINTS, DRAG };
        Random RR = new Random();

        BezierCurve obj = new BezierCurve();
        BezierCurve obj2 = new BezierCurve();
        BezierCurve obj3 = new BezierCurve();
        BezierCurve obj4 = new BezierCurve();
        int hint = 1;
        float linex;
        float liney;
        float linex2;
        float liney2;
        public int randclr;
        public int ballsct;
        public int randct;
        public int indexshoot = 0;
        public int randclrshoot;
        public int randclrhold;
        public bool end = false;
        //Bitmap back = new Bitmap("back.png");

        Frog frog;
        bool squish = false;
        public int timertick = 0;

        public int lastclr;



        private System.Windows.Forms.MainMenu mainMenu1;

        int count = 0;
        private IContainer components;
        int state = 0;
        Bitmap Background = new Bitmap("Background.png");
        Bitmap State1 = new Bitmap("gameinterface.png");
        Bitmap Map = new Bitmap("map.png");
        Bitmap Gate = new Bitmap("Gate.png");
        float rotation;
        public bool breaking = false;

        HoldBall HoldBall;
        Timer t = new Timer();
        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;
            this.Load += Form1_Load;
            this.Paint += Form1_Paint;
            this.MouseDown += Form1_MouseDown;
            this.MouseMove += Form1_MouseMove;
            this.KeyDown += Form1_KeyDown;
            t.Tick += new EventHandler(t_Tick);
            t.Start();
            //InitializeComponent();
        }

        private void t_Tick(object sender, EventArgs e)
        {
            if (indexshoot == 0)
            {
                ShootingBalls[0].dead = false;

            }
            if (Balls.Count <= 0 && state == 1 && timertick > 20)
            {
                t.Stop();
                MessageBox.Show("Level Finished");
                ShootingBalls.Clear();
                end = true;
            }
            //if(timertick==0)
            //{
            //    ShootingBalls[0].iCurrFrame = RR.Next(0, 6);
            //    HoldBall.iCurrFrame = RR.Next(0, 6);
            //}

            if (Balls.Count > 0)
            {
                float dx = Balls[Balls.Count - 1].BallPoint.X - 14;
                float dy = Balls[Balls.Count - 1].BallPoint.Y - 242;

                if (Math.Sqrt((dy * dy) + (dx * dx)) >= Balls[0].w)
                {

                    squish = false;
                }

            }
            if (!squish && state == 1 && Balls.Count + removect < 20)
            {

                CreateBalls(randclr);
                squish = true;
                randct--;
            }

            timertick++;


            if (randct <= -1)
            {
                randclr = RR.Next(0, 6);

                lastclr = randclr;
                randct = RR.Next(0, 3);

            }
           
            if (state == 1)
            {
                for (int i = 0; i < ShootingBalls.Count; i++)
                {
                    if (ShootingBalls[i].fired)
                    {
                        ShootingBalls[i].Move();
                    }
                }
            }
            for (int i = 0; i < Balls.Count; i++)
            {
                if (state == 1)
                {
                    if (i == 0)
                    {
                        if (Balls[i].hint == 1)
                        {
                            Balls[i].my_t_inForm += Balls[i].speed;
                            if (Balls[i].my_t_inForm >= 1)
                            {
                                Balls[i].my_t_inForm = 0.0001f;
                                Balls[i].hint++;

                            }
                        }
                        if (Balls[i].hint == 2)
                        {
                            Balls[i].my_t_inForm += Balls[i].speed;
                            if (Balls[i].my_t_inForm >= 1)
                            {
                                Balls[i].my_t_inForm = 0.0001f;
                                Balls[i].hint++;

                            }
                        }
                        if (Balls[i].hint == 3)
                        {
                            Balls[i].my_t_inForm += Balls[i].speed;
                            if (Balls[i].my_t_inForm >= 1)
                            {
                                Balls[i].my_t_inForm = 0.0001f;
                                Balls[i].hint++;

                            }
                        }
                        if (Balls[i].hint == 4)
                        {
                            Balls[i].my_t_inForm += Balls[i].speed;
                            if (Balls[i].my_t_inForm >= 1)
                            {
                                t.Stop();
                                MessageBox.Show("Game over");

                            }
                        }
                    }
                    if (Balls.Count >= 2 && i > 0)
                    {
                        if (Balls[i].hint == 1)
                        {

                            while (true)
                            {
                                float dx = Balls[i - 1].BallPoint.X - Balls[i].BallPoint.X;
                                float dy = Balls[i - 1].BallPoint.Y - Balls[i].BallPoint.Y;
                                Balls[i].my_t_inForm += 0.0001f;
                                Balls[i].BallPoint = obj.CalcCurvePointAtTime(Balls[i].my_t_inForm);

                                if (Math.Sqrt((dy * dy) + (dx * dx)) <= Balls[0].w)
                                {

                                    break;
                                }
                            }

                            if (Balls[i].my_t_inForm >= 1)
                            {
                                Balls[i].my_t_inForm = 0.0001f;
                                Balls[i].hint++;

                            }
                        }
                        if (Balls[i].hint == 2)
                        {
                            while (true)
                            {
                                float dx = Balls[i - 1].BallPoint.X - Balls[i].BallPoint.X;
                                float dy = Balls[i - 1].BallPoint.Y - Balls[i].BallPoint.Y;
                                Balls[i].my_t_inForm += 0.0009f;
                                Balls[i].BallPoint = obj2.CalcCurvePointAtTime(Balls[i].my_t_inForm);

                                if (Math.Sqrt((dy * dy) + (dx * dx)) <= Balls[0].w)
                                {

                                    break;

                                }
                            }
                            if (Balls[i].my_t_inForm >= 1)
                            {
                                Balls[i].my_t_inForm = 0.0001f;
                                Balls[i].hint++;

                            }
                        }
                        if (Balls[i].hint == 3)
                        {
                            while (true)
                            {
                                float dx = Balls[i - 1].BallPoint.X - Balls[i].BallPoint.X;
                                float dy = Balls[i - 1].BallPoint.Y - Balls[i].BallPoint.Y;
                                Balls[i].my_t_inForm += 0.0001f;
                                Balls[i].BallPoint = obj3.CalcCurvePointAtTime(Balls[i].my_t_inForm);

                                if (Math.Sqrt((dy * dy) + (dx * dx)) <= Balls[0].w)
                                {

                                    break;
                                }
                            }
                            if (Balls[i].my_t_inForm >= 1)
                            {
                                Balls[i].my_t_inForm = 0.0001f;
                                Balls[i].hint++;

                            }
                        }
                        if (Balls[i].hint == 4)
                        {
                            int ct = 0;
                            while (true)
                            {
                                ct++;
                                float dx = Balls[i - 1].BallPoint.X - Balls[i].BallPoint.X;
                                float dy = Balls[i - 1].BallPoint.Y - Balls[i].BallPoint.Y;
                                Balls[i].my_t_inForm += 0.0001f;
                                Balls[i].BallPoint = obj4.CalcCurvePointAtTime(Balls[i].my_t_inForm);

                                if (Math.Sqrt((dy * dy) + (dx * dx)) <= Balls[0].w)
                                {

                                    break;
                                }



                            }
                            if (Balls[i].my_t_inForm >= 1)
                            {
                                t.Stop();
                                MessageBox.Show("Game over");

                            }
                        }
                    }




                }
            }
           
            if (state == 1)
            {
                int coll = CheckCollision();
                if (coll != -1)
                {
                    CheckExplosion(coll);
                }

            }
            DrawDubb(CreateGraphics());





        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C)
            {
                MessageBox.Show(ShootingBalls.Count.ToString() + "-" + ShootingBalls[0].iCurrFrame + "//" + ShootingBalls[0].MovingBall.X + "//" + ShootingBalls[0].dead);
                MessageBox.Show(Balls.Count.ToString());
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (state == 1 && !end)
            {
                rotation = (float)Math.Atan2(e.Y - frog.y, e.X - frog.x);
                float rotation2 = (float)(rotation * 180 / Math.PI);
                linex = (float)(50 * Math.Cos(Math.PI / 180 * rotation2) + frog.x);
                liney = (float)(50 * Math.Sin(Math.PI / 180 * rotation2) + frog.y);
                linex2 = (float)(-50 * Math.Cos(Math.PI / 180 * rotation2) + frog.x);
                liney2 = (float)(-50 * Math.Sin(Math.PI / 180 * rotation2) + frog.y);
                ShootingBalls[indexshoot].MovingBall.X = linex;
                ShootingBalls[indexshoot].MovingBall.Y = liney;
                HoldBall.BallPoint.X = linex2;
                HoldBall.BallPoint.Y = liney2;
                if (indexshoot == 0)
                {
                    ShootingBalls[0].BallPoint.X = linex;
                    ShootingBalls[0].BallPoint.Y = liney;
                }
            }




        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (state == 0)
            {
                if ((e.X >= ((this.ClientSize.Width / 2) + 120))
                && e.X <= ((this.ClientSize.Width / 2) + 700)
                && e.Y >= 230
                && e.Y <= 400
                && state == 0)
                {

                    //SoundPlayer.Stop();
                    state = 1;
                }



                if ((e.X >= ((this.ClientSize.Width / 2) + 20))
                    && e.X <= ((this.ClientSize.Width / 2) + 700)
                    && e.Y >= 415
                    && e.Y <= 550
                    && state == 0)
                {
                    MessageBox.Show("Random colored balls are generated with random numbers, you need to shoot balls from the frog to the ball train on a ball with the same color, when 3 balls of the same color stack they expldoe, if the balls train reach the end gate you lose.");
                }


                if ((e.X >= ((this.ClientSize.Width / 2)))
                    && e.X <= ((this.ClientSize.Width / 2) + 700)
                    && e.Y >= 570
                    && e.Y <= 700
                    && state == 0)
                {
                    MessageBox.Show("Abdallah Mohamed Abdalla ID: 200879");
                }

            }
            if (Balls.Count > 0)
            {
                if (e.Button == MouseButtons.Left)
                {

                    ShootingBalls[indexshoot].EndPoint.X = e.X;
                    ShootingBalls[indexshoot].EndPoint.Y = e.Y;
                    //  MessageBox.Show(ShootingBalls[0].EndPoint.X.ToString());
                    //   MessageBox.Show(ShootingBalls[0].EndPoint.Y.ToString());
                    ShootingBalls[indexshoot].fired = true;

                    CreateShootingBalls(HoldBall.iCurrFrame, 0, 0);
                    indexshoot++;
                    HoldBall.iCurrFrame = RR.Next(0, 6);
                }
            }

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            obj.SetControlPoint(new Point(14, 242));
            obj.SetControlPoint(new Point(750, -10));
            obj.SetControlPoint(new Point(1556, -20));
            obj.SetControlPoint(new Point(1525, 109));
            obj.SetControlPoint(new Point(1881, 371));
            obj.SetControlPoint(new Point(1764, 416));
            obj.SetControlPoint(new Point(1945, 593));
            obj.SetControlPoint(new Point(1604, 559));
            ///////////////////////////////////////////

            obj2.SetControlPoint(new Point(1604, 559));
            obj2.SetControlPoint(new Point(1546, 513));
            obj2.SetControlPoint(new Point(1497, 432));
            obj2.SetControlPoint(new Point(1448, 355));
            obj2.SetControlPoint(new Point(1395, 281));
            obj2.SetControlPoint(new Point(1289, 154));
            obj2.SetControlPoint(new Point(1227, 168));
            obj2.SetControlPoint(new Point(1137, 160));
            obj2.SetControlPoint(new Point(917, 131));
            obj2.SetControlPoint(new Point(820, 220));
            obj2.SetControlPoint(new Point(567, 215));
            obj2.SetControlPoint(new Point(329, 263));
            obj2.SetControlPoint(new Point(143, 333));
            obj2.SetControlPoint(new Point(83, 507));
            ///////////////////////////////////////

            obj3.SetControlPoint(new Point(83, 507));
            obj3.SetControlPoint(new Point(36, 744));
            obj3.SetControlPoint(new Point(181, 839));
            obj3.SetControlPoint(new Point(217, 980));
            obj3.SetControlPoint(new Point(418, 896));
            obj3.SetControlPoint(new Point(611, 918));
            obj3.SetControlPoint(new Point(911, 908));
            obj3.SetControlPoint(new Point(1242, 982));
            obj3.SetControlPoint(new Point(1393, 813));
            obj3.SetControlPoint(new Point(1595, 884));
            obj3.SetControlPoint(new Point(1700, 736));
            obj3.SetControlPoint(new Point(1676, 702));

            /////////////////////////////////////////

            obj4.SetControlPoint(new Point(1676, 702));
            obj4.SetControlPoint(new Point(1680, 621));
            obj4.SetControlPoint(new Point(1568, 646));
            obj4.SetControlPoint(new Point(1505, 577));
            obj4.SetControlPoint(new Point(1392, 572));
            obj4.SetControlPoint(new Point(1349, 639));
            obj4.SetControlPoint(new Point(1243, 695));
            obj4.SetControlPoint(new Point(1186, 838));
            obj4.SetControlPoint(new Point(1057, 822));
            obj4.SetControlPoint(new Point(992, 771));
            obj4.SetControlPoint(new Point(900, 877));
            obj4.SetControlPoint(new Point(815, 725));
            obj4.SetControlPoint(new Point(716, 846));
            obj4.SetControlPoint(new Point(592, 824));
            obj4.SetControlPoint(new Point(542, 694));
            obj4.SetControlPoint(new Point(471, 779));
            obj4.SetControlPoint(new Point(428, 705));
            obj4.SetControlPoint(new Point(345, 681));
            obj4.SetControlPoint(new Point(367, 607));
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            CreateShootingBalls(RR.Next(0, 6), 0, 0);
            randclr = RR.Next(0, 6);
            randct = RR.Next(0, 3);




            CreateFrog();

            HoldBall = new HoldBall();
            for (int i = 0; i < 6; i++)
            {
                Bitmap im = new Bitmap("ball" + (i + 1) + ".png");
                im.MakeTransparent(im.GetPixel(0, 0));
                HoldBall.imgs.Add(im);
            }

            //HoldBall.iCurrFrame = RR.Next(0, 6);
            SoundPlayer.PlayLooping();



        }
        void DrawScene(Graphics g2)
        {
            g2.Clear(Color.White);
            if (state == 0)
            {
                Rectangle rcDst = new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height);

                g2.DrawImage(Background, rcDst, rcDst, GraphicsUnit.Pixel);
            }
            if (state == 1)
            {
                g2.DrawImage(Map, 0, 0, ClientSize.Width, ClientSize.Height);
                g2.DrawImage(State1, 0, 0, ClientSize.Width, ClientSize.Height);

                //obj.DrawCurve(g2);
                //obj2.DrawCurve(g2);
                //obj3.DrawCurve(g2);
                //obj4.DrawCurve(g2);
                for (int i = 0; i < Balls.Count; i++)
                {
                    if (Balls[i].hint == 1)
                    {
                        Balls[i].BallPoint = obj.CalcCurvePointAtTime(Balls[i].my_t_inForm);
                    }
                    if (Balls[i].hint == 2)
                    {
                        Balls[i].BallPoint = obj2.CalcCurvePointAtTime(Balls[i].my_t_inForm);
                    }
                    if (Balls[i].hint == 3)
                    {
                        Balls[i].BallPoint = obj3.CalcCurvePointAtTime(Balls[i].my_t_inForm);
                    }
                    if (Balls[i].hint == 4)
                    {
                        Balls[i].BallPoint = obj4.CalcCurvePointAtTime(Balls[i].my_t_inForm);
                    }

                }

                Gate.MakeTransparent(Gate.GetPixel(0, 0));

                for (int i = 0; i < Balls.Count; i++)
                {
                    g2.DrawImage(Balls[i].imgs[Balls[i].iCurrFrame],
               Balls[i].BallPoint.X, Balls[i].BallPoint.Y, Balls[i].w, Balls[i].h);

                    //g2.FillEllipse(Brushes.Red, Balls[i].BallPoint.X + 20, Balls[i].BallPoint.Y + 20, 10, 10);
                }


                g2.DrawImage(Gate,
                 250, (this.ClientSize.Height / 2) - 90, 250, 190);

                if (frog != null)
                {
                    Bitmap bmp = new Bitmap((int)frog.w, (int)frog.h);
                    Graphics g3 = Graphics.FromImage(bmp);

                    //now we set the rotation point to the center of our image
                    g3.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

                    //now rotate the image
                    g3.RotateTransform((float)((rotation * 180 / Math.PI) - 90));

                    //now we return the transformation we applied
                    g3.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

                    //now draw our the new image
                    g3.DrawImage(frog.image, 0, 0, frog.w, frog.h);
                    g2.DrawImage(bmp, frog.x - (frog.w / 2), frog.y - (frog.h / 2), frog.w, frog.h);
                }





                for (int i = 0; i < ShootingBalls.Count; i++)
                {
                    if (!ShootingBalls[i].dead)
                    {
                        g2.DrawImage(ShootingBalls[i].imgs[ShootingBalls[i].iCurrFrame],
                        ShootingBalls[i].MovingBall.X - ShootingBalls[i].w / 2, ShootingBalls[i].MovingBall.Y - ShootingBalls[i].h / 2, ShootingBalls[i].w, ShootingBalls[i].h);
                    }

                }
                g2.DrawImage(HoldBall.imgs[HoldBall.iCurrFrame],
                      (HoldBall.BallPoint.X - HoldBall.w / 2), HoldBall.BallPoint.Y - HoldBall.h / 2, HoldBall.w, HoldBall.h);



            }


            //x axis lines

            //g2.DrawLine(new Pen(Color.Red, 3), (this.ClientSize.Width / 2) , 570, (this.ClientSize.Width / 2) + 700, 570);

            //g2.DrawLine(new Pen(Color.Red, 3), (this.ClientSize.Width / 2), 700, (this.ClientSize.Width / 2) + 700, 700);
            ////y axis lines
            //g2.DrawLine(new Pen(Color.Red, 3), (this.ClientSize.Width / 2) , 570, (this.ClientSize.Width / 2) , 700);

            //g2.DrawLine(new Pen(Color.Red, 3), (this.ClientSize.Width / 2) + 700, 570, (this.ClientSize.Width / 2) + 700, 700);

            //The image you want to rotate


        }
        void CreateBalls(int k)
        {
            Ball pnn = new Ball();
            for (int i = 0; i < 6; i++)
            {
                Bitmap im = new Bitmap("ball" + (i + 1) + ".png");
                im.MakeTransparent(im.GetPixel(0, 0));
                pnn.imgs.Add(im);
            }
            pnn.iCurrFrame = k;
            pnn.hint = 1;
            Balls.Add(pnn);

        }
        void CreateShootingBalls(int k, float endx, float endy)
        {
            ShootingBall pnn = new ShootingBall();
            for (int i = 0; i < 6; i++)
            {
                Bitmap im = new Bitmap("ball" + (i + 1) + ".png");
                im.MakeTransparent(im.GetPixel(0, 0));
                pnn.imgs.Add(im);
            }
            pnn.BallPoint.X = linex;
            pnn.BallPoint.Y = liney;
            pnn.EndPoint.X = endx;
            pnn.EndPoint.Y = endy;
            pnn.MovingBall = pnn.BallPoint;
            pnn.iCurrFrame = k;
            ShootingBalls.Add(pnn);

        }
        void CreateFrog()
        {
            frog = new Frog();
            frog.image = new Bitmap("frog.png");
            frog.x = 1000;
            frog.y = (this.ClientSize.Height / 2) - 30;
            frog.w = 190;
            frog.h = 190;

        }
        void CheckExplosion(int coll)
        {
            int ctexpl = 1;
            int rightside = -1;
            int leftside = -1;
            List<int> removings = new List<int>();
            removings.Add(coll);
            int clr1 = -1;
            int clr2 = -1;
            for (int i = coll + 1; i < Balls.Count(); i++)
            {
                if (Balls[i].iCurrFrame == Balls[coll].iCurrFrame)
                {
                    ctexpl++;
                    removings.Add(i);
                }
                else
                {
                    leftside = i;
                    clr2 = Balls[i].iCurrFrame;
                    break;
                }
            }
            for (int i = coll - 1; i >= 0; i--)
            {
                if (Balls[i].iCurrFrame == Balls[coll].iCurrFrame)
                {
                    
                    ctexpl++;
                    removings.Add(i);
                }
                else
                {
                    rightside = i;
                    clr1 = Balls[i].iCurrFrame;
                    break;
                }
            }
            removings.Sort();
            if (ctexpl >= 3)
            {
                if(leftside>=0 &&rightside>0)
                {
                    //Balls[leftside].hint = Balls[rightside +1 ].hint;

                    //Balls[leftside].my_t_inForm = Balls[rightside+1].my_t_inForm;
                    //Balls[leftside].BallPoint = Balls[rightside].BallPoint;

                    if (Balls[leftside].hint == 1)
                    {
                        while (true)
                        {
          
                            float dx2 = Balls[leftside].BallPoint.X - Balls[rightside].BallPoint.X;
                            float dy2 = Balls[leftside].BallPoint.Y - Balls[rightside].BallPoint.Y;
                            Balls[leftside].my_t_inForm += 0.0001f;
                            Balls[leftside].BallPoint = obj.CalcCurvePointAtTime(Balls[leftside].my_t_inForm);

                            if (Math.Sqrt((dy2 * dy2) + (dx2 * dx2)) <= Balls[0].w)
                            {

                                break;
                            }



                        }
                        if (Balls[leftside].my_t_inForm >= 1)
                        {
                            t.Stop();
                            MessageBox.Show("Game over");

                        }
                        Balls[leftside].BallPoint = obj.CalcCurvePointAtTime(Balls[leftside].my_t_inForm);
                    }
                    if (Balls[leftside].hint == 2)
                    {
                        while (true)
                        {

                            float dx2 = Balls[leftside].BallPoint.X - Balls[rightside].BallPoint.X;
                            float dy2 = Balls[leftside].BallPoint.Y - Balls[rightside].BallPoint.Y;
                            Balls[leftside].my_t_inForm += 0.0009f;
                            Balls[leftside].BallPoint = obj2.CalcCurvePointAtTime(Balls[leftside].my_t_inForm);

                            if (Math.Sqrt((dy2 * dy2) + (dx2 * dx2)) <= Balls[0].w)
                            {

                                break;
                            }



                        }
                        if (Balls[leftside].my_t_inForm >= 1)
                        {
                            t.Stop();
                            MessageBox.Show("Game over");

                        }
                        Balls[leftside].BallPoint = obj.CalcCurvePointAtTime(Balls[leftside].my_t_inForm);
                    }
                    if (Balls[leftside].hint == 3)
                    {
                        while (true)
                        {

                            float dx2 = Balls[leftside].BallPoint.X - Balls[rightside].BallPoint.X;
                            float dy2 = Balls[leftside].BallPoint.Y - Balls[rightside].BallPoint.Y;
                            Balls[leftside].my_t_inForm += 0.0001f;
                            Balls[leftside].BallPoint = obj3.CalcCurvePointAtTime(Balls[leftside].my_t_inForm);

                            if (Math.Sqrt((dy2 * dy2) + (dx2 * dx2)) <= Balls[0].w)
                            {

                                break;
                            }



                        }
                        if (Balls[leftside].my_t_inForm >= 1)
                        {
                            t.Stop();
                            MessageBox.Show("Game over");

                        }
                        Balls[leftside].BallPoint = obj.CalcCurvePointAtTime(Balls[leftside].my_t_inForm);
                    }
                    if (Balls[leftside].hint == 4)
                    {
                        while (true)
                        {

                            float dx2 = Balls[leftside].BallPoint.X - Balls[rightside].BallPoint.X;
                            float dy2 = Balls[leftside].BallPoint.Y - Balls[rightside].BallPoint.Y;
                            Balls[leftside].my_t_inForm += 0.0001f;
                            Balls[leftside].BallPoint = obj4.CalcCurvePointAtTime(Balls[leftside].my_t_inForm);

                            if (Math.Sqrt((dy2 * dy2) + (dx2 * dx2)) <= Balls[0].w)
                            {

                                break;
                            }



                        }
                        if (Balls[leftside].my_t_inForm >= 1)
                        {
                            t.Stop();
                            MessageBox.Show("Game over");

                        }

                    }

                }

                for (int i = removings.Count-1; i >=0 ; i--)
                {






          


                    Balls.RemoveAt(removings[i]);
                    removect++;

                    for (int j = i; j < removings.Count; j++)
                    {
                        if (j != removings.Count - 1)
                        {
                            removings[j + 1]--;
                        }

                    }

                }
                if (clr1 == clr2 && clr1 != -1 && rightside != -1)
                {
                    CheckExplosion(rightside);
                }
            }


        }
        int CheckCollision()
        {
            for (int i = 0; i < ShootingBalls.Count; i++)
            {
                if (!ShootingBalls[i].dead)
                {
                    for (int j = 0; j < Balls.Count; j++)
                    {
                        if (!ShootingBalls[i].dead)
                        {


                            float dx = (Balls[j].BallPoint.X + 20) - (ShootingBalls[i].MovingBall.X + 20);
                            float dy = (Balls[j].BallPoint.Y + 20) - (ShootingBalls[i].MovingBall.Y + 20);

                            if (Math.Sqrt((dy * dy) + (dx * dx)) <= Balls[j].w)
                            {

                                if (true)
                                {

                                    Ball pnn = new Ball();
                                    pnn.imgs = ShootingBalls[i].imgs;
                                    pnn.iCurrFrame = ShootingBalls[i].iCurrFrame;
                                    //Balls.Add(pnn);
                                    Balls.Insert(j, pnn);
                                    if (j == 0)
                                    {
                                        Balls[0].hint = Balls[0 + 1].hint;

                                        Balls[0].my_t_inForm = Balls[0 + 1].my_t_inForm;
                                        Balls[0].BallPoint = Balls[1].BallPoint;
                                        while (true)
                                        {
                                            float dx2 = (Balls[0].BallPoint.X + 20) - (Balls[0 + 1].BallPoint.X + 20);
                                            float dy2 = (Balls[0].BallPoint.Y + 20) - (Balls[0 + 1].BallPoint.Y + 20);
                                            Balls[0].my_t_inForm += 0.0001f;
                                            if (Balls[0].hint == 1)
                                            {
                                                Balls[0].BallPoint = obj.CalcCurvePointAtTime(Balls[0].my_t_inForm);
                                            }
                                            if (Balls[0].hint == 2)
                                            {
                                                Balls[0].BallPoint = obj2.CalcCurvePointAtTime(Balls[0].my_t_inForm);
                                            }
                                            if (Balls[0].hint == 3)
                                            {
                                                Balls[0].BallPoint = obj3.CalcCurvePointAtTime(Balls[0].my_t_inForm);
                                            }
                                            if (Balls[0].hint == 4)
                                            {
                                                Balls[0].BallPoint = obj4.CalcCurvePointAtTime(Balls[0].my_t_inForm);
                                            }
                                            if (Balls[0].my_t_inForm >= 1)
                                            {
                                                Balls[0].my_t_inForm = 0.0001f;
                                                Balls[0].hint++;

                                            }
                                            if (Math.Sqrt((dy2 * dy2) + (dx2 * dx2)) >= Balls[0].w)
                                            {
                                                //MessageBox.Show(Math.Sqrt((dy2 * dy2) + (dx2 * dx2)).ToString());
                                                break;


                                            }
                                        }

                                        ShootingBalls[i].dead = true;
                                        return j;
                                    }



                                    for (int f = j; f >= 0; f--)
                                    {
                                        if (f == 0)
                                        {
                                            while (true)
                                            {
                                                float dx3 = (Balls[f].BallPoint.X + 20) - (Balls[f + 1].BallPoint.X + 20);
                                                float dy3 = (Balls[f].BallPoint.Y + 20) - (Balls[f + 1].BallPoint.Y + 20);
                                                Balls[f].my_t_inForm += 0.0001f;
                                                if (Balls[f].hint == 1)
                                                {
                                                    Balls[f].BallPoint = obj.CalcCurvePointAtTime(Balls[f].my_t_inForm);
                                                }
                                                if (Balls[f].hint == 2)
                                                {
                                                    Balls[f].BallPoint = obj2.CalcCurvePointAtTime(Balls[f].my_t_inForm);
                                                }
                                                if (Balls[f].hint == 3)
                                                {
                                                    Balls[f].BallPoint = obj3.CalcCurvePointAtTime(Balls[f].my_t_inForm);
                                                }
                                                if (Balls[f].hint == 4)
                                                {
                                                    Balls[f].BallPoint = obj4.CalcCurvePointAtTime(Balls[f].my_t_inForm);
                                                }
                                                if (Balls[f].my_t_inForm >= 1)
                                                {
                                                    Balls[f].my_t_inForm = 0.0001f;
                                                    Balls[f].hint++;

                                                }
                                                if (Math.Sqrt((dy3 * dy3) + (dx3 * dx3)) >= Balls[0].w)
                                                {

                                                    break;
                                                }
                                            }
                                        }

                                        if (f > 0)
                                        {
                                            //MessageBox.Show("enter");
                                            //MessageBox.Show(f.ToString());
                                            Balls[f].hint = Balls[f - 1].hint;
                                            Balls[f].BallPoint = Balls[f - 1].BallPoint;
                                            Balls[f].my_t_inForm = Balls[f - 1].my_t_inForm;
                                        }

                                    }

                                    ShootingBalls[i].dead = true;
                                    return j;
                                }

                            }
                        }
                    }
                }


            }
            return -1;
        }
        void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }

    }
}
