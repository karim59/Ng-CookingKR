using NgCookingKR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NgCookingKR.Models.ViewModel
{
    public class RecipeHome
    {
     
     public List<RecipeNote> RecipeListDate { get; set; }
     public List<RecipeNote> RecipeNote { get; set; }  
     public List<RecipeNote> RecipeSearch { get; set; }
              
    }

     public class RecipeNote
    {
        public Recipe recipe { get; set; }
        public double Calorie { get; set; }
        public float note{ get; set; }
    }
}