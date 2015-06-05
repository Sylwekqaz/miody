using System.Reflection;
using System.Windows;
using APP.Helpers;
using APP.Helpers.FileHandling;
using APP.Helpers.Measures;
using APP.View;
using Autofac;

namespace APP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            ComposeObjects();
            Current.MainWindow.Show();
        }

        private void ConfigureContainer()
        {
            var  builder  = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());

            builder.RegisterType<HammingDistance>().As<Comparison>();
            builder.RegisterType<HausdorffDistance>().As<Comparison>();
            builder.RegisterType<JaccardIndex>().As<Comparison>();

            builder.RegisterType<ErrorLog>().As<IErrorLog>().SingleInstance();



            var container = builder.Build();
                


            IoC.Initialize(container);
        }

        private void ComposeObjects()
        {
            Current.MainWindow = IoC.Resolve<MainWindow>();
        }
    }
}