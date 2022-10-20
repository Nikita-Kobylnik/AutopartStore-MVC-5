using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutopartStore.Domain.Entities;

namespace AutopartStore.WebUI.Models
{
    public class AutopartsListViewModel
    {
        public IEnumerable<Autopart> Autoparts { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}