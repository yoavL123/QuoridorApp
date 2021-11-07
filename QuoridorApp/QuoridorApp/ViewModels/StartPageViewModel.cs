using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using QuoridorApp.Views;

namespace QuoridorApp.ViewModels
{
    class StartPageViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChnaged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        public StartPageViewModel()
        {

        }
        public void ToSignUp_clicked(object sender, EventArgs e)
        {
            App theApp = (App)App.Current;
            theApp.MainPage = new SignUp();

        }
    }
}
