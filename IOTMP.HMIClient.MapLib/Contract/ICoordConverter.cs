using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTMP.HMIClient.MapLib.Contract
{
    public interface ICoordConverter
    {
        double ConverterToUwbX(double left);
        double ConverterToUwbY(double top);
        double ConverterToTop(double uwby);
        double ConverterToLeft(double uwbx);
    }
}
