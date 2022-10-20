using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutopartStore.Domain.Abstract;

namespace AutopartStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IAutopartRepository repository;
        public NavController(IAutopartRepository repo)
        {
            repository = repo;
        }
        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<string> categories = repository.Autoparts
                .Select(autopart => autopart.Category)
                .Distinct()
                .OrderBy(x => x);
            return PartialView(categories);
        }
    }
}