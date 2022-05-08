using QuoridorApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QuoridorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RatingHistory : ContentPage
    {
        public RatingHistory()
        {
            InitializeComponent();
            RatingHistoryViewModel vm = new RatingHistoryViewModel();
            BindingContext = vm;
            
        }
    }
}