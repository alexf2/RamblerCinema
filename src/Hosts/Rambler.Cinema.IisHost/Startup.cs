﻿using System;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Castle.Windsor.Installer;
using Microsoft.Owin;
using Owin;
using Rambler.Cinema.OwinService.Configuration;
using Rambler.Cinema.OwinService.Extensions;

[assembly: OwinStartup(typeof(Rambler.Cinema.IisHost.Startup))]

namespace Rambler.Cinema.IisHost
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/api", bld =>
            {
                bld.UseRamblerCinema(new RamblerCinemaServiceOptions(FromAssembly.This()));
            });
        }
    }
}
