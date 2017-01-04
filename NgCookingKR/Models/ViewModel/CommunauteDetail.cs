using System.Collections.Generic;

namespace NgCookingKR.Models.ViewModel
{
    public class CommunauteDetail
    {
        public Creator Creator { get; set; }
        public double Note { get; set; }
        public List<RecipeNote> RecipeNote { get; set; }
    }
}