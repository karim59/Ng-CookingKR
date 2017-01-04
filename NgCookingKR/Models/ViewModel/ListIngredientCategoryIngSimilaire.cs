using NgCookingKR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NgCookingKR.Models.ViewModel
{
    public class ListIngredientCategoryIngSimilaire
    {
        public List<IngredientCategoryIngSimilaire> ListIngredientCategory { get; set; }
    }

    public class IngredientCategoryIngSimilaire
    {
        public Ingredient Ingredient { get; set; }
        public IngredientCategory CategoryIngredient { get; set; }
        public List<Ingredient> ListIngredientSimilaire { get; set; } 
        public float LevelCal { get; set; }
    }
}