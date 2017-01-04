using NgCookingKR.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NgCookingKR.Models.ViewModel;

namespace NgCookingKR.Controllers
{
    public class IngredientsController : Controller
    {
        private CookingEntities db = new CookingEntities();
        HelpMethod met = new HelpMethod();
        // GET: Ingredients
        public ActionResult Ingredient()
        {
            ListIngredientCategoryIngSimilaire listeIngredientCatIngSimil = new ListIngredientCategoryIngSimilaire();
            listeIngredientCatIngSimil = met.GetIngredient();
            return View(listeIngredientCatIngSimil);
        }

        [HttpPost]
        public ActionResult Ingredient(string ingred = null, string category = null, int calmin = 0, int calmax = 1000)
        {
            List<Ingredient> listIngredient = db.Ingredient.ToList();
            ListIngredientCategoryIngSimilaire listeIngredientCatIngSimil = new ListIngredientCategoryIngSimilaire();
            List<IngredientCategoryIngSimilaire> IngredientCategoryIngSimilaire = new List<IngredientCategoryIngSimilaire>();
            for (int i = 0; i < listIngredient.Count; i++)
            {
                IngredientCategoryIngSimilaire element = new IngredientCategoryIngSimilaire();
                int idIngredientCategory = listIngredient[i].IdIngredientCategory;
                IngredientCategory ingredientCategory = db.IngredientCategory.Find(idIngredientCategory);
                List<Ingredient> ingredientSimilaire = db.Ingredient.Where(m => m.IdIngredientCategory == idIngredientCategory).ToList();
                element.Ingredient = listIngredient[i];
                element.CategoryIngredient = ingredientCategory;
                element.ListIngredientSimilaire = ingredientSimilaire;
                IngredientCategoryIngSimilaire.Add(element);
            }
            listeIngredientCatIngSimil.ListIngredientCategory = IngredientCategoryIngSimilaire;


            if (ingred.Equals("") && category.Equals(""))
            {
                IngredientCategoryIngSimilaire = IngredientCategoryIngSimilaire.Where(m => m.Ingredient.Calorie >= calmin & m.Ingredient.Calorie <= calmax).ToList();
                listeIngredientCatIngSimil.ListIngredientCategory = IngredientCategoryIngSimilaire;
                return View(listeIngredientCatIngSimil);
            }
            else if (ingred.Equals(""))
            {
                IngredientCategoryIngSimilaire = IngredientCategoryIngSimilaire.Where(m => m.Ingredient.Calorie >= calmin && m.Ingredient.Calorie <= calmax && m.CategoryIngredient.Name.Equals(category)).ToList();
                listeIngredientCatIngSimil.ListIngredientCategory = IngredientCategoryIngSimilaire;
                return View(listeIngredientCatIngSimil);
            }
            else if (category.Equals(""))
            {
                IngredientCategoryIngSimilaire = IngredientCategoryIngSimilaire.Where(m => m.Ingredient.Calorie >= calmin && m.Ingredient.Calorie <= calmax /*&& m.Ingredient.Name.Equals(ingred.ToUpper())*/ && m.Ingredient.Name.ToUpper().StartsWith(ingred.ToUpper())).ToList();
                listeIngredientCatIngSimil.ListIngredientCategory = IngredientCategoryIngSimilaire;
                return View(listeIngredientCatIngSimil);
            }
            else
            {
                IngredientCategoryIngSimilaire = IngredientCategoryIngSimilaire.Where(m => m.Ingredient.Calorie >= calmin & m.Ingredient.Calorie <= calmax & m.Ingredient.Name.Equals(category) & m.CategoryIngredient.Ingredient.Equals(ingred)).ToList();
                listeIngredientCatIngSimil.ListIngredientCategory = IngredientCategoryIngSimilaire;
                return View(listeIngredientCatIngSimil);
            }
        }
    }
}