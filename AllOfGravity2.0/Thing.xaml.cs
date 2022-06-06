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
    //Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate { loadData(); }));
    /// <summary>
    /// Thing.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Thing : UserControl
    {
        Vector Speed = new Vector(0, 0);
        double mass = 10;

        public int life = 1000;

        void Log(string title, object content)
        {
            Console.WriteLine($"[{title}] >> {content}");
        }

        /// <summary>
        /// await(new Action(delegate {}))
        /// </summary>
        private void AwaitCode(Action code)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate {  }));
        }

        public Thing()
        {
            InitializeComponent();
        }

        double absolute(double value)
        {
            if (value < 0) return -value;
            return value;
        }

        public Color BackGroundColor
        {
            set
            {
                BG.Fill = new SolidColorBrush(Color.FromArgb(30, value.R, value.G, value.B));
                BG.Stroke = new SolidColorBrush(value);
            }
        }

        public bool DebugMode
        {
            set
            {
                if (value)
                {
                    VectorX.Visibility = Visibility.Visible;
                    VectorY.Visibility = Visibility.Visible;
                    DebugViewer.Visibility = Visibility.Visible;
                }
                else
                {
                    VectorX.Visibility = Visibility.Hidden;
                    VectorY.Visibility = Visibility.Hidden;
                    DebugViewer.Visibility = Visibility.Hidden;
                }
            }
        }

        void UpdateDebugData()
        {
            DebugViewer.Text = "[ mass ]\n";
            DebugViewer.Text += $"{mass}\n";
            DebugViewer.Text += "[ velocity ]\n";
            DebugViewer.Text += $"{toString(Speed)}\n";
            DebugViewer.Text += "[ position ]\n";
            DebugViewer.Text += $"{toString(Position)}\n";
        }

        public void ApplyForce(Vector F)
        {
            //Log("Apply", $"V{Velocity}\tF {F}");
            if(absolute(F.X) < 1000 && F.X != 0)   Vx += F.X / Mass;
            if(absolute(F.Y) < 1000 && F.Y != 0)   Vy += F.Y / Mass;
            //Log("Apply", $"V {Velocity}");
        }

        string toString(Vector v)
        {
            return String.Format("({0:0.00}, {1:0.00})", v.X, v.Y);
        }

        Vector CopyVector(Vector toCopy)
        {
            return new Vector(toCopy.X, toCopy.Y);
        }

        public bool Move()
        {
            if(Speed.X == 0 & Speed.Y == 0)
            {
                return false;
            }

            //Log("M", $"S:{Speed}, P:{Position}");
            Position += Speed / 10;
            //Log("M2", $"S:{Speed}, P:{Position}");
            if (Position.X < 0)
            {
                Margin = new Thickness(0, Margin.Top, 0, 0);
            }
            else if(Position.X > 2300)
            {
                Margin = new Thickness(2300, Margin.Top, 0, 0);
            }
            if (Position.Y < 0)
            {
                Margin = new Thickness(Margin.Left, 0, 0, 0);
            }
            else if (Position.Y > 1300)
            {
                Margin = new Thickness(Margin.Left, 1300, 0, 0);
            }

            return true;
        }

        // GET SET
        public double Mass
        {
            get
            {
                return mass;
            }
            set
            {
                mass = value;
                Width = value;
                Height = value;
                if(value > 100)
                {
                    Width = 100 + value / 100;
                    Height = Width;
                }
                UpdateDebugData();
            }
        }

        public Vector Position
        {
            get
            {
                return new Vector(Margin.Left, Margin.Top);
            }
            set
            {
                //*Await하기
               // Log("V", $"S:{Speed}, P:{value.ToString()}");
                Margin = new Thickness(value.X, value.Y, 0, 0);
                UpdateDebugData();
            }
        }

        public Vector Center
        {
            get
            {
                return this.Position/2;
            }
        }

        public Vector Velocity
        {
            get
            {
                return Speed;
            }
            set
            {
                Speed = CopyVector(value);
                UpdateDebugData();
            }
        }

        /// <summary>
        /// Speed X
        /// </summary>
        public double Vx
        {
            get
            {
                return Speed.X;
            }
            set
            {
                Speed.X = value;
                UpdateDebugData();
            }
        }
        
        /// <summary>
        /// Speed Y
        /// </summary>
        public double Vy
        {
            get
            {
                return Speed.Y;
            }
            set
            {
                Speed.Y = value;
                UpdateDebugData();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
