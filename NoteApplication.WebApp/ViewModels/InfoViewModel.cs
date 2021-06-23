using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteApplication.WebApp.ViewModels
{
    public class InfoViewModel : NotifyViewModelBase<string>
    {
        public InfoViewModel()
        {
            Title = "Information";
        }
    }
}