using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuotationAppv1.Models
{
    public class Quotation
    {
        public int QuotationID { get; set; }
        [Required(ErrorMessage = "Quote Required!")]
        public string Quote { get; set; }
        [Required(ErrorMessage = "Author Required!")]
        public string Author { get; set; }
        [Required(ErrorMessage = "Category Required!")]
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        public DateTime Date { get; set; }
        public virtual ApplicationUser User { get; set; }
     
    }
}
      
 