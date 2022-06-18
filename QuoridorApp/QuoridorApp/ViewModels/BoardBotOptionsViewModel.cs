using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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


        private async Task<bool> ValidateAsync()
        {
            if(String.IsNullOrEmpty(Type1))
            {
                await Application.Current.MainPage.DisplayAlert($"Choose the type of the first player", "", "ok");
                return false;
            }
            if (String.IsNullOrEmpty(Type2))
            {
                await Application.Current.MainPage.DisplayAlert($"Choose the type of the second player", "", "ok");
                return false;
            }
            return true;
        }

        public async void OnSubmitBoardBotOptionsCommand()
        {
            if (!(await ValidateAsync())) return;
            Page page = new Views.Board(Type1, Type2);
            await App.Current.MainPage.Navigation.PushAsync(page);
        }
    }
}
