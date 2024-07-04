using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;

namespace LukeBlog.Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        void Session_Start(object sender, EventArgs e)
        {
            Session["Account"] = string.Empty;
            Session["Password"] = string.Empty;
            Session["VerificationCode"] = string.Empty;
        }

        void Application_BeginRequest(object sender, EventArgs e)
        {
            //if (Context.Request.FilePath == "/") Context.RewritePath("index.html");
        }
    }
}