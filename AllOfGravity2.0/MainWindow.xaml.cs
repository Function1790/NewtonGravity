using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AllOfGravity2._0
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        //Constnat
        const double GRAVITY_CONSTANT = 10;

        //Variable

        //Velocity Variable
        bool isHeldLeftButton = false;
        Vector startHeldPosition;
        Line VelocityViewer = new Line();

        //Systen Variable
        List<Thing> ExperimentObjects = new List<Thing>();
        DispatcherTimer NewtonEngin = new DispatcherTimer();
        
        Random Rand;

        //Main
        void main()
        {
            Log("Start", "Loading");
            Rand = new Random();
            NewtonEngin.Interval = TimeSpan.FromMilliseconds(1);
            NewtonEngin.Tick += new EventHandler(NewtonEngin_Tick);

            /*
            ExperimentObjects.Add(ObjectA);
            ExperimentObjects.Add(ObjectB);
            ExperimentObjects.Add(ObjectC);
            ExperimentObjects.Add(ObjectD);
            ExperimentObjects.Add(ObjectE);
            ExperimentObjects.Add(ObjectF);
            */

            Log("Finish", "Loading");
            FinishedLoad();
        }

        //About System
        void Log(string title, object content)
        {
            //Console.WriteLine($"[{title}] >> {content}");
        }

        void print(object content)
        {
            Console.WriteLine(content);
        }

        /// <summary>
        /// await(new Action(delegate {}))
        /// </summary>
        private void AwaitCode(Action code)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate { }));
        }

        void FinishedLoad()
        {
            NewtonEngin.Start();
        }

        //Operation Engin
        /*/
         * 최적화 필요
        /*/
        double Square(double n) { return n * n; }

        void GravityForceBetween(Thing A, Thing B)
        {
            Vector CenterA = A.Center;
            Vector CenterB = B.Center;
            double Δx = -(CenterA.X - CenterB.X);
            double Δy = -(CenterA.Y - CenterB.Y);
            double r = Math.Sqrt(Square(Δx) + Square(Δy));
            

            double Fg = (GRAVITY_CONSTANT * A.Mass * B.Mass) / Square(r);

            //about A
            double sinA = Δy / r;
            double cosA = Δx / r;

            Vector F = new Vector(Fg * cosA, Fg * sinA);

            A.ApplyForce(F);
            B.ApplyForce(-F);

            //Console.WriteLine(F);
        }

        void NewtonEngin_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < ExperimentObjects.Count; i++)
            {
                ExperimentObjects[i].Move();
                //ExperimentObjects[i].life--;
                //if (ExperimentObjects.Count - 1 == i) continue;
                for (int j = i + 1; j < ExperimentObjects.Count; j++)
                {
                    GravityForceBetween(ExperimentObjects[i], ExperimentObjects[j]);
                }
            }
            /*for (int i = ExperimentObjects.Count - 1; i >= 0; i--)
            {
                if (ExperimentObjects[i].life <= 0)
                {
                    WorldGrid.Children.Remove(ExperimentObjects[i]);
                    ExperimentObjects.RemoveAt(i);
                }
            }*/
        }  

        //About Object
        void OnCreatingNewObject(Point mp)
        {
            isHeldLeftButton = false;
            int mass = Convert.ToInt32(InputMass.Text);
            Vector applySpeed = (toVector(mp) - startHeldPosition)/10;
            
            if (mass > 0)
            {
                CreateNewExperimentObject(toPoint(startHeldPosition), mass, applySpeed);
            }
            else
            {
                Log("ValueError", "InputMass.Text <= 0");
            }
        }

        void CreateNewExperimentObject(Point pos, int mass, Vector speed)
        {
            int n = ExperimentObjects.Count;
            ExperimentObjects.Add(new Thing());

            double halfOfMass = mass / 2;
            if (mass > 100)
            {
                halfOfMass = 0;
            }
            ExperimentObjects[n].Margin = new Thickness(pos.X - halfOfMass, pos.Y - halfOfMass, 0, 0);
            ExperimentObjects[n].Mass = mass;
            byte R = (byte)Rand.Next(0, 255);
            byte G = (byte)Rand.Next(0, 255);
            byte B = (byte)Rand.Next(0, 255);
            ExperimentObjects[n].BackGroundColor = Color.FromRgb(R, G, B);
            ExperimentObjects[n].Velocity = speed;

            WorldGrid.Children.Add(ExperimentObjects[n]);

            Log("Create", $"Object[{n}] -> Mass : {mass}\tPosition : ({ExperimentObjects[n].Position})");
        }

        void CancelDrawingVelocityViwer()
        {
            isHeldLeftButton = false;
            WorldGrid_Canvas.Children.Remove(VelocityViewer);
        }

        //Convert Unit
        Vector toVector(Point p)
        {
            return new Vector(p.X, p.Y);
        }
        
        Point toPoint(Vector p)
        {
            return new Point(p.X, p.Y);
        }

        //Event
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            main();
        }

        private void WorldGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point mp = e.GetPosition(WorldGrid);
                startHeldPosition = toVector(mp);
                isHeldLeftButton = true;

                VelocityViewer = new Line();
                VelocityViewer.X1 = mp.X;
                VelocityViewer.Y1 = mp.Y;
                VelocityViewer.Stroke = Brushes.Red;
                VelocityViewer.StrokeThickness = 1;
                VelocityViewer.StrokeDashArray = null;

                WorldGrid_Canvas.Children.Add(VelocityViewer);
            }
            if (e.LeftButton == MouseButtonState.Released && isHeldLeftButton)
            {
                CancelDrawingVelocityViwer();
            }
        }

        private void WorldGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (isHeldLeftButton)
            {
                if(e.LeftButton == MouseButtonState.Released)
                {
                    CancelDrawingVelocityViwer();
                }
                Point mp = e.GetPosition(WorldGrid);
                VelocityViewer.X2 = mp.X;
                VelocityViewer.Y2 = mp.Y;
            }
            
        }

        private void WorldGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released && isHeldLeftButton)
            {
                Point mp = e.GetPosition(WorldGrid);
                try
                {
                    OnCreatingNewObject(mp);
                    WorldGrid_Canvas.Children.Remove(VelocityViewer);
                }
                catch (Exception err)
                {
                    CancelDrawingVelocityViwer();
                    Log("Error", err.ToString());
                }
            }
        }
    }
}
