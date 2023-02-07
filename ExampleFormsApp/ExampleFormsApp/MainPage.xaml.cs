using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExampleFormsApp.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ExampleFormsApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            var giniBankSdkService = DependencyService.Get<IGiniBankSDKService>();
            giniBankSdkService.Start();
        }
    }
}

