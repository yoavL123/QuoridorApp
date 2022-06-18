using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using QuoridorApp.Services;
using QuoridorApp.Models;
using QuoridorApp.ViewModels;

namespace QuoridorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RatingGraph : ContentPage
    {

        RatingGraphViewModel vm;
        

        public RatingGraph(AbsoluteLayout theGraph)
        {
            InitializeComponent();
            vm = new RatingGraphViewModel();

            //AbsoluteLayout graph = new AbsoluteLayout();
            //Rectangle graph = new Rectangle(START_X, START_Y, ;

            //vm.ConstructGraphBackground(theGraph);
            //vm.ConstructRatingLines(theGraph);

        }

        
    }
}