//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BargainHunter.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Deal
    {
        public string DealCode { get; set; }
        public string DealNick { get; set; }
        public Nullable<double> Price { get; set; }
        public string Url { get; set; }
        public System.DateTime DateCreated { get; set; }
    }
}