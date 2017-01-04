using NgCookingKR.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NgCookingKR.Models;

namespace NgCookingKR.Controllers
{
    public class RecipesController : Controller
    {
        private CookingEntities db = new CookingEntities();
        HelpMethod met = new HelpMethod();
        
        
        // GET: Recipes
        public ActionResult Recette()
        {        
            RecipeHome recipe = new RecipeHome();
            recipe = met.GetRecipe();
            return View(recipe);
        }
        [HttpPost]
        public ActionResult Recette(string recipeName=null,string ingredientUsed=null,int calMin=0,int calMax=1000)
        {
            RecipeHome recipeHome = new RecipeHome();
            List<RecipeNote> listeRec = new List<RecipeNote>();
            recipeHome = met.GetRecipe();
            if (recipeName.Equals("") & ingredientUsed.Equals(""))
            {
                listeRec=recipeHome.RecipeNote.Where(m=>m.Calorie >= calMin & m.Calorie <= calMax).ToList();
                return View(recipeHome);
            }
            else if (!recipeName.Equals("") & ingredientUsed == "")
            {
                listeRec = recipeHome.RecipeNote.Where(m => (m.recipe.Name.ToUpper().Equals(recipeName.ToUpper())||m.recipe.Name.ToUpper().StartsWith(recipeName.ToUpper()) & m.Calorie>=calMin & m.Calorie<=calMax)).ToList();
                recipeHome.RecipeSearch = listeRec;
                return View(recipeHome);
            }
            else if (!recipeHome.Equals("") & ingredientUsed!="")
            {
                string[] ingredients = ingredientUsed.Split(';');
                foreach (var substring in ingredients)
                    Console.WriteLine(substring);
                return View(recipeHome);
            }
            
            else
                return View(recipeHome);

        }
        // GET: Recipes/Create
        public ActionResult Create()
        {
            RecipeCreate recipeCreate = new RecipeCreate();
            recipeCreate.Description = "";
            return View(recipeCreate);
        }

        // POST: Recipes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RecipeCreate recipeCreate, HttpPostedFileBase file)
        {         
            Recipe recipe = new Recipe();
            if(Session["LogedUserID"]!=null)
            {
                recipe.IdCreator = Convert.ToInt32(Session["LogedUserID"]);
            }
            var recipeExist = db.Recipe.SingleOrDefault(m => m.Name.ToUpper() == recipeCreate.Name.ToUpper());
            string[] ingredients = recipeCreate.AddedIngredients.Split(';');
            List<string> ingredientRemoved = new List<string>();
            if(recipeCreate.RemovedIngredients!=null)
            { 
                ingredientRemoved = recipeCreate.RemovedIngredients.Split(';').ToList();
            }
            int ingredientsNbre = ingredients.Count() - ingredientRemoved.Count();
            if (ModelState.IsValid & recipeExist==null & ingredientsNbre>3)
                {
                file.SaveAs(HttpContext.Server.MapPath("~/img/recettes/") + file.FileName);
                recipe.Photo = file.FileName;
                recipe.CreationDate = DateTime.Now;
                recipe.Name = recipeCreate.Name;
                recipe.Preparation = recipeCreate.Description;
                recipe.IdRecipeCategory = Convert.ToInt32(recipeCreate.idCategory);
                db.Recipe.Add(recipe);
                db.SaveChanges();
                Recipe recSave = db.Recipe.Single(m => m.Name == recipeCreate.Name);
                
                
                foreach (string ing in ingredients)
                {
                    RecipeIngredient recipeIngredient = new RecipeIngredient();
                   if (!ing.Equals("") & !ingredientRemoved.Contains(ing))
                    {
                        Ingredient ingredient = db.Ingredient.Single(m => m.Name == ing);
                        recipeIngredient.IdIngredient = ingredient.IdIngredient;
                        recipeIngredient.IdRecipe = recSave.IdRecipe;
                        db.RecipeIngredient.Add(recipeIngredient);
                        db.SaveChanges();
                    }
                    ingredientRemoved.Remove(ing);
                }
            }

            return View(recipeCreate);
        }

        [HttpPost]
        public JsonResult GetImgIngredient(string nameIngredient, string nbCalor)
        {
            double calorie = Convert.ToDouble(nbCalor);
            Ingredient ingredient = new Ingredient();
            Ingredient ing = db.Ingredient.Single(m => m.Name == nameIngredient);            
            ingredient.IngredientImage = ing.IngredientImage;
            ingredient.Name = ing.Name;
            ingredient.Calorie = ing.Calorie + calorie;
            return Json(ingredient, JsonRequestBehavior.AllowGet);
        }      
        public ActionResult RecipeDetail (int id,string note)
        {
            Recipe recipe = db.Recipe.Single(m => m.IdRecipe == id);

            List<Recipe> listRecipe = db.Recipe.Where(m=>m.IdRecipeCategory==recipe.IdRecipeCategory).ToList();
            List<RecipeNote> recipeLike = new List<RecipeNote>();

            if (listRecipe!=null)
            {
                for(int i=0;i<listRecipe.Count;i++)
                {
                    RecipeNote recipeNote = new RecipeNote();
                    float noteRecipe;
                    int idRecipe = listRecipe[i].IdRecipe;
                    List<Note> listNote = db.Note.Where(m => m.IdRecipe == idRecipe).ToList();
                    if (listNote != null & listNote.Count!=0)
                    {
                        noteRecipe = (float)listNote.Average(m => m.Record);
                    }
                    else
                    {
                        noteRecipe = 1;
                    }
                    recipeNote.recipe = listRecipe[i];
                    recipeNote.note = noteRecipe;
                    //recipeNote.note = note;
                    recipeLike.Add(recipeNote);               
                }
            }


            RecipeDetail recipedetail = new RecipeDetail();
            recipedetail = met.GetRecipeDetail(id,note);
            recipedetail.RecipeLike = recipeLike;
            return View(recipedetail);
        }

        [HttpPost]
        public ActionResult RecipeDetail(RecipeDetail recipeDetail)
        {
            if(ModelState.IsValid)
            { 
            Note nt = new Note();
            nt.Comment = recipeDetail.RecipeListIngredient.Comm;
            nt.Record = recipeDetail.RecipeListIngredient.Note;
            nt.TitleComment = recipeDetail.RecipeListIngredient.Title;
            nt.IdCreator = Convert.ToInt32(Session["LogedUserID"]);
            nt.IdRecipe = recipeDetail.RecipeListIngredient.Recipe.IdRecipe;
            db.Note.Add(nt);
            db.SaveChanges();
            }
            return RedirectToAction("RecipeDetail", new RouteValueDictionary(new { action = "RecipeDetail", id = recipeDetail.RecipeListIngredient.Recipe.IdRecipe, note = 4 }));          
        }

        public JsonResult GetCategory()
        {
            List<string> ingredientCat = db.IngredientCategory.Select(m => m.Name).ToList();
            return Json(ingredientCat, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetIngredientByCategoryId(string categoryName)
        {      
            if (!categoryName.Equals(""))
            {
                int idCategory = db.IngredientCategory.Single(m => m.Name == categoryName).IdIngredientCategory;
                List<string> ingredient = db.Ingredient.Where(m => m.IdIngredientCategory == idCategory).Select(m => m.Name).ToList();
                return Json(ingredient);
            }
            return Json(null);
        }
        public ActionResult Accueil()
        {
            Session["NbreRecipe"] = met.GetRecipeNumber();
            RecipeHome recipeHome = new RecipeHome();
            var note = db.Note.Include(r => r.Creator).Include(r => r.Recipe);
            List<Note> listNote = note.ToList();
            listNote = listNote.OrderByDescending(j => j.IdRecipe).ToList();
            List<RecipeNote> listeRecipeNote = new List<RecipeNote>();
            int idrecipe = listNote[0].IdRecipe;
            float somme = (float)(listNote[0].Record);
            int count = 1;

            for (int i=1;i<listNote.Count;i++)
            { 
                if (listNote[i].IdRecipe==idrecipe)
                {
                somme = somme + (float)(listNote[i].Record);
                count++;
                }

                if(listNote[i].IdRecipe!=idrecipe||i==(listNote.Count-1))
                {
                    Recipe rec = db.Recipe.Find(listNote[i-1].IdRecipe);
                    RecipeNote recipenote = new RecipeNote();
                    recipenote.note = (float)(somme / count);
                    recipenote.recipe = rec;
                    listeRecipeNote.Add(recipenote);
                    somme = (float)listNote[i].Record;
                    count = 1;
                    idrecipe = listNote[i].IdRecipe;
                }
            }
            
            listeRecipeNote = listeRecipeNote.OrderByDescending(j => j.note).ToList();
            recipeHome.RecipeNote = listeRecipeNote;
            List<RecipeNote> listeRecipeDate = new List<RecipeNote>();
            List<Recipe> recipeListe = new List<Recipe>();
            recipeListe = db.Recipe.ToList().OrderByDescending(j => j.CreationDate).ToList();

            for(int i=0;i<recipeListe.Count;i++)
            {
                List<Note> listeNote = db.Note.ToList();
                listeNote = listeNote.Where(j => j.IdRecipe == recipeListe[i].IdRecipe).ToList();
                if (listeNote.Count==0)
                {
                    RecipeNote recipenote = new RecipeNote();
                    recipenote.recipe = recipeListe[i];
                    recipenote.note = 1;
                    listeRecipeDate.Add(recipenote);
                }
                else
                {
                    double a = (float)listeNote.Average(j => j.Record);
                    RecipeNote recipenote = new RecipeNote();
                    recipenote.recipe = recipeListe[i];
                    recipenote.note = (float)a;
                    listeRecipeDate.Add(recipenote);
                }
            }
            recipeHome.RecipeListDate = listeRecipeDate;
            return View(recipeHome);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }     
    }
}
