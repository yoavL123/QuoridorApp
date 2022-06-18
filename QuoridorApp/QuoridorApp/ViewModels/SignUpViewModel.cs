using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using QuoridorApp.Views;
using QuoridorApp.Services;
using QuoridorApp.Models;
using System.Threading.Tasks;

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

        private bool IsValidUserName(string s)
        {
            string[] banned = new string[] { "Me", "Guest", "Player", "EasyBot", "MediumBot", "HardBot", "Bot" };
            foreach (var v in banned)
            {
                if (s == v) return false;
            }
            return true;
        }

        private async Task<bool> ValidateAsync()
        {
            string msg = " cannot be empty";
            if (String.IsNullOrEmpty(Email))
            {
                await Application.Current.MainPage.DisplayAlert($"Email" + msg, "", "ok");
                return false;
            }
            if (String.IsNullOrEmpty(UserName))
            {
                await Application.Current.MainPage.DisplayAlert($"User Name" + msg, "", "ok");
                return false;
            }
            if (String.IsNullOrEmpty(FirstName))
            {
                await Application.Current.MainPage.DisplayAlert($"First Name" + msg, "", "ok");
                return false;
            }
            if (String.IsNullOrEmpty(LastName))
            {
                await Application.Current.MainPage.DisplayAlert($"Last Name" + msg, "", "ok");
                return false;
            }
            if (String.IsNullOrEmpty(PlayerPass))
            {
                await Application.Current.MainPage.DisplayAlert($"Password" + msg, "", "ok");
                return false;
            }


            QuoridorAPIProxy proxy = QuoridorAPIProxy.CreateProxy();
            Player p = await proxy.GetPlayer(UserName);
            if (p != null)
            {
                await Application.Current.MainPage.DisplayAlert($"User Name is already taken", "", "ok");
                return false;
            }
            if(!IsValidUserName(UserName))
            {
                await Application.Current.MainPage.DisplayAlert($"Invalid User Name", "", "ok");
                return false;
            }
            return true;
        }
        public async void OnSubmitSignUpCommand()
        {
            if (!(await ValidateAsync())) return;
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
