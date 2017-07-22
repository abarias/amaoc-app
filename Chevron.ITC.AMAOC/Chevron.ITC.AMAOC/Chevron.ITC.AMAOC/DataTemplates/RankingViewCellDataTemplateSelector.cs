using Chevron.ITC.AMAOC.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Chevron.ITC.AMAOC
{
    public class RankingViewCellDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EllipseTemplate { get; set; }
        public DataTemplate EmployeeTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var currentEmp = item as Employee;

            if (currentEmp.RankCounter == 11)
                return currentEmp.UserId == Chevron.ITC.AMAOC.Helpers.Settings.UserId ? EmployeeTemplate : EllipseTemplate;
            else
                return EmployeeTemplate;            
        }
    }
}
