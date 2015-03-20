using System;
using System.ComponentModel.DataAnnotations;

namespace BargainHunter.Models
{
    public class DealMetadata
    {
        [Display(Name = "Deal URL")]
        [Required(ErrorMessage = "Please enter a deal URL", AllowEmptyStrings = false)]
        public string DealCode;

        [Display(Name = "Deal Name")]
        [Required(ErrorMessage = "Please enter a deal name", AllowEmptyStrings = false)]
        public string DealNick;

        [DisplayFormat(DataFormatString = "{0:g}")]
        public DateTime DateCreated;

        [DisplayFormat(DataFormatString = "{0:c}")]
        public float Price;
    }
}