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
    
    public partial class RESIMLER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RESIMLER()
        {
            this.MUSTERIRESIMLER = new HashSet<MUSTERIRESIMLER>();
        }
    
        public int RESIMID { get; set; }
        public string RESIMYOL { get; set; }
        public Nullable<int> REZERVEID { get; set; }
        public Nullable<int> MUSTERIID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MUSTERIRESIMLER> MUSTERIRESIMLER { get; set; }
    }
}
