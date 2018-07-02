using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taste_it.Models;

namespace taste_it.Additionals.Messages
{
    public class FiltersMessage
    {
        public ObservableCollection<Category> FilterCategories { get; set; }
        public ObservableCollection<Tag> FilterTags { get; set; }
        public string FilterName { get; set; }


    }

}
