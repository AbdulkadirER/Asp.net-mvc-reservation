//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Foto.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SOZDETAY
    {
        public int DETAYID { get; set; }
        public string DETAYAD { get; set; }
        public Nullable<int> SOZLESMEID { get; set; }
    
        public virtual SOZLESMELER SOZLESMELER { get; set; }
    }
}
