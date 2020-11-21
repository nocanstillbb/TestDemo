using DrawingBoard.Utilities;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DrawingBoard.Controls
{
    /// <summary>
    /// LayerElement.xaml 的交互逻辑
    /// </summary>
    [ContentProperty("Child")]
    public partial class LayerElement : UserControl
    {
        public LayerElement()
        {
            InitializeComponent();
            this.SizeChanged += LayerElement_SizeChanged;
            this.Loaded += LayerElement_Loaded;
            this.PreviewMouseLeftButtonDown += LayerElement_PreviewMouseLeftButtonDown;
            this.MouseLeftButtonDown += LayerElement_MouseLeftButtonDown;
            this.MouseMove += LayerElement_MouseMove;

        }

        private void LayerElement_MouseMove(object sender, MouseEventArgs e)
        {
            if (haspopMouseClick && e.LeftButton == MouseButtonState.Pressed)
            {
                DataObject data = new DataObject();
                data.SetData("from",this.GetHashCode().ToString());

                DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
            }
        }

        private void LayerElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.EnableDrag && Parent is Canvas c)
            {
                e.Handled = true;
                haspopMouseClick = true;
                _top0 = this.Top;
                _left0 = this.Left;
                _point0 = e.GetPosition(c);


            }
        }

        private void LayerElement_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            haspopMouseClick = false;
            _top0 = 0d;
            _left0 = 0d;
            _point0 = default(Point);
        }

        private void LayerElement_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is UIElement uc)
            {
                var boad = uc.GetParent<Board>();
                if (boad?.clientArea != null)
                {
                    boad.clientArea.DragOver -= Boad_DragOver;
                    boad.clientArea.DragOver += Boad_DragOver;
                }
            }
        }

        private void Boad_DragOver(object sender, DragEventArgs e)
        {
            if (Parent is Canvas c  && e.Data.GetData("from") is string str && str == this.GetHashCode().ToString())
            {
                var point1 = e.GetPosition(c);
                Console.WriteLine(point1);
                this.Left = _left0 + point1.X - _point0.X;
                this.Top = _top0 + point1.Y - _point0.Y;

            }
        }

        private void LayerElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            switch (CenterModel)
            {
                case Enum.ScaleCenterEnum.Center:
                    this.stf.CenterX = ActualWidth / 2;
                    this.stf.CenterY = ActualHeight / 2;
                    break;
                case Enum.ScaleCenterEnum.TopLeft:
                    this.stf.CenterX = 0;
                    this.stf.CenterY = 0;
                    break;
                case Enum.ScaleCenterEnum.TopRight:
                    this.stf.CenterX = ActualWidth;
                    this.stf.CenterY = 0;
                    break;
                case Enum.ScaleCenterEnum.BottomRight:
                    this.stf.CenterX = ActualWidth;
                    this.stf.CenterY = ActualHeight;
                    break;
                case Enum.ScaleCenterEnum.BottomLeft:
                    this.stf.CenterX = 0;
                    this.stf.CenterY = ActualHeight;
                    break;
                default:
                    break;
            }
            this.Top += 0.0001;
            this.Left += 0.0001;
        }

        #region 依赖属性


        public Enum.ScaleCenterEnum CenterModel
        {
            get { return (Enum.ScaleCenterEnum)GetValue(CenterModelProperty); }
            set { SetValue(CenterModelProperty, value); }
        }
        public static readonly DependencyProperty CenterModelProperty =
            DependencyProperty.Register("CenterModel", typeof(Enum.ScaleCenterEnum), typeof(LayerElement), new PropertyMetadata(Enum.ScaleCenterEnum.Center, (a, b) =>
             {
                 if (a is LayerElement layerElement)
                 {
                     layerElement.LayerElement_SizeChanged(null, null);
                 }
             }));



        public double Top
        {
            get { return (double)GetValue(TopProperty); }
            set { SetValue(TopProperty, value); }
        }
        public static readonly DependencyProperty TopProperty =
            DependencyProperty.Register("Top", typeof(double), typeof(LayerElement), new PropertyMetadata(0d, SetCanvasAP));

        private static void SetCanvasAP(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LayerElement uc)
            {
                Canvas.SetTop(uc, uc.Top - uc.stf.CenterY);
                Canvas.SetLeft(uc, uc.Left - uc.stf.CenterX);
            }
        }

        public double Left
        {
            get { return (double)GetValue(LeftProperty); }
            set { SetValue(LeftProperty, value); }
        }
        public static readonly DependencyProperty LeftProperty =
            DependencyProperty.Register("Left", typeof(double), typeof(LayerElement), new PropertyMetadata(0d, SetCanvasAP));



        public double UwbX
        {
            get { return (double)GetValue(UwbXProperty); }
            set { SetValue(UwbXProperty, value); }
        }
        public static readonly DependencyProperty UwbXProperty =
            DependencyProperty.Register("UwbX", typeof(double), typeof(LayerElement), new PropertyMetadata(0d, TransformUwbXY));

        private static void TransformUwbXY(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LayerElement uc)
            {
                uc.Top = uc.UwbY;
                uc.Left = uc.UwbX;
            }
        }

        public double UwbY
        {
            get { return (double)GetValue(UwbYProperty); }
            set { SetValue(UwbYProperty, value); }
        }
        public static readonly DependencyProperty UwbYProperty =
            DependencyProperty.Register("UwbY", typeof(double), typeof(LayerElement), new PropertyMetadata(0d, TransformUwbXY));




        public UIElement Child
        {
            get { return (UIElement)GetValue(ChildProperty); }
            set { SetValue(ChildProperty, value); }
        }
        public static readonly DependencyProperty ChildProperty =
            DependencyProperty.Register("Child", typeof(UIElement), typeof(LayerElement), new PropertyMetadata(null));





        public bool EnableDrag
        {
            get { return (bool)GetValue(EnableDragProperty); }
            set { SetValue(EnableDragProperty, value); }
        }
        public static readonly DependencyProperty EnableDragProperty =
            DependencyProperty.Register("EnableDrag", typeof(bool), typeof(LayerElement), new PropertyMetadata(false));
        private bool haspopMouseClick;
        private double _top0;
        private double _left0;
        private Point _point0;
        private Grid _clientArea;



        #endregion
    }
}
