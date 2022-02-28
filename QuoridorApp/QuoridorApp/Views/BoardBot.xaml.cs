﻿using QuoridorApp.ViewModels;
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
    public partial class BoardBot : ContentPage
    {
        BoardViewModel vm;
        /*
        public BoardBot()
        {
            InitializeComponent();
        }
        */
        public BoardBot()
        {
            InitializeComponent();
            vm = new BoardViewModel(theBoard, false, true);
            BindingContext = vm;
        }
    }
}