using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chevron.ITC.AMAOC
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AMAOCGroupHeaderView : ContentView
    {
        public AMAOCGroupHeaderView()
        {
            InitializeComponent();
        }
    }

    public class AMAOCGroupHeader : ViewCell
    {
        public AMAOCGroupHeader()
        {
            View = new AMAOCGroupHeaderView();
        }
    }
}