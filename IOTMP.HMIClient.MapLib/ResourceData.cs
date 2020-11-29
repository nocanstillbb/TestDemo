using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IOTMP.HMIClient.MapLib
{
    public class ResourceData
    {
        private static ResourceDictionary dictionary;
        public static ResourceDictionary Dictionary
        {
            get
            {
                if (ResourceData.dictionary == null)
                {
                    ResourceData.dictionary = new ResourceDictionary();
                    ResourceData.dictionary.MergedDictionaries.Add(ResourceData.LoadDictionary("/IOTMP.HMIClient.MapLib;component/Symbols/DefaultSymbol.xaml"));
                }
                return ResourceData.dictionary;
            }
        }
        private static ResourceDictionary LoadDictionary(string key)
        {
            ResourceDictionary resourceDictionary = new ResourceDictionary();
            resourceDictionary.Source = new Uri(key, UriKind.RelativeOrAbsolute);
            return resourceDictionary;
        }
    }
}
