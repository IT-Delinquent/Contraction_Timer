using Contraction_Timer.Helpers;
using Xamarin.Forms;

namespace Contraction_Timer
{
    public partial class App : Application
    {
        /// <summary>
        /// The path to store contraction data files
        /// </summary>
        public static string FolderPath { get; private set; }

        public App()
        {
            InitializeComponent();

            FolderPath = IOHelpers.FolderPathLocation;

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}