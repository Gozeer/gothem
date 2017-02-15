Imports System.Globalization
Imports System.Web
Imports System.Web.Routing

Namespace gozeer.web.mvc
    Public Class ViewLocation
        Protected ReadOnly _virtualPathFormatString As String
        Public Sub New(virtualPathFormatString As String)
            _virtualPathFormatString = virtualPathFormatString
        End Sub
        Public Overridable Function Format(viewName As String, controllerName As String, plugin As String, Optional area As String = "") As String
            Dim currentContext As HttpContextBase = New HttpContextWrapper(HttpContext.Current)
            Dim routeData As RouteData = RouteTable.Routes.GetRouteData(currentContext)


            Try
                Return String.Format(CultureInfo.InvariantCulture, _virtualPathFormatString, gozeer.web.mvc.ThemeSettings.ThemeName, viewName, controllerName, plugin, area)
            Catch ex As Exception
                Throw New ArgumentException(_virtualPathFormatString)
            End Try
        End Function
    End Class

End Namespace
