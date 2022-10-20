using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutopartStore.Domain.Entities;
using AutopartStore.Domain.Abstract;
using AutopartStore.WebUI.Models;

namespace AutopartStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IAutopartRepository repository;
        private IOrderProcessor orderProcessor;

        public CartController(IAutopartRepository repo, IOrderProcessor processor)
        {
            repository = repo;
            orderProcessor = processor;
        }

        public RedirectToRouteResult AddToCart(Cart cart, int autopartId, string returnUrl)
        {
            Autopart autopart = repository.Autoparts
                .FirstOrDefault(g => g.AutopartId == autopartId);

            if (autopart != null)
            {
                cart.AddItem(autopart, 1);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int autopartId, string returnUrl)
        {
            Autopart autopart = repository.Autoparts
                .FirstOrDefault(g => g.AutopartId == autopartId);

            if (autopart != null)
            {
                cart.RemoveLine(autopart);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        //public Cart GetCart()
        //{
        //    Cart cart = (Cart)Session["Cart"];
        //    if (cart == null)
        //    {
        //        cart = new Cart();
        //        Session["Cart"] = cart;
        //    }
        //    return cart;
        //}

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Извините, ваша корзина пуста!");
            }

            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }

    }
}