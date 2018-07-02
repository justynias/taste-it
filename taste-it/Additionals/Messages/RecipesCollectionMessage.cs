using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taste_it.Models;

namespace taste_it.Additionals.Messages
{
   public class RecipesCollectionMessage
    {
       public ObservableCollection<Recipe> RecipesCollection { get; set; }
    }
}
