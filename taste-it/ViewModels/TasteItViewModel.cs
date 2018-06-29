using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taste_it.Additionals.ContentNavigationService;

namespace taste_it.ViewModels
{
    public class TasteItViewModel : ViewModelBase, IPageViewModel
    {
        public string name
        {
            get
            {
                return "T A S T E . I T";
            }
        }
    }
}
