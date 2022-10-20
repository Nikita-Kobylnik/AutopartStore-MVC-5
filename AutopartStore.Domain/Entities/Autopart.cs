using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AutopartStore.Domain.Entities
{
    public class Autopart
    {
        [HiddenInput(DisplayValue = false)]
        public int AutopartId { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Пожалуйста, введите название запчасти")]
        public string Name { get; set; }

        [Display(Name = "Цена (грн)")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Пожалуйста, введите положительное значение для цены")]
        public decimal Price { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Пожалуйста, введите описание для запчасти")]
        public string Description { get; set; }

        [Display(Name = "Количество на складе")]
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Пожалуйста, введите положительное значение для количества")]
        public int Amount { get; set; }

        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Пожалуйста, укажите категорию для запчасти")]
        public string Category { get; set; }
    }
}
