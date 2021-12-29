using QuoridorApp.Models;
using QuoridorApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QuoridorApp
{
    public partial class App : Application
    {

        public static bool IsDevEnv
        {
            get
            {
                return true; //change this before release!
            }
        }

        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
            // Setting starting page as sign up page:
            //MainPage = new SignUp();
            MainPage = new NavigationPage(new StartPage());
        }

        // The current logged in user
        public Player CurrentPlayer { get; set; }

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
