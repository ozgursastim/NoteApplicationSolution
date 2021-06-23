using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteApplication.WebApp.ViewModels
{
    public class NotifyViewModelBase<T>
    {
        public List<T> Items { get; set; }
        public string Header { get; set; }
        public string Title { get; set; }
        public bool IsRedirect { get; set; }
        public string RedirectUrl { get; set; }
        public int RedirectTimeout { get; set; }

        public NotifyViewModelBase()
        {
            Header = "Directing...";
            Title = "Invalid Proccess";
            IsRedirect = true;
            RedirectUrl = "/Home/Index";
            RedirectTimeout = 5000;
            Items = new List<T>();
        }
    }
}