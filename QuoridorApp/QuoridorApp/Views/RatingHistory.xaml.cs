using QuoridorApp.ViewModels;
using QuoridorApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using QuoridorApp.Models;

namespace QuoridorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RatingHistory : ContentPage
    {
        public RatingHistory()
        {
            RatingHistoryViewModel vm = new RatingHistoryViewModel();
            BindingContext = vm;
            InitializeComponent();
            
            
        }


       void make()
        {
            TableView table = new TableView();
        }
    }
}