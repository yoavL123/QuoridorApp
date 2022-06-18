using QuoridorApp.Models;
using QuoridorApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace QuoridorApp.ViewModels
{
    public class RatingGraphViewModel : ViewModelBase
    {
        const double START_X = 0, START_Y = 0;
        const double X_LEN = 1000;
        const double Y_LEN = 20; // denotes the Y_LEN per 100 rating points
        const double POINT_RADIUS = 30;
        int[] ratings = { 0, 1200, 1400, 1600, 1900, 2100, 2300, 2400, 2600, 3000 };
        Color[] colors = { Color.Gray, Color.LightGreen, Color.Cyan, Color.Blue, Color.DarkViolet, Color.Gold, Color.Orange, Color.Red, Color.DarkRed };

        /*
        public void ConstructGraphBackground(AbsoluteLayout graph, double startX = START_X, double startY = START_Y)
        {
            //graph.Bounds
            double xVal = START_X;
            for (int i = ratings.Length - 1; i >= 1; i--)
            {
                Label colRect = new Label();
                colRect.BackgroundColor = colors[i - 1];
                double xLen = (ratings[i] - ratings[i - 1]) / 100 * Y_LEN;
                //Rectangle bounds = new Rectangle(xVal, START_Y, Y_LEN, xLen);
                Xamarin.Forms.Rectangle bounds = new Xamarin.Forms.Rectangle(START_Y, xVal, X_LEN, xLen);
                graph.Children.Add(colRect); // "theBoard" is defined in the view. Handled via binding
                AbsoluteLayout.SetLayoutBounds(colRect, bounds);
                xVal += xLen;
            }
        }

        public async void ConstructRatingLines(AbsoluteLayout graph)
        {
            QuoridorAPIProxy proxy = QuoridorAPIProxy.CreateProxy();
            List<RatingChange> ratingChanges = await proxy.GetRatingChanges(CurrentApp.CurrentPlayer);
            foreach(var ratingChange in ratingChanges)
            {
                double y = ratingChange.AlteredRating / Y_LEN;
                double x = (ratingChanges[ratingChanges.Count() - 1].RatingChangeDate - ratingChanges[0].RatingChangeDate).TotalSeconds;

                Xamarin.Forms.Re

                //Xamarin.Forms.Brush brush = new Xamarin.Forms.SolidColorBrush()
                Point p1 = new Xamarin.Forms.Point();
                p1.


                Point center = new Point(x, y);
                //Ellipse ellipseGeometry = new Ellipse(center, POINT_RADIUS, POINT_RADIUS);
                Ellipse ratingPoint = new Ellipse();
                ratingPoint.AnchorX = x;
                ratingPoint.AnchorY = y;
                ratingPoint.BackgroundColor = Color.Black;
                ratingPoint.ScaleX = POINT_RADIUS;
                ratingPoint.ScaleY = POINT_RADIUS;

                graph.Children.Add(ratingPoint);
                Xamarin.Forms.Rectangle bounds = new Xamarin.Forms.Rectangle(y, POINT_RADIUS, x, POINT_RADIUS);
                AbsoluteLayout.SetLayoutBounds(ratingPoint, bounds);
            }
        }



        public async void ConstructRatingLines(Canvas graph)
        {
            QuoridorAPIProxy proxy = QuoridorAPIProxy.CreateProxy();
            List<RatingChange> ratingChanges = await proxy.GetRatingChanges(CurrentApp.CurrentPlayer);
            foreach (var ratingChange in ratingChanges)
            {
                double y = ratingChange.AlteredRating / Y_LEN;
                double x = (ratingChanges[ratingChanges.Count() - 1].RatingChangeDate - ratingChanges[0].RatingChangeDate).TotalSeconds;

                Xamarin.Forms.Re

                //Xamarin.Forms.Brush brush = new Xamarin.Forms.SolidColorBrush()
                Point p1 = new Xamarin.Forms.Point();
                p1.


                Point center = new Point(x, y);
                //Ellipse ellipseGeometry = new Ellipse(center, POINT_RADIUS, POINT_RADIUS);
                Ellipse ratingPoint = new Ellipse();
                ratingPoint.AnchorX = x;
                ratingPoint.AnchorY = y;
                ratingPoint.BackgroundColor = Color.Black;
                ratingPoint.ScaleX = POINT_RADIUS;
                ratingPoint.ScaleY = POINT_RADIUS;

                graph.Children.Add(ratingPoint);
                Xamarin.Forms.Rectangle bounds = new Xamarin.Forms.Rectangle(y, POINT_RADIUS, x, POINT_RADIUS);
                AbsoluteLayout.SetLayoutBounds(ratingPoint, bounds);
            }
        }
        */
        public RatingGraphViewModel()
        {

        }
    }
}
