using NgCookingKR.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using NgCookingKR.Models.ViewModel;


namespace NgCookingKR.Controllers
{
    public class CommunautesController : Controller
    {
        private CookingEntities db = new CookingEntities();

        // GET: Communaute
        public ActionResult Index()
        {
            return View(db.Creator.ToList());
        }

        public ActionResult Communaute()
        {
            Communaute communaute = new Communaute();
            List<Creator> listeCreator = db.Creator.ToList();
            List<CreatorNote> listeCreatorNote = new List<CreatorNote>();
            for (int i = 0; i < listeCreator.Count; i++)
            {
                double note = 0;
                int idCreator = listeCreator[i].IdCreator;
                List<Recipe> listeRecipe = new List<Recipe>();
                listeRecipe = db.Recipe.Where(j => j.IdCreator == idCreator).ToList();

                if (listeRecipe.Count != 0)
                {
                    for (int j = 0; j < listeRecipe.Count; j++)
                    {
                        int idRecipe = listeRecipe[j].IdRecipe;


                        List<Note> listNote = db.Note.Where(m => m.IdRecipe == idRecipe).ToList();

                        if (listNote.Count != 0 && listNote != null)
                        {
                            double nt = (double)listNote.Average(r => r.Record);
                            note = note + nt;
                        }
                        else
                            note = 1;
                    }
                    note = note / listeRecipe.Count;

                    CreatorNote creatorNote = new CreatorNote();
                    creatorNote.Creator = listeCreator[i];
                    creatorNote.Note = note;
                    listeCreatorNote.Add(creatorNote);
                }
                else
                {
                    CreatorNote creatorNote = new CreatorNote();
                    creatorNote.Creator = listeCreator[i];
                    creatorNote.Note = 1;
                    listeCreatorNote.Add(creatorNote);
                }
            }
            communaute.CreatorNote = listeCreatorNote;
            return View(communaute);
        }

        public ActionResult CommunauteDetail(int idCreator, string note)
        {
            double nt = Convert.ToDouble(note);
            CommunauteDetail communauteDetails = new CommunauteDetail();
            Creator creator = db.Creator.Find(idCreator);
            List<Recipe> listeRecipe = db.Recipe.Where(m => m.IdCreator == idCreator).ToList();
            communauteDetails.Creator = creator;
            communauteDetails.Note = nt;
            List<RecipeNote> listeRecipeNote = new List<RecipeNote>();

            for (int i = 0; i < listeRecipe.Count; i++)
            {
                RecipeNote recipeNote = new RecipeNote();
                int idRecipe = listeRecipe[i].IdRecipe;
                List<Note> listeNote = db.Note.Where(m => m.IdRecipe == idRecipe).ToList();
                float noteMoyenne;
                if (listeNote.Count != 0)
                {
                    noteMoyenne = (float)listeNote.Average(m => m.Record);
                }
                else
                { 
                    noteMoyenne = 1;
                }
                recipeNote.note = noteMoyenne;
                recipeNote.recipe = listeRecipe[i];
                listeRecipeNote.Add(recipeNote);
            }

            communauteDetails.RecipeNote = listeRecipeNote;
            return View(communauteDetails);
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
