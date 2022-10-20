using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AutopartStore.Domain.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage = "Укажите Имя и фамилию")]
        public string Name { get; set; }
        public string Lastname { get; set; }
        [Required(ErrorMessage = "Укажите номер телефона")]
        [Display(Name = "Номер телефона")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Укажите адрес доставки")]
        [Display(Name = "Улица, дом")]
        public string Line1 { get; set; }

        //[Display(Name = "Второй адрес")]
        //public string Line2 { get; set; }


        [Required(ErrorMessage = "Укажите город")]
        [Display(Name = "Город")]
        public string City { get; set; }

        [Required(ErrorMessage = "Укажите страну")]
        [Display(Name = "Страна")]
        public string Country { get; set; }

        public bool DeliveryMethod { get; set; }
        
    }
}
