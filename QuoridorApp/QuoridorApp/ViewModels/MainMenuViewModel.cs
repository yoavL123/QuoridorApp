using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using QuoridorApp.Services;
using QuoridorApp.Models;
using QuoridorApp.Views;

namespace QuoridorApp.ViewModels
{
    class MainMenuViewModel : ViewModelBase
    {
        #region Submit SignIn
        public ICommand SubmitSignOutCommand => new Command(OnSubmitSignOutCommand);


        public async void OnSubmitSignOutCommand()
        {
            QuoridorAPIProxy proxy = QuoridorAPIProxy.CreateProxy();
            CurrentApp.CurrentPlayer = null;
            _ = CurrentApp.MainPage.Navigation.PushAsync(new StartPage());
        }
        #endregion
    }
}
