using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using AutopartStore.Domain.Entities;
using System.Linq;
using Moq;
using AutopartStore.Domain.Abstract;
using AutopartStore.WebUI.Controllers;
using System.Web.Mvc;
using AutopartStore.WebUI.Models;

namespace AutopartStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // Организация - создание нескольких тестовых игр
            Autopart autopart1 = new Autopart { AutopartId = 1, Name = "Часть1" };
            Autopart autopart2 = new Autopart { AutopartId = 2, Name = "Часть2" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(autopart1, 1);
            cart.AddItem(autopart2, 1);
            List<CartLine> results = cart.Lines.ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Autopart, autopart1);
            Assert.AreEqual(results[1].Autopart, autopart2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // Организация - создание нескольких тестовых игр
            Autopart autopart1 = new Autopart { AutopartId = 1, Name = "Часть1" };
            Autopart autopart2 = new Autopart { AutopartId = 2, Name = "Часть2" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(autopart1, 1);
            cart.AddItem(autopart2, 1);
            cart.AddItem(autopart1, 5);
            List<CartLine> results = cart.Lines.OrderBy(c => c.Autopart.AutopartId).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Quantity, 6);    // 6 экземпляров добавлено в корзину
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            // Организация - создание нескольких тестовых игр
            Autopart autopart1 = new Autopart { AutopartId = 1, Name = "Часть1" };
            Autopart autopart2 = new Autopart { AutopartId = 2, Name = "Часть2" };
            Autopart autopart3 = new Autopart { AutopartId = 3, Name = "Часть3" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - добавление нескольких игр в корзину
            cart.AddItem(autopart1, 1);
            cart.AddItem(autopart2, 4);
            cart.AddItem(autopart3, 2);
            cart.AddItem(autopart2, 1);

            // Действие
            cart.RemoveLine(autopart2);

            // Утверждение
            Assert.AreEqual(cart.Lines.Where(c => c.Autopart == autopart2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            // Организация - создание нескольких тестовых игр
            Autopart autopart1 = new Autopart { AutopartId = 1, Name = "Часть1", Price = 100 };
            Autopart autopart2 = new Autopart { AutopartId = 2, Name = "Часть2", Price = 55 };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(autopart1, 1);
            cart.AddItem(autopart2, 1);
            cart.AddItem(autopart1, 5);
            decimal result = cart.ComputeTotalValue();

            // Утверждение
            Assert.AreEqual(result, 655);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            // Организация - создание нескольких тестовых игр
            Autopart autopart1 = new Autopart { AutopartId = 1, Name = "Часть1", Price = 100 };
            Autopart autopart2 = new Autopart { AutopartId = 2, Name = "Часть2", Price = 55 };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(autopart1, 1);
            cart.AddItem(autopart2, 1);
            cart.AddItem(autopart1, 5);
            cart.Clear();

            // Утверждение
            Assert.AreEqual(cart.Lines.Count(), 0);
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            // Организация - создание имитированного хранилища
            Mock<IAutopartRepository> mock = new Mock<IAutopartRepository>();
            mock.Setup(m => m.Autoparts).Returns(new List<Autopart> {
        new Autopart {AutopartId = 1, Name = "Часть1", Category = "Кат1"},
    }.AsQueryable());

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController controller = new CartController(mock.Object, null);

            // Действие - добавить игру в корзину
            controller.AddToCart(cart, 1, null);

            // Утверждение
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToList()[0].Autopart.AutopartId, 1);
        }

        /// <summary>
        /// После добавления игры в корзину, должно быть перенаправление на страницу корзины
        /// </summary>
        [TestMethod]
        public void Adding_Game_To_Cart_Goes_To_Cart_Screen()
        {
            // Организация - создание имитированного хранилища
            Mock<IAutopartRepository> mock = new Mock<IAutopartRepository>();
            mock.Setup(m => m.Autoparts).Returns(new List<Autopart> {
            new Autopart {AutopartId = 1, Name = "Часть1", Category = "Кат1"},
            }.AsQueryable());

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController controller = new CartController(mock.Object, null);

            // Действие - добавить игру в корзину
            RedirectToRouteResult result = controller.AddToCart(cart, 2, "myUrl");

            // Утверждение
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        // Проверяем URL
        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController target = new CartController(null, null);

            // Действие - вызов метода действия Index()
            CartIndexViewModel result
                = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            // Утверждение
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            Cart cart = new Cart();
            cart.AddItem(new Autopart(), 1);

            // Организация — создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Организация — добавление ошибки в модель
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Утверждение - проверка, что заказ не передается обработчику
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Утверждение - проверка, что метод вернул стандартное представление
            Assert.AreEqual("", result.ViewName);

            // Утверждение - проверка, что-представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            Cart cart = new Cart();
            cart.AddItem(new Autopart(), 1);

            // Организация — создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Утверждение - проверка, что заказ передан обработчику
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Once());

            // Утверждение - проверка, что метод возвращает представление 
            Assert.AreEqual("Completed", result.ViewName);

            // Утверждение - проверка, что представлению передается допустимая модель
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }

    }
}
