﻿using System;
using System.Web;

namespace FubuMovies.Infrastructure
{
    class AuthenticationService : IAuthenticationService
    {
        private readonly HttpContextBase httpContext;

        public AuthenticationService(HttpContextBase httpContext)
        {
            this.httpContext = httpContext;
        }

        public bool Authenticate(string username, string password)
        {
            var authenticated = (username == "admin" && password == "admin");
            if(authenticated)
            {
                httpContext.Session["user"] = "admin";
            }
            return authenticated;
        }

        public void Logout()
        {
            httpContext.Session["user"] = null;
        }
    }
}