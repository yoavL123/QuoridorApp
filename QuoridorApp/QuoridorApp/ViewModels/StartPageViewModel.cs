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

        #region Go To Sign Up
        public ICommand ToSignUpCommand => new Command(OnToSignUpCommand);
        
        public async void OnToSignUpCommand()
        {
            App theApp = (App)App.Current;
            StartPageViewModel vm = new StartPageViewModel();
            Page p = new Views.SignUp();
            await theApp.MainPage.Navigation.PushAsync(p);
            
        }
        #endregion

        
    }
}
