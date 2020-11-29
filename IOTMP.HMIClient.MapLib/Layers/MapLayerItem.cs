using IOTMP.HMIClient.MapLib.Controls;
using IOTMP.HMIClient.MapLib.Utilities;
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

namespace IOTMP.HMIClient.MapLib.Layers
{
    [ContentProperty("Content")]
    public class MapLayerItem : Control
    {
        ScaleTransform stf
        {
            get
            {
                return (this.RenderTransform as TransformGroup)?.Children?.FirstOrDefault() as ScaleTransform;
            }
        }
        private bool haspopMouseClick;
        private double _top0;
        private double _left0;
        private Point _point0;
        private Map _board;


        static MapLayerItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MapLayerItem), new FrameworkPropertyMetadata(typeof(MapLayerItem)));
        }


        private void CanvasItemsControlItem_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            SetCanvasTopLeft(this);
            this.DataContextChanged += CanvasItemsControlItem_DataContextChanged;

        }

        public MapLayerItem()
        {
            DefaultStyleKey = typeof(MapLayerItem);
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
                data.SetData("from", this.GetHashCode().ToString());

                DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
            }
        }

        private void LayerElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.EnableDrag && _board != null)
            {
                e.Handled = true;
                haspopMouseClick = true;
                _top0 = this.Top;
                _left0 = this.Left;
                _point0 = e.GetPosition(_board.level0Img);


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
                _board = uc.GetParent<Map>();
                if (_board?.clientArea != null)
                {
                    _board.clientArea.DragOver -= Boad_DragOver;
                    _board.clientArea.DragOver += Boad_DragOver;
                }
            }
        }

        private void Boad_DragOver(object sender, DragEventArgs e)
        {
            if (_board != null && e.Data.GetData("from") is string str && str == this.GetHashCode().ToString())
            {
                var point1 = e.GetPosition(_board.level0Img);
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
            if (!(ActualWidth == 0 || ActualHeight == 0))
            {
                Left += 1; Top += 1;
                Left -= 1; Top -= 1;
            }
        }

        #region 依赖属性


        public bool FixSizeOnScale
        {
            get { return (bool)GetValue(FixSizeOnScaleProperty); }
            set { SetValue(FixSizeOnScaleProperty, value); }
        }
        public static readonly DependencyProperty FixSizeOnScaleProperty =
            DependencyProperty.Register("FixSizeOnScale", typeof(bool), typeof(MapLayerItem), new PropertyMetadata(false));



        public Enum.ScaleCenterEnum CenterModel
        {
            get { return (Enum.ScaleCenterEnum)GetValue(CenterModelProperty); }
            set { SetValue(CenterModelProperty, value); }
        }
        public static readonly DependencyProperty CenterModelProperty =
            DependencyProperty.Register("CenterModel", typeof(Enum.ScaleCenterEnum), typeof(MapLayerItem), new PropertyMetadata(Enum.ScaleCenterEnum.Center, (a, b) =>
            {
                if (a is MapLayerItem layerElement)
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
            DependencyProperty.Register("Top", typeof(double), typeof(MapLayerItem), new PropertyMetadata(0d, SetCanvasAP));

        private static void SetCanvasAP(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MapLayerItem uc && uc.stf != null)
            {
                SetCanvasTopLeft(uc);
            }
        }

        private static void SetCanvasTopLeft(MapLayerItem uc)
        {
            Canvas.SetTop(uc, uc.Top - uc.stf.CenterY);
            Canvas.SetLeft(uc, uc.Left - uc.stf.CenterX);
        }

        public double Left
        {
            get { return (double)GetValue(LeftProperty); }
            set { SetValue(LeftProperty, value); }
        }
        public static readonly DependencyProperty LeftProperty =
            DependencyProperty.Register("Left", typeof(double), typeof(MapLayerItem), new PropertyMetadata(0d, SetCanvasAP));





        public bool EnableDrag
        {
            get { return (bool)GetValue(EnableDragProperty); }
            set { SetValue(EnableDragProperty, value); }
        }
        public static readonly DependencyProperty EnableDragProperty =
            DependencyProperty.Register("EnableDrag", typeof(bool), typeof(MapLayerItem), new PropertyMetadata(false));


        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(MapLayerItem), new PropertyMetadata(null));





        public DataTemplate DataTemplate
        {
            get { return (DataTemplate)GetValue(DataTemplateProperty); }
            set { SetValue(DataTemplateProperty, value); }
        }
        public static readonly DependencyProperty DataTemplateProperty =
            DependencyProperty.Register("DataTemplate", typeof(DataTemplate), typeof(MapLayerItem), new PropertyMetadata(null));






        #endregion

    }
}
