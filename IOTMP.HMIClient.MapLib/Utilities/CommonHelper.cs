using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace IOTMP.HMIClient.MapLib.Utilities
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

        public static bool IsNaNumber(this double d)
        {
            return double.IsNaN(d) || double.IsInfinity(d) || double.IsPositiveInfinity(d) || double.IsNegativeInfinity(d);
        }
    }
}
