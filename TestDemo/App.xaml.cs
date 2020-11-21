
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using System.ComponentModel;
using System.Windows;
using System.Windows.Navigation;
namespace TestDemo
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Myapp
    {
     
    }

    public class Myapp : PrismApplication
    {
        protected override Window CreateShell()
        {
            MainWindow shell = Container.Resolve<MainWindow>();
            return shell;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterSingleton<IMessagebox, HScada.SystemElement.Messagebox>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<TestDemo.NewCenterStorage.Bootstrap>();
        }
    }
}
