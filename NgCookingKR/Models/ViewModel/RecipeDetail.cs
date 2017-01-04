using System.Collections.Generic;

namespace NgCookingKR.Models.ViewModel
{
    public class RecipeDetail
    {
        public RecipeListIngredient RecipeListIngredient { get; set; }
        public List<RecipeNote> RecipeLike { get; set; }
    

    }

    public class RecipeListIngredient
    {
        public Recipe Recipe { get; set; }
        public List<Ingredient> ListIngredient { get; set; }

        public List<CommentCreator> ListCommentCreator { get; set; }

        public float Calorie { get; set; }

        public float Note { get; set; }

        public string Title { get; set; }
        public string Comm { get; set; }
    }

    public class CommentCreator
    {
        public string NameCreator { get; set; }

        public double NoteCreator { get; set; }
        public string Comment { get; set; }
        
        public string TitleComment { get; set; }

    }
   
}