using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using QuoridorApp.Views;
using System.Windows.Input;
using Xamarin.Forms;
using QuoridorApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;


namespace QuoridorApp.ViewModels
{
    class StartPageViewModel : ViewModelBase
    {
        public StartPageViewModel()
        {

        }

        #region Go To Sign Up
        public ICommand ToSignUpCommand => new Command(OnToSignUpCommand);
        
        public async void OnToSignUpCommand()
        {
            Page p = new Views.SignUp();
            await App.Current.MainPage.Navigation.PushAsync(p);
            
        }
        #endregion


        #region Go To Sign In

        public ICommand ToSignInCommand => new Command(OnToSignInCommand);

        public async void OnToSignInCommand()
        {
            Page p = new Views.SignIn();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
        
        #endregion

    }
}
