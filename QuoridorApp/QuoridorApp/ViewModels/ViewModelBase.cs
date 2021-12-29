using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace QuoridorApp.ViewModels
{
    public class ViewModelBase
    {
        public App CurrentApp => Application.Current as App;
        public string UserName => CurrentApp.CurrentPlayer?.UserName;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
