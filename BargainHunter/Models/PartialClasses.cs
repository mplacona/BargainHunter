using System;
using System.ComponentModel.DataAnnotations;

namespace BargainHunter.Models
{
    [MetadataType(typeof(DealMetadata))]
    public partial class Deal : ICloneable
    {
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}