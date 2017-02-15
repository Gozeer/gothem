Imports System.Globalization

Namespace gozeer.web.mvc
    Public Class AreaViewLocation
        Inherits ViewLocation

        Public Sub New(virtualPathFormatString As String)
            MyBase.New(virtualPathFormatString)
        End Sub

        Public Overrides Function Format(viewName As String, controllerName As String, plugin As String, Optional area As String = "") As String
            Return String.Format(CultureInfo.InvariantCulture, _virtualPathFormatString, gozeer.web.mvc.ThemeSettings.ThemeName, viewName, controllerName, plugin, area)
        End Function


    End Class

End Namespace
