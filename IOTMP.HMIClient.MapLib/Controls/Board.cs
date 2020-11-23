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
    [ContentProperty("Layers")]
    public class Board : Control
    {

        #region 方法
        static Board()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Board), new FrameworkPropertyMetadata(typeof(Board)));
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

            Console.WriteLine("board 被点击");
        }
        private void clientArea_MouseMove(object sender, MouseEventArgs e)
        {
            var point_temp = e.GetPosition(clientArea);
            var point_temp2 = e.GetPosition(level0Img);
            MouseX = point_temp2.X;
            MouseY = point_temp2.Y;

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
        }

        private void ClientArea_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            Mouse.SetCursor(Cursors.SizeAll);
            e.Handled = true;
        }

        private void ClientArea_DragOver(object sender, DragEventArgs e)
        {
            var point_temp2 = e.GetPosition(level0Img);
            MouseX = point_temp2.X;
            MouseY = point_temp2.Y;

            if (e.Data.GetData("from") is string str && str == this.GetHashCode().ToString())
            {
                var point1 = e.GetPosition(clientArea);
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
                    //缩小
                    ttf.X += px0;
                    ttf.Y += py0;
                }
            }
        }

        #endregion

        #region 依赖属性



        public double MouseX
        {
            get { return (double)GetValue(MouseXProperty); }
            set { SetValue(MouseXProperty, value); }
        }
        public static readonly DependencyProperty MouseXProperty =
            DependencyProperty.Register("MouseX", typeof(double), typeof(Board), new PropertyMetadata(0d));



        public double MouseY
        {
            get { return (double)GetValue(MouseYProperty); }
            set { SetValue(MouseYProperty, value); }
        }
        public static readonly DependencyProperty MouseYProperty =
            DependencyProperty.Register("MouseY", typeof(double), typeof(Board), new PropertyMetadata(0d));




        /// <summary>
        /// 底图
        /// </summary>
        public new ImageSource Background
        {
            get { return (ImageSource)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }
        public new static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(ImageSource), typeof(Board), new PropertyMetadata(null, (a, b) =>
            {
                if (a is Board board && board.level0Img != null)
                {
                    board.level0Img.Source = BitmapFrame.Create(new Uri(b.NewValue.ToString()));
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
            DependencyProperty.Register("BackgroundGridSideLen", typeof(int), typeof(Board), new PropertyMetadata(10, (a, b) =>
            {
                if (a is Board board && board.gridDrawingBrush != null)
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
            DependencyProperty.Register("BackgroundGridOffsetX", typeof(int), typeof(Board), new PropertyMetadata(0, (a, b) =>
            {
                if (a is Board board && board.GridSquareOffset != null)
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
            DependencyProperty.Register("BackgroundGridOffsetY", typeof(int), typeof(Board), new PropertyMetadata(0, (a, b) =>
            {
                if (a is Board board && board.GridSquareOffset != null)
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
            DependencyProperty.Register("Layers", typeof(ObservableCollection<FrameworkElement>), typeof(Board), new PropertyMetadata(new ObservableCollection<FrameworkElement>(), (a, b) =>
             {
                 if (a is Board board && board.LayerCombobox != null)
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
            DependencyProperty.Register("stf", typeof(ScaleTransform), typeof(Board), new PropertyMetadata(null));



        #endregion

        #region 属性字段

        private const string clientAreaName = "clientArea";
        public Grid clientArea;

        private const string stfName = "stf";

        private const string ttfName = "ttf";
        public TranslateTransform ttf;

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

        private Point point0;
        private double ttfx0;
        private double ttfy0;
        private bool hasPopClick;


        #endregion
    }
}
