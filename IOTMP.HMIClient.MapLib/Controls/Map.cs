using IOTMP.HMIClient.MapLib.Contract;
using IOTMP.HMIClient.MapLib.Layers;
using IOTMP.HMIClient.MapLib.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
namespace IOTMP.HMIClient.MapLib.Controls
{
    [TemplatePart(Name = clientAreaName, Type = typeof(Grid))]
    [TemplatePart(Name = stfName, Type = typeof(ScaleTransform))]
    [TemplatePart(Name = ttfName, Type = typeof(TranslateTransform))]
    [TemplatePart(Name = level0ImgName, Type = typeof(Image))]
    [TemplatePart(Name = gridDrawingBrushName, Type = typeof(DrawingBrush))]
    [TemplatePart(Name = GridSquareOffsetName, Type = typeof(TranslateTransform))]
    [TemplatePart(Name = LayerComboboxName, Type = typeof(ComboBox))]
    [TemplatePart(Name = BackgroundGridLineBorderName, Type = typeof(Border))]
    [TemplatePart(Name = layerContainerName, Type = typeof(ItemsControl))]
    [TemplatePart(Name = btn_draw_CancleName, Type = typeof(RadioButton))]
    [TemplatePart(Name = btn_draw_LineName, Type = typeof(RadioButton))]
    [TemplatePart(Name = btn_draw_RectangleName, Type = typeof(RadioButton))]
    [TemplatePart(Name = btn_draw_PolygonsName, Type = typeof(RadioButton))]
    [ContentProperty("Layers")]
    public class Map : Control
    {

        #region 方法

        static Map()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Map), new FrameworkPropertyMetadata(typeof(Map)));
        }

        //拖动
        private void ClientArea_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            hasPopClick = false;
        }
        private void clientArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            hasPopClick = true;

            point0 = e.GetPosition(clientArea);
            ttfx0 = ttf.X;
            ttfy0 = ttf.Y;

        }
        private void clientArea_MouseMove(object sender, MouseEventArgs e)
        {
            var point_temp = e.GetPosition(clientArea);
            var point_temp2 = e.GetPosition(level0Img);
            if (this.CoordConverter != null)
            {
                MouseX = CoordConverter.ConverterToUwbX(point_temp2.X);
                MouseY = CoordConverter.ConverterToUwbY(point_temp2.Y);
            }
            else
            {
                MouseX = point_temp2.X;
                MouseY = point_temp2.Y;
            }

            var dx = Math.Abs(point0.X - point_temp.X);
            var dy = Math.Abs(point0.Y - point_temp.Y);
            if (e.LeftButton == MouseButtonState.Pressed && (dx > 5 || dy > 5)
                && (dx < 50) && (dy < 50) && hasPopClick)
            {
                DataObject data = new DataObject();
                data.SetData("from", this.GetHashCode().ToString());

                DragDrop.DoDragDrop(clientArea, data, DragDropEffects.Move);
            }
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            layerContainer = Template.FindName(layerContainerName, this) as ItemsControl;
            BackgroundGridLineBorder = Template.FindName(BackgroundGridLineBorderName, this) as Border;
            BackgroundGridLineBorder = Template.FindName(BackgroundGridLineBorderName, this) as Border;

            clientArea = Template.FindName(clientAreaName, this) as Grid;
            stf = Template.FindName(stfName, this) as ScaleTransform;
            ttf = Template.FindName(ttfName, this) as TranslateTransform;
            level0Img = Template.FindName(level0ImgName, this) as Image;
            level0Img.Source = this.Background;
            gridDrawingBrush = Template.FindName(gridDrawingBrushName, this) as DrawingBrush;
            if (gridDrawingBrush != null)
            {
                gridDrawingBrush.Viewport = new Rect(default(Point), new Point(this.BackgroundGridSideLen, this.BackgroundGridSideLen));
                GridSquareOffset = Template.FindName(GridSquareOffsetName, this) as TranslateTransform;
            }
            if (GridSquareOffset != null)
            {
                GridSquareOffset.X = this.BackgroundGridOffsetX;
                GridSquareOffset.Y = this.BackgroundGridOffsetY;
            }
            LayerCombobox = Template.FindName(LayerComboboxName, this) as ComboBox;
            if (LayerCombobox != null)
            {
                LayerCombobox.SelectedIndex = 0;
            }

            clientArea.MouseMove -= clientArea_MouseMove;
            clientArea.MouseMove += clientArea_MouseMove;
            clientArea.PreviewMouseLeftButtonDown -= ClientArea_PreviewMouseLeftButtonDown; ;
            clientArea.PreviewMouseLeftButtonDown += ClientArea_PreviewMouseLeftButtonDown; ;
            clientArea.MouseLeftButtonDown -= clientArea_MouseLeftButtonDown;
            clientArea.MouseLeftButtonDown += clientArea_MouseLeftButtonDown;
            clientArea.DragOver += ClientArea_DragOver;
            clientArea.DragOver += ClientArea_DragOver;
            clientArea.GiveFeedback -= ClientArea_GiveFeedback;
            clientArea.GiveFeedback += ClientArea_GiveFeedback;
            clientArea.PreviewMouseWheel -= Grid_clientArea_PreviewMouseWheel;
            clientArea.PreviewMouseWheel += Grid_clientArea_PreviewMouseWheel;
            level0Img.SizeChanged -= Level0Img_SizeChanged;
            level0Img.SizeChanged += Level0Img_SizeChanged;

            this.RequestBringIntoView -= Map_RequestBringIntoView;
            this.RequestBringIntoView += Map_RequestBringIntoView;
        }




        private void Level0Img_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.LayerHeight = level0Img.ActualHeight;
            this.LayerWidth = level0Img.ActualWidth;
        }

        private void Map_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            if (e.OriginalSource is Map)
            {
                var distacn = level0Img.TranslatePoint(default(Point), this);
                var scaleTarget = new Point(e.TargetRect.Width * stf.ScaleX, e.TargetRect.Height * stf.ScaleY);
                if (scaleTarget.X.IsNaNumber() || distacn.X.IsNaNumber())
                    return;
                ttf.X += this.ActualWidth / 2 - (distacn.X + scaleTarget.X);
                ttf.Y += this.ActualHeight / 2 - (distacn.Y + scaleTarget.Y);
            }

        }

        private void ClientArea_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            Mouse.SetCursor(Cursors.Arrow);
            e.Handled = true;
        }

        private void ClientArea_DragOver(object sender, DragEventArgs e)
        {
            var point_temp2 = e.GetPosition(level0Img);
            if (this.CoordConverter != null)
            {
                MouseX = CoordConverter.ConverterToUwbX(point_temp2.X);
                MouseY = CoordConverter.ConverterToUwbY(point_temp2.Y);
            }
            else
            {
                MouseX = point_temp2.X;
                MouseY = point_temp2.Y;
            }
            if (e.Data.GetData("from") is string str && str == this.GetHashCode().ToString())
            {
                var point1 = e.GetPosition(clientArea);
                if (ttfx0.IsNaNumber() || point1.X.IsNaNumber())
                {
                    e.Handled = true;
                    return;
                }
                ttf.X = ttfx0 + (point1.X - point0.X);
                ttf.Y = ttfy0 + (point1.Y - point0.Y);

                e.Handled = true;
            }
        }
        //缩放
        private void Grid_clientArea_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var px0 = e.GetPosition(level0Img).X * 0.1;
            var py0 = e.GetPosition(level0Img).Y * 0.1;



            if (e.Delta > 0)
            {
                stf.ScaleX += 0.1;
                stf.ScaleY += 0.1;
                //放大
                if (px0.IsNaNumber() || py0.IsNaNumber())
                {
                    return;
                }

                ttf.X -= px0;
                ttf.Y -= py0;
            }
            else if (e.Delta < 0)
            {
                stf.ScaleX -= 0.1;
                stf.ScaleY -= 0.1;
                if (stf.ScaleX <= 0.1)
                {
                    stf.ScaleX = 0.1;
                    stf.ScaleY = 0.1;
                }
                else
                {
                    if (px0.IsNaNumber() || py0.IsNaNumber())
                    {
                        return;
                    }
                    //缩小
                    ttf.X += px0;
                    ttf.Y += py0;
                }
            }
        }

        public Map()
        {
            this.Layers = new ObservableCollection<FrameworkElement>();
            Layers.CollectionChanged += Layers_CollectionChanged;
        }

        private void Layers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (this.LayerCombobox != null)
                this.LayerCombobox.SelectedIndex = e.NewStartingIndex;
        }



        #endregion

        #region 依赖属性


        public double MouseX
        {
            get { return (double)GetValue(MouseXProperty); }
            set { SetValue(MouseXProperty, value); }
        }
        public static readonly DependencyProperty MouseXProperty =
            DependencyProperty.Register("MouseX", typeof(double), typeof(Map), new PropertyMetadata(0d));



        public double MouseY
        {
            get { return (double)GetValue(MouseYProperty); }
            set { SetValue(MouseYProperty, value); }
        }
        public static readonly DependencyProperty MouseYProperty =
            DependencyProperty.Register("MouseY", typeof(double), typeof(Map), new PropertyMetadata(0d));




        /// <summary>
        /// 底图
        /// </summary>
        public new ImageSource Background
        {
            get { return (ImageSource)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }
        public new static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(ImageSource), typeof(Map), new PropertyMetadata(null, (a, b) =>
            {
                if (a is Map board && board?.level0Img != null)
                {
                    if (!string.IsNullOrEmpty(b.NewValue?.ToString()))
                    {
                        var str = b.NewValue.ToString();
                        var path = str.Substring(8, str.Length - 8);
                        System.Drawing.Image img = System.Drawing.Image.FromFile(path);
                        board.level0Img.Width = img.Width;
                        board.level0Img.Height = img.Height;
                        board.level0Img.Source = BitmapFrame.Create(new Uri(b.NewValue.ToString()));
                    }
                    else
                    {
                        board.level0Img.Source = null;
                    }
                }
            }));


        /// <summary>
        /// 背景网格边长
        /// </summary>
        public int BackgroundGridSideLen
        {
            get { return (int)GetValue(BackgroundGridSideLenProperty); }
            set { SetValue(BackgroundGridSideLenProperty, value); }
        }
        public static readonly DependencyProperty BackgroundGridSideLenProperty =
            DependencyProperty.Register("BackgroundGridSideLen", typeof(int), typeof(Map), new PropertyMetadata(10, (a, b) =>
            {
                if (a is Map board && board.gridDrawingBrush != null)
                {
                    var sideLen = Math.Abs((int)b.NewValue);
                    board.gridDrawingBrush.Viewport = new Rect(default(Point), new Point(sideLen, sideLen));
                }
            }));



        /// <summary>
        /// 背景网格偏移量X
        /// </summary>
        public int BackgroundGridOffsetX
        {
            get { return (int)GetValue(BackgroundGridOffsetXProperty); }
            set { SetValue(BackgroundGridOffsetXProperty, value); }
        }
        public static readonly DependencyProperty BackgroundGridOffsetXProperty =
            DependencyProperty.Register("BackgroundGridOffsetX", typeof(int), typeof(Map), new PropertyMetadata(0, (a, b) =>
            {
                if (a is Map board && board.GridSquareOffset != null)
                {
                    var offset = -Math.Abs((int)b.NewValue);
                    board.GridSquareOffset.X = offset;
                }

            }));


        /// <summary>
        /// 背景网格偏移量y
        /// </summary>
        public int BackgroundGridOffsetY
        {
            get { return (int)GetValue(BackgroundGridOffsetYProperty); }
            set { SetValue(BackgroundGridOffsetYProperty, value); }
        }
        public static readonly DependencyProperty BackgroundGridOffsetYProperty =
            DependencyProperty.Register("BackgroundGridOffsetY", typeof(int), typeof(Map), new PropertyMetadata(0, (a, b) =>
            {
                if (a is Map board && board.GridSquareOffset != null)
                {
                    var offset = -Math.Abs((int)b.NewValue);
                    board.GridSquareOffset.Y = offset;
                }

            }));




        public ObservableCollection<FrameworkElement> Layers
        {
            get { return (ObservableCollection<FrameworkElement>)GetValue(LayersProperty); }
            set { SetValue(LayersProperty, value); }
        }
        public static readonly DependencyProperty LayersProperty =
            DependencyProperty.Register("Layers", typeof(ObservableCollection<FrameworkElement>), typeof(Map), new PropertyMetadata(new ObservableCollection<FrameworkElement>(), (a, b) =>
             {
                 if (a is Map board && board.LayerCombobox != null)
                 {
                     board.LayerCombobox.SelectedIndex = 0;
                 }
             }));







        public ScaleTransform stf
        {
            get { return (ScaleTransform)GetValue(stfProperty); }
            set { SetValue(stfProperty, value); }
        }
        public static readonly DependencyProperty stfProperty =
            DependencyProperty.Register("stf", typeof(ScaleTransform), typeof(Map), new PropertyMetadata(null));




        public TranslateTransform ttf
        {
            get { return (TranslateTransform)GetValue(ttfProperty); }
            set { SetValue(ttfProperty, value); }
        }

        public static readonly DependencyProperty ttfProperty =
            DependencyProperty.Register("ttf", typeof(TranslateTransform), typeof(Map), new PropertyMetadata(null));



        /// <summary>
        /// 坐标转换器/网格大小/平面图尺寸
        /// </summary>
        public ICoordConverter CoordConverter
        {
            get { return (ICoordConverter)GetValue(CoordConverterProperty); }
            set { SetValue(CoordConverterProperty, value); }
        }

        public static readonly DependencyProperty CoordConverterProperty =
            DependencyProperty.Register("CoordConverter", typeof(ICoordConverter), typeof(Map), new PropertyMetadata(null));



        /// <summary>
        /// 平面图宽,用于传送值到viewmodel,绑定时用onwaytosource
        /// </summary>
        public double LayerWidth
        {
            get { return (double)GetValue(LayerWidthProperty); }
            set { SetValue(LayerWidthProperty, value); }
        }
        public static readonly DependencyProperty LayerWidthProperty =
            DependencyProperty.Register("LayerWidth", typeof(double), typeof(Map), new PropertyMetadata(0d));


        /// <summary>
        /// 平面图高,用于传送值到viewmodel,绑定时用onwaytosource
        /// </summary>
        public double LayerHeight
        {
            get { return (double)GetValue(LayerHeightProperty); }
            set { SetValue(LayerHeightProperty, value); }
        }
        public static readonly DependencyProperty LayerHeightProperty =
            DependencyProperty.Register("LayerHeight", typeof(double), typeof(Map), new PropertyMetadata(0d));



        public Visibility DrawToolVisibility
        {
            get { return (Visibility)GetValue(DrawToolVisibilityProperty); }
            set { SetValue(DrawToolVisibilityProperty, value); }
        }
        public static readonly DependencyProperty DrawToolVisibilityProperty =
            DependencyProperty.Register("DrawToolVisibility", typeof(Visibility), typeof(Map), new PropertyMetadata(Visibility.Collapsed, (a, b) =>
             {
                 if (a is Map m && b.NewValue is Visibility v)
                 {
                     if (v != Visibility.Visible)
                     {
                         m.btn_draw_Cancle.IsChecked = true;
                     }
                     else
                     {
                         m.btn_draw_Line.IsChecked = true;
                     }
                 }

             }));



        #endregion


        #region 属性字段


        private const string clientAreaName = "clientArea";
        public Grid clientArea;

        private const string stfName = "stf";

        private const string ttfName = "ttf";

        private const string level0ImgName = "level0Img";
        public Image level0Img;

        private const string gridDrawingBrushName = "gridDrawingBrush";
        private DrawingBrush gridDrawingBrush;

        private const string GridSquareOffsetName = "GridSquareOffset";
        private TranslateTransform GridSquareOffset;

        private const string LayerComboboxName = "LayerCombobox";
        private ComboBox LayerCombobox;

        private const string BackgroundGridLineBorderName = "gridLine";
        private Border BackgroundGridLineBorder;


        private const string btn_draw_CancleName = "btn_draw_Cancle";
        private const string btn_draw_LineName = "btn_draw_Line";
        private const string btn_draw_RectangleName = "btn_draw_Rectangle";
        private const string btn_draw_PolygonsName = "btn_draw_Polygons";
        private RadioButton btn_draw_Cancle;
        private RadioButton btn_draw_Line;
        private RadioButton btn_draw_Rectangle;
        private RadioButton btn_draw_Polygons;

        private const string layerContainerName = "layerContainer";
        private ItemsControl layerContainer;


        private Point point0;
        private double ttfx0;
        private double ttfy0;
        private bool hasPopClick;


        #endregion
    }
}
