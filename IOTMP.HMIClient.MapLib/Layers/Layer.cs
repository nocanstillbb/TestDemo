using IOTMP.HMIClient.MapLib.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace IOTMP.HMIClient.MapLib.Layers
{
    public abstract class Layer : Canvas
    {
        protected ScaleTransform MapScaleTransform = new ScaleTransform();



        public double ScaleX
        {
            get { return (double)GetValue(ScaleXProperty); }
            set { SetValue(ScaleXProperty, value); }
        }
        public static readonly DependencyProperty ScaleXProperty =
            DependencyProperty.Register("ScaleX", typeof(double), typeof(Layer), new PropertyMetadata(0d, OnScaleChanged));

        private static void OnScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Layer l)
            {
                l.OnScaleChanged();
            }
        }

        public double ScaleY
        {
            get { return (double)GetValue(ScaleYProperty); }
            set { SetValue(ScaleYProperty, value); }
        }
        public static readonly DependencyProperty ScaleYProperty =
            DependencyProperty.Register("ScaleY", typeof(double), typeof(Layer), new PropertyMetadata(0d, OnScaleChanged));





        public Layer()
        {
            this.Loaded += Layer_Loaded;

        }

        private void Layer_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.GetParent<MapLib.Controls.Map>() is MapLib.Controls.Map m)
            {
                var xbinding = new Binding();
                xbinding.Path = new PropertyPath("stf.ScaleX");
                xbinding.Source = m;
                var ybinding = new Binding();
                xbinding.Path = new PropertyPath("stf.ScaleY");
                xbinding.Source = m;
                this.SetBinding(ScaleXProperty, xbinding);
                this.SetBinding(ScaleYProperty, xbinding);
            }
        }



        private void OnScaleChanged()
        {
            MapScaleTransform.ScaleX = this.ScaleX;
            MapScaleTransform.ScaleY = this.ScaleY;
            OnMapScaleChange(MapScaleTransform);
        }
        /// <summary>
        /// map的缩放变换通知
        /// </summary>
        /// <param name="st"></param>
        protected abstract void OnMapScaleChange(ScaleTransform st);
    }
}
