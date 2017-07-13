using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chevron.ITC.AMAOC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        RootPageAndroid root;
        public MenuPage(RootPageAndroid root)
        {
            this.root = root;
            InitializeComponent();

            NavView.NavigationItemSelected += async (sender, e) =>
            {
                this.root.IsPresented = false;

                await Task.Delay(225);
                await this.root.NavigateAsync(e.Index);
            };
        }
    }
}