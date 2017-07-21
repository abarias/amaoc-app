using Chevron.ITC.AMAOC.ViewModels;
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
    public partial class SettingsPage : ContentPage
    {
        SettingsViewModel vm;
        public SettingsPage()
        {
            InitializeComponent();

            BindingContext = vm = new SettingsViewModel();        
        }
    }
}