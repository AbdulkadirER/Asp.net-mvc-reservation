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
    
    public partial class MUSTERILER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MUSTERILER()
        {
            this.MUSTERIEXTRALAR = new HashSet<MUSTERIEXTRALAR>();
            this.MUSTERIRESIMLER = new HashSet<MUSTERIRESIMLER>();
            this.MUSTERISOZLESMELER = new HashSet<MUSTERISOZLESMELER>();
            this.REZERVASYON = new HashSet<REZERVASYON>();
        }
    
        public int MUSTERIID { get; set; }
        public string MUSTERIADSOYAD { get; set; }
        public string MUSTERITELEFON { get; set; }
        public Nullable<int> MUSTERISIFRE { get; set; }
        public Nullable<bool> YETKILIMI { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MUSTERIEXTRALAR> MUSTERIEXTRALAR { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MUSTERIRESIMLER> MUSTERIRESIMLER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MUSTERISOZLESMELER> MUSTERISOZLESMELER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<REZERVASYON> REZERVASYON { get; set; }
    }
}