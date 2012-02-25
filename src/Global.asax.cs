using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Security;
using System.Web.SessionState;
using FubuCore;
using FubuCore.Binding;
using FubuCore.Reflection;
using FubuMovies.Core;
using FubuMovies.FubuConfiguration;
using FubuMovies.Infrastructure;
using FubuMovies.Login;
using FubuMovies.Timetable;
using FubuMovies.Web;
using FubuMovies.Web.Api;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Diagnostics;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Conventions;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.Core.Registration.Routes;
using FubuMVC.Core.Security;
using FubuMVC.Core.Security.AntiForgery;
using FubuMVC.Core.UI;
using FubuMVC.Core.UI.Configuration;
using FubuMVC.Core.UI.Extensibility;
using FubuMVC.Core.Urls;
using FubuMVC.Spark;
using FubuMVC.StructureMap;
using FubuMVC.Validation;
using FubuValidation;
using HtmlTags;
using StructureMap;

namespace FubuMovies
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            FubuApplication
                .For<FubuMoviesRegistry>()
                .StructureMap(() => new Container(new NHibernateRegistry()))
                .Bootstrap();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }

    
}