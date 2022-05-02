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
    public partial class RatingGraph : ContentPage
    {
        const double START_X = 0, START_Y = 0;
        const double X_LEN = 1000;
        const double Y_LEN = 20; // denotes the Y_LEN per 100 rating points
        int[] ratings = { 0, 1200, 1400, 1600, 1900, 2100, 2300, 2400, 2600, 3000 };
        Color[] colors = { Color.Gray, Color.LightGreen, Color.Cyan, Color.Blue, Color.DarkViolet, Color.Gold, Color.Orange, Color.Red, Color.DarkRed };


        void construct_graph_background(AbsoluteLayout graph, double startX = START_X, double startY = START_Y)
        {
            //graph.Bounds
            double xVal = START_X;
            for(int i = ratings.Length - 1; i >= 1; i--)
            {
                Label colRect = new Label();
                colRect.BackgroundColor = colors[i-1];
                double xLen = (ratings[i] - ratings[i - 1]) / 100 * Y_LEN;
                //Rectangle bounds = new Rectangle(xVal, START_Y, Y_LEN, xLen);
                Rectangle bounds = new Rectangle(START_Y, xVal, X_LEN, xLen);
                graph.Children.Add(colRect); // "theBoard" is defined in the view. Handled via binding
                AbsoluteLayout.SetLayoutBounds(colRect, bounds);
                xVal += xLen;
            }
        }

        public RatingGraph(AbsoluteLayout theGraph)
        {
            InitializeComponent();
            //AbsoluteLayout graph = new AbsoluteLayout();
            //Rectangle graph = new Rectangle(START_X, START_Y, ;

            construct_graph_background(theGraph);
        }

        
    }
}