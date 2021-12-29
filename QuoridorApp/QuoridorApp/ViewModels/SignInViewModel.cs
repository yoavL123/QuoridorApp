using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using QuoridorApp.Services;
using QuoridorApp.Models;

namespace QuoridorApp.ViewModels
{
    class SignInViewModel : ViewModelBase
    {

        #region UserName
        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }
        #endregion

        #region PlayerPass
        private string playerPass;
        public string PlayerPass
        {
            get { return playerPass; }
            set
            {
                playerPass = value;
                OnPropertyChanged("PlayerPass");
            }
        }
        #endregion


        public SignInViewModel()
        {
        }
        #region To StartPage
        public ICommand ToStartPageCommand => new Command(OnToStartPageCommand);

        public async void OnToStartPageCommand()
        {
            Page p = new Views.StartPage();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
        #endregion
        
        #region Submit SignIn
        public ICommand SubmitSignInCommand => new Command(OnSubmitSignInCommand);


        public async void OnSubmitSignInCommand()
        {
            QuoridorAPIProxy proxy = QuoridorAPIProxy.CreateProxy();
            Player player = await proxy.SignInAsync(userName, playerPass);
            if(player == null)
            {
                return;
            }
            CurrentApp.CurrentPlayer = player;
            Page p = new Views.MainMenu();
            
            await CurrentApp.MainPage.Navigation.PushAsync(p);
        }
        #endregion
    }
}
