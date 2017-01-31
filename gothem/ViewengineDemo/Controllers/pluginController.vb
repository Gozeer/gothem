Imports System.Web.Mvc

Namespace Controllers
    <ThemeSettings(PluginName:="testplugin")>
    Public Class pluginController
        Inherits Controller

        ' GET: plugin
        Function Index() As ActionResult
            Return View()
        End Function
    End Class
End Namespace