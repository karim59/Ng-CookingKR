//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NgCookingKR.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Note
    {
        public int IdNote { get; set; }
        public string Comment { get; set; }
        public int IdCreator { get; set; }
        public int IdRecipe { get; set; }
        public Nullable<double> Record { get; set; }
        public string TitleComment { get; set; }
    
        public virtual Creator Creator { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
