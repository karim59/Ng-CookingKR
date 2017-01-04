using NgCookingKR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using NgCookingKR.Models.ViewModel;

namespace NgCookingKR.Controllers
{
    public class HelpMethod
    {
        private CookingEntities db = new CookingEntities();        
        public ListIngredientCategoryIngSimilaire GetIngredient()
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
                element.LevelCal = (float)(listIngredient[i].Calorie * 100) / 400;
                IngredientCategoryIngSimilaire.Add(element);
            }
            listeIngredientCatIngSimil.ListIngredientCategory = IngredientCategoryIngSimilaire;
            return listeIngredientCatIngSimil;
        }
        public RecipeHome GetRecipe()
        {
            RecipeHome recipe = new RecipeHome();
            List<Recipe> listeRecipe = db.Recipe.ToList();
            List<RecipeNote> listeRecipeNote = new List<RecipeNote>();
            for (int i = 0; i < listeRecipe.Count; i++)
            {
                RecipeNote recipeNote = new RecipeNote();
                int idRecipe = listeRecipe[i].IdRecipe;
                List<Note> listeNote = db.Note.Where(m => m.IdRecipe == idRecipe).ToList();
                if (listeNote.Count==0)
                {
                    recipeNote.note = 1;
                }
                else
                {
                    float note = (float)listeNote.Average(m => m.Record);
                    recipeNote.note = note;
                }
                
                recipeNote.recipe = listeRecipe[i];
                
                listeRecipeNote.Add(recipeNote);
                recipeNote.Calorie = GetRecipeDetail(listeRecipe[i].IdRecipe, "1").RecipeListIngredient.Calorie;
            }
            recipe.RecipeSearch = listeRecipeNote;
            recipe.RecipeNote = listeRecipeNote.OrderByDescending(m => m.note).ToList();
            recipe.RecipeListDate = listeRecipeNote.OrderByDescending(m => m.recipe.CreationDate).ToList();
            return recipe;
        }

        public RecipeDetail GetRecipeDetail(int id, string note)
        {
            double nt = Convert.ToDouble(note);
            RecipeDetail recipedetail = new RecipeDetail();
            Recipe recipe = db.Recipe.Find(id);
            float Calor = 0;
            List<Ingredient> listIngredient = new List<Ingredient>();
            List<RecipeIngredient> recipeIngredient = db.RecipeIngredient.Where(j => j.IdRecipe == id).ToList();

            for (int i = 0; i < recipeIngredient.Count; i++)
            {
                Ingredient ingredient = db.Ingredient.Find(recipeIngredient[i].IdIngredient);
                Calor = Calor + (float)(ingredient.Calorie);
                listIngredient.Add(ingredient);
            }

            List<Note> listeNote = db.Note.Where(j => j.IdRecipe == id).ToList();
            List<CommentCreator> listeCommentCreator = new List<CommentCreator>();

            for (int i = 0; i < listeNote.Count; i++)
            {
                CommentCreator commentCreator = new CommentCreator();
                commentCreator.Comment = listeNote[i].Comment;
                commentCreator.NoteCreator = (double)listeNote[i].Record;
                commentCreator.TitleComment = listeNote[i].TitleComment;
                Creator creator = db.Creator.Find(listeNote[i].IdCreator);
                commentCreator.NameCreator = creator.Name;
                listeCommentCreator.Add(commentCreator);
            }

            RecipeListIngredient recipelistingredient = new RecipeListIngredient();
            recipelistingredient.Recipe = recipe;
            recipelistingredient.ListIngredient = listIngredient;
            recipelistingredient.ListCommentCreator = listeCommentCreator;
            recipelistingredient.Calorie = Calor;
            recipelistingredient.Note = (float)nt;
            recipedetail.RecipeListIngredient = recipelistingredient;
            return recipedetail;
        }

        public int GetRecipeNumber()
        {
            Recipe recipe = new Recipe();
            List<Recipe> listRecipe = db.Recipe.ToList();
            int nbrRecipe = listRecipe.Count;
            return nbrRecipe;
        }

        public bool VerifyRecipeName(string name)
        {
            Recipe recipe = db.Recipe.SingleOrDefault(m => m.Name == name);
            if (recipe != null)
                return false;
            else
            return true;
        }

        public float GetRecord(int id)
        {
            List<Recipe> listRecipe = new List<Recipe>();
            listRecipe = db.Recipe.Where(m => m.IdCreator == id).ToList();
            if (listRecipe.Count > 0 && listRecipe.Count < 3)
                return 2;
            else if (listRecipe.Count >= 3 && listRecipe.Count < 5)
                return 3;
            else
                return 5;

        }
    }
}