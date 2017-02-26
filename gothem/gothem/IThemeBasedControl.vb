Imports System.Web.Mvc



Public Class ThemeBaseControl
    Inherits Controller
End Class


Public Interface IThemeBaseSettings
    ReadOnly Property PluginName As String
End Interface
