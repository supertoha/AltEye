using AltEye.Services;
using AltEye.ViewModels;
using AltEye.Views;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;

namespace AltEye
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        private Window _window;

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            this.ConfigureServices();
            this._window = new MainWindow();
            
            var mainViewModel = new MainViewModel();
            var view = new MainView { DataContext = mainViewModel };
            this._window.Content = view;
            this._window.Activate();
        }

        private void ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddTransient<FilePickerService>((x) => new FilePickerService(() => WinRT.Interop.WindowNative.GetWindowHandle(this._window)));

            // ViewModel

            Ioc.Default.ConfigureServices(services.BuildServiceProvider());
        }
    }
}
