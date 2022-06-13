using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace QuoridorApp.ViewModels
{
    public class BoardBotOptionsViewModel : ViewModelBase
    {
        #region Type1
        private string type1;
        public string Type1
        {
            get => type1;
            set
            {
                type1 = value;
                OnPropertyChanged("Type1");
            }
        }
        #endregion

        #region Type2
        private string type2;
        public string Type2
        {
            get => type2;
            set
            {
                type2 = value;
                OnPropertyChanged("Type2");
            }
        }
        #endregion


        public ICommand SubmitBoardBotOptionsCommand => new Command(OnSubmitBoardBotOptionsCommand);


        public async void OnSubmitBoardBotOptionsCommand()
        {
            Page page = new Views.Board(Type1, Type2);
            await App.Current.MainPage.Navigation.PushAsync(page);
        }
    }
}
