using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.Metronic.UI;

namespace Web
{
    public class DashboardModel : PageModel
    {
        public List<SelectOption> Genders { get; set; }

        public List<SelectOption> DepartmentOptions { get; set; }

        public void OnGet()
        {
            Genders = new List<SelectOption> {
                new SelectOption{  Name = "男",Value =  "男"},
                new SelectOption{  Name = "女",Value =  "女"}
            };

            DepartmentOptions = new List<SelectOption> {
                new SelectOption{  Name = "部门1",Value =  "1"},
                new SelectOption{  Name = "部门2",Value =  "2"}
            };
        }
    }
}