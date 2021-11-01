using QuoridorApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QuoridorApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
            // Setting starting page as sign up page:
            MainPage = new SignUp();
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
