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
    
    public partial class REZERVASYON
    {
        public int REZERVEID { get; set; }
        public Nullable<int> MUSTERIID { get; set; }
        public string REZERVETARIH { get; set; }
        public string BASLANGICSAAT { get; set; }
        public string BITISAAT { get; set; }
        public Nullable<int> PROGRAM { get; set; }
        public string DURUM { get; set; }
        public string DAMATAD { get; set; }
        public string GELINAD { get; set; }
        public string DAMATTEL { get; set; }
        public string GELINTEL { get; set; }
        public string DAMATEMAIL { get; set; }
        public string GELINEMAIL { get; set; }
        public Nullable<System.DateTime> EVLILIKTARIHI { get; set; }
        public Nullable<int> MUSTERISOZID { get; set; }
        public Nullable<int> MUSTERIEXTID { get; set; }
        public Nullable<int> MUSTERIRESID { get; set; }
        public Nullable<int> SOZLESMEFIYAT { get; set; }
        public Nullable<int> EXTRAFIYAT { get; set; }
        public Nullable<int> ISKONTO { get; set; }
        public Nullable<int> FIYAT { get; set; }
        public Nullable<int> GENELTOPLAM { get; set; }
        public string NOTLAR { get; set; }
        public string CEKIMYERI { get; set; }
        public Nullable<int> EVENTS { get; set; }
        public string REZERVERESIM { get; set; }
    
        public virtual MUSTERILER MUSTERILER { get; set; }
        public virtual PROGRAM PROGRAM1 { get; set; }
    }
}
