using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using APP.Helpers.FileHandling;
using Ninject;

namespace APP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IKernel _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            ComposeObjects();
            Current.MainWindow.Show();
        }

        private void ConfigureContainer()
        {
            _container = new StandardKernel();

            //Helpers
            //File FileHandling
            _container.Bind<IBitmapHandler>().To<BitmapHandler>();
            _container.Bind<ITxtHandler>().To<TxtHandler>();
            _container.Bind<IContourLoader>().To<ContourLoader>().InSingletonScope();
        }

        private void ComposeObjects()
        {
            Current.MainWindow = _container.Get<MainWindow>();
        }
    }
}