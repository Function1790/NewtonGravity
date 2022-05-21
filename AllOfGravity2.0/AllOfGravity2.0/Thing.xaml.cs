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

namespace AllOfGravity2._0
{
    /// <summary>
    /// Thing.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Thing : UserControl
    {
        Vector Speed;
        double mass = 0;

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
            if(absolute(F.X) < 1000)   Vx += F.X / Mass;
            if(absolute(F.Y) < 1000)   Vy += F.Y / Mass;
        }

        string toString(Vector v)
        {
            return $"({v.X}, {v.Y})";
        }

        Vector CopyVector(Vector toCopy)
        {
            return new Vector(toCopy.X, toCopy.Y);
        }

        public void Move()
        {
            Position += Speed / 10;
            if(Position.X < 0)
            {
                Margin = new Thickness(0, Margin.Top, 0, 0);
            }
            if (Position.Y < 0)
            {
                Margin = new Thickness(Margin.Left, 0, 0, 0);
            }
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
                Margin = new Thickness(value.X, value.Y, 0, 0);
                UpdateDebugData();
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
