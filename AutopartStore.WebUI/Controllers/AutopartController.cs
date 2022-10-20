using AutopartStore.Domain.Abstract;
using AutopartStore.Domain.Entities;
using AutopartStore.WebUI.Controllers;
using AutopartStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutopartStore.WebUI.Controllers
{
    public class AutopartController : Controller
    {
        private IAutopartRepository repository;
        public int pageSize = 6;
        public AutopartController(IAutopartRepository repo)
        {
            repository = repo;
        }
        //public ViewResult List()
        //{
        //    return View(repository.Autoparts);
        //}
        public ViewResult List(string category, int page = 1)
        {
            //return View(repository.Autoparts.OrderBy(autopart => autopart.AutopartId).Skip((page - 1) * pageSize).Take(pageSize));
            AutopartsListViewModel model = new AutopartsListViewModel
            {
                Autoparts = repository.Autoparts
                .Where(p => category == null || p.Category == category)
                .OrderBy(autopart => autopart.AutopartId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ? 
                    repository.Autoparts.Count() :
                    repository.Autoparts.Where(autopart => autopart.Category == category).Count()
                },
                CurrentCategory = category
            };
            return View(model);
        }
    }
}