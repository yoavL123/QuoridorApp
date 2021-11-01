using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace QuoridorApp.ViewModels
{
    class SignUpViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChnaged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

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
                OnPropertyChnaged("Email");
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
                OnPropertyChnaged("UserName");
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
                OnPropertyChnaged("FirstName");
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
                OnPropertyChnaged("LastName");
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
                OnPropertyChnaged("PlayerPass");
            }
        }
        #endregion
        public ICommand SignUpCommand { get; protected set; }

        public SignUpViewModel()
        {
            SignUpCommand = new Command(OnSubmit);
        }


        /*
        TODO
        Currently not really signing up
        */
        public async void OnSubmit()
        {
            
        }
    }
}
