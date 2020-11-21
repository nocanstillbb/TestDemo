using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo.NewCenterStorage
{
    public class Bootstrap : IModule
    {
        public Bootstrap(IContainerExtension ce, IRegionManager rm)
        {
            _ce = ce;
            _rm = rm;
        }

        public IContainerExtension _ce { get; }
        public IRegionManager _rm { get; }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            string mynamespace = "TestDemo.NewCenterStorage.Pages";

            var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && t.Namespace == mynamespace
                    select t;
            foreach (var item in q)
            {
                _ce.RegisterForNavigation(item, item.Name);

                _rm.RequestNavigate("TabRegion", item.Name);
            }
        }
    }
}
