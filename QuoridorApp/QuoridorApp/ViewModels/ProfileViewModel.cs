using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace QuoridorApp.ViewModels
{
    class ProfileViewModel : ViewModelBase
    {
        public ProfileViewModel()
        {

        }
        public ICommand ToMainMenuCommand => new Command(OnToMainMenuCommand);

        public async void OnToMainMenuCommand()
        {
            Page p = new Views.MainMenu();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
    }
}
