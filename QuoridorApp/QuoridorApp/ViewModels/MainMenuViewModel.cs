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
        #region Submit SignOut
        public ICommand SubmitSignOutCommand => new Command(OnSubmitSignOutCommand);


        public async void OnSubmitSignOutCommand()
        {
            QuoridorAPIProxy proxy = QuoridorAPIProxy.CreateProxy();
            CurrentApp.CurrentPlayer = null;
            _ = CurrentApp.MainPage.Navigation.PushAsync(new StartPage());
        }
        #endregion

        #region Go To Board
        public ICommand ToBoardCommand => new Command(OnToBoardCommand);

        public async void OnToBoardCommand()
        {
            Page p = new Views.Board();
            await App.Current.MainPage.Navigation.PushAsync(p);

        }
        #endregion

        #region Go To BoardBot
        public ICommand ToBoardBotCommand => new Command(OnToBoardBotCommand);

        public async void OnToBoardBotCommand()
        {
            Page p = new Views.BoardBot();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
        #endregion

        #region Go To Profile
        public ICommand ToProfileCommand => new Command(OnToProfileCommand);
        public async void OnToProfileCommand()
        {
            Page p = new Views.Profile();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
        #endregion
    }
}
