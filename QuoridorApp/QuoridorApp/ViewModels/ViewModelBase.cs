using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace QuoridorApp.ViewModels
{
    public class ViewModelBase
    {

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChnaged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
