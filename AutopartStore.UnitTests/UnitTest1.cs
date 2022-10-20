using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AutopartStore.Domain.Abstract;
using AutopartStore.Domain.Entities;
using AutopartStore.WebUI.Controllers;
using AutopartStore.WebUI.Models;
using AutopartStore.WebUI.HtmlHelpers;

namespace AutopartStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Организация (arrange)
            Mock<IAutopartRepository> mock = new Mock<IAutopartRepository>();
            mock.Setup(m => m.Autoparts).Returns(new List<Autopart>
            {
                new Autopart { AutopartId = 1, Name = "Name1"},
                new Autopart { AutopartId = 2, Name = "Name2"},
                new Autopart { AutopartId = 3, Name = "Name3"},
                new Autopart { AutopartId = 4, Name = "Name4"},
                new Autopart { AutopartId = 5, Name = "Name5"}
            });
            AutopartController controller = new AutopartController(mock.Object);
            controller.pageSize = 3;

            // Действие (act)
            AutopartsListViewModel result = (AutopartsListViewModel)controller.List(null,2).Model;

            // Утверждение (assert)
            List<Autopart> autoparts = result.Autoparts.ToList();
            Assert.IsTrue(autoparts.Count == 2);
            Assert.AreEqual(autoparts[0].Name, "Name4");
            Assert.AreEqual(autoparts[1].Name, "Name5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {

            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }


        [TestMethod]

        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            Mock<IAutopartRepository> mock = new Mock<IAutopartRepository>();
            mock.Setup(m => m.Autoparts).Returns(new List<Autopart>
                {
                    new Autopart { AutopartId = 1, Name = "Часть1"},
                    new Autopart { AutopartId = 2, Name = "Часть2"},
                    new Autopart { AutopartId = 3, Name = "Часть3"},
                    new Autopart { AutopartId = 4, Name = "Часть4"},
                    new Autopart { AutopartId = 5, Name = "Часть5"}
                });
            AutopartController controller = new AutopartController(mock.Object);
            controller.pageSize = 3;

            // Act
            AutopartsListViewModel result = (AutopartsListViewModel)controller.List(null,2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }
        [TestMethod]
        public void Can_Filter_Games()
        {
            // Организация (arrange)
            Mock<IAutopartRepository> mock = new Mock<IAutopartRepository>();
            mock.Setup(m => m.Autoparts).Returns(new List<Autopart>
            {
                new Autopart { AutopartId = 1, Name = "Часть1", Category="Cat1"},
                new Autopart { AutopartId = 2, Name = "Часть2", Category="Cat2"},
                new Autopart { AutopartId = 3, Name = "Часть3", Category="Cat1"},
                new Autopart { AutopartId = 4, Name = "Часть4", Category="Cat2"},
                new Autopart { AutopartId = 5, Name = "Часть5", Category="Cat3"}
            });
            AutopartController controller = new AutopartController(mock.Object);
            controller.pageSize = 3;

            // Action
            List<Autopart> result = ((AutopartsListViewModel)controller.List("Cat2", 1).Model)
                .Autoparts.ToList();

            // Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Часть2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "Часть4" && result[1].Category == "Cat2");
        }
    }
}
