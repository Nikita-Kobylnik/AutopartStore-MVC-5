using System.Linq;
using System.Web.Mvc;
using AutopartStore.Domain.Abstract;
using AutopartStore.Domain.Entities;

namespace AutopartStore.WebUI.Controllers
{
    public class AdminController : Controller
    {
        IAutopartRepository repository;

        public AdminController(IAutopartRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View(repository.Autoparts);
        }
        public ViewResult Edit(int autopartId)
        {
            Autopart autopart = repository.Autoparts
                .FirstOrDefault(g => g.AutopartId == autopartId);
            return View(autopart);
        }
        // Перегруженная версия Edit() для сохранения изменений
        [HttpPost]
        public ActionResult Edit(Autopart autopart)
        {
            if (ModelState.IsValid)
            {
                repository.SaveAutopart(autopart);
                TempData["message"] = string.Format("Изменения в товаре \"{0}\" были сохранены", autopart.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(autopart);
            }
        }
        public ViewResult Create()
        {
            return View("Edit", new Autopart());
        }
        [HttpPost]
        public ActionResult Delete(int autopartId)
        {
            Autopart deletedAutopart = repository.DeleteAutopart(autopartId);
            if (deletedAutopart != null)
            {
                TempData["message"] = string.Format("Товар \"{0}\" был удален",
                    deletedAutopart.Name);
            }
            return RedirectToAction("Index");
        }

    }
}