﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using taste_it.Additionals.NavigationService;
using taste_it.DataService;
using taste_it.Models;

namespace taste_it.ViewModels
{
   
    public class AddRecipeViewModel : ViewModelBase
    {

        private User currentUser; // to merged author with the recipe
        private IFrameNavigationService _navigationService;
        private readonly IRecipeDataService _recipeDataService;
        public AddRecipeViewModel(IRecipeDataService  recipeData, IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            _recipeDataService = recipeData;

        }
    }
}