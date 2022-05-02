using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using QuoridorApp.ViewModels;
namespace QuoridorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {

        
        public Profile()
        {
            InitializeComponent();
            ProfileViewModel vm = new ProfileViewModel(theGraph);
            BindingContext = vm;
            
        }
    }
}