Imports System.Web.Mvc
Imports System.Web.Routing

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Protected Sub Application_Start()
        AreaRegistration.RegisterAllAreas()
        RouteConfig.RegisterRoutes(RouteTable.Routes)

        ViewEngines.Engines.Clear()
        ViewEngines.Engines.Add(New gozeer.web.mvc.ViewEngine)

    End Sub
End Class
