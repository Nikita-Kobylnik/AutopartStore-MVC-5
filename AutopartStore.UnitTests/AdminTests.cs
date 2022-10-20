using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AutopartStore.Domain.Abstract;
using AutopartStore.Domain.Entities;
using AutopartStore.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AutopartStore.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Games()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IAutopartRepository> mock = new Mock<IAutopartRepository>();
            mock.Setup(m => m.Autoparts).Returns(new List<Autopart>
            {
                new Autopart { AutopartId = 1, Name = "Часть1"},
                new Autopart { AutopartId = 2, Name = "Часть2"},
                new Autopart { AutopartId = 3, Name = "Часть3"},
                new Autopart { AutopartId = 4, Name = "Часть4"},
                new Autopart { AutopartId = 5, Name = "Часть5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            List<Autopart> result = ((IEnumerable<Autopart>)controller.Index().
                ViewData.Model).ToList();

            // Утверждение
            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual("Часть1", result[0].Name);
            Assert.AreEqual("Часть2", result[1].Name);
            Assert.AreEqual("Часть3", result[2].Name);
        }
        [TestMethod]
        public void Can_Edit_Game()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IAutopartRepository> mock = new Mock<IAutopartRepository>();
            mock.Setup(m => m.Autoparts).Returns(new List<Autopart>
            {
                new Autopart { AutopartId = 1, Name = "Часть1"},
                new Autopart { AutopartId = 2, Name = "Часть2"},
                new Autopart { AutopartId = 3, Name = "Часть3"},
                new Autopart { AutopartId = 4, Name = "Часть4"},
                new Autopart { AutopartId = 5, Name = "Часть5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            Autopart autopart1 = controller.Edit(1).ViewData.Model as Autopart;
            Autopart autopart2 = controller.Edit(2).ViewData.Model as Autopart;
            Autopart autopart3 = controller.Edit(3).ViewData.Model as Autopart;

            // Assert
            Assert.AreEqual(1, autopart1.AutopartId);
            Assert.AreEqual(2, autopart2.AutopartId);
            Assert.AreEqual(3, autopart3.AutopartId);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Game()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IAutopartRepository> mock = new Mock<IAutopartRepository>();
            mock.Setup(m => m.Autoparts).Returns(new List<Autopart>
            {
                new Autopart { AutopartId = 1, Name = "Часть1"},
                new Autopart { AutopartId = 2, Name = "Часть2"},
                new Autopart { AutopartId = 3, Name = "Часть3"},
                new Autopart { AutopartId = 4, Name = "Часть4"},
                new Autopart { AutopartId = 5, Name = "Часть5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            Autopart result = controller.Edit(6).ViewData.Model as Autopart;

            // Assert
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IAutopartRepository> mock = new Mock<IAutopartRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Game
            Autopart autopart = new Autopart { Name = "Test" };

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(autopart);

            // Утверждение - проверка того, что к хранилищу производится обращение
            mock.Verify(m => m.SaveAutopart(autopart));

            // Утверждение - проверка типа результата метода
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IAutopartRepository> mock = new Mock<IAutopartRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Game
            Autopart autopart = new Autopart { Name = "Test" };

            // Организация - добавление ошибки в состояние модели
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(autopart);

            // Утверждение - проверка того, что обращение к хранилищу НЕ производится 
            mock.Verify(m => m.SaveAutopart(It.IsAny<Autopart>()), Times.Never());

            // Утверждение - проверка типа результата метода
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

    }
}