using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.Metronic.UI;

namespace Web
{
    public class SelectBoxModel : PageModel
    {
        public List<SelectOption> Options { get; set; }

        public List<SelectOption> Sections { get; set; }

        public void OnGet()
        {
            Options = new List<SelectOption> {
                new SelectOption{Name="Option 1", Value = "1" },
                new SelectOption{Name="Option 2", Value = "2" },
                new SelectOption{Name="Option 3", Value = "3", Order= 2 },
                new SelectOption{Name="Option 4", Value = "4" },
                new SelectOption{Name="Option 5", Value = "5" }
            };

            Sections = new List<SelectOption> {
                new SelectOption{Name="Option 1", Value = "1", Section= "Section 1" },
                new SelectOption{Name="Option 2", Value = "2", Section= "Section 1", Order= 2 },
                new SelectOption{Name="Option 3", Value = "3", Section= "Section 2", Order= 1 },
                new SelectOption{Name="Option 4", Value = "4", Section= "Section 2" },
                new SelectOption{Name="Option 5", Value = "5", Section= "Section 3" }
            };
        } 
    }
}