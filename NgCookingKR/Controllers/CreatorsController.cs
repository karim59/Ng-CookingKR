using NgCookingKR.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NgCookingKR.Controllers
{
    public class CreatorsController : Controller
    {
        private CookingEntities db = new CookingEntities();
        HelpMethod met = new HelpMethod();

        [HttpPost]
        public ActionResult Connexion(Creator creator)
        {
            if (ModelState.IsValid)
            {
                var v = db.Creator.Where(a => a.Name == creator.Name && a.Password == creator.Password).FirstOrDefault();
                if (v != null)
                {
                    Session["LogedUserID"] = v.IdCreator.ToString();
                    Session["LogedUserName"] = v.Name;
                    Session["LogedUserPassword"] = v.Password;
                    return RedirectToAction("Accueil", "Recipes");
                }
                
                return RedirectToAction("Accueil", "Recipes");

            }
           
            return RedirectToAction("Communaute", "Communautes");
            
        }

        public ActionResult Deconnexion()
        {
            if(Session["LogedUserID"]!=null)
            {
                Session["LogedUserID"] = null;
                Session["LogedUserName"] = null;
                Session["LogedUserPassword"] = null;
                return RedirectToAction("Accueil", "Recipes");
            }
            return RedirectToAction("Accueil", "Recipes");
        }

        public ActionResult MyProfil()
        {
            int id = Convert.ToInt32(Session["LogedUserID"]);
            if (id != null)
            {
                float noteCreator = met.GetRecord(id);
                return RedirectToAction("CommunauteDetail", "Communautes", new RouteValueDictionary(new { idCreator = id, note = noteCreator }));
            }
            else
                return View("Error (vous devez être identifié pour afficher cette page)");
            
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
