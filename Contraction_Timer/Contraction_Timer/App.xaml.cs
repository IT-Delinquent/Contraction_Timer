using Contraction_Timer.Helpers;
using Contraction_Timer.ViewModels;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Contraction_Timer
{
    public partial class App : Application
    {

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
