﻿using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(TfgDAW.Startup)) ]

namespace TfgDAW
{  public  class Startup
    {

        public void Configuration(IAppBuilder app) 
        {
            app.MapSignalR();

        }

    }
}