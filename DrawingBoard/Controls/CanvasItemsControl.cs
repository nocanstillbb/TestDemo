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

namespace DrawingBoard.Controls
{

    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(CanvasItemsControlItem))]
    public class CanvasItemsControl : ItemsControl
    {
        public CanvasItemsControl()
        {
            DefaultStyleKey = typeof(CanvasItemsControl);
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is CanvasItemsControlItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var item = new CanvasItemsControlItem();
            return item;
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);

        }



        public DataTemplate ItemsTemplate
        {
            get { return (DataTemplate)GetValue(ItemsTemplateProperty); }
            set { SetValue(ItemsTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsTemplateProperty =
            DependencyProperty.Register("ItemsTemplate", typeof(DataTemplate), typeof(CanvasItemsControl), new PropertyMetadata(null));


    }
}
