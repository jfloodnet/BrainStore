﻿using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace IEventSourcedMyBrain
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
 }