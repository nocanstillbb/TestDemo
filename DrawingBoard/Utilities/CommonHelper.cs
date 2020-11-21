using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DrawingBoard.Utilities
{
    public static class CommonHelper
    {
        public static T GetParent<T>(this DependencyObject uc) where T : DependencyObject, new()
        {
            var dobj = uc;
            do
            {
                dobj = VisualTreeHelper.GetParent(dobj);
            } while (dobj != null && !(dobj is T));

            return dobj as T;
        }
    }
}
