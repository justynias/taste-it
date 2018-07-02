using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
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

        private string recipeName;
        private ObservableCollection<Category> categoriesCollection;
        private ObservableCollection<Tag> tags;
        private ObservableCollection<Category> filterCategories;
        private Tag currentTag;
        private string tagName;
        private string tagError;

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
                Debug.WriteLine("dupa");
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

        public FilterViewModel()
        {
            // RecipesCollection = new ObservableCollection<Recipe>();

            Tags = new ObservableCollection<Tag>();
            LoadCategories();

            //test
            this.PropertyChanged += Filter_PropertyChanged;

            AddFilterCategoriesCommand = new RelayCommand<object>(AddFilterCategories);
            RemoveFilterCategoriesCommand = new RelayCommand<object>(RemoveFilterCategories);
            AddTagCommand = new RelayCommand(AddTag);
            RemoveTagCommand = new RelayCommand<object>(RemoveTag);

        }

        void Filter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)   //here methods to filtering 
            {
                case "RecipeName":
                    Debug.WriteLine(RecipeName); 
                    break;
                case "FilterCategories":
                    Debug.WriteLine(FilterCategories?.Count());
                    break;
                case "Tags":
                    Debug.WriteLine(Tags?.Count());
                    break;

            }
        }

        private void AddFilterCategories(object parameter)
        {

            //int id = Convert.ToInt32(parameter);
            string name = (string)parameter;
            var currentCategory = CategoriesCollection.First(c => c.name == name);
            FilterCategories.Add(currentCategory);
            RaisePropertyChanged(() => FilterCategories);
        }
        private void RemoveFilterCategories(object parameter)
        {
            string name = (string)parameter;
            var currentCategory = CategoriesCollection.First(c => c.name == name);
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
           // if (IsTagsValid(ref tagError))
            //{
                CurrentTag.name = "#" + CurrentTag.name;
                Tags.Add(CurrentTag);
                RaisePropertyChanged(() => Tags);
            //}

            TagName = string.Empty;
        }

        private void LoadCategories()  //temporary , no sense to load from db, messenger?
        {
            CategoriesCollection = new ObservableCollection<Category>();
            FilterCategories = new ObservableCollection<Category>();

            CategoriesCollection.Add(new Category() { name = "Breakfast" });
            CategoriesCollection.Add(new Category() { name = "Snack" });
            CategoriesCollection.Add(new Category() { name = "Lunch" });
            CategoriesCollection.Add(new Category() { name = "Tea" });
            CategoriesCollection.Add(new Category() { name = "Dinner" });
            CategoriesCollection.Add(new Category() { name = "Supper" });

            //var categoriesTemp = await _categoryDataService.GetCategoriesAsync();
            //CategoriesCollection = new ObservableCollection<Category>();
            //foreach (var c in categoriesTemp)
            //{
            //    CategoriesCollection.Add(c);
            //}
        }
        //private bool IsTagsValid(ref string message)
        //{
        //    message = string.Empty;

        //    if (Tags.Count > 10)
        //    {
        //        message = "Number of tags must be maximum 10";
        //        return false;
        //    }
        //    else if (string.IsNullOrWhiteSpace(TagName) || string.IsNullOrEmpty(TagName))
        //    {
        //        message = "Field should not be empty";
        //        return false;
        //    }
        //    else if (TagName.Length > 20)
        //    {
        //        message = "Tag should contains maximum 20 characters";
        //        return false;
        //    }
        //    else if (TagName.Contains(" "))
        //    {
        //        message = "Tag should not contains spaces!";
        //        return false;
        //    }
        //    else if (Tags.Any(item => item.name.Substring(1) == CurrentTag.name))
        //    {
        //        message = "Tag already exists!";
        //        return false;
        //    }

        //    return true;
        //}
    }
}
