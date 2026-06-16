using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStoreOnline.Areas.Admin.Controllers
{
    public abstract class ControllerTemplateMethod : Controller
    {
        protected abstract void PrintRouter();
        public abstract void PrintDIs();

        //Template method
        public void PrintInfomation()
        {
            PrintRouter();
            PrintDIs();
        }
    }
}