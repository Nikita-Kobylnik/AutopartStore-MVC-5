using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutopartStore.Domain.Entities;

namespace AutopartStore.WebUI.Models
{
    public class CartIndexViewModel
    {
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }
    }
}