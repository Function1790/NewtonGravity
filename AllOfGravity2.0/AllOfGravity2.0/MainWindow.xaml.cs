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
        List<Thing> ExperimentObjects = new List<Thing>();
        DispatcherTimer NewtonEngin = new DispatcherTimer();
        const double GRAVITY_CONSTANT = 10;

        void Log(string title, object content)
        {
            Console.WriteLine($"[{title}] >> {content}");
        }

        void print(object content)
        {
            Console.WriteLine(content);
        }

        void FinishedLoad()
        {
            NewtonEngin.Start();
        }

        double Square(double n) { return n * n; }

        void GravityForceBetween(Thing A, Thing B)
        {
            double Δx = -(A.Position.X - B.Position.X);
            double Δy = -(A.Position.Y - B.Position.Y);
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

        //Main
        void main()
        {
            NewtonEngin.Interval = TimeSpan.FromMilliseconds(1);
            NewtonEngin.Tick += new EventHandler(NewtonEngin_Tick);

            ExperimentObjects.Add(ObjectA);
            ExperimentObjects.Add(ObjectB);
            ExperimentObjects.Add(ObjectC);
            ExperimentObjects.Add(ObjectD);
            ExperimentObjects.Add(ObjectE);
            ExperimentObjects.Add(ObjectF);

            FinishedLoad();
        }

        void NewtonEngin_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < ExperimentObjects.Count; i++)
            {
                ExperimentObjects[i].Move();
                if (ExperimentObjects.Count - 1 == i) continue;
                for (int j = i + 1; j < ExperimentObjects.Count; j++)
                {
                    GravityForceBetween(ExperimentObjects[i], ExperimentObjects[j]);
                }
            }
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
    }
}
