using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using taste_it.Additionals.ContentNavigationService;
using taste_it.Additionals.Messages;
using taste_it.DataService;
using taste_it.Models;

namespace taste_it.ViewModels
{
    public class FilterViewModel : ViewModelBase, IPageViewModel
    {
        public string name
        {
            get
            {
                return "Filter";
            }
        }

        private string recipeName=string.Empty;
        private ObservableCollection<Category> categoriesCollection;
        private ObservableCollection<Tag> tags;
        private ObservableCollection<Category> filterCategories;
     

        private Tag currentTag;
        private string tagName;
        private string tagError;
        //temporary
        private readonly ICategoryDataService _categoryDataService;
       

        //
        public string RecipeName
        {
            get
            {
                return recipeName;
            }

            set
            {

                Set(ref recipeName, value);

            }
        }

        public ObservableCollection<Category> FilterCategories
        {
            get
            {
                return filterCategories;
            }

            set
            {

                Set(ref filterCategories, value);

            }
        }
        public ObservableCollection<Category> CategoriesCollection
        {
            get
            {
                return categoriesCollection;
            }

            set
            {

                Set(ref categoriesCollection, value);

            }
        }


        public ObservableCollection<Tag> Tags
        {
            get
            {
                return tags;
            }

            set
            {
                Set(ref tags, value);

            }
        }

        public Tag CurrentTag
        {
            get
            {
                return currentTag;
            }

            set
            {
                Set(ref currentTag, value);
            }
        }

        public string TagName
        {
            get
            {
                return tagName;
            }

            set
            {
                Set(ref tagName, value);
            }
        }
        public ICommand AddFilterCategoriesCommand { get; private set; }
        public ICommand RemoveFilterCategoriesCommand { get; private set; }
        public ICommand AddTagCommand { get; private set; }
        public ICommand RemoveTagCommand { get; private set; }
        public ICommand SendFiltersCommand { get; private set; }
       

        public FilterViewModel(ICategoryDataService categoryData)
        {
            
            _categoryDataService = categoryData;
        

            
            LoadCategories();
            LoadTags();

            AddFilterCategoriesCommand = new RelayCommand<object>(AddFilterCategories);
            RemoveFilterCategoriesCommand = new RelayCommand<object>(RemoveFilterCategories);
            AddTagCommand = new RelayCommand(AddTag);
            RemoveTagCommand = new RelayCommand<object>(RemoveTag);
            SendFiltersCommand = new RelayCommand(SendFilters);


        }

        private void SendFilters()
        {
        
            Messenger.Default.Send<FiltersMessage>(new FiltersMessage
            {
                FilterCategories = this.FilterCategories,
                FilterName = this.RecipeName,
                FilterTags = this.Tags

            });
        }

        private void LoadTags()
        {
            Tags = new ObservableCollection<Tag>();

        }

        private void AddFilterCategories(object parameter)
        {

            int id = Convert.ToInt32(parameter);
            var currentCategory = CategoriesCollection.First(c => c.id_c == id);
            FilterCategories.Add(currentCategory);
            RaisePropertyChanged(() => FilterCategories);
        }
        private void RemoveFilterCategories(object parameter)
        {
            int id = Convert.ToInt32(parameter);
            var currentCategory = CategoriesCollection.First(c => c.id_c == id);
            FilterCategories.Remove(currentCategory);
            RaisePropertyChanged(() => FilterCategories);

        }
        private void RemoveTag(object parameter)
        {
            string name = (string)parameter;
            Tags.Remove(Tags.Where(i => i.name == name).Single());
            RaisePropertyChanged(() => Tags);
        }
        private void AddTag()
        {
            CurrentTag = new Tag() { name = TagName };
      
                CurrentTag.name = "#" + CurrentTag.name;
                Tags.Add(CurrentTag);
                RaisePropertyChanged(() => Tags);
           
            TagName = string.Empty;
        }


        private async void LoadCategories()  
        {
            CategoriesCollection = new ObservableCollection<Category>();
            FilterCategories = new ObservableCollection<Category>();
            var categoriesTemp = await _categoryDataService.GetCategoriesAsync();
            CategoriesCollection = new ObservableCollection<Category>();
            foreach (var c in categoriesTemp)
            {
                CategoriesCollection.Add(c);
            }
        }

        private void SetLoaderOn()
        {
            Messenger.Default.Send<GenericMessage<bool>>(new GenericMessage<bool>(true));
        }
        private void SetLoaderOff()
        {
            Messenger.Default.Send<GenericMessage<bool>>(new GenericMessage<bool>(false));
        }


    }
}
