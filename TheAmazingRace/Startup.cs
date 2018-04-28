using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using TheAmazingRace.DAL;
using TheAmazingRace.Models;

[assembly: OwinStartupAttribute(typeof(TheAmazingRace.Startup))]
namespace TheAmazingRace
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
