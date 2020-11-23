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
using TestDemo.NewCenterStorage.Pages2;

namespace TestDemo.NewCenterStorage.Pages2
{
    public class Mypoint : Prism.Mvvm.BindableBase
    {
        public Mypoint()
        {

        }
        public Mypoint(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        private double _x;
        public double X
        {
            get { return _x; }
            set { SetProperty(ref _x, value); }
        }

        private double _y;
        public double Y
        {
            get { return _y; }
            set { SetProperty(ref _y, value); }
        }
    }
}
namespace TestDemo.NewCenterStorage.Pages
{
    /// <summary>
    /// aaa.xaml 的交互逻辑
    /// </summary>
    public partial class aaa : UserControl
    {
        public aaa()
        {
            InitializeComponent();
            TestPointSet = new List<Mypoint>
            {
                new Mypoint(10,10),
                new Mypoint(30,30),
                new Mypoint(50,50),
                new Mypoint(70,70),
                new Mypoint(90,90),
            };
        }


        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("图层元素被点击");
            e.Handled = true;

        }

        #region 依赖属性


        public List<Mypoint> TestPointSet
        {
            get { return (List<Mypoint>)GetValue(TestPointSetProperty); }
            set { SetValue(TestPointSetProperty, value); }
        }
        public static readonly DependencyProperty TestPointSetProperty =
            DependencyProperty.Register("TestPointSet", typeof(List<Mypoint>), typeof(aaa), new PropertyMetadata(null));


        #endregion
    }
}
