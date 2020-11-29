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

namespace IOTMP.HMIClient.MapLib.Layers
{

    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(MapLayerItem))]
    public class MapLayer : ItemsControl
    {
        public MapLayer()
        {
            DefaultStyleKey = typeof(MapLayer);
        }
        static object obj = null;
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            obj = item;
            return item is MapLayerItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var item = new MapLayerItem();
            item.DataContext = obj;
            return item;
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);

        }


        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
        }


    }
}
