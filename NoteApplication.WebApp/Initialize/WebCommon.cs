using NoteApplication.Common;
using NoteApplication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteApplication.WebApp.Initialize
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUsername()
        {
            if (HttpContext.Current.Session["login"] != null)
            {
                NoteUser user = HttpContext.Current.Session["login"] as NoteUser;
                return user.Username;
            }

            return "System";
        }
    }
}