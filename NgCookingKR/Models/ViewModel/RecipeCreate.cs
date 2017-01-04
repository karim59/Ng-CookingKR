using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NgCookingKR.Models.ViewModel
{
    public class RecipeCreate
    {

        public string Name { get; set; }
        public int idCategory { get; set; }
        public string Description { get; set; }
        public string AddedIngredients { get; set; }
        public string RemovedIngredients { get; set; }
        public bool RecipeIsValid { get; set; }
    }
}