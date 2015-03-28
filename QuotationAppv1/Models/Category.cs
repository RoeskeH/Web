using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace QuotationAppv1.Models
{
    public class Category 
    {
   
      
        public int CategoryID { get; set;  }

        
        [Display(Name = "Category")] 
        public string Name { get; set; }

        
        public virtual List<Quotation> Quotations { get; set; }
    
    
    }
}
