using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using QuoridorApp.Views;
using QuoridorApp.Services;
using QuoridorApp.Models;

namespace QuoridorApp.ViewModels
{
    class SignUpViewModel : ViewModelBase
    {

        /*
        public int PlayerId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PlayerPass { get; set; }
         */
        #region Email
        
        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }
        
        #endregion
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
        #region FirstName
        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        #endregion
        #region LastName
        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
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
        public ICommand SubmitSignUpCommand => new Command(OnSubmitSignUpCommand);

        public SignUpViewModel()
        {
        }

        public ICommand ToStartPageCommand => new Command(OnToStartPageCommand);

        public async void OnToStartPageCommand()
        {
            Page p = new Views.StartPage();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
        /*
        TODO
        Currently not really signing up
        TODO: add exceptions
        */
        public async void OnSubmitSignUpCommand()
        {
            QuoridorAPIProxy proxy = QuoridorAPIProxy.CreateProxy();
            Player player = new Player
            {
                Email = Email,
                UserName = UserName,
                FirstName = FirstName,
                LastName = LastName,
                PlayerPass = PlayerPass
            };
            Player p = await proxy.SignUpPlayer(player);
            Page page = new Views.StartPage();
            await App.Current.MainPage.Navigation.PushAsync(page);
        }
    }
}
